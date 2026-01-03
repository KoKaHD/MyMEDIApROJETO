namespace MyMEDIA.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage()); // MainPage é só host
        }
    }
}
