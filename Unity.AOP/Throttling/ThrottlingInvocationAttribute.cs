using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.AOP.Throttling
{
    public class ThrottlingInvocationAttribute : GenericHandlerAttribute
    {
        public ThrottlingInvocationAttribute()
            : base(typeof(ThrottlingCallHandler))
        {
        }
    }
}
