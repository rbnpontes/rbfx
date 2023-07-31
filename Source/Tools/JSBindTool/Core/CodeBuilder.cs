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

        public CodeBuilder AddSameLine(string code)
        {
            if (pChunks.Count == 0)
                return Add(code);
            pChunks[pChunks.Count - 1] = pChunks[pChunks.Count - 1] + code;
            return this;
        }
        public CodeBuilder AddSameLine(IEnumerable<string> codeParts)
        {
            foreach (string arg in codeParts)
                AddSameLine(arg);
            return this;
        }
        public CodeBuilder AddSameLine(params string[] args)
        {
            foreach (string arg in args)
                AddSameLine(arg);
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
        public CodeBuilder Lambda(Action<CodeBuilder> body)
        {
            CodeBuilder scope = new CodeBuilder();
            body(scope);

            Add("{");
            Add(scope);
            Add("};");
            return this;
        }
        public CodeBuilder Function(Action<CodeBuilder> body, string args)
        {
            CodeBuilder scope = new CodeBuilder();
            body(scope);

            Add("duk_push_c_function(ctx, [](duk_context* ctx)");
            Add("{");
            Add(scope);
            Add("}"+$", {args});");

            return this;
        }
        public CodeBuilder LightFunction(Action<CodeBuilder> body, string args)
        {
            CodeBuilder scope = new CodeBuilder();
            body(scope);

            Add("duk_push_c_lightfunc(ctx, [](duk_context* ctx)");
            Add("{");
            Add(scope);
            Add($"}}, {args}, {args}, 0);");

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
