using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using VectorPaint.ViewModels;
using VectorPaint.ViewModels.Tools;

namespace VectorPaint.Controls;

public class VectorCanvas : Control
{
    private AvaloniaList<Tool> _tools;
    private Tool? _currentTool;

    public static readonly DirectProperty<VectorCanvas, AvaloniaList<Tool>> ToolsProperty = 
        AvaloniaProperty.RegisterDirect<VectorCanvas, AvaloniaList<Tool>>(
            nameof(Tools), 
            o => o.Tools, 
            (o, v) => o.Tools = v);

    public static readonly DirectProperty<VectorCanvas, Tool?> CurrentToolProperty = 
        AvaloniaProperty.RegisterDirect<VectorCanvas, Tool?>(
            nameof(CurrentTool), 
            o => o.CurrentTool, 
            (o, v) => o.CurrentTool = v);

    public VectorCanvas()
    {
        _tools = new AvaloniaList<Tool>()
        {
            new SelectionTool(),
            new LineTool(),
            new RectangleTool(),
            new EllipseTool()
        };

        _currentTool = Tools[0];
    }

    public AvaloniaList<Tool> Tools
    {
        get => _tools;
        set => SetAndRaise(ToolsProperty, ref _tools, value);
    }

    public Tool? CurrentTool
    {
        get => _currentTool;
        set => SetAndRaise(CurrentToolProperty, ref _currentTool, value);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (DataContext is IDrawing drawing)
        {
            CurrentTool?.OnPointerPressed(drawing, e);
        }

        base.OnPointerPressed(e);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (DataContext is IDrawing drawing)
        {
            CurrentTool?.OnPointerReleased(drawing, e);
        }
        
        base.OnPointerReleased(e);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        if (DataContext is IDrawing drawing)
        {
            CurrentTool?.OnPointerMoved(drawing, e);
        }

        base.OnPointerMoved(e);
    }

    public override void Render(DrawingContext context)
    {
        if (DataContext is IDrawing drawing)
        {
            drawing.Draw(context, new Rect(new Point(0, 0), Bounds.Size));
        }

        base.Render(context);
    }
}
