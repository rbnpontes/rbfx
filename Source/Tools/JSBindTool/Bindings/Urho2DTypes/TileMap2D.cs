using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/TileMap2D.h")]
    public class TileMap2D : Component
    {
        [PropertyMap("GetTmxFile", "SetTmxFile")]
        public TmxFile2D TmxFile { get => new TmxFile2D(); set { } }
        [PropertyMap("GetInfo")]
        public TileMapInfo2D Info { get => new TileMapInfo2D(); }
        [PropertyMap("GetNumLayers")]
        public uint NumLayers { get; }
        [PropertyMap("GetTmxFileAttr", "SetTmxFileAttr")]
        public ResourceRef TmxFileAttr { get => new ResourceRef(); set { } }

        public TileMap2D() : base(typeof(TileMap2D)) { }

        [Method]
        public TileMapLayer2D GetLayer(uint index) => new TileMapLayer2D();
        [Method]
        public Vector2 TileIndexToPosition(int x, int y) => new Vector2();
        [Method]
        public bool PositionToTileIndex(int x, int y, Vector2 position) => false;
        [Method]
        public Vector<SharedPtr<TileMapObject2D>> GetTileCollisionShapes(uint gid) => new Vector<SharedPtr<TileMapObject2D>>();
    }
}
