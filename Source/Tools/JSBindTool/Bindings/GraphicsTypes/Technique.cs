using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core.Annotations;
using JSBindTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.GraphicsTypes
{
    [Include("Urho3D/Graphics/Material.h")]
    public class TechniqueEntry : PrimitiveObject
    {
        [Variable("technique_")]
        public SharedPtr<Technique> Technique = new SharedPtr<Technique>();
        [Variable("original_")]
        public SharedPtr<Technique> Original = new SharedPtr<Technique>();
        [Variable("qualityLevel_")]
        public MaterialQuality QualityLevel;
        [Variable("lodDistance_")]
        public float LodDistance;

        public TechniqueEntry() : base(typeof(TechniqueEntry)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Technique tech, MaterialQuality qualityLevel, float lodDistance) { }

        [OperatorMethod(OperatorType.Less)]
        public bool LessOperator(TechniqueEntry entry) => false;
        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(TechniqueEntry entry) => false;
    }
    [Include("Urho3D/Graphics/Technique.h")]
    public class Technique : Resource
    {
        [PropertyMap("IsDesktop", "SetIsDesktop")]
        public bool IsDesktop { get; set; }
        [PropertyMap("IsSupported")]
        public bool IsSupported { get; }
        [PropertyMap("GetNumPasses")]
        public uint NumPasses { get; }
        [PropertyMap("GetPassNames")]
        public Vector<string> PassNames { get; } = new Vector<string>();

        public Technique() : base(typeof(Technique)) { }

        // TODO: https://github.com/users/rbnpontes/projects/1/views/1?pane=issue&itemId=35228017
        [Method]
        public IntPtr CreatePass(string name) => IntPtr.Zero;
        [Method]
        public void RemovePass(string name) { }
        [Method]
        public void ReleaseShaders() { }

        [Method]
        public SharedPtr<Technique> Clone() => new SharedPtr<Technique>();
        [Method]
        public SharedPtr<Technique> Clone(string name) => new SharedPtr<Technique>();

        [Method]
        public bool HasPass(uint passIndex) => false;
        [Method]
        public bool HasPass(string name) => false;

        // TODO: https://github.com/users/rbnpontes/projects/1/views/1?pane=issue&itemId=35228017
        [Method]
        public IntPtr GetPass(uint passIndex) => IntPtr.Zero;
        // TODO: https://github.com/users/rbnpontes/projects/1/views/1?pane=issue&itemId=35228017
        [Method]
        public IntPtr GetPass(string name) => IntPtr.Zero;
        // TODO: https://github.com/users/rbnpontes/projects/1/views/1?pane=issue&itemId=35228017
        [Method]
        public IntPtr GetSupportedPass(uint passIndex) => IntPtr.Zero;
        // TODO: https://github.com/users/rbnpontes/projects/1/views/1?pane=issue&itemId=35228017
        [Method]
        public IntPtr GetSupportedPass(string name) => IntPtr.Zero;
        // TODO: https://github.com/users/rbnpontes/projects/1/views/1?pane=issue&itemId=35228017
        // TODO: convert to property
        [Method]
        [CustomCode]
        public void GetPasses(CodeBuilder code)
        {
            code
                .Add("ea::vector<Pass*> passes = instance->GetPasses();")
                .Add("duk_push_array(ctx);")
                .Add("for (unsigned i = 0; i < passes.size(); ++i)")
                .Scope(code =>
                {
                    code
                        .Add("duk_push_pointer(ctx, passes[i]);")
                        .Add("duk_put_prop_index(ctx, -2, i);");
                })
                .Add("result_code = 1;");
        }
        [Method]
        public SharedPtr<Technique> CloneWithDefines(string vsDefines, string psDefines) => new SharedPtr<Technique>();

        [Method]
        public static uint GetPassIndex(string passName) => 0;

        [CustomField("basePassIndex")]
        public static void GetBasePassIndex(CodeBuilder code)
        {
            EmitFieldRead("basePassIndex", code);
        }
        [CustomField("alphaPassIndex")]
        public static void GetAlphaPassIndex(CodeBuilder code)
        {
            EmitFieldRead("alphaPassIndex", code);
        }
        [CustomField("materialPassIndex")]
        public static void GetMaterialPassIndex(CodeBuilder code)
        {
            EmitFieldRead("materialPassIndex", code);
        }
        [CustomField("deferredPassIndex")]
        public static void GetDeferredPassIndex(CodeBuilder code)
        {
            EmitFieldRead("deferredPassIndex", code);
        }
        [CustomField("lightPassIndex")]
        public static void GetLightPassIndex(CodeBuilder code)
        {
            EmitFieldRead("lightPassIndex", code);
        }
        [CustomField("litBasePassIndex")]
        public static void GetLitBasePassIndex(CodeBuilder code)
        {
            EmitFieldRead("litBasePassIndex", code);
        }
        [CustomField("litAlphaPassIndex")]
        public static void GetLitAlphaPassIndex(CodeBuilder code)
        {
            EmitFieldRead("litAlphaPassIndex", code);
        }
        [CustomField("shadowPassIndex")]
        public static void GetShadowPassIndex(CodeBuilder code)
        {
            EmitFieldRead("shadowPassIndex", code);
        }
        private static void EmitFieldRead(string field, CodeBuilder code)
        {
            code.Add($"duk_push_uint(ctx, Technique::{field});");
        }
    }
}
