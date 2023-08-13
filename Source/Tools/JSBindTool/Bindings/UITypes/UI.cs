using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/UI.h")]
    public class UI : EngineObject
    {
        [PropertyMap("GetCursor", "SetCursor")]
        public Cursor Cursor { get => new Cursor(); set { } }
        [PropertyMap("GetUICursorPosition")]
        public IntVector2 UICursorPosition { get => new IntVector2(); set { } }
        [PropertyMap("GetSystemCursorPosition")]
        public IntVector2 SystemCursorPosition { get => new IntVector2(); set { } }
        [PropertyMap("GetClipboardText", "SetClipboardText")]
        public string ClipboardText { get; set; } = string.Empty;
        [PropertyMap("GetDoubleClickInterval", "SetDoubleClickInterval")]
        public float DoubleClickInterval { get; set; }
        [PropertyMap("GetMaxDoubleClickDistance", "SetMaxDoubleClickDistance")]
        public float MaxDoubleClickDistance { get; set; }
        [PropertyMap("GetDragBeginInterval", "SetDragBeginInterval")]
        public float DragBeginInterval { get; set; }
        [PropertyMap("GetDragBeginDistance", "SetDragBeginDistance")]
        public int DragBeginDistance { get; set; }
        [PropertyMap("GetDefaultToolTipDelay", "SetDefaultToolTipDelay")]
        public float DefaultToolTipDelay { get; set; }
        [PropertyMap("GetMaxFontTextureSize", "SetMaxFontTextureSize")]
        public float MaxFontTextureSize { get; set; }
        [PropertyMap("IsNonFocusedMouseWheel", "SetNonFocusedMouseWheel")]
        public bool IsNonFocusedMouseWheel { get; set; }
        [PropertyMap("GetUseSystemClipboard", "SetUseSystemClipboard")]
        public bool UseSystemClipboard { get; set; }
        [PropertyMap("GetUseScreenKeyboard", "SetUseScreenKeyboard")]
        public bool UseScreenKeyboard { get; set; }
        [PropertyMap("GetUseMutableGlyphs", "SetUseMutableGlyphs")]
        public bool UseMutableGlyphs { get; set; }
        [PropertyMap("GetForceAutoHint", "SetForceAutoHint")]
        public bool ForceAutoHint { get; set; }
        [PropertyMap("GetFontHintLevel", "SetFontHintLevel")]
        public FontHintLevel FontHintLevel { get; set; }
        [PropertyMap("GetFontSubpixelThreshold", "SetFontSubpixelThreshold")]
        public float FontSubpixelThreshold { get; set; }
        [PropertyMap("GetFontOversampling", "SetFontOversampling")]
        public int FontOversampling { get; set; }
        [PropertyMap("HasModalElement")]
        public bool HasModalElement { get; }
        [PropertyMap("IsDragging")]
        public bool IsDragging { get; }
        [PropertyMap("GetScale", "SetScale")]
        public float Scale { get; set; }
        [PropertyMap("GetSize")]
        public IntVector2 Size { get => new IntVector2(); }
        [PropertyMap("GetCustomSize", "SetCustomSize")]
        public IntVector2 CustomSize { get => new IntVector2(); set { } }
        [PropertyMap("GetRoot", "SetRoot")]
        public UIElement Root { get; set; } = new UIElement();
        [PropertyMap("GetRootModalElement", "SetRootModalElement")]
        public UIElement RootModalElement { get => new UIElement(); set { } }
        [PropertyMap("GetFocusElement")]
        public UIElement GetFocusElement { get => new UIElement(); }
        [PropertyMap("GetFrontElement")]
        public UIElement FrontElement { get => new UIElement(); }
        [PropertyMap("GetDragElements")]
        public Vector<UIElement> DragElements { get => new Vector<UIElement>(); }
        [PropertyMap("GetNumDragElements")]
        public uint NumDragElements { get; }
        [PropertyMap("GetRenderTarget", "SetRenderTarget")]
        public Texture2D RenderTarget { get => new Texture2D(); set { } }
        [PropertyMap("IsRendered")]
        public bool IsRendered { get; }

        public UI() : base(typeof(UI)) { }
        public UI(Type type) : base(type) { }

        [Method]
        public void SetFocusElement(UIElement element) { }
        [Method]
        public void SetFocusElement(UIElement element, bool byKey) { }
        [Method]
        public void SetModalElement(UIElement modalElement, bool enable) { }
        [Method]
        public void Clear() { }
        [Method]
        public void Update(float timeStep) { }
        [Method]
        public void RenderUpdate() { }
        [Method]
        public void Render() { }
        [Method]
        public void DebugDraw(UIElement element) { }
        [Method]
        public void SetCustomSize(int width, int height) { }
        [Method]
        public void SetRenderTarget(Texture2D texture, Color clearColor) { }
        [Method]
        public UIElement GetElementAt(IntVector2 position) => new UIElement();
        [Method]
        public UIElement GetElementAt(int x, int y) => new UIElement();
        [Method]
        public UIElement GetElementAt(IntVector2 position, bool enabledOnly) => new UIElement();
        [Method]
        public UIElement GetElementAt(int x, int y, bool enabledOnly) => new UIElement();
        [Method]
        public UIElement GetElementAt(UIElement root, IntVector2 position) => new UIElement();
        [Method]
        public UIElement GetElementAt(UIElement root, IntVector2 position, bool enabledOnly) => new UIElement();
        [Method]
        public IntVector2 ConvertSystemToUI(IntVector2 systemPos) => new IntVector2();
        [Method]
        public IntVector2 ConvertUIToSystem(IntVector2 uiPos) => new IntVector2();
        [Method]
        public UIElement GetDragElement(uint index) => new UIElement();
        [Method]
        public void SetWidth(float width) { }
        [Method]
        public void SetHeight(float height) { }
    }

    [Include("Urho3D/UI/UI.h")]
    [Include("Urho3D/UI/Font.h")]
    [Namespace(Constants.ProjectName + ".UI")]
    public class UIModule : ModuleObject
    {
        public UIModule() : base(typeof(UIModule)) { }

        [Field("fontTextureMinSize", "FONT_TEXTURE_MIN_SIZE")]
        public static int FontTextureMinSize = 0;
        [Field("fontDpi", "FONT_DPI")]
        public static int FontDpi = 0;
    }
}
