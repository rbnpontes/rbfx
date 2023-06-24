using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class PrimitiveGen : CodeGen
    {
        public PrimitiveGen(Type type) : base(type)
        {
        }

        public override CodeBuilder BuildHeader()
        {
            PrimitiveObject obj = PrimitiveObject.Create(Target);

            CodeBuilder code = new CodeBuilder();
            code.IndentationSize = 0;
            HeaderUtils.EmitNotice(code);
            code.Add("#pragma once");

            obj.EmitHeaderIncludes(code);
            code.AddNewLine();

            code.Namespace("Urho3D", signatures =>
            {
                obj.EmitResolveSignature(signatures);
                obj.EmitSetSignature(signatures);
                obj.EmitConstructorSignature(signatures);
                obj.EmitPushSignature(signatures);
                obj.EmitSetupSignature(signatures);
            });

            return code;
        }

        public override CodeBuilder BuildSource()
        {
            PrimitiveObject obj = PrimitiveObject.Create(Target);

            CodeBuilder code = new CodeBuilder();
            code.IndentationSize = 0;
            HeaderUtils.EmitNotice(code);

            obj.EmitSourceIncludes(code);

            code.AddNewLine();
            code.Namespace("Urho3D", source =>
            {
                obj.EmitResolveSource(source);
                obj.EmitSetSource(source);
                obj.EmitConstructorSource(source);
                obj.EmitPushSource(source);
                obj.EmitSetupSetupSource(source);
            });
            
            return code;
        }
    }
}
