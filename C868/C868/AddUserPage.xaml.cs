using C868.Models;
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
    public partial class AddUserPage : ContentPage
    {
        public AddUserPage()
        {
            InitializeComponent();
        }

        private async void AddUserSaveButton_Clicked(object sender, EventArgs e)
        {
            // Validate form inputs
            bool userNameResult = App.PlannerRepo.EntryChecker(addUserUserNameEntry.Text);

            if (userNameResult == false)
            {
                await DisplayAlert("Alert", "User Name cannot be empty", "OK");
            }

            bool passwordResult = App.PlannerRepo.EntryChecker(addUserPasswordEntry.Text);

            if (userNameResult == false)
            {
                await DisplayAlert("Alert", "Password cannot be empty", "OK");
            }

            if (userNameResult == true && passwordResult == true)
            {
                // Add the user to the database
                App.PlannerRepo.AddUser(addUserUserNameEntry.Text, addUserPasswordEntry.Text);

                // Return to the Terms page
                await Navigation.PopAsync();
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}