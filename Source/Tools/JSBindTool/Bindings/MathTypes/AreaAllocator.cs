using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/AreaAllocator.h")]
    public class AreaAllocator : PrimitiveObject
    {
        [PropertyMap("GetWidth")]
        public int Width { get => 0; }
        [PropertyMap("GetHeight")]
        public int Height { get => 0; }
        [PropertyMap("GetFastMode")]
        public bool FastMode { get => false; }

        public AreaAllocator() : base(typeof(AreaAllocator))
        {
        }

        [Method("Reset")]
        public void Reset(int width, int height) { }
        [Method("Reset")]
        public void Reset(int width, int height, int maxWidth) { }
        [Method("Reset")]
        public void Reset(int width, int height, int maxWidth, int maxHeight) { }
        [Method("Reset")]
        public void Reset(int width, int height, int maxWidth, int maxHeight, bool fastMode) { }

        [Method("Allocate")]
        [CustomCode(typeof(bool), new Type[] { typeof(int), typeof(int), typeof(JSObject) })]
        public void Allocate(CodeBuilder code)
        {
            code
                .Add("int x = 0;")
                .Add("int y = 0;")
                .Add("bool result = instance->Allocate(arg0, arg1, x, y);")
                .Add("// update map")
                .Add("duk_push_number(ctx, x);")
                .Add("duk_put_prop_string(ctx, arg2, \"x\");")
                .Add("duk_push_number(ctx, y);")
                .Add("duk_put_prop_string(ctx, arg2, \"y\");");
        }
    }
}
