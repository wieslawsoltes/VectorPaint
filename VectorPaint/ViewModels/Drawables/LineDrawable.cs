using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
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

    public LineDrawable()
    {
        Brush = null;
        Pen = new ImmutablePen(new ImmutableSolidColorBrush(Colors.Red), 4, null, PenLineCap.Round, PenLineJoin.Miter, 10D);
    }

    public override bool HitTest(Point point)
    {
        if (Geometry is null || Pen is null)
        {
            return false;
        }

        return Geometry.StrokeContains(Pen, point);
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

    protected sealed override IGeometryImpl? CreateGeometry()
    {
        if (_start is null || _end is null)
        {
            return null;
        }

        var geometry = AvaloniaExtensions.Factory?.CreateStreamGeometry();
        if (geometry is null)
        {
            return null;
        }

        using var context = geometry.Open();
        context.SetFillRule(FillRule.EvenOdd);
        context.BeginFigure(new Point(_start.X, _start.Y), false);
        context.LineTo(new Point(_end.X, _end.Y));
        context.EndFigure(false);
        return geometry;
    }
}
