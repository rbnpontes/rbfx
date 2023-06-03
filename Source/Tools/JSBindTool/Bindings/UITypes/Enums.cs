using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/UIElement.h")]
    public enum HorizontalAlignment
    {
        Left = 0,
        Center,
        Right,
        Custom
    }
    [Include("Urho3D/UI/UIElement.h")]
    public enum VerticalAlignment
    {
        Top = 0,
        Center,
        Bottom,
        Custom
    }
    [Include("Urho3D/UI/UIElement.h")]
    public enum Corner
    {
        TopLeft = 0,
        BottomLeft,
        BottomRight
    }
    [Include("Urho3D/UI/UIElement.h")]
    public enum Orientation
    {
        Horizontal = 0,
        Vertical
    }
    [Include("Urho3D/UI/UIElement.h")]
    public enum FocusMode
    {
        NotFocusable = 0,
        ResetFocus,
        Focusable,
        FocusableDefocusable
    }
    [Include("Urho3D/UI/UIElement.h")]
    public enum LayoutMode
    {
        Free = 0,
        Horizontal,
        Vertical
    }
    [Include("Urho3D/UI/UIElement.h")]
    public enum TraversalMode
    {
        BreadthFirst = 0,
        DepthFirst
    }
    [Include("Urho3D/UI/UIElement.h")]
    public enum DragAndDropMode
    {
        Disabled = 0x0,
        Source = 0x1,
        Target = 0x2,
        SourceAndTarget = 0x3
    }
}
