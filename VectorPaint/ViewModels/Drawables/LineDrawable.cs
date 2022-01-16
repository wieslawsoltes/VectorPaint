using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using ReactiveUI;

namespace VectorPaint.ViewModels.Drawables;

public class LineDrawable : Drawable
{
    private PointDrawable? _start;
    private PointDrawable? _end;
    private IGeometryImpl? _geometry;
    private ImmutablePen? _pen;

    public override IGeometryImpl? Geometry => _geometry;

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
        _start = new PointDrawable(30, 30);
        _end = new PointDrawable(300, 300);
        _geometry = CreateGeometry();
        _pen = new ImmutablePen(new ImmutableSolidColorBrush(Colors.Red), 4, null, PenLineCap.Round, PenLineJoin.Miter, 10D);
    }

    public override bool HitTest(Point point)
    {
        if (_geometry is null || _pen is null)
        {
            return false;
        }

        return _geometry.StrokeContains(_pen, point);
    }
    
    public override bool Contains(Point point)
    {
        if (_geometry is null || _pen is null)
        {
            return false;
        }

        return _geometry.Bounds.Contains(point);
    }

    public override bool Intersects(Rect rect)
    {
        if (_geometry is null || _pen is null)
        {
            return false;
        }

        return _geometry.Bounds.Intersects(rect);
    }

    public override void Move(Vector delta)
    {
        if (_start is { } && _end is { })
        {
            _start.Move(delta);
            _end.Move(delta);
            _geometry = CreateGeometry();
        }
    }

    private IGeometryImpl? CreateGeometry()
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

    public override void Draw(IDrawingContextImpl context)
    {
        context.DrawGeometry(null, _pen, _geometry);
    }
}
