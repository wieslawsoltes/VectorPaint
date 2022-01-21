using System.Linq;
using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Media;
using ReactiveUI;
using VectorPaint.ViewModels.Drawables;
using VectorPaint.ViewModels.Tools;

namespace VectorPaint.ViewModels;

public class DrawingEditor : ReactiveObject
{
    private readonly IDrawing _drawing;
    private AvaloniaList<Tool>? _tools;
    private Tool? _currentTool;

    public DrawingEditor(IDrawing drawing)
    {
        _drawing = drawing;

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
            _drawing.Invalidate();
        }
    }

    private void Combine(GeometryCombineMode combineMode)
    {
        if (_drawing.Drawables is null)
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
                    _drawing.Drawables.Remove(drawable);
                }

                _drawing.Drawables.Add(@group);
                _drawing.Invalidate();
            }
        }
    }

    private void Group(FillRule fillRule)
    {
        if (_drawing.Drawables is null)
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
                    _drawing.Drawables.Remove(drawable);
                }

                _drawing.Drawables.Add(@group);
                _drawing.Invalidate();
            }
        }
    }
}
