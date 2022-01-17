using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;

namespace VectorPaint.ViewModels.Drawables;

public abstract class GeometryDrawable : Drawable
{
    protected IGeometryImpl? Geometry { get; set; }

    protected IBrush? Brush { get; set; }

    protected IPen? Pen { get; set; }

    public override bool HitTest(Point point)
    {
        if (Geometry is null)
        {
            return false;
        }

        if (Pen is { })
        {
            if (Geometry.StrokeContains(Pen, point))
            {
                return true;
            }
        }

        if (Geometry.FillContains(point))
        {
            return true;
        }

        return false;
    }

    public override bool Contains(Point point)
    {
        if (Geometry is null)
        {
            return false;
        }

        return Geometry.Bounds.Contains(point);
    }

    public override bool Intersects(Rect rect)
    {
        if (Geometry is null || Pen is null)
        {
            return false;
        }

        return Geometry.Bounds.Intersects(rect);
    }

    protected abstract IGeometryImpl? CreateGeometry();

    public override void Draw(IDrawingContextImpl context)
    {
        Geometry ??= CreateGeometry();

        if (Geometry is { })
        {
            context.DrawGeometry(Brush, Pen, Geometry);
        }
    }
}
