using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C868
{
    public partial class App : Application
    {
        public static PlannerRepository PlannerRepo { get; private set; }

        public App(string dbPath)
        {
            InitializeComponent();

            PlannerRepo = new PlannerRepository(dbPath);

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
