using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/TileMapLayer2D.h")]
    public class TileMapLayer2D : Component
    {
        [PropertyMap("GetTileMap")]
        public TileMap2D TileMap { get => new TileMap2D(); }
        [PropertyMap("GetDrawOrder", "SetDrawOrder")]
        public int DrawOrder { get; }
        [PropertyMap("IsVisible", "SetVisible")]
        public bool IsVisible { get; set; }
        [PropertyMap("GetLayerType")]
        public TileMapLayerType2D LayerType { get; }
        [PropertyMap("GetWidth")]
        public int Width { get; }
        [PropertyMap("GetHeight")]
        public int Height { get; }
        [PropertyMap("GetNumObjects")]
        public uint NumObjects { get; }
        [PropertyMap("GetImageNode")]
        public Node ImageNode { get => new Node(); }

        public TileMapLayer2D() : base(typeof(TileMapLayer2D)) { }

        [Method]
        public bool HasProperty(string name) => false;
        [Method]
        public string GetProperty(string name) => string.Empty;
        [Method]
        public Node GetTileNode(int x, int y) => new Node();
        [Method]
        public Tile2D GetTile(int x, int y) => new Tile2D();
        [Method]
        public TileMapObject2D GetObject(uint index) => new TileMapObject2D();
        [Method]
        public Node GetObjectNode(uint index) => new Node();
    }
}
