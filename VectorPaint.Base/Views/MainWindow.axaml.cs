using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VectorPaint.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        Renderer.DrawFps = false;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}