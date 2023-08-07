namespace JSBindTool.Core.Annotations
{
    public class ResolverAttribute : Attribute
    {
        public Type ResolverType { get; set; }
        public ResolverAttribute(Type target)
        {
            ResolverType = target;
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyResolverAttribute : ResolverAttribute
    {
        public PropertyResolverAttribute(Type target) : base(target)
        {
            if (!target.IsAssignableTo(typeof(IPropertyResolver)))
                throw new ArgumentException("Invalid Type. Target type must implement IPropertyResolver");
            if (target.GetConstructor(new Type[0]) is null)
                throw new ArgumentException("Invalid Type. Target type must contains at least 1 constructor with zero parameters.");
        }
    }
}
