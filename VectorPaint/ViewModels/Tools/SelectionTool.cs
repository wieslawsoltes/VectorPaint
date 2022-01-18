using Avalonia;
using Avalonia.Input;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels.Tools;

public class SelectionTool : Tool
{
    private Drawable? _drawable;
    private Point _start;

    public override void OnPointerPressed(IDrawing drawing, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(drawing.Input).Position;

        var drawable = drawing.HitTest(point);
        if (drawable is { })
        {
            _drawable = drawable;
            _start = point;
            e.Pointer.Capture(drawing.Input);
        }
    }

    public override void OnPointerReleased(IDrawing drawing, PointerReleasedEventArgs e)
    {
        if (_drawable is { })
        {
            e.Pointer.Capture(null);
            _drawable = null;
        }
    }

    public override void OnPointerMoved(IDrawing drawing, PointerEventArgs e)
    {
        if (_drawable is { })
        {
            var point = e.GetCurrentPoint(drawing.Input).Position;

            _drawable.Move(point - _start);
            _start = point;

            drawing.Invalidate();
        }
    }
}
