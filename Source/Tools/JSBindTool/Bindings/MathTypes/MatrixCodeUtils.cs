using JSBindTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    public static class MatrixCodeUtils
    {
        public static void EmitBulkTranspose(string nativeTypeName, CodeBuilder code)
        {
            code
                .Add("if(arg0.size() != arg1.size())")
                .Scope(ifScope =>
                {
                    ifScope
                        .Add("duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments. value arrays does not have same size.\");")
                        .Add("duk_throw(ctx);");
                })
                .AddNewLine()
                .Add($"{nativeTypeName}::BulkTranspose(arg0.data(), arg1.data(), arg0.size());")
                .Add("for(duk_idx_t i =0; i < arg0.size(); ++i)")
                .Scope(loopScope =>
                {
                    loopScope
                        .Add("duk_push_number(ctx, arg0[i]);")
                        .Add("duk_put_prop_index(ctx, 0, i);")
                        .Add("// fill second argument")
                        .Add("duk_push_number(ctx, arg1[i]);")
                        .Add("duk_put_prop_index(ctx, 1, i);");
                });
        }
    }
}
