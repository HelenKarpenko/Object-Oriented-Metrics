namespace ObjectOrientedMetrics.Extensions
{
    using System.Reflection;

    public static class MethodInfoExtension
    {
        public static bool IsOverride(this MethodInfo m)
        {
            return m.GetBaseDefinition().DeclaringType != m.DeclaringType;
        }
    }
}
