using Avalonia.ReactiveUI;
using Avalonia.Web.Blazor;

namespace VectorPaint.Web;

public partial class App
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        WebAppBuilder.Configure<VectorPaint.App>()
            .UseReactiveUI()
            .SetupWithSingleViewLifetime();
    }
}
