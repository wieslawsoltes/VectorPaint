using Avalonia.Input;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels.Tools;

public class LineTool : Tool
{
    private LineDrawable? _line;

    public override string Title => "Line";

    public override void OnPointerPressed(IDrawing drawing, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(drawing.Input).Position;

        _line = new LineDrawable()
        {
            Start = new PointDrawable(point.X, point.Y),
            End = new PointDrawable(point.X, point.Y)
        };
        drawing.Drawables.Add(_line);

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

        if (_line is { })
        {
            e.Pointer.Capture(null);
            _line = null;
        }
    }

    public override void OnPointerMoved(IDrawing drawing, PointerEventArgs e)
    {
        if (!Equals(e.Pointer.Captured, drawing.Input))
        {
            return;
        }

        if (_line?.End is { })
        {
            var point = e.GetCurrentPoint(drawing.Input).Position;

            _line.End.X = point.X;
            _line.End.Y = point.Y;
            _line.Invalidate();

            drawing.Invalidate();
            e.Handled = true;
        }
    }
}
