using System.Collections.Generic;
using System.Linq;

namespace Unity.AOP.Utilities
{
    public class ArgumentsList : List<object>
    {
        public ArgumentsList(IEnumerable<object> arguments)
            : base(arguments)
        {
        }

        public override bool Equals(object obj)
        {
            var anotherList = obj as IEnumerable<object>;

            return anotherList != null && this.SequenceEqual(anotherList);
        }

        public override int GetHashCode()
        {
            return this.Aggregate(0, (hash, x) => hash ^ x.GetHashCode());
        }
    }
}
