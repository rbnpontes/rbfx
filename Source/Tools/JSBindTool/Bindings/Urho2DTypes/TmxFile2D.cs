using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/TileMapDefs2D.h")]
    public class TileMapInfo2D : PrimitiveObject
    {
        [Variable("orientation_")]
        public Orientation2D Orientation;
        [Variable("width_")]
        public int Width;
        [Variable("height_")]
        public int Height;
        [Variable("tileWidth_")]
        public float TileWidth;
        [Variable("tileHeight_")]
        public float TileHeight;

        [PropertyMap("GetMapWidth")]
        public float MapWidth { get; }
        [PropertyMap("GetMapHeight")]
        public float MapHeight { get; }

        public TileMapInfo2D() : base(typeof(TileMapInfo2D)) { }

        [Method]
        public Vector2 ConvertPosition(Vector2 position) => new Vector2();
        [Method]
        public Vector2 TileIndexToPosition(int x, int y) => new Vector2();
        [Method]
        public bool PositionToTileIndex(int x, int y, Vector2 position) => false;
    }

    [Include("Urho3D/Urho2D/TileMapDefs2D.h")]
    public class PropertySet2D : ClassObject
    {
        public PropertySet2D() : base(typeof(PropertySet2D)) { }

        [Method]
        public bool HasProperty(string name) => false;
        [Method]
        public string GetProperty(string name) => string.Empty;
    }

    [Include("Urho3D/Urho2D/TileMapDefs2D.h")]
    public class Tile2D : ClassObject
    {
        [PropertyMap("GetGid")]
        public uint Gid { get; }
        [PropertyMap("GetFlipX")]
        public bool FlipX { get; }
        [PropertyMap("GetFlipY")]
        public bool FlipY { get; }
        [PropertyMap("GetSwapXY")]
        public bool SwapXY { get; }
        [PropertyMap("GetSprite")]
        public Sprite2D Sprite { get => new Sprite2D(); }

        public Tile2D() : base(typeof(Tile2D)) { }

        [Method]
        public bool HasProperty(string name) => false;
        [Method]
        public string GetProperty(string name) => string.Empty;
    }

    [Include("Urho3D/Urho2D/TileMapDefs2D.h")]
    public class TileMapObject2D : ClassObject
    {
        [PropertyMap("GetObjectType")]
        public TileMapObjectType2D ObjectType { get; }
        [PropertyMap("GetName")]
        public string Name { get; set; } = string.Empty;
        [PropertyMap("GetType")]
        public string Type { get; set; } = string.Empty;
        [PropertyMap("GetPosition")]
        public Vector2 Position { get => new Vector2(); set { } }
        [PropertyMap("GetSize")]
        public Vector2 Size { get => new Vector2(); set { } }
        [PropertyMap("GetNumPoints")]
        public uint NumPoints { get; }
        [PropertyMap("GetTileGid")]
        public uint TileGid { get; }
        [PropertyMap("GetTileFlipX")]
        public bool TileFlipX { get; }
        [PropertyMap("GetTileFlipY")]
        public bool TileFlipY { get; }
        [PropertyMap("GetTileSwapXY")]
        public bool TileSwapXY { get; }
        [PropertyMap("GetTileSprite")]
        public Sprite2D TileSprite { get => new Sprite2D(); }

        public TileMapObject2D() : base(typeof(TileMapObject2D)) { }

        [Method]
        public Vector2 GetPoint(uint index) => new Vector2();
        [Method]
        public bool HasProperty(string name) => false;
        [Method]
        public string GetProperty(string name) => string.Empty;
    }

    [Include("Urho3D/Urho2D/TmxFile2D.h")]
    public class TmxFile2D : Resource
    {
        [PropertyMap("GetInfo")]
        public TileMapInfo2D Info { get => new TileMapInfo2D(); }
        [PropertyMap("GetNumLayers")]
        public uint NumLayers { get; }
        [PropertyMap("GetSpriteTextureEdgeOffset", "SetSpriteTextureEdgeOffset")]
        public float SpriteTextureEdgeOffset { get; set; }
        public TmxFile2D() : base(typeof(TmxFile2D)) { }

        [Method]
        public bool SetInfo(Orientation2D orientation, int width, int height, float tileWidth, float tileHeight) => false;
        [Method]
        public Sprite2D GetTileSprite(uint gid) => new Sprite2D();
        [Method]
        public Vector<SharedPtr<TileMapObject2D>> GetTileCollisionShapes(uint gid) => new Vector<SharedPtr<TileMapObject2D>>();
        [Method]
        public PropertySet2D GetTilePropertySet(uint gid) => new PropertySet2D();
    }
}
