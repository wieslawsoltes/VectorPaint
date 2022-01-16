using Avalonia;
using Avalonia.Platform;

namespace VectorPaint.ViewModels.Drawables;

internal static class AvaloniaExtensions
{
    internal static IPlatformRenderInterface? Factory 
        => AvaloniaLocator.Current.GetService<IPlatformRenderInterface>();
}
