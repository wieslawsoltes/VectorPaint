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
        
        _drawables.Add(new LineDrawable());
    }
    
    public void Draw(IDrawingContextImpl context)
    {
        foreach (var drawable in _drawables)
        {
            drawable.Draw(context);
        }
    }
}
