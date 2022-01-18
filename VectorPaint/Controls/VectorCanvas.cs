using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using VectorPaint.ViewModels;
using VectorPaint.ViewModels.Tools;

namespace VectorPaint.Controls;

public class VectorCanvas : Control
{
    public ObservableCollection<Tool> Tools { get; set; }

    public Tool? CurrentTool { get; set; }

    public VectorCanvas()
    {
        Tools = new ObservableCollection<Tool>()
        {
            new SelectionTool(),
            new LineTool()
        };

        CurrentTool = Tools[0];
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
            drawing.Draw(context, Bounds);
        }

        base.Render(context);
    }
}
