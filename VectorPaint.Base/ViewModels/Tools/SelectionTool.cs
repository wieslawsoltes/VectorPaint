using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels.Tools;

public class SelectionTool : Tool
{
    private readonly HashSet<Drawable> _selected = new();
    private Point _start;
    private bool _moving;

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
            if (!e.KeyModifiers.HasFlag(KeyModifiers.Shift))
            {
                _selected.Clear();
            }
        }

        _start = point;
        e.Pointer.Capture(drawing.Input);
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

            if (selected.Count > 0)
            {
                drawing.Invalidate();
            }
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

        if (!_moving)
        {
            return;
        }

        var point = e.GetCurrentPoint(drawing.Input).Position;

        if (_selected.Count > 0)
        {
            foreach (var drawable in _selected)
            {
                drawable.Move(point - _start);
            }

            _start = point;

            drawing.Invalidate();
        }
    }
}
