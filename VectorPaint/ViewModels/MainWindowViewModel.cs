using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using ReactiveUI;
using VectorPaint.ViewModels.Drawables;
using VectorPaint.ViewModels.Tools;

namespace VectorPaint.ViewModels;

public class MainWindowViewModel : ViewModelBase, IDrawing
{
    private ObservableCollection<Drawable> _drawables;
    private AvaloniaList<Tool> _tools;
    private Tool? _currentTool;

    public MainWindowViewModel()
    {
        _drawables = new ObservableCollection<Drawable>();

        _tools = new AvaloniaList<Tool>()
        {
            new SelectionTool(),
            new LineTool(),
            new RectangleTool(),
            new EllipseTool()
        };

        _currentTool = _tools[0];

        ToolSelectionCommand = ReactiveCommand.Create(SetCurrentTool<SelectionTool>);

        ToolLineCommand = ReactiveCommand.Create(SetCurrentTool<LineTool>);

        ToolRectangleCommand = ReactiveCommand.Create(SetCurrentTool<RectangleTool>);

        ToolEllipseCommand = ReactiveCommand.Create(SetCurrentTool<EllipseTool>);

        CombineUnionCommand = ReactiveCommand.Create(() => Combine(GeometryCombineMode.Union));

        CombineIntersectCommand = ReactiveCommand.Create(() => Combine(GeometryCombineMode.Intersect));

        CombineXorCommand = ReactiveCommand.Create(() => Combine(GeometryCombineMode.Xor));

        CombineExcludeCommand = ReactiveCommand.Create(() => Combine(GeometryCombineMode.Exclude));

        GroupEvenOddCommand = ReactiveCommand.Create(() => Group(FillRule.EvenOdd));

        GroupNonZeroCommand = ReactiveCommand.Create(() => Group(FillRule.NonZero));

        Demo();
    }

    public ObservableCollection<Drawable> Drawables
    {
        get => _drawables;
        set => this.RaiseAndSetIfChanged(ref _drawables, value);
    }

    public AvaloniaList<Tool> Tools
    {
        get => _tools;
        set => this.RaiseAndSetIfChanged(ref _tools, value);
    }

    public Tool? CurrentTool
    {
        get => _currentTool;
        set => this.RaiseAndSetIfChanged(ref _currentTool, value);
    }

    public IVisual? Canvas { get; set; }

    public IInputElement? Input { get; set; }

    public ICommand ToolSelectionCommand { get; }

    public ICommand ToolLineCommand { get; }

    public ICommand ToolRectangleCommand { get; }

    public ICommand ToolEllipseCommand { get; }

    public ICommand CombineUnionCommand { get; }

    public ICommand CombineIntersectCommand { get; }

    public ICommand CombineXorCommand { get; }

    public ICommand CombineExcludeCommand { get; }

    public ICommand GroupEvenOddCommand { get; }

    public ICommand GroupNonZeroCommand { get; }

    private void SetCurrentTool<T>() where T: Tool
    {
        var tool = Tools.OfType<T>().FirstOrDefault();
        if (tool is { })
        {
            CurrentTool = tool;
            Invalidate();
        }
    }
    
    private void Combine(GeometryCombineMode combineMode)
    {
        if (CurrentTool is SelectionTool selectionTool)
        {
            var selected = selectionTool.Selected.OfType<GeometryDrawable>().ToList();
            if (selected.Count >= 2)
            {
                var group = GeometryDrawable.Combine(combineMode, selected);
                if (group is { })
                {
                    foreach (var drawable in selected)
                    {
                        _drawables.Remove(drawable);
                    }

                    _drawables.Add(group);
                    Invalidate();
                }
            }
        }
    }

    private void Group(FillRule fillRule)
    {
        if (CurrentTool is SelectionTool selectionTool)
        {
            var selected = selectionTool.Selected.OfType<GeometryDrawable>().ToList();
            if (selected.Count >= 2)
            {
                var group = GeometryDrawable.Group(fillRule, selected);
                if (group is { })
                {
                    foreach (var drawable in selected)
                    {
                        _drawables.Remove(drawable);
                    }

                    _drawables.Add(group);
                    Invalidate();
                }
            }
        }
    }

    private void Demo()
    {
        var line0 = new LineDrawable()
        {
            Start = new PointDrawable(30, 30),
            End = new PointDrawable(150, 150)
        };
        _drawables.Add(line0);

        var ellipse0 = new EllipseDrawable()
        {
            TopLeft = new PointDrawable(30, 210),
            BottomRight = new PointDrawable(90, 270)
        };
        _drawables.Add(ellipse0);

        var rect0 = new RectangleDrawable()
        {
            TopLeft = new PointDrawable(210, 30),
            BottomRight = new PointDrawable(270, 130)
        };
        _drawables.Add(rect0);

        var rect1 = new RectangleDrawable()
        {
            TopLeft = new PointDrawable(240, 90),
            BottomRight = new PointDrawable(300, 190)
        };
        _drawables.Add(rect1);

        var combined0 = GeometryDrawable.Combine(GeometryCombineMode.Union, new GeometryDrawable[] { rect0, rect1 });
        if (combined0 is { })
        {
            combined0.Move(new Vector(120, 0));
            _drawables.Add(combined0);
        }

        var group0 = GeometryDrawable.Group(FillRule.EvenOdd, new GeometryDrawable[] { rect0, rect1 });
        if (group0 is { })
        {
            group0.Move(new Vector(240, 0));
            _drawables.Add(group0);
        }
    }

    public Drawable? HitTest(Point point)
    {
        for (var i = _drawables.Count - 1; i >= 0; i--)
        {
            var drawable = _drawables[i];
            if (drawable.HitTest(point))
            {
                return drawable;
            }
        }

        return null;
    }

    public void Draw(DrawingContext context, Rect bounds)
    {
        context.DrawRectangle(Brushes.WhiteSmoke, null, bounds);

        foreach (var drawable in _drawables)
        {
            drawable.Draw(context);
        }
    }

    public void Invalidate()
    {
        Canvas?.InvalidateVisual();
    }
}
