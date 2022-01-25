using Avalonia;
using Avalonia.Media;
using ReactiveUI;

namespace VectorPaint.ViewModels.Drawables;

public abstract class Drawable : ReactiveObject
{
    public IBrush? Fill { get; set; }

    public IPen? Stroke { get; set; }

    public abstract void Draw(DrawingContext context);

    public abstract void Move(Vector delta);

    public abstract bool HitTest(Point point);

    public abstract bool Contains(Point point);

    public abstract bool Intersects(Rect rect);

    public abstract void Invalidate();
}
