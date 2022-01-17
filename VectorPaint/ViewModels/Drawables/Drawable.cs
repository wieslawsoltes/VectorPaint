using Avalonia;
using Avalonia.Platform;
using ReactiveUI;

namespace VectorPaint.ViewModels.Drawables;

public abstract class Drawable : ReactiveObject
{
    public abstract void Draw(IDrawingContextImpl context);

    public abstract void Move(Vector delta);

    public abstract bool HitTest(Point point);

    public abstract bool Contains(Point point);

    public abstract bool Intersects(Rect rect);
}
