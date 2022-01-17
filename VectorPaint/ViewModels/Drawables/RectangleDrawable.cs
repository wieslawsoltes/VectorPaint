﻿using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using ReactiveUI;

namespace VectorPaint.ViewModels.Drawables;

public class RectangleDrawable : GeometryDrawable
{
    private PointDrawable? _topLeft;
    private PointDrawable? _bottomRight;

    public PointDrawable? TopLeft
    {
        get => _topLeft;
        set => this.RaiseAndSetIfChanged(ref _topLeft, value);
    }

    public PointDrawable? BottomRight
    {
        get => _bottomRight;
        set => this.RaiseAndSetIfChanged(ref _bottomRight, value);
    }

    public RectangleDrawable()
    {
        Brush = new ImmutableSolidColorBrush(Colors.Yellow);
        Pen = new ImmutablePen(new ImmutableSolidColorBrush(Colors.Red), 2, null, PenLineCap.Round, PenLineJoin.Miter, 10D);
    }

    public override void Move(Vector delta)
    {
        if (_topLeft is { } && _bottomRight is { })
        {
            _topLeft.Move(delta);
            _bottomRight.Move(delta);
            Geometry = CreateGeometry();
        }
    }

    private Rect ToRect()
    {
        if (_topLeft is null || _bottomRight is null)
        {
            return Rect.Empty;
        }

        return new Rect(
            new Point(_topLeft.X, _topLeft.Y),
            new Point(_bottomRight.X, _bottomRight.Y));
    }
    
    protected sealed override IGeometryImpl? CreateGeometry()
    {
        if (_topLeft is null || _bottomRight is null)
        {
            return null;
        }

        var geometry = AvaloniaExtensions.Factory?.CreateRectangleGeometry(ToRect());
        if (geometry is null)
        {
            return null;
        }

        return geometry;
    }
}
