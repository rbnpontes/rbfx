using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/UIElement.h")]
    [Dependencies(new Type[] { typeof(UIBatch) })]
    public class UIElement : Animatable
    {
        [PropertyMap("IsEnabled", "SetEnabled")]
        public bool Enabled { get; set; }
        [PropertyMap("GetName", "SetName")]
        public string Name { get; set; } = string.Empty;
        [PropertyMap("GetChildren")]
        public Vector<SharedPtr<UIElement>> Children { get; set; } = new Vector<SharedPtr<UIElement>>();
        [PropertyMap("GetHorizontalAlignment", "SetHorizontalAlignment")]
        public HorizontalAlignment HorizontalAlignment { get; set; }
        [PropertyMap("GetVerticalAlignment", "SetVerticalAlignment")]
        public VerticalAlignment VerticalAlignment { get; set; }
        [PropertyMap(null, "SetColor")] 
        public Color Color { set { } }
        [PropertyMap("GetScreenPosition")]
        public IntVector2 ScreenPosition { get => new IntVector2(); set { } }
        [PropertyMap("GetSize", "SetSize")]
        public IntVector2 Size { get => new IntVector2(); set { } }
        [PropertyMap("GetWidth", "SetWidth")]
        public int Width { get; set; }
        [PropertyMap("GetHeight", "SetHeight")]
        public int Height { get; set; }
        [PropertyMap("GetMinSize", "SetMinSize")]
        public IntVector2 MinSize { get => new IntVector2(); set { } }
        [PropertyMap("GetMaxSize", "SetMaxSize")]
        public IntVector2 MaxSize { get => new IntVector2(); set { } }
        [PropertyMap("GetMinWidth", "SetMinWidth")]
        public int MinWidth { get; set; }
        [PropertyMap("GetMinHeight", "SetMinHeight")]
        public int MinHeight { get; set; }
        [PropertyMap("GetMaxWidth", "SetMaxWidth")]
        public int MaxWidth { get; set; }
        [PropertyMap("GetMaxHeight", "SetMaxHeight")]
        public int MaxHeight { get; set; }
        [PropertyMap("GetEnableAnchor", "SetEnableAnchor")]
        public bool EnableAnchor { get; set; }
        [PropertyMap("GetMinAnchor", "SetMinAnchor")]
        public Vector2 MinAnchor { get => new Vector2(); set { } }
        [PropertyMap("GetMaxAnchor", "SetMaxAnchor")]
        public Vector2 MaxAnchor { get => new Vector2(); set { } }
        [PropertyMap("GetMinOffset", "SetMinOffset")]
        public IntVector2 MinOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetMaxOffset", "SetMaxOffset")]
        public IntVector2 MaxOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetChildOffset", "SetChildOffset")]
        public IntVector2 ChildOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetPivot", "SetPivot")]
        public Vector2 MaxPivot { get => new Vector2(); set { } }
        [PropertyMap("GetClipBorder", "SetClipBorder")]
        public IntRect ClipBorder { get => new IntRect(); set { } }
        [PropertyMap("GetOpacity", "SetOpacity")]
        public float Opacity { get; set; }
        [PropertyMap("GetPriority", "SetPriority")]
        public int Priority { get; set; }
        [PropertyMap("GetClipChildren", "SetClipChildren")]
        public bool ClipChildren { get; set; }
        [PropertyMap("GetUseDerivedOpacity")]
        public float UseDerivedOpacity { get; set; }
        [PropertyMap("GetDerivedOpacity")]
        public float DerivedOpacity { get; set; }
        [PropertyMap("IsEditable", "SetEditable")]
        public bool IsEditable { get; set; }
        [PropertyMap("HasFocus", "SetFocus")]
        public bool HasFocus { get; set; }
        [PropertyMap("IsSelected", "SetSelected")]
        public bool IsSelected { get; set; }
        [PropertyMap("IsVisible", "SetVisible")]
        public bool IsVisible { get; set; }
        [PropertyMap("IsVisibleEffective")]
        public bool IsVisibleEffective { get; }
        [PropertyMap("GetFocusMode", "SetFocusMode")]
        public FocusMode FocusMode { get; set; }
        [PropertyMap("GetDragDropMode", "SetDragDropMode")]
        public FlagSet<DragAndDropMode> DragDropMode { get => new FlagSet<DragAndDropMode>(); set { } }
        [PropertyMap("GetLayoutMode", "SetLayoutMode")]
        public LayoutMode LayoutMode { get; set; }
        [PropertyMap("GetLayoutSpacing", "SetLayoutSpacing")]
        public int LayoutSpacing { get; set; }
        [PropertyMap("GetLayoutBorder", "SetLayoutBorder")]
        public IntRect LayoutBorder { get => new IntRect(); set { } }
        [PropertyMap("GetLayoutFlexScale", "SetLayoutFlexScale")]
        public Vector2 LayoutFlexScale { get => new Vector2(); set { } }
        [PropertyMap("GetIndent", "SetIndent")]
        public int Indent { get; set; }
        [PropertyMap("GetIndentSpacing", "SetIndentSpacing")]
        public int IndentSpacing { get; set; }
        [PropertyMap("GetParent", "SetParent")]
        public UIElement Parent { get => new UIElement(); set { } }
        [PropertyMap("IsInternal", "SetInternal")]
        public bool IsInternal { get; set; }
        [PropertyMap("GetTraversalMode", "SetTraversalMode")]
        public TraversalMode TraversalMode { get; set; }
        [PropertyMap("GetTags", "SetTags")]
        public Vector<string> Tags { get => new Vector<string>(); set { } }
        [PropertyMap("IsFixedSize")]
        public bool IsFixedSize { get; }
        [PropertyMap("IsFixedWidth")]
        public bool IsFixedWidth { get; }
        [PropertyMap("IsFixedHeight")]
        public bool IsFixedHeight { get; }
        [PropertyMap("IsEnabledSelf")]
        public bool IsEnabledSelf { get; }
        [PropertyMap("IsHovering", "SetHovering")]
        public bool IsHovering { get; set; }
        [PropertyMap("HasColorGradient")]
        public bool HasColorGradient { get; }
        [PropertyMap("GetAppliedStyle")]
        public string AppliedStyle { get => string.Empty; }
        [PropertyMap("GetNumChildren")]
        public int NumChildren { get; }
        [PropertyMap("GetRoot")]
        public UIElement Root { get => new UIElement(); }
        [PropertyMap("GetDerivedColor")]
        public Color DerivedColor { get => new Color(); }
        [PropertyMap("GetDragButtonCombo")]
        public FlagSet<MouseButton> DragButtonCombo { get => new FlagSet<MouseButton>(); }
        [PropertyMap("GetDragButtonCount")]
        public uint DragButtonCount { get; }
        [PropertyMap("GetLayoutElementMaxSize")]
        public int LayoutElementMaxSize { get; }
        [PropertyMap("GetColorAttr")]
        public Color ColorAttr { get => new Color(); }
        [PropertyMap("IsElementEventSender", "SetElementEventSender")]
        public bool ElementEventSender { get; set; }
        [PropertyMap("GetEffectiveMinSize")]
        public IntVector2 EffectiveMinSize { get => new IntVector2(); }

        public UIElement() : base(typeof(UIElement)) { }
        public UIElement(Type type) : base(type) { }

        [Method("AddChild")]
        public void AddChild(UIElement arg) { }
        [Method("GetColor")]
        public Color GetColor(Corner corner) { return new Color(); }
        [Method("SetColor")]
        public void SetColor(Color color) { }
        [Method("SetColor")]
        public void SetColor(Corner corner, Color color) { }

        [Method("ApplyAttributes")]
        public void ApplyAttributes() { }

        [Method]
        public void Update(float timeStep) { }
        [Method]
        public bool IsWithinScissor(IntRect currentScissor) => false;
        [Method]
        [CustomCode(new Type[]
        {
            typeof(Array),
            typeof(Array),
            typeof(IntRect)
        })]
        public void GetBatches(CodeBuilder code)
        {
            EmitGetBatches("GetBatches", code);
        }
        [Method]
        [CustomCode(new Type[]
        {
            typeof(Array),
            typeof(Array),
            typeof(IntRect)
        })]
        public void GetDebugDrawBatches(CodeBuilder code)
        {
            EmitGetBatches("GetDebugDrawBatches", code);
        }
        private void EmitGetBatches(string accessor, CodeBuilder code)
        {
            code
                .Add("ea::vector<UIBatch> batches;")
                .Add("ea::vector<float> vertexData;")
                .Add($"instance->{accessor}(batches, vertexData, arg2);")
                .Add("for (unsigned i = 0; i < Max(batches.size(), vertexData.size()); ++i)")
                .Scope(code =>
                {
                    code
                        .Add("if (i < batches.size())")
                        .Scope(code =>
                        {
                            code
                                .Add($"{CodeUtils.GetPushSignature(typeof(UIBatch))}(ctx, batches[i]);")
                                .Add("duk_put_prop_index(ctx, arg0, i);");
                        })
                        .Add("if (i < vertexData.size())")
                        .Scope(code =>
                        {
                            code
                                .Add($"duk_push_number(ctx, vertexData[i]);")
                                .Add("duk_put_prop_index(ctx, arg1, i);");
                        });
                });
        }

        [Method]
        public void OnHover(IntVector2 position, IntVector2 screenPosition, MouseButton buttons, Qualifier qualifier, Cursor cursor) { }
        [Method]
        public void OnClickBegin(IntVector2 position, IntVector2 screenPosition, MouseButton button, FlagSet<MouseButton> buttons, FlagSet<Qualifier> qualifier, Cursor cursor) { }
        [Method]
        public void OnClickEnd(IntVector2 position, IntVector2 screenPosition, MouseButton button, FlagSet<MouseButton> buttons, FlagSet<Qualifier> qualifier, Cursor cursor, UIElement beginElement) { }
        [Method]
        public void OnDoubleClick(IntVector2 position, IntVector2 screenPosition, MouseButton button, FlagSet<MouseButton> buttons, FlagSet<Qualifier> qualifier, Cursor cursor) { }
        [Method]
        public void OnDragBegin(IntVector2 position, IntVector2 screenPosition, FlagSet<MouseButton> buttons, FlagSet<Qualifier> qualifier, Cursor cursor) { }
        [Method]
        public void OnDragMove(IntVector2 position, IntVector2 screenPosition, IntVector2 deltaPos, FlagSet<MouseButton> buttons, FlagSet<Qualifier> qualifier, Cursor cursor) { }
        [Method]
        public void OnDragEnd(IntVector2 position, IntVector2 screenPosition, FlagSet<MouseButton> dragButtons, FlagSet<MouseButton> releaseButton, Cursor cursor) { }
        [Method]
        public void OnDragCancel(IntVector2 position, IntVector2 screenPosition, FlagSet<MouseButton> dragButtons, FlagSet<MouseButton> cancelButton, Cursor cursor) { }
        [Method]
        public void OnDragDropTest(UIElement source) { }
        [Method]
        public void OnDragDropFinish(UIElement source) { }
        [Method]
        public void OnWheel(int delta, MouseButton buttons, Qualifier qualifiers) { }
        [Method]
        public void OnKey(Key key, MouseButton buttons, Qualifier qualifiers) { }
        [Method]
        public void OnTextInput(string text) { }
        [Method]
        public void OnResize(IntVector2 newSize, IntVector2 delta) { }
        [Method]
        public void OnPositionSet(IntVector2 newPosition) { }
        [Method]
        public void OnSetEditable() { }
        [Method]
        public void OnIndentSet() { }
        [Method]
        public IntVector2 ScreenToElement(IntVector2 screenPosition) => new IntVector2();
        [Method]
        public IntVector2 ElementToScreen(IntVector2 position) => new IntVector2();
        [Method]
        public bool IsWheelHandler() => false;
        [Method]
        public void SetSize(int width, int height) { }
        [Method]
        public void SetMinSize(int width, int height) { }
        [Method]
        public void SetMaxSize(int width, int height) { }
        [Method]
        public void SetFixedSize(int width, int height) { }
        [Method]
        public void SetAlignment(HorizontalAlignment hAlign, VerticalAlignment vAlign) { }
        [Method]
        public void SetMinAnchor(int width, int height) { }
        [Method]
        public void SetMaxAnchor(int width, int height) { }
        [Method]
        public void SetDeepEnabled(bool enable) { }
        [Method]
        public void ResetDeepEnabled() { }
        [Method]
        public void SetEnabledRecursive(bool enable) { }
        [Method]
        public void SetLayout(LayoutMode mode) { }
        [Method]
        public void SetLayout(LayoutMode mode, int spacing) { }
        [Method]
        public void SetLayout(LayoutMode mode, int spacing, IntRect border) { }
        [Method]
        public void UpdateLayout() { }
        [Method]
        public void DisableLayoutUpdate() { }
        [Method]
        public void EnableLayoutUpdate() { }
        [Method]
        public bool GetBringToFront() => false;
        [Method]
        public void SetBringToFront(bool enable) { }
        [Method]
        public bool GetBringToBack() => false;
        [Method]
        public void SetBringToBack(bool enable) { }
        [Method]
        public void BringToFront() { }
        [Method]
        public UIElement CreateChild(StringHash type) => new UIElement();
        [Method]
        public UIElement CreateChild(StringHash type, string name) => new UIElement();
        [Method]
        public UIElement CreateChild(StringHash type, string name, uint index) => new UIElement();
        [Method]
        public void InsertChild(uint index, UIElement element) { }
        [Method]
        public void RemoveChild(UIElement element) { }
        [Method]
        public void RemoveChild(UIElement element, uint index) { }
        [Method]
        public void RemoveChildAtIndex(uint index) { }
        [Method]
        public void RemoveAllChildren() { }
        [Method]
        public void Remove() { }
        [Method]
        public void SetParent(UIElement parent, uint index) { }
        [Method]
        public void SetVar(string key, Variant value) { }
        [Method]
        public void SetVarByHash(StringHash has, Variant value) { }
        [Method]
        public void AddTag(string tag) { }
        [Method]
        public void AddTags(string tags, char separator) { }
        [Method]
        public void RemoveTag(string tag) { }
        [Method]
        public void RemoveAllTags() { }
        [Method]
        public bool IsChildOf(UIElement element) => false;
        [Method]
        public uint GetNumChildren(bool recursive) => 0;
        [Method]
        public UIElement GetChild(uint index) => new UIElement();
        [Method]
        public UIElement GetChild(string name) => new UIElement();
        [Method]
        public UIElement GetChild(string name, bool recursive) => new UIElement();
        [Method]
        public UIElement GetChild(StringHash key) => new UIElement();
        [Method]
        public UIElement GetChild(StringHash key, Variant value) => new UIElement();
        [Method]
        public UIElement GetChild(StringHash key, Variant value, bool recursive) => new UIElement();
        [Method]
        public Vector<UIElement> GetChildren(bool recursive) => new Vector<UIElement>();
        [Method]
        [CustomCode]
        public void GetVars(CodeBuilder code)
        {
            code
                .Add("auto& vars = instance->GetVars();")
                .Add("duk_push_object(ctx);")
                .Add("for (auto pair : vars)")
                .Scope(code =>
                {
                    code
                        .Add("rbfx_push_variant(ctx, pair.second);")
                        .Add("duk_put_prop_string(ctx, -2, pair.first.c_str());");
                })
                .Add("result_code = 1;");
        }
        [Method]
        public Vector<UIElement> GetChildrenWithTag(string tab) => new Vector<UIElement>();
        [Method]
        public Vector<UIElement> GetChildrenWithTag(string tab, bool recursive) => new Vector<UIElement>();
        [Method]
        public bool IsInside(IntVector2 position, bool isScreen) => false;
        [Method]
        public bool IsInsideCombined(IntVector2 position, bool isScreen) => false;
        [Method]
        public IntRect GetCombinedScreenRect() => new IntRect();
        [Method]
        public bool GetSortChildren() => false;
        [Method]
        public void SetSortChildren(bool value) { }
        [Method]
        public void SortChildren() { }
        [Method]
        public void AdjustScissor(IntRect currentScissor) { }
        [Method]
        [CustomCode(new Type[]
        {
            typeof(IntVector2),
            typeof(Array),
            typeof(Array),
            typeof(IntRect)
        })]
        public void GetBatchesWithOffset(CodeBuilder code)
        {
            code
                .Add("ea::vector<UIBatch> batches;")
                .Add("ea::vector<float> vertexData;")
                .Add("instance->GetBatchesWithOffset(arg0, batches, vertexData, arg3);")
                .Add("for (unsigned i =0; i < Max(batches.size(), vertexData.size()); ++i)")
                .Scope(code =>
                {
                    code
                        .Add("if (i < batches.size())")
                        .Scope(code =>
                        {
                            code
                                .Add($"{CodeUtils.GetPushSignature(typeof(UIBatch))}(ctx, batches[i]);")
                                .Add("duk_put_prop_index(ctx, arg1, i);");
                        })
                        .Add("if (i < vertexData.size())")
                        .Scope(code =>
                        {
                            code
                                .Add($"duk_push_number(ctx, vertexData[i]);")
                                .Add("duk_put_prop_index(ctx, arg2, i);");
                        });
                        
                });
        }
        [Method]
        public UIElement GetElementEventSender() => new UIElement();
        public void SetFixedWidth(int width) { }
        public void SetFixedHeight(int height) { }
    }
}
