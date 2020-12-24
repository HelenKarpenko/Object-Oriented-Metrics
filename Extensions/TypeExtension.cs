namespace ObjectOrientedMetrics.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class TypeExtension
    {
        public static IEnumerable<Type> GetInheritanceHierarchy(this Type type)
        {
            for (var current = type; current != null; current = current.BaseType)
                if (current != type)
                    yield return current;
        }

        public static IEnumerable<Type> GetAllSubclassOf(this Type type, Assembly assembly)
        {
            foreach (var t in assembly.GetTypes())
                if (t.BaseType == type) yield return t;
        }
    }
}
