using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using VectorPaint.ViewModels;
using VectorPaint.ViewModels.Drawables;

namespace VectorPaint.Controls;

public class VectorCanvas : Control
{
    public override void Render(DrawingContext context)
    {
        if (DataContext is MainWindowViewModel mainWindowViewModel)
        {
            context.DrawRectangle(Brushes.WhiteSmoke, null, Bounds);

            foreach (var drawable in mainWindowViewModel.Drawables)
            {
                drawable.Draw(context);
            }
        }
        
        base.Render(context);
    }

    private Drawable? _drawable;
    private Point _start;
    
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (DataContext is MainWindowViewModel mainWindowViewModel)
        {
            var point = e.GetCurrentPoint(this).Position;

            _start = point;

            foreach (var drawable in mainWindowViewModel.Drawables)
            {
                var contains = drawable.HitTest(point);
                if (contains)
                {
                    _drawable = drawable;
                    break;
                }
            }

            if (_drawable is { })
            {
                e.Pointer.Capture(this);
            }
        }
        
        base.OnPointerPressed(e);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (_drawable is { })
        {
            e.Pointer.Capture(null);
            _drawable = null;
        }
        
        base.OnPointerReleased(e);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        if (_drawable is { })
        {
            var point = e.GetCurrentPoint(this).Position;

            _drawable.Move(point - _start);
            
            _start = point;
            
            InvalidateVisual();
        }

        base.OnPointerMoved(e);
    }
}
