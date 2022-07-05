using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public static class AddStepHandler
    {
        private abstract class AddStepHandlerBase : IAddStepHandler
        {
            private static readonly MethodInfo CreateFuncMethod = typeof(AddStepHandlerBase).GetMethod(nameof(CreateFunc), BindingFlags.Static | BindingFlags.NonPublic)!;

            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate?> _fastDict = new();
            
            protected AddStepHandlerBase() : this(ImmutableDictionary<Type, Delegate>.Empty)
            {
            }

            protected AddStepHandlerBase(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public virtual Traversal AddStep<TStep>(Traversal traversal, TStep step, IGremlinQueryEnvironment environment) where TStep : Step
            {
                return TryGetAddHandler(typeof(TStep), step.GetType()) is Func<Traversal, TStep, IGremlinQueryEnvironment, IAddStepHandler, Traversal> del
                    ? del(traversal, step, environment, this)
                    : traversal.Push(step);
            }

            public virtual IAddStepHandler Override<TStep>(Func<Traversal, TStep, IGremlinQueryEnvironment, Func<Traversal, TStep, IGremlinQueryEnvironment, IAddStepHandler, Traversal>, IAddStepHandler, Traversal> addStepHandler) where TStep : Step
            {
                return new AddStepHandlerImpl(
                    _dict.SetItem(
                        typeof(TStep),
                        TryGetAddHandler(typeof(TStep), typeof(TStep)) is Func<Traversal, TStep, IGremlinQueryEnvironment, IAddStepHandler, Traversal> existingAddHandler
                            ? (steps, step, env, _, recurse) => addStepHandler(steps, step, env, existingAddHandler, recurse)
                            : addStepHandler));
            }

            private Delegate? TryGetAddHandler(Type staticType, Type actualType)
            {
                return _fastDict
                    .GetOrAdd(
                        (staticType, actualType),
                        static (typeTuple, @this) =>
                        {
                            var (staticType, actualType) = typeTuple;

                            if (@this.InnerLookup(actualType) is { } del)
                            {
                                var effectiveType = del
                                    .GetType()
                                    .GetGenericArguments()[1];

                                var method = CreateFuncMethod
                                    .MakeGenericMethod(staticType, effectiveType);

                                return (Delegate)method
                                    .Invoke(null, new object[] { del })!;
                            }

                            return null;
                        },
                        this);
            }

            private Delegate? InnerLookup(Type actualType)
            {
                return _dict.TryGetValue(actualType, out var ret)
                    ? ret
                    : actualType.BaseType is { } baseType && InnerLookup(baseType) is { } baseHandler
                        ? baseHandler
                        : null;
            }

            private static Func<Traversal, TStatic, IGremlinQueryEnvironment, IAddStepHandler, Traversal> CreateFunc<TStatic, TEffective>(Func<Traversal, TEffective, IGremlinQueryEnvironment, Func<Traversal, TEffective, IGremlinQueryEnvironment, IAddStepHandler, Traversal>, IAddStepHandler, Traversal> del)
                where TEffective : Step
                where TStatic : Step
            {
                return (steps, step, environment, recurse) => del(steps, (TEffective)(Step)step!, environment, static (steps, step, _, _) => steps.Push(step), recurse);
            }
        }

        private sealed class AddStepHandlerImpl : AddStepHandlerBase
        {
            public AddStepHandlerImpl(IImmutableDictionary<Type, Delegate> dict) : base(dict)
            {
            }
        }

        private sealed class EmptyAddStepHandler : AddStepHandlerBase
        {
            public override Traversal AddStep<TStep>(Traversal traversal, TStep step, IGremlinQueryEnvironment environment) => traversal.Push(step);
        }

        public static readonly IAddStepHandler Empty = new EmptyAddStepHandler();

        public static readonly IAddStepHandler Default = Empty;
    }
}


