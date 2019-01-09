using System;

namespace LanguageExt
{
    internal static class OptionExtensions
    {
        public static Option<T> IfNone<T>(this Option<T> option, Func<Option<T>> none)
        {
            return option.IsSome ? option : none();
        }
    }
}
