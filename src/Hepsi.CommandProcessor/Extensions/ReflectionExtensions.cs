using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hepsi.CommandProcessor.Attributes;


namespace Hepsi.CommandProcessor.Extensions
{
    internal static class ReflectionExtensions
    {
        internal static IEnumerable<RequestHandlerAttribute> GetOtherHandlersInPipeline(this MethodInfo targetMethod)
        {
            var customAttributes = targetMethod.GetCustomAttributes(true);
            return customAttributes
                     .Select(attr => (Attribute)attr)
                     .Where(a => a.GetType().BaseType == typeof(RequestHandlerAttribute))
                     .Cast<RequestHandlerAttribute>()
                     .ToList();
        }
    }
}
