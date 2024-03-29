﻿using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using VectorPaint.ViewModels.Drawables;
using VectorPaint.ViewModels.Tools;

namespace VectorPaint.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public Drawing Drawing { get; }
    
    public DrawingEditor Editor { get; }

    public MainWindowViewModel()
    {
        Drawing = new Drawing
        {
            DefaultFill = new ImmutableSolidColorBrush(Colors.Transparent),
            DefaultStroke = new ImmutablePen(new ImmutableSolidColorBrush(Colors.Black), 10, null, PenLineCap.Round),
            Drawables = new ObservableCollection<Drawable>(),
            OverlayDrawables = new ObservableCollection<Drawable>()
        };

        Editor = new DrawingEditor(Drawing) 
        { 
            Tools = new AvaloniaList<Tool>()
            {
                new SelectionTool(),
                new LineTool(),
                new RectangleTool(),
                new EllipseTool()
            }
        };

        Editor.CurrentTool = Editor.Tools[0];

        // Demo(Drawing);
    }

    private void Demo(IDrawing drawing)
    {
        if (drawing.Drawables is null)
        {
            return;
        }

        var line0 = new LineDrawable()
        {
            Start = new PointDrawable(30, 30),
            End = new PointDrawable(150, 150),
            Fill = null,
            Stroke = Drawing.DefaultStroke
        };
        drawing.Drawables.Add(line0);

        var ellipse0 = new EllipseDrawable()
        {
            TopLeft = new PointDrawable(30, 210),
            BottomRight = new PointDrawable(90, 270),
            Fill = Drawing.DefaultFill,
            Stroke = Drawing.DefaultStroke
        };
        drawing.Drawables.Add(ellipse0);

        var rect0 = new RectangleDrawable()
        {
            TopLeft = new PointDrawable(210, 30),
            BottomRight = new PointDrawable(270, 130),
            Fill = Drawing.DefaultFill,
            Stroke = Drawing.DefaultStroke
        };
        drawing.Drawables.Add(rect0);

        var rect1 = new RectangleDrawable()
        {
            TopLeft = new PointDrawable(240, 90),
            BottomRight = new PointDrawable(300, 190),
            Fill = Drawing.DefaultFill,
            Stroke = Drawing.DefaultStroke
        };
        drawing.Drawables.Add(rect1);

        var combined0 = GeometryDrawable.Combine(GeometryCombineMode.Union, new GeometryDrawable[] { rect0, rect1 });
        if (combined0 is { })
        {
            combined0.Move(new Vector(120, 0));
            drawing.Drawables.Add(combined0);
        }

        var group0 = GeometryDrawable.Group(FillRule.EvenOdd, new GeometryDrawable[] { rect0, rect1 });
        if (group0 is { })
        {
            group0.Move(new Vector(240, 0));
            drawing.Drawables.Add(group0);
        }
    }
}
