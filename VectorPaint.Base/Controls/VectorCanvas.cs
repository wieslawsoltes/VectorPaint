using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using VectorPaint.ViewModels;

namespace VectorPaint.Controls;

public class VectorCanvas : Control
{
    private IDrawing? _drawing;
    private AvaloniaList<Tool>? _tools;
    private Tool? _currentTool;

    public static readonly DirectProperty<VectorCanvas, IDrawing?> DrawingProperty = 
        AvaloniaProperty.RegisterDirect<VectorCanvas, IDrawing?>(
            nameof(Drawing), 
            o => o.Drawing, 
            (o, v) => o.Drawing = v);

    public static readonly DirectProperty<VectorCanvas, AvaloniaList<Tool>?> ToolsProperty = 
        AvaloniaProperty.RegisterDirect<VectorCanvas, AvaloniaList<Tool>?>(
            nameof(Tools), 
            o => o.Tools, 
            (o, v) => o.Tools = v);

    public static readonly DirectProperty<VectorCanvas, Tool?> CurrentToolProperty = 
        AvaloniaProperty.RegisterDirect<VectorCanvas, Tool?>(
            nameof(CurrentTool), 
            o => o.CurrentTool, 
            (o, v) => o.CurrentTool = v);

    public IDrawing? Drawing
    {
        get => _drawing;
        set => SetAndRaise(DrawingProperty, ref _drawing, value);
    }

    public AvaloniaList<Tool>? Tools
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
        if (Drawing is { } drawing)
        {
            CurrentTool?.OnPointerPressed(drawing, e);
        }

        base.OnPointerPressed(e);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (Drawing is { } drawing)
        {
            CurrentTool?.OnPointerReleased(drawing, e);
        }

        base.OnPointerReleased(e);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        if (Drawing is { } drawing)
        {
            CurrentTool?.OnPointerMoved(drawing, e);
        }

        base.OnPointerMoved(e);
    }

    public override void Render(DrawingContext context)
    {
        if (Drawing is { } drawing)
        {
            drawing.Draw(context, new Rect(new Point(0, 0), Bounds.Size));
        }

        base.Render(context);
    }
}
