namespace JSBindTool.Core
{
    public interface ICodeEmitter
    {
        void EmitWrite(string accessor, CodeBuilder code);
        void EmitRead(string varName, string accessor, CodeBuilder code);
        string GetNativeDeclaration();
    }
}
