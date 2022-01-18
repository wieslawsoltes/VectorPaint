using Avalonia.Input;

namespace VectorPaint.ViewModels.Tools;

public abstract class Tool
{
    public abstract string Title { get; }

    public abstract void OnPointerPressed(IDrawing drawing, PointerPressedEventArgs e);

    public abstract void OnPointerReleased(IDrawing drawing, PointerReleasedEventArgs e);

    public abstract void OnPointerMoved(IDrawing drawing, PointerEventArgs e);
}
