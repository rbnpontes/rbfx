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
            header.Namespace(Constants.Namespace, scope =>
            {
                scope.Add($"void {CodeUtils.GetMethodPrefix(Target)}_setup(duk_context* ctx);");
            });

            return header;
        }

        public override CodeBuilder BuildSource()
        {
            CodeBuilder source = new CodeBuilder();
            source.IndentationSize = 0;
            HeaderUtils.EmitNotice(source);
            source.Add($"#include \"{HeaderName}.h\"");
            source.AddNewLine();
            source.Namespace(Constants.Namespace, sourceBody =>
            {
                sourceBody.Add($"void {CodeUtils.GetMethodPrefix(Target)}_setup(duk_context* ctx)");
                sourceBody.Scope(scope =>
                {
                    BuildSourceBody(scope);
                });
            });
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
