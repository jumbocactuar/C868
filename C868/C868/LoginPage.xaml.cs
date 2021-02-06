using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C868
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LogInButton_Clicked(object sender, EventArgs e)
        {
            bool result = App.PlannerRepo.LoginChecker(userNameEntry.Text, passwordEntry.Text);

            if (result == true)
            {
                await Navigation.PushAsync(new TermsPage());
            }

            else
            {
                await DisplayAlert("Alert", "Invalid user name or password", "OK");
            }
        }
    }
}