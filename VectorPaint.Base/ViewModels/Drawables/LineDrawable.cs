using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using ReactiveUI;

namespace VectorPaint.ViewModels.Drawables;

public class LineDrawable : GeometryDrawable
{
    private PointDrawable? _start;
    private PointDrawable? _end;

    public PointDrawable? Start
    {
        get => _start;
        set => this.RaiseAndSetIfChanged(ref _start, value);
    }

    public PointDrawable? End
    {
        get => _end;
        set => this.RaiseAndSetIfChanged(ref _end, value);
    }

    public override bool HitTest(Point point)
    {
        if (Geometry is null || Stroke is null)
        {
            return false;
        }

        return Geometry.StrokeContains(Stroke, point);
    }

    public override void Move(Vector delta)
    {
        if (_start is { } && _end is { })
        {
            _start.Move(delta);
            _end.Move(delta);
            Geometry = CreateGeometry();
        }
    }

    protected sealed override Geometry? CreateGeometry()
    {
        if (_start is null || _end is null)
        {
            return null;
        }

        var geometry = new StreamGeometry();
        using var context = geometry.Open();
        context.SetFillRule(FillRule.EvenOdd);
        context.BeginFigure(new Point(_start.X, _start.Y), false);
        context.LineTo(new Point(_end.X, _end.Y));
        context.EndFigure(false);
        return geometry;
    }
}
