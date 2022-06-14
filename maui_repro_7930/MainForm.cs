using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;

namespace maui_repro_7930;

public partial class MainForm : Form
{
    private readonly BlazorWebView _blazorWebView;
    
    public MainForm()
    {
        InitializeComponent();
        
        var services = new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();
        
        _blazorWebView = new()
        {
            Dock = DockStyle.Fill,
            HostPage = "wwwroot/index.html",
            Services = services.BuildServiceProvider()
        };
        
        _blazorWebView.RootComponents.Add<App>("#app");
        _blazorWebView.WebView.CoreWebView2InitializationCompleted += WebViewOnCoreWebView2InitializationCompleted;
        
        Controls.Add(_blazorWebView);
    }
    
    private void WebViewOnCoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
    {
        if (!e.IsSuccess) return;
        
        var webView = _blazorWebView!.WebView;
        var coreWebView = webView.CoreWebView2;

        coreWebView.NewWindowRequested += CoreWebViewOnNewWindowRequested;
    }

    private void CoreWebViewOnNewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs args)
    {
        if (!args.Uri.StartsWith("file://")) return;
        
        args.Handled = true;
    }
}