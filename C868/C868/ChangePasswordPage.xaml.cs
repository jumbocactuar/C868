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
    public partial class ChangePasswordPage : ContentPage
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ChangePasswordSaveButton_Clicked(object sender, EventArgs e)
        {
            // Validate form inputs
            bool newPassResult = App.PlannerRepo.EntryChecker(newPasswordEntry.Text);

            if (newPassResult == false)
            {
                await DisplayAlert("Alert", "New Password cannot be empty", "OK");
            }

            bool confirmResult = App.PlannerRepo.EntryChecker(confirmPasswordEntry.Text);

            if (confirmResult == false)
            {
                await DisplayAlert("Alert", "Confirm Password cannot be empty", "OK");
            }

            if (newPassResult == true && confirmResult == true)
            {
                List<bool> results = App.PlannerRepo.PasswordChecker(currentPasswordEntry.Text, newPasswordEntry.Text, confirmPasswordEntry.Text);

                if (results[0] == false)
                {
                    await DisplayAlert("Alert", "Current Password is incorrect", "OK");
                }

                if (results[1] == false)
                {
                    await DisplayAlert("Alert", "New Password and Confirm Password do not match", "OK");
                }

                if (results[0] == true && results[1] == true)
                {
                    // Update the user in the database
                    App.PlannerRepo.UpdateUser(App.PlannerRepo.CurrentUser.UserID, newPasswordEntry.Text);

                    // Return to the Terms page
                    await Navigation.PopAsync();
                }
            }
        }
    }
}