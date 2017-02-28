using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.AOP.Utilities
{
    public class Optional<T>
    {
        public Optional()
        {
            HasValue = false;
        }

        public Optional(T value)
        {
            HasValue = true;
            Value = value;
        }

        public bool HasValue { get; set; }

        public T Value { get; set; }
    }
}
