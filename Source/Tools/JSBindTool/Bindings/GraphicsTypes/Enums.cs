using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.GraphicsTypes
{
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum MaterialQuality
    {
        Low,
        Medium,
        High
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum TextureFilterMode
    {
        Nearest = 0,
        Bilinear,
        Trilinear,
        Anisotropic,
        NearestAnisotropic,
        Default
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum TextureAddressMode
    {
        Wrap,
        Mirror,
        Clamp,
        Border
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum TextureCoordinate
    {
        CoordU,
        CoordV,
        CoordW
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum TextureUsage
    {
        Static,
        Dynamic,
        RenderTarget,
        DepthStencil
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum CubeMapFace
    {
        PositiveX,
        NegativeX,
        PositiveY,
        NegativeY,
        PositiveZ,
        NegativeZ
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum CubeMapLayout
    {
        Horizontal=0,
        HorizontalNvidia,
        HorizontalCross,
        VerticalCross,
        Blender
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum RenderSurfaceUpdateMode
    {
        ManualUpdate,
        UpdateVisible,
        UpdateAlways
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum ShaderType
    {
        VS = 0,
        PS,
        GS,
        HS,
        DS,
        CS
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum ShaderParameterGroup
    {
        Frame,
        Camera,
        Zone,
        Light,
        Material,
        Object,
        Custom
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum BlendMode
    {
        Replace,
        Add,
        Multiply,
        Alpha,
        AddAlpha,
        PreMulAlpha,
        InvDestAlpha,
        Subtract,
        SubtractAlhpa,
        DeferredDecal
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum CullMode
    {
        None,
        CCW,
        CW
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum FillMode
    {
        Solid,
        WireFrame,
        Point
    }
    [Include("Urho3D/Graphics/GraphicsDefs.h")]
    public enum TextureUnit
    {
        Diffuse =0,
        AlbedoBuffer=0,
        Normal = 1,
        NormalBuffer = 1,
        Specular = 2,
        Emissive = 3,
        Environment = 4,
#if URHO3D_DESKTOP
        VolumeMap = 5,
        Custom1 = 6,
        Custom2 = 7,
        LightRamp = 8,
        LightShape = 9,
        ShadowMap = 10,
        FaceSelect = 11,
        Indirection = 12,
        DepthBuffer = 13,
        LightBuffer = 14,
        TuZone = 15,
        MaxMaterialTextureUnits = 8,
        MaxTextureUnits = 16
#elif URHO3D_MOBILE
        LightRamp = 5,
        LightShape = 6,
        ShadowMap = 7,
        MaxMaterialTextureUnits = 5,
        MaxTextureUnits = 8
#endif
    }
}
