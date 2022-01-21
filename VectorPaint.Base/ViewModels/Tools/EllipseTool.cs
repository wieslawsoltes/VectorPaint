using Avalonia.Input;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels.Tools;

public class EllipseTool : Tool
{
    private EllipseDrawable? _ellipse;

    public override string Title => "Ellipse";

    public override void OnPointerPressed(IDrawing drawing, PointerPressedEventArgs e)
    {
        if (drawing.Drawables is null)
        {
            return;
        }

        var point = e.GetCurrentPoint(drawing.Input).Position;

        _ellipse = new EllipseDrawable()
        {
            TopLeft = new PointDrawable(point.X, point.Y),
            BottomRight = new PointDrawable(point.X, point.Y)
        };
        drawing.Drawables.Add(_ellipse);

        e.Pointer.Capture(drawing.Input);
        e.Handled = true;

        drawing.Invalidate();
    }

    public override void OnPointerReleased(IDrawing drawing, PointerReleasedEventArgs e)
    {
        if (!Equals(e.Pointer.Captured, drawing.Input))
        {
            return;
        }

        if (_ellipse is { })
        {
            e.Pointer.Capture(null);
            _ellipse = null;
        }
    }

    public override void OnPointerMoved(IDrawing drawing, PointerEventArgs e)
    {
        if (!Equals(e.Pointer.Captured, drawing.Input))
        {
            return;
        }

        if (_ellipse?.BottomRight is { })
        {
            var point = e.GetCurrentPoint(drawing.Input).Position;

            _ellipse.BottomRight.X = point.X;
            _ellipse.BottomRight.Y = point.Y;
            _ellipse.Invalidate();

            drawing.Invalidate();
            e.Handled = true;
        }
    }
}
