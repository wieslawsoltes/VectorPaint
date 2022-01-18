using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VectorPaint.Controls;
using VectorPaint.ViewModels;

namespace VectorPaint.Views;

public class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (DataContext is IDrawing drawing)
        {
            var vectorCanvas = this.FindControl<VectorCanvas>("VectorCanvas");
            if (vectorCanvas is { })
            {
                drawing.Canvas = vectorCanvas;
                drawing.Input = vectorCanvas;
            }
        }

        base.OnAttachedToVisualTree(e);
    }
}

