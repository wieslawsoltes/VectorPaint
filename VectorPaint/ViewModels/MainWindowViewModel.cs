using System.Collections.ObjectModel;
using Avalonia.Platform;
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
        
        _drawables.Add(new LineDrawable()
        {
            Start = new PointDrawable(30, 30),
            End = new PointDrawable(150, 150)
        });
        _drawables.Add(new RectangleDrawable()
        {
            TopLeft = new PointDrawable(210, 30),
            BottomRight = new PointDrawable(270, 130)
        });
    }
    
    public void Draw(IDrawingContextImpl context)
    {
        foreach (var drawable in _drawables)
        {
            drawable.Draw(context);
        }
    }
}
