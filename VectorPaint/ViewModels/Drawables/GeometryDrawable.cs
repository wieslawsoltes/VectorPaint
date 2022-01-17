using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;

namespace VectorPaint.ViewModels.Drawables;

public abstract class GeometryDrawable : Drawable
{
    protected Geometry? Geometry { get; set; }

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

    protected abstract Geometry? CreateGeometry();

    public override void Draw(DrawingContext context)
    {
        Geometry ??= CreateGeometry();
        if (Geometry is null)
        {
            return;
        }

        DrawingContext.PushedState? pushedState = null; 
 
        if (Geometry.Transform is {  })
        {
            pushedState = context.PushPreTransform(Geometry.Transform.Value);
        }

        context.DrawGeometry(Brush, Pen, Geometry);

        pushedState?.Dispose();
    }

    public static GeometryDrawable? Combine(GeometryCombineMode combineMode, GeometryDrawable g1, GeometryDrawable g2)
    {
        g1.Geometry ??= g1.CreateGeometry();
        g2.Geometry ??= g2.CreateGeometry();

        if (g1.Geometry is { } && g2.Geometry is { })
        {
            var combinedGeometry = new CombinedGeometry()
            {
                GeometryCombineMode = combineMode,
                Geometry1 = g1.Geometry,
                Geometry2 = g2.Geometry
            };

            var path = new PathDrawable
            {
                Geometry = combinedGeometry
            };

            return path;
        }

        return null;
    }

    public static GeometryDrawable? Group(FillRule fillRule, IEnumerable<GeometryDrawable> drawables)
    {
        var children = new GeometryCollection();

        foreach (var child in drawables)
        {
            child.Geometry ??= child.CreateGeometry();
            if (child.Geometry is null)
            {
                return null;
            }

            children.Add(child.Geometry);
        }
 
        var geometryGroup = new GeometryGroup()
        {
            FillRule = fillRule, 
            Children = children
        };

        var path = new PathDrawable
        {
            Geometry = geometryGroup
        };

        return path;
    }
}
