using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class CodeBuilder
    {
        interface IChunk
        {
            uint IndentationSize { get; set; }
            void Append(string str);
            string GetString();
        }

        class BaseChunk
        {
            public uint IndentationSize { get; set; }
            protected string GetIndentation()
            {
                char[] parts = new char[IndentationSize];
                Array.Fill(parts, '\t');
                return new string(parts);
            }
        }
        class StringChunk : BaseChunk, IChunk
        {
            private string pBuffer;
            public StringChunk(string buffer, uint indentation)
            {
                pBuffer = buffer;
                IndentationSize = indentation;
            }
            public void Append(string str)
            {
                pBuffer = pBuffer + str;
            }
            public string GetString()
            {
                return GetIndentation() + pBuffer;
            }
        }
        class CodeChunk : BaseChunk, IChunk
        {
            private CodeBuilder pCode;
            public CodeChunk(CodeBuilder pCode, uint indentationSize)
            {
                this.pCode = pCode;
                IndentationSize = indentationSize;
            }

            public void Append(string str)
            {
                pCode.AddSameLine(str);
            }

            public string GetString()
            {
                StringBuilder finalStr = new StringBuilder();
                for(int i =0; i < pCode.pChunks.Count; ++i)
                {
                    var chunk = pCode.pChunks[i];
                    uint currIndentationSize = chunk.IndentationSize;
                    chunk.IndentationSize += IndentationSize;
                    string result = chunk.GetString();
                    chunk.IndentationSize = currIndentationSize;

                    finalStr.Append(result);
                    if (i < pCode.pChunks.Count - 1)
                        finalStr.AppendLine();
                }
                return finalStr.ToString();
            }
        }

        private List<IChunk> pChunks = new List<IChunk>();
        public uint IndentationSize { get; set; } = 1;

        public CodeBuilder Add(CodeBuilder code)
        {
            if (code == this)
                throw new InvalidOperationException("Is not possible to add self reference of CodeBuilder.");
            pChunks.Add(new CodeChunk(code, IndentationSize));
            return this;
        }
        public CodeBuilder Add(string code)
        {
            pChunks.Add(new StringChunk(code, IndentationSize));
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
            pChunks[pChunks.Count - 1].Append(code);
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
            pChunks.ForEach(x => builder.AppendLine(x.GetString()));
            return builder.ToString();
        }
    }
}
