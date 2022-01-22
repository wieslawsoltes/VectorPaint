using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels;

public interface IDrawing
{
    IBrush? DefaultFill { get; set; }
    IPen? DefaultStroke { get; set; }
    ObservableCollection<Drawable>? Drawables { get; set; }
    ObservableCollection<Drawable>? OverlayDrawables { get; set; }
    Drawable? HitTest(Point point);
    IEnumerable<Drawable> HitTest(Rect rect);
    void Draw(DrawingContext context, Rect bounds);
    void Invalidate();
    IVisual? Canvas { get; set; }
    IInputElement? Input { get; set; }
}
