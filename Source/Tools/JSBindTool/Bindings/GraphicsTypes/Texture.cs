using JSBindTool.Bindings.MathTypes;
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
    [Include("Urho3D/Graphics/Texture.h")]
    [Abstract]
    public class Texture : ResourceWithMetadata
    {
        [PropertyMap("GetLevels", "SetNumLevels")]
        public uint NumLevels { get; set; }
        [PropertyMap("GetFilterMode", "SetFilterMode")]
        public TextureFilterMode FilterMode { get; set; }
        [PropertyMap("GetAnisotropy", "SetAnisotropy")]
        public uint Anisotropy { get; set; }
        [PropertyMap("GetShadowCompare", "SetShadowCompare")]
        public bool ShadowCompare { get; set; }
        [PropertyMap("GetLinear", "SetLinear")]
        public bool Linear { get; set; }
        [PropertyMap("GetSRGB", "SetSRGB")]
        public bool SRGB { get; set; }
        [PropertyMap("GetBackupTexture", "SetBackupTexture")]
        public Texture BackupTexture { get => new Texture(); set { } }
        [PropertyMap("GetUnorderedAccess", "SetUnorderedAccess")]
        public bool UnorderedAccess { get; set; }
        [PropertyMap("GetFormat")]
        public uint Format { get; }
        [PropertyMap("IsCompressed")]
        public bool IsCompressed { get; }
        [PropertyMap("GetWidth")]
        public int Width { get; }
        [PropertyMap("GetHeight")]
        public int Height { get; }
        [PropertyMap("GetSize")]
        public IntVector2 Size { get; } = new IntVector2();
        [PropertyMap("GetRect")]
        public IntRect Rect { get; } = new IntRect();
        [PropertyMap("GetDepth")]
        public int Depth { get; }
        [PropertyMap("GetMultiSample")]
        public int MultiSample { get; }
        [PropertyMap("GetAutoResolve")]
        public bool AutoResolve { get; }
        [PropertyMap("GetLevelsDirty")]
        public bool LevelsDirty { get; }
        [PropertyMap("GetUsage")]
        public TextureUsage Usage { get => TextureUsage.Static; }
        [PropertyMap("GetComponents")]
        public uint Components { get; }
        [PropertyMap("GetParametersDirty")]
        public bool ParametersDirty { get; set; }
        [PropertyMap("IsUnorderedAccessSupported")]
        public bool IsUnorderedAccessSupported { get; }

        public Texture() : base(typeof(Texture)) { }
        public Texture(Type type) : base(type) { }

        [Method]
        public void SetAddressMode(TextureCoordinate coord, TextureAddressMode mode) { }
        [Method]
        public void SetMipsToSkip(MaterialQuality quality, int toSkip) { }
        [Method]
        public TextureAddressMode GetAddressMode(TextureCoordinate coord) => TextureAddressMode.Wrap;
        [Method]
        public int GetMipsToSkip(MaterialQuality quality) => 0;
        [Method]
        public int GetLevelWidth(uint level) => 0;
        [Method]
        public int GetLevelHeight(uint level) => 0;
        [Method]
        public int GetLevelDepth(uint level) => 0;
        [Method]
        public uint GetDataSize(int width, int height) => 0;
        [Method]
        public uint GetDataSize(int width, int height, int depth) => 0;
        [Method]
        public uint GetRowDataSize(int width) => 0;
        [Method]
        public void UpdateParameters() { }

        [Method]
        public uint GetSRGBFormat(uint format) => 0;
        [Method]
        public void SetResolveDirty(bool enable) { }
        [Method]
        public void SetParametersDirty() { }
        [Method]
        public void SetLevelsDirty() { }
        [Method]
        public void RegenerateLevels() { }

        [Method]
        public static uint CheckMaxLevels(int width, int height, uint requestedLevels) => 0;
        [Method]
        public static uint CheckMaxLevels(int width, int height, int depth, uint requestedLevels) => 0;
        [Method]
        public static uint GetSRVFormat(uint format) => 0;
        [Method]
        public static uint GetDSVFormat(uint format) => 0;
        [Method]
        public static uint GetExternalFormat(uint format) => 0;
        [Method]
        public static uint GetDataType(uint format) => 0;
        [Method]
        public static bool IsComputeWriteable(uint format) => false;
    }
}
