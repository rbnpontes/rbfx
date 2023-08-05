using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class StructGen : CodeGen
    {
        public StructGen(Type type) : base(type) { }
        public override CodeBuilder BuildHeader()
        {
            StructObject obj = StructObject.Create(Target);
            CodeBuilder code = new CodeBuilder();
            code.IndentationSize = 0;
            HeaderUtils.EmitNotice(code);
            code.Add("#pragma once");

            obj.EmitHeaderIncludes(code);

            code.AddNewLine();
            code.Namespace(Constants.Namespace, obj.EmitHeaderSignatures);

            return code;
        }

        public override CodeBuilder BuildSource()
        {
            StructObject obj = StructObject.Create(Target);
            CodeBuilder code = new CodeBuilder();
            code.IndentationSize = 0;
            HeaderUtils.EmitNotice(code);

            obj.EmitSourceIncludes(code);

            code.AddNewLine();
            code.Namespace(Constants.Namespace, obj.EmitSource);

            return code;
        }
    }
}
