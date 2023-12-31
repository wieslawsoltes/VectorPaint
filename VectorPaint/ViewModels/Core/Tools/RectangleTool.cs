using Avalonia.Input;
using VectorPaint.ViewModels.Core;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels.Tools;

public class RectangleTool : Tool
{
    private RectangleDrawable? _rectangle;

    public override string Title => "Rectangle";

    public override void OnPointerPressed(IDrawing drawing, PointerPressedEventArgs e)
    {
        if (drawing.Drawables is null)
        {
            return;
        }

        var point = e.GetCurrentPoint(drawing.Input).Position;
        
        point = SnapHelper.SnapPoint(point);

        _rectangle = new RectangleDrawable()
        {
            Fill = drawing.DefaultFill,
            Stroke = drawing.DefaultStroke,
            TopLeft = new PointDrawable(point.X, point.Y),
            BottomRight = new PointDrawable(point.X, point.Y)
        };
        drawing.Drawables.Add(_rectangle);

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

        if (_rectangle is { })
        {
            e.Pointer.Capture(null);
            _rectangle = null;
        }
    }

    public override void OnPointerMoved(IDrawing drawing, PointerEventArgs e)
    {
        if (!Equals(e.Pointer.Captured, drawing.Input))
        {
            return;
        }

        if (_rectangle?.BottomRight is { })
        {
            var point = e.GetCurrentPoint(drawing.Input).Position;

            point = SnapHelper.SnapPoint(point);
            
            _rectangle.BottomRight.X = point.X;
            _rectangle.BottomRight.Y = point.Y;
            _rectangle.Invalidate();

            drawing.Invalidate();
            e.Handled = true;
        }
    }
}
