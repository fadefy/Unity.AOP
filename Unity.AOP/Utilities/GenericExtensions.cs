using System;
using System.Text;

namespace Unity.AOP.Utilities
{
    public static class GenericExtensions
    {
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, Func<object> valueFactory)
        {
            if (condition)
                builder.Append(valueFactory());

            return builder;
        }
    }
}
