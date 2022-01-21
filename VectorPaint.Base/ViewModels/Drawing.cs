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

public class Drawing : ReactiveObject, IDrawing
{
    private ObservableCollection<Drawable>? _drawables;
    private AvaloniaList<Tool>? _tools;
    private Tool? _currentTool;

    public Drawing()
    {
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
    }

    public ObservableCollection<Drawable>? Drawables
    {
        get => _drawables;
        set => this.RaiseAndSetIfChanged(ref _drawables, value);
    }

    public AvaloniaList<Tool>? Tools
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
        var tool = Tools?.OfType<T>().FirstOrDefault();
        if (tool is { })
        {
            CurrentTool = tool;
            Invalidate();
        }
    }
    
    private void Combine(GeometryCombineMode combineMode)
    {
        if (_drawables is null)
        {
            return;
        }

        if (CurrentTool is not SelectionTool selectionTool)
        {
            return;
        }

        var selected = selectionTool.Selected.OfType<GeometryDrawable>().ToList();
        if (selected.Count >= 2)
        {
            var group = GeometryDrawable.Combine(combineMode, selected);
            if (@group is { })
            {
                foreach (var drawable in selected)
                {
                    _drawables.Remove(drawable);
                }

                _drawables.Add(@group);
                Invalidate();
            }
        }
    }

    private void Group(FillRule fillRule)
    {
        if (_drawables is null)
        {
            return;
        }

        if (CurrentTool is not SelectionTool selectionTool)
        {
            return;
        }

        var selected = selectionTool.Selected.OfType<GeometryDrawable>().ToList();
        if (selected.Count >= 2)
        {
            var group = GeometryDrawable.Group(fillRule, selected);
            if (@group is { })
            {
                foreach (var drawable in selected)
                {
                    _drawables.Remove(drawable);
                }

                _drawables.Add(@group);
                Invalidate();
            }
        }
    }

    public Drawable? HitTest(Point point)
    {
        if (_drawables is null)
        {
            return null;
        }

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

        if (_drawables is { })
        {
            foreach (var drawable in _drawables)
            {
                drawable.Draw(context);
            }
        }
    }

    public void Invalidate()
    {
        Canvas?.InvalidateVisual();
    }
}
