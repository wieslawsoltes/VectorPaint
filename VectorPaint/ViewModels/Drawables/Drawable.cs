using Avalonia;
using Avalonia.Platform;
using ReactiveUI;

namespace VectorPaint.ViewModels.Drawables;

public abstract class Drawable : ReactiveObject
{
    public abstract IGeometryImpl? Geometry { get; }

    public abstract void Draw(IDrawingContextImpl context);

    public abstract bool HitTest(Point point);

    public abstract bool Contains(Point point);

    public abstract bool Intersects(Rect rect);

    public abstract void Move(Vector delta);
}
