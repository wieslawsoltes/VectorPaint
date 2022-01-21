using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels;

public interface IDrawing
{
    ObservableCollection<Drawable>? Drawables { get; set; }
    Drawable? HitTest(Point point);
    void Draw(DrawingContext context, Rect bounds);
    void Invalidate();
    IVisual? Canvas { get; set; }
    IInputElement? Input { get; set; }
}
