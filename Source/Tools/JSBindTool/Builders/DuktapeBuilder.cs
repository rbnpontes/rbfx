using JSBindTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Builders
{
    public class DuktapeBuilder
    {
        private CodeBuilder pCode;
        public CodeBuilder Code { get => pCode; }
        public DuktapeBuilder(CodeBuilder code)
        {
            pCode = code;
        }

        public void GetGlobalString(string input)
        {
            pCode.Add($"duk_get_global_string(ctx, \"{input}\");");
        }
        public void PutPropString(string value, int idx)
        {
            pCode.Add($"duk_put_prop_string(ctx, {idx}, \"{value}\");");
        }

        public void PushUint(string accessor)
        {
            pCode.Add($"duk_push_uint(ctx, {accessor});");
        }
        public void PushUint(uint value)
        {
            pCode.Add($"duk_push_uint(ctx, {value});");
        }
        #region Requires
        public void RequireString(int idx)
        {
            pCode.Add($"duk_require_string(ctx, {idx});");
        }
        #endregion
        #region Function Push
        public void PushFunction(string functionCallName)
        {
            pCode.Add($"duk_push_c_function(ctx, {functionCallName}, DUK_VARARGS);");
        }
        public void PushFunction(string functionCallName, uint length)
        {
            pCode.Add($"duk_push_c_function(ctx, {functionCallName}, {length});");
        }
        public void PushFunction(Action<DuktapeBuilder> functionCall)
        {
            pCode.Function(scope =>
            {
                functionCall(new DuktapeBuilder(scope));
            }, "DUK_VARARGS");
        }
        public void PushFunction(Action<DuktapeBuilder> functionCall, uint length)
        {
            pCode.Function(scope =>
            {
                functionCall(new DuktapeBuilder(scope));
            }, length.ToString());
        }

        public void PushLightFunction(string functionCallName)
        {
            pCode.Add($"duk_push_lightfunc(ctx, {functionCallName}, DUK_VARARGS, DUK_VARARGS, 0);");
        }
        public void PushLightFunction(string functionCallName, int length)
        {
            pCode.Add($"duk_push_lightfunc(ctx, {functionCallName}, {length}, {length}, 0);");
        }
        public void PushLightFunction(Action<DuktapeBuilder> functionCall)
        {
            pCode.LightFunction(scope =>
            {
                functionCall(new DuktapeBuilder(scope));
            }, "DUK_VARARGS");
        }
        public void PushLightFunction(Action<DuktapeBuilder> functionCall, int length)
        {
            pCode.LightFunction(scope =>
            {
                functionCall(new DuktapeBuilder(scope));
            }, length.ToString());
        }
        #endregion
        #region Engine Operations
        public void GetStringHash(string fieldName, int idx)
        {
            pCode.Add($"StringHash {fieldName} = rbfx_get_string_hash(ctx, {idx});");
        }
        #endregion

        public void Return(int code)
        {
            pCode.Add($"return {code};");
        }
    }
}
