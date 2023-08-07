using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.GraphicsTypes
{
    [Include("Urho3D/Graphics/Material.h")]
    public class MaterialShaderParameter : PrimitiveObject
    {
        [Variable("name_")]
        public string Name = string.Empty;
        [Variable("value_")]
        public Variant Value = new Variant();
        [Variable("isCustom_")]
        public bool IsCustom;

        public MaterialShaderParameter() : base(typeof(MaterialShaderParameter)) { }

        [Constructor]
        public void Constructor() { }
    }
    [Include("Urho3D/Graphics/Material.h")]
    [Dependencies(new Type[] { typeof(MaterialShaderParameter) })]
    public class Material : Resource, IPipelineStateTracker, IDFamily
    {
        [PropertyMap("GetPipelineStateHash")]
        public uint PipelineStateHash { get => 0; }
        [PropertyMap("GetObjectID")]
        public uint ObjectID { get => 0; }
        [PropertyMap("GetNumTechniques", "SetNumTechniques")]
        public uint NumTechniques { get; set; }

        [PropertyMap("GetVertexShaderDefines", "SetVertexShaderDefines")]
        public string VertexShaderDefines { get; set; } = string.Empty;
        [PropertyMap("GetPixelShaderDefines", "SetPixelShaderDefines")]
        public string PixelShaderDefines { get; set; } = string.Empty;
        [PropertyMap("GetCullMode", "SetCullMode")]
        public CullMode CullMode { get; set; }
        [PropertyMap("GetShadowCullMode", "SetShadowCullMode")]
        public CullMode ShadowCullMode { get; set; }
        [PropertyMap("GetFillMode", "SetFillMode")]
        public FillMode FillMode { get; set; }
        [PropertyMap("GetDepthBias", "SetDepthBias")]
        public BiasParameters DepthBias { get => new BiasParameters(); set { } }
        [PropertyMap("GetAlphaToCoverage", "SetAlphaToCoverage")]
        public bool AlphaToCoverage { get; set; }
        [PropertyMap("GetLineAntiAlias", "SetLineAntiAlias")]
        public bool LineAntiAlias { get; set; }
        [PropertyMap("GetRenderOrder", "SetRenderOrder")]
        public byte RenderOrder { get; set; }
        [PropertyMap("GetOcclusion", "SetOcclusion")]
        public bool Occlusion { get; set; }
        [PropertyMap("GetScene", "SetScene")]
        public Scene Scene { get => new Scene(); set { } }
        [PropertyMap("GetTechniques", "SetTechniques")]
        public Vector<TechniqueEntry> Techniques { get => new Vector<TechniqueEntry>(); }

        public Material() : base(typeof(Material)) { }

        [Method]
        public void MarkPipelineStateHashDirty() { }
        [Method]
        public void AcquireObjectID() { }
        [Method]
        public void ReleaseObjectID() { }

        [Method]
        [CustomCode(typeof(SharedPtr<Material>), new Type[]
        {
            typeof(string),
            typeof(string),
            typeof(string)
        })]
        public static void CreateBaseMaterial(CodeBuilder code)
        {
            code.Add("SharedPtr<Material> result = Material::CreateBaseMaterial(JavaScriptSystem::GetContext(), arg0, arg1, arg2);");
        }
        [Method]
        public void SetTechnique(uint index, Technique tech) { }
        [Method]
        public void SetTechnique(uint index, Technique tech, MaterialQuality qualityLevel) { }
        [Method]
        public void SetTechnique(uint index, Technique tech, MaterialQuality qualityLevel, float lodDistance) { }
        [Method]
        public void SetShaderParameter(string name, Variant value) { }
        [Method]
        public void SetShaderParameter(string name, Variant value, bool isCustom) { }
        [Method]
        public void SetShaderParameterAnimation(string name, ValueAnimation animation) { }
        [Method]
        public void SetShaderParameterAnimation(string name, ValueAnimation animation, WrapMode wrapMode) { }
        [Method]
        public void SetShaderParameterAnimation(string name, ValueAnimation animation, WrapMode wrapMode, float speed) { }
        [Method]
        public void SetShaderParameterAnimationWrapMode(string name, WrapMode wrapMode) { }
        [Method]
        public void SetShaderParameterAnimationSpeed(string name, float speed) { }
        [Method]
        public void SetTexture(TextureUnit texUnit, Texture texture) { }
        [Method]
        public void SetUVTransform(Vector2 offset, float rotation, Vector2 repeat) { }
        [Method]
        public void SetUVTransform(Vector2 offset, float rotation, float repeat) { }
        [Method]
        public void RemoveShaderParameter(string name) { }
        [Method]
        public void ReleaseShaders() { }
        [Method]
        public SharedPtr<Material> Clone() => new SharedPtr<Material>();
        [Method]
        public SharedPtr<Material> Clone(string name) => new SharedPtr<Material>();
        [Method]
        public void SortTechniques() { }
        [Method]
        public void MarkForAuxView(uint frameNumber) { }
        [Method]
        public TechniqueEntry GetTechniqueEntry(uint index) => new TechniqueEntry();
        [Method]
        public Technique GetTechnique(uint index) => new Technique();
        [Method]
        public Technique FindTechnique(Drawable drawable, MaterialQuality materialQuality) => new Technique();
        // TODO: https://github.com/users/rbnpontes/projects/1/views/1
        [Method]
        public IntPtr GetPass(uint index, string passName) => IntPtr.Zero;
        // TODO: https://github.com/users/rbnpontes/projects/1/views/1
        [Method]
        public IntPtr GetDefaultPass() => IntPtr.Zero;
        [Method]
        public Texture GetTexture(TextureUnit texUnit) => new Texture();
        [Method]
        [CustomCode]
        public void GetTextures(CodeBuilder code)
        {
            code
                .Add("auto& textures = instance->GetTextures();")
                .Add("duk_push_object(ctx);")
                .Add("for (auto pair : textures)")
                .Scope(code =>
                {
                    code
                        .Add("rbfx_push_object(ctx, pair.second);")
                        .Add("duk_put_prop_index(ctx, -2, pair.first);");
                })
                .Add("result_code = 1;");
        }
        [Method]
        public Variant GetShaderParameter(string name) => new Variant();
        [Method]
        public ValueAnimation GetShaderParameterAnimation(string name) => new ValueAnimation();
        [Method]
        public float GetShaderParameterAnimationSpeed(string name) => 0f;
        [Method]
        [CustomCode]
        public void GetShaderParameters(CodeBuilder code)
        {
            code
                .Add("auto& shaderParams = instance->GetShaderParameters();")
                .Add("duk_push_object(ctx);")
                .Add("for (auto pair : shaderParams)")
                .Scope(code =>
                {
                    code
                        .Add($"{CodeUtils.GetPushSignature(typeof(MaterialShaderParameter))}(ctx, pair.second);")
                        .Add("duk_put_prop_index(ctx, -2, pair.first.Value());");
                })
                .Add("result_code = 1;");
        }
    }
}
