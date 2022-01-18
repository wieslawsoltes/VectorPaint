using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using ReactiveUI;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels;

public interface IDrawing
{
    ObservableCollection<Drawable> Drawables { get; set; }
    Drawable? HitTest(Point point);
    void Draw(DrawingContext context, Rect bounds);
    void Invalidate();
    IVisual? Canvas { get; set; }
    IInputElement? Input { get; set; }
}

public class MainWindowViewModel : ViewModelBase, IDrawing
{
    private ObservableCollection<Drawable> _drawables;

    public ObservableCollection<Drawable> Drawables
    {
        get => _drawables;
        set => this.RaiseAndSetIfChanged(ref _drawables, value);
    }

    public IVisual? Canvas { get; set; }

    public IInputElement? Input { get; set; }

    public MainWindowViewModel()
    {
        _drawables = new ObservableCollection<Drawable>();

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

        var combined0 = GeometryDrawable.Combine(GeometryCombineMode.Union, rect0, rect1);
        if (combined0 is { })
        {
            combined0.Move(new Vector(120, 0));
            _drawables.Add(combined0);
        }

        var group0 = GeometryDrawable.Group(FillRule.EvenOdd, new [] { rect0, rect1 });
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
