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
                .Add("jsbind_vector3_set(ctx, 0, &arg0);")
                .Add("jsbind_quaternion_set(ctx, 1, &arg1);")
                .Add("jsbind_vector3_set(ctx, 2, &arg2);");
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
        public static void EmitVectorLerp(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorLerp(arg0, arg1, arg2);");
        }
        public static void EmitVectorMax(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorMax(arg0, arg1);");
        }
        public static void EmitVectorMin(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorMin(arg0, arg1);");
        }
        public static void EmitVectorFloor(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorFloor(arg0);");
        }
        public static void EmitVectorRound(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorRound(arg0);");
        }
        public static void EmitVectorCeil(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorCeil(arg0);");
        }
        public static void EmitVectorAbs(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorAbs(arg0);");
        }
        public static void EmitVectorSqrt(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorSqrt(arg0);");
        }
        public static void EmitVectorFloorToInt(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorFloorToInt(arg0);");
        }
        public static void EmitVectorRoundToInt(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorRoundToInt(arg0);");
        }
        public static void EmitVectorCeilToInt(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorCeilToInt(arg0);");
        }
        public static void EmitVectorStableRandom(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = StableRandom(arg0);");
        }
        public static void EmitVectorClamp(string typeName, CodeBuilder code)
        {
            code.Add($"{typeName} result = VectorClamp(arg0, arg1, arg2);");
        }

        public static void EmitIndexOutOfBoundsValidation(uint max, CodeBuilder code)
        {
            code
                .Add($"if (arg0 >= {max})")
                .Scope(code =>
                {
                    code
                        .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR,\"Index is out of bounds. Index must be in the range of [0, {max - 1}]\");")
                        .Add("return duk_throw(ctx);");
                });
        }
        public static void EmitGetProp(string propType, string prop, uint propSize, CodeBuilder code)
        {
            EmitIndexOutOfBoundsValidation(propSize, code);
            code.Add($"{propType} result = instance->{prop}[arg0];");
        }
        public static void EmitSetProp(string prop, uint propSize, CodeBuilder code)
        {
            EmitIndexOutOfBoundsValidation(propSize, code);
            code.Add($"instance->{prop}[arg0] = arg1;");
        }
    }
}
