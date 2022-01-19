using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace VectorPaint.ViewModels.Drawables;

public class PathDrawable : GeometryDrawable
{
    public PathDrawable()
    {
        Brush = new ImmutableSolidColorBrush(Colors.Yellow);
        Pen = new ImmutablePen(new ImmutableSolidColorBrush(Colors.Red), 2, null, PenLineCap.Round, PenLineJoin.Miter, 10D);
    }

    public override void Move(Vector delta)
    {
        if (Geometry is null)
        {
            return;
        }

        if (Geometry.Transform is MatrixTransform matrixTransform)
        {
            matrixTransform.Matrix = Matrix.CreateTranslation(delta) * matrixTransform.Matrix;
        }
        else
        {
            Geometry.Transform = new MatrixTransform(Matrix.CreateTranslation(delta));
        }
    }

    protected sealed override Geometry? CreateGeometry()
    {
        if (Geometry is null)
        {
            return null;
        }

        // TODO: Create path geometry.
        return null;
    }
}
