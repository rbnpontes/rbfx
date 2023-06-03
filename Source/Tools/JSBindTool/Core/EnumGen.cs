using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class EnumGen : CodeGen
    {
        public EnumGen(Type type) : base(type)
        {
        }

        public override CodeBuilder BuildHeader()
        {
            CodeBuilder header = new CodeBuilder();
            header.IndentationSize = 0;
            HeaderUtils.EmitNotice(header);
            header.Add("#pragma once");
            header.AddNewLine();
            header.Add(GetIncludes());
            header.Add("namespace Urho3D");
            header.Add("{");
            {
                CodeBuilder headerSignatures = new CodeBuilder();
                headerSignatures.Add($"void {Target.Name}_setup(duk_context* ctx);");
                header.Add(headerSignatures);
            }
            header.Add("}");

            return header;
        }

        public override CodeBuilder BuildSource()
        {
            CodeBuilder source = new CodeBuilder();
            source.IndentationSize = 0;
            HeaderUtils.EmitNotice(source);
            source.Add($"#include \"{HeaderName}.h\"");
            source.AddNewLine();
            source.Add("namespace Urho3D");
            source.Add("{");
            {
                CodeBuilder sourceBody = new CodeBuilder();
                sourceBody.Add($"void {Target.Name}_setup(duk_context* ctx)");
                sourceBody.Add("{");
                {
                    CodeBuilder scope = new CodeBuilder();
                    BuildSourceBody(scope);
                    sourceBody.Add(scope);
                }
                sourceBody.Add("}");

                source.Add(sourceBody);
            }
            source.Add("}");
            return source;
        }

        private void BuildSourceBody(CodeBuilder code)
        {
            code.Add("duk_push_object(ctx);");
            Enum.GetNames(Target).ToList().ForEach(enumKey =>
            {
                int value = (int)Enum.Parse(Target, enumKey);

                code.Add($"duk_push_int(ctx, {value});");
                code.Add($"duk_put_prop_string(ctx, -2, \"{ToCamelCase(enumKey)}\");");
            });
            code.Add(
                "duk_seal(ctx, -1);",
                $"duk_put_global_string(ctx, \"{Target.Name}\");"
            );
        }
    }
}
