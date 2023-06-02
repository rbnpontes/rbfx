using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    public enum HorizontalAlignment
    {
        Left = 0,
        Center,
        Right,
        Custom
    }
    public enum VerticalAlignment
    {
        Top = 0,
        Center,
        Bottom,
        Custom
    }
    public enum Corner
    {
        TopLeft = 0,
        BottomLeft,
        BottomRight
    }
    public enum Orientation
    {
        Horizontal = 0,
        Vertical
    }
    public enum FocusMode
    {
        NotFocusable = 0,
        ResetFocus,
        Focusable,
        FocusableDefocusable
    }
    public enum LayoutMode
    {
        Free = 0,
        Horizontal,
        Vertical
    }
    public enum TraversalMode
    {
        BreadthFirst = 0,
        DepthFirst
    }
    public enum DragAndDropMode
    {
        Disabled = 0x0,
        Source = 0x1,
        Target = 0x2,
        SourceAndTarget = 0x3
    }
}
