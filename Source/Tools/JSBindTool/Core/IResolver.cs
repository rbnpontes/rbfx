namespace JSBindTool.Core.Annotations
{
    public interface IPropertyResolver
    {
        bool HasGetter(string propName);
        bool HasSetter(string propName);

        void EmitGetter(string propName, CodeBuilder code);
        void EmitSetter(string propName, CodeBuilder code);
    }
}
