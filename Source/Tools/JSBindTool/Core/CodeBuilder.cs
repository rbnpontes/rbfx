using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class CodeBuilder
    {
        private StringBuilder pBuilder = new StringBuilder();
        public uint IndentationSize { get; set; } = 1;

        public CodeBuilder Add(CodeBuilder code)
        {
            pBuilder.AppendLine(code.ToString());
            return this;
        }
        public CodeBuilder Add(string code)
        {
            StringBuilder indentation = new StringBuilder();
            for (uint i = 0; i < IndentationSize; ++i)
                indentation.Append("\t");
            pBuilder.Append(indentation);
            pBuilder.AppendLine(code);
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
            return Add(args);
        }
        public CodeBuilder Add(params string[] args)
        {
            return Add(args);
        }

        public CodeBuilder Clear()
        {
            pBuilder.Clear();
            return this;
        }

        public override string ToString()
        {
            return pBuilder.ToString();
        }
    }
}
