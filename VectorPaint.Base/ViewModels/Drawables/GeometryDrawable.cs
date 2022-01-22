using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;

namespace VectorPaint.ViewModels.Drawables;

public abstract class GeometryDrawable : Drawable
{
    protected Geometry? Geometry { get; set; }

    public override bool HitTest(Point point)
    {
        if (Geometry is null)
        {
            return false;
        }

        if (Stroke is { })
        {
            if (Geometry.StrokeContains(Stroke, point))
            {
                return true;
            }
        }

        if (Geometry.FillContains(point))
        {
            return true;
        }

        /*
        if (Geometry.Bounds.Contains(point))
        {
            return true;
        }
        */

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
        if (Geometry is null || Stroke is null)
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

        context.DrawGeometry(Fill, Stroke, Geometry);

    }

    public override void Invalidate()
    {
        Geometry = CreateGeometry();
    }

    public static GeometryDrawable? Combine(GeometryCombineMode combineMode, IList<GeometryDrawable> drawables)
    {
        if (drawables.Count <= 1)
        {
            return null;
        }

        drawables[0].Geometry ??= drawables[0].CreateGeometry();
      
        var g1 = drawables[0].Geometry;

        for (var i = 1; i < drawables.Count; i++)
        {
            drawables[i].Geometry ??= drawables[i].CreateGeometry();

            var g2 = drawables[i].Geometry;

            if (g1 is null || g2 is null)
            {
                return null;
            }

            g1 = new CombinedGeometry()
            {
                GeometryCombineMode = combineMode,
                Geometry1 = g1,
                Geometry2 = g2
            };
        }

        var path = new PathDrawable
        {
            Geometry = g1,
            Fill = drawables[0].Fill,
            Stroke = drawables[0].Stroke
        };

        return path;
    }

    public static GeometryDrawable? Group(FillRule fillRule, IList<GeometryDrawable> drawables)
    {
        if (drawables.Count <= 1)
        {
            return null;
        }

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
            Geometry = geometryGroup,
            Fill = drawables[0].Fill,
            Stroke = drawables[0].Stroke
        };

        return path;
    }
}
