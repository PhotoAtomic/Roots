    namespace PhotoAtomic.Reflection
    {


        using System;
        using System.Diagnostics;
        using System.Linq.Expressions;
        using System.Reflection;

        public static class Reflect
        {
            public static string NameOf<T>(Expression<Action<T>> action)
            {
                var expression = action.Body as MethodCallExpression;
                if (expression != null)
                {
                    return expression.Method.Name;
                }
                throw new InvalidOperationException("Lambda is not a method call");
            }

            public static MethodInfo CallGenericOn<T>(Expression<Action<T>> action, params Type[] types)
            {
                return
                    typeof(T).GetMethod(Reflect.NameOf<T>(action))
                    .GetGenericMethodDefinition()
                    .MakeGenericMethod(types);
            }

            public static string CurrentMethodName()
            {
                StackTrace stackTrace = new StackTrace();           // get call stack
                StackFrame[] stackFrames = stackTrace.GetFrames();
                if (stackFrames != null && stackFrames.Length >= 1) return stackFrames[1].GetMethod().Name;
                throw new InvalidOperationException("Could not retrieve caller method name");
            }

            public static T OfType<T>()
            {
                return default(T);
            }
        }
    }

