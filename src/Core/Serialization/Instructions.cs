using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    internal static class Instructions
    {
        public static readonly Instruction barrier = new(nameof(barrier));
        public static readonly Instruction bothV = new(nameof(bothV));
        public static readonly Instruction by = new(nameof(by));
        public static readonly Instruction count = new(nameof(count));
        public static readonly Instruction cyclicPath = new(nameof(cyclicPath));
        public static readonly Instruction drop = new(nameof(drop));
        public static readonly Instruction emit = new(nameof(emit));
        public static readonly Instruction explain = new(nameof(explain));
        public static readonly Instruction dedup = new(nameof(dedup));
        public static readonly Instruction fail = new(nameof(fail));
        public static readonly Instruction fold = new(nameof(fold));
        public static readonly Instruction group = new(nameof(group));
        public static readonly Instruction id = new(nameof(id));
        public static readonly Instruction identity = new(nameof(identity));
        public static readonly Instruction inV = new(nameof(inV));
        public static readonly Instruction key = new(nameof(key));
        public static readonly Instruction label = new(nameof(label));
        public static readonly Instruction max = new(nameof(max));
        public static readonly Instruction mean = new(nameof(mean));
        public static readonly Instruction min = new(nameof(min));
        public static readonly Instruction none = new(nameof(none));
        public static readonly Instruction order = new(nameof(order));
        public static readonly Instruction otherV = new(nameof(otherV));
        public static readonly Instruction outV = new(nameof(outV));
        public static readonly Instruction path = new(nameof(path));
        public static readonly Instruction profile = new(nameof(profile));
        public static readonly Instruction simplePath = new(nameof(simplePath));
        public static readonly Instruction sum = new(nameof(sum));
        public static readonly Instruction unfold = new(nameof(unfold));
        public static readonly Instruction value = new(nameof(value));
    }
}

