﻿namespace RomanSPA {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Web;

    public static class InvocationHelper {
        
        public static object InvokeGenericMethodWithDynamicTypeArguments<T>(T target, Expression<Func<T, object>> expression, object[] methodArguments, params Type[] typeArguments) {
            
            var methodInfo = ReflectionHelper.GetMethod(expression);
            if (methodInfo.GetGenericArguments().Length != typeArguments.Length)
                throw new ArgumentException(
                    string.Format("The method '{0}' has {1} type argument(s) but {2} type argument(s) were passed. The amounts must be equal.",
                                methodInfo.Name,
                                methodInfo.GetGenericArguments().Length,
                                typeArguments.Length));

            return methodInfo
                .GetGenericMethodDefinition()
                .MakeGenericMethod(typeArguments)
                .Invoke(target, methodArguments);
        }
    }

    public class ReflectionHelper {
        public static MethodInfo GetMethod<T>(Expression<Func<T, object>> expression) {
            MethodCallExpression methodCall = (MethodCallExpression)expression.Body;
            return methodCall.Method;
        }
    }
}