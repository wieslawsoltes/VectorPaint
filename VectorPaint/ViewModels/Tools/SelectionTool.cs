using System.Collections.Generic;
using Avalonia;
using Avalonia.Input;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels.Tools;

public class SelectionTool : Tool
{
    private readonly HashSet<Drawable> _selected = new();
    private Point _start;

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

            _start = point;
            e.Pointer.Capture(drawing.Input);
        }
        else
        {
            _selected.Clear();
        }
    }

    public override void OnPointerReleased(IDrawing drawing, PointerReleasedEventArgs e)
    {
        if (!Equals(e.Pointer.Captured, drawing.Input))
        {
            return;
        }

        e.Pointer.Capture(null);
    }

    public override void OnPointerMoved(IDrawing drawing, PointerEventArgs e)
    {
        if (!Equals(e.Pointer.Captured, drawing.Input))
        {
            return;
        }

        if (_selected.Count > 0)
        {
            var point = e.GetCurrentPoint(drawing.Input).Position;

            foreach (var drawable in _selected)
            {
                drawable.Move(point - _start);
            }

            _start = point;

            drawing.Invalidate();
        }
    }
}
