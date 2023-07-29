using JSBindTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    public static class MathCodeUtils
    {
        public static void EmitMatrixBulkTranspose(string nativeTypeName, CodeBuilder code)
        {
            code
                .Add($"if(arg0.size() != arg1.size() || arg0.size() != (sizeof({nativeTypeName}) / sizeof(float)))")
                .Scope(ifScope =>
                {
                    ifScope
                        .Add("duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments. value arrays does not have same size or does not fit matrix size.\");")
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
        public static void EmitMatrixDecompose(CodeBuilder code)
        {
            code
                .Add("instance->Decompose(arg0, arg1, arg2);")
                .Add("jsbind_vector3_set(ctx, 0, arg0);")
                .Add("jsbind_quaternion_set(ctx, 1, arg1);")
                .Add("jsbind_vector3_set(ctx, 2, arg2);");
        }
        public static void EmitEqualOperator(CodeBuilder code)
        {
            code.Add("bool result = instance.Equals(arg0, arg1);");
        }
        public static void EmitGetData(CodeBuilder code, int sizeOf, string dataType = "float")
        {
            code
                .Add($"ea::vector<{dataType}> result({sizeOf});")
                .Add($"memcpy(result.data(), instance->Data(), sizeof({dataType}) * {sizeOf});");
        }
        public static void EmitVectorClamp(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorClamp(arg0, arg1, arg2);");
        }
    }
}
