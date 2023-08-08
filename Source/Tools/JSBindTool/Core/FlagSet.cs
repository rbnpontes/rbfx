namespace JSBindTool.Core
{
    public class FlagSet<T> : TemplateObject
    {
        public FlagSet() : base(typeof(T), TemplateType.FlagSet)
        {
            if (!typeof(T).IsEnum)
                throw new Exception("Invalid Type on FlagSet, type must be a enum type.");
        }
    }
}
