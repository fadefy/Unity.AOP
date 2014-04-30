using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Unity.AOP.Utilities
{
    public static class ExpressionExtensions
    {
        public static string GetName(this LambdaExpression expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null)
                return member.Member.Name;

            throw new ArgumentException();
        }
    }
}
