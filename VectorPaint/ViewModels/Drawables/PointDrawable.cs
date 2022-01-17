using Avalonia;
using Avalonia.Media;
using ReactiveUI;

namespace VectorPaint.ViewModels.Drawables;

public class PointDrawable : Drawable
{
    private double _x;
    private double _y;

    public double X
    {
        get => _x;
        set => this.RaiseAndSetIfChanged(ref _x, value);
    }

    public double Y
    {
        get => _y;
        set => this.RaiseAndSetIfChanged(ref _y, value);
    }

    public PointDrawable()
    {
    }
    
    public PointDrawable(double x, double y)
    {
        _x = x;
        _y = y;
    }

    public override bool HitTest(Point point)
    {
        throw new System.NotImplementedException();
    }

    public override bool Contains(Point point)
    {
        throw new System.NotImplementedException();
    }

    public override bool Intersects(Rect rect)
    {
        throw new System.NotImplementedException();
    }

    public override void Draw(DrawingContext context)
    {
        throw new System.NotImplementedException();
    }

    public override void Move(Vector delta)
    {
        X += delta.X;
        Y += delta.Y;
    }
}
