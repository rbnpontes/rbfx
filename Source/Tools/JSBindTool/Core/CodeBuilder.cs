using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class CodeBuilder
    {
        private List<string> pChunks = new List<string>();
        public uint IndentationSize { get; set; } = 1;

        private string GetIndentation()
        {
            char[] parts = new char[IndentationSize];
            Array.Fill(parts, '\t');
            return new string(parts);
        }
        public CodeBuilder Add(CodeBuilder code)
        {
            code.pChunks.ForEach(chunk => Add(chunk));
            return this;
        }
        public CodeBuilder Add(string code)
        {
            pChunks.Add(GetIndentation() + code);
            return this;
        }
        public CodeBuilder Add(IEnumerable<CodeBuilder> codeParts)
        {
            codeParts.ToList().ForEach(x => Add(x));
            return this;
        }
        public CodeBuilder Add(IEnumerable<string> codeParts)
        {
            codeParts.ToList().ForEach(x => Add(x));
            return this;
        }
        public CodeBuilder Add(params CodeBuilder[] args)
        {
            foreach (CodeBuilder arg in args)
                Add(arg);
            return this;
        }
        public CodeBuilder Add(params string[] args)
        {
            foreach (string arg in args)
                Add(arg);
            return this;
        }

        public CodeBuilder AddNewLine(uint count = 1)
        {
            for (uint i = 0; i < count; ++i)
                Add("");
            return this;
        }

        public CodeBuilder Namespace(string namespaceName, Action<CodeBuilder> body)
        {
            return Add($"namespace {namespaceName}").Scope(body);
        }
        public CodeBuilder Scope(Action<CodeBuilder> body)
        {
            CodeBuilder scope = new CodeBuilder();
            body(scope);

            Add("{");
            Add(scope);
            Add("}");
            return this;
        }

        public CodeBuilder Clear()
        {
            pChunks.Clear();
            return this;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            pChunks.ForEach(x => builder.AppendLine(x));
            return builder.ToString();
        }
    }
}
