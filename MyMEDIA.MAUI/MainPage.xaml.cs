using Microsoft.AspNetCore.Components.WebView.Maui;
namespace MyMEDIA.MAUI

{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            blazorWebView.RootComponents.Add(new RootComponent
            {
                Selector = "#app",
                ComponentType = typeof(Main),
                Parameters = null
            });
        }
    }
}
