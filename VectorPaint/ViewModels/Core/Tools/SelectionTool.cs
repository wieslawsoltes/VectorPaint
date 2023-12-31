using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using VectorPaint.ViewModels.Core;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels.Tools;

public class SelectionTool : Tool
{
    private readonly HashSet<Drawable> _selected = new();
    private Point _start;
    private bool _moving;
    private RectangleDrawable? _rectangle;

    public override string Title => "Selection";

    public HashSet<Drawable> Selected => _selected;

    public override void OnPointerPressed(IDrawing drawing, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(drawing.Input).Position;

        var drawable = drawing.HitTest(point);
        if (drawable is { })
        {
            if (e.KeyModifiers == KeyModifiers.Shift)
            {
                _selected.Add(drawable);
            }
            else
            {
                if (!_selected.Contains(drawable))
                {
                    _selected.Clear();
                    _selected.Add(drawable);
                }
            }

            _moving = true;
        }
        else
        {
            if (drawing.OverlayDrawables is { })
            {
                _rectangle = new RectangleDrawable()
                {
                    Fill = new ImmutableSolidColorBrush(Colors.Blue, 0.4),
                    Stroke = new ImmutablePen(new ImmutableSolidColorBrush(Colors.Blue), 1, null, PenLineCap.Round),
                    TopLeft = new PointDrawable(point.X, point.Y),
                    BottomRight = new PointDrawable(point.X, point.Y)
                };

                drawing.OverlayDrawables.Add(_rectangle);
            }

            if (!e.KeyModifiers.HasFlag(KeyModifiers.Shift))
            {
                _selected.Clear();
            }
        }

        _start = point;
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

        if (!_moving)
        {
            var point = e.GetCurrentPoint(drawing.Input).Position;
            
            var rect = new Rect(_start, point);

            var selected = drawing.HitTest(rect).ToList();

            foreach (var drawable in selected)
            {
                _selected.Add(drawable);
            }

            if (drawing.OverlayDrawables is { } && _rectangle is { })
            {
                drawing.OverlayDrawables.Remove(_rectangle);
                _rectangle = null;
            }

            drawing.Invalidate();
        }

        _moving = false;
        e.Pointer.Capture(null);
    }

    public override void OnPointerMoved(IDrawing drawing, PointerEventArgs e)
    {
        if (!Equals(e.Pointer.Captured, drawing.Input))
        {
            return;
        }

        var point = e.GetCurrentPoint(drawing.Input).Position;
        
        if (_moving)
        {
            if (_selected.Count > 0)
            {
                foreach (var drawable in _selected)
                {
                    point = SnapHelper.SnapPoint(point);

                    drawable.Move(point - _start);
                }

                _start = point;

                drawing.Invalidate();
                e.Handled = true;
            }
        }
        else
        {
            if (_rectangle?.BottomRight is { })
            {
                _rectangle.BottomRight.X = point.X;
                _rectangle.BottomRight.Y = point.Y;
                _rectangle.Invalidate();

                drawing.Invalidate();
                e.Handled = true;
            }
        }
    }
}
