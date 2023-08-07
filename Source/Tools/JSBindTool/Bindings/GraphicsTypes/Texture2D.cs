using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.GraphicsTypes
{
    [Include("Urho3D/Graphics/Texture2D.h")]
    public class Texture2D : Texture
    {
        [PropertyMap("GetImage")]
        public SharedPtr<Image> Image { get => new SharedPtr<Image>(); }
        public Texture2D() : base(typeof(Texture2D)) { }

        [Method]
        public bool SetSize(int width, int height, uint format) => false;
        [Method]
        public bool SetSize(int width, int height, uint format, TextureUsage usage) => false;
        [Method]
        public bool SetSize(int width, int height, uint format, TextureUsage usage, int multisample) => false;
        [Method]
        public bool SetSize(int width, int height, uint format, TextureUsage usage, int multisample, bool autoResolve) => false;
        [Method]
        public bool SetData(Image image) => false;
        [Method]
        public bool SetData(Image image, bool useAlpha) => false;
        [Method]
        [CustomCode(typeof(bool), new Type[]
        {
            typeof(uint),
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(Array)
        })]
        public void SetData(CodeBuilder code)
        {
            code
                .Add("ea::vector<unsigned char> buffer(duk_get_length(ctx, arg5));")
                .Add("for (duk_idx_t i = 0; i < buffer.size(); ++i)")
                .Scope(code =>
                {
                    code
                        .Add("duk_get_prop_index(ctx, arg5, i);")
                        .Add("buffer[i] = (unsigned char)duk_get_uint(ctx, -1);")
                        .Add("duk_pop(ctx);");
                })
                .Add("bool result = instance->SetData(arg0, arg1, arg2, arg3, arg4, buffer.data());");
        }
    }
}
