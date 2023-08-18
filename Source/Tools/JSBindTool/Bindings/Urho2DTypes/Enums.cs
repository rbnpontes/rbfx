using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/AnimatedSprite2D.h")]
    public enum LoopMode2D
    {
        Default,
        Looped,
        Clamped
    }
    [Include("Urho3D/Urho2D/ParticleEffect2D.h")]
    public enum EmitterType2D
    {
        Gravity = 0,
        Radial
    }
    [Include("Urho3D/Urho2D/TileMapDefs2D.h")]
    public enum Orientation2D
    {
        Orthogonal =0,
        Isometric,
        Staggered,
        Hexagonal
    }
    [Include("Urho3D/Urho2D/TileMapDefs2D.h")]
    public enum TileMapLayerType2D
    {
        TileLayer = 0,
        ObjectGroup,
        ImageLayer,
        Invalid = 0xffff
    }
    [Include("Urho3D/Urho2D/TileMapDefs2D.h")]
    public enum TileMapObjectType2D
    {
        Rectangle,
        Ellipse,
        Polygon,
        Polyline,
        Tile,
        Invalid = 0xffff
    }
}
