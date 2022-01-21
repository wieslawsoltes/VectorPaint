using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using ReactiveUI;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels;

public class Drawing : ReactiveObject, IDrawing
{
    private ObservableCollection<Drawable>? _drawables;

    public Drawing()
    {
    }

    public ObservableCollection<Drawable>? Drawables
    {
        get => _drawables;
        set => this.RaiseAndSetIfChanged(ref _drawables, value);
    }

    public IVisual? Canvas { get; set; }

    public IInputElement? Input { get; set; }

    public Drawable? HitTest(Point point)
    {
        if (_drawables is null)
        {
            return null;
        }

        for (var i = _drawables.Count - 1; i >= 0; i--)
        {
            var drawable = _drawables[i];
            if (drawable.HitTest(point))
            {
                return drawable;
            }
        }

        return null;
    }

    public IEnumerable<Drawable> HitTest(Rect rect)
    {
        if (_drawables is null)
        {
            yield break;
        }

        for (var i = _drawables.Count - 1; i >= 0; i--)
        {
            var drawable = _drawables[i];
            if (drawable.Intersects(rect))
            {
                yield return drawable;
            }
        }
    }

    public void Draw(DrawingContext context, Rect bounds)
    {
        context.DrawRectangle(Brushes.WhiteSmoke, null, bounds);

        if (_drawables is { })
        {
            foreach (var drawable in _drawables)
            {
                drawable.Draw(context);
            }
        }
    }

    public void Invalidate()
    {
        Canvas?.InvalidateVisual();
    }
}
