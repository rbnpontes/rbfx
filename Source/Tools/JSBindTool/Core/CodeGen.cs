using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class CodeGen
    {
        public Type Target { get; private set; }
        public CodeGen(Type type)
        {
            Target = type;
        }
    }
}
