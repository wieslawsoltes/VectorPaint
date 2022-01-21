using Avalonia;
using Avalonia.Media;
using ReactiveUI;

namespace VectorPaint.ViewModels.Drawables;

public abstract class Drawable : ReactiveObject
{
    public abstract void Draw(DrawingContext context);

    public abstract void Move(Vector delta);

    public abstract bool HitTest(Point point);

    public abstract bool Contains(Point point);

    public abstract bool Intersects(Rect rect);

    public abstract void Invalidate();
}
