using System;
using System.Linq.Expressions;
using Microsoft.Practices.Unity;
using Unity.AOP.Utilities;

namespace Unity.AOP.Framework
{
    public class LambdaPropertyOverride : PropertyOverride
    {
        public LambdaPropertyOverride(Expression<Func<object>> propertyExpression, object value)
            : base(propertyExpression.GetName(), value)
        {
        }
    }

    public class LambdaPropertyOverride<T> : PropertyOverride
    {
        public LambdaPropertyOverride(Expression<Func<T, object>> propertyExpression, object value)
            : base(propertyExpression.GetName(), value)
        {
        }
    }
}
