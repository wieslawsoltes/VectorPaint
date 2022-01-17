using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Media;
using ReactiveUI;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Drawable> _drawables;
    
    public ObservableCollection<Drawable> Drawables
    {
        get => _drawables;
        set => this.RaiseAndSetIfChanged(ref _drawables, value);
    }
    
    public MainWindowViewModel()
    {
        _drawables = new ObservableCollection<Drawable>();

        var line0 = new LineDrawable()
        {
            Start = new PointDrawable(30, 30),
            End = new PointDrawable(150, 150)
        };
        _drawables.Add(line0);

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

    public void Draw(DrawingContext context)
    {
        foreach (var drawable in _drawables)
        {
            drawable.Draw(context);
        }
    }
}
