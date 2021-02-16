using C868.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            // If no data exists, add evaluation data
            ObservableCollection<Term> initialList = App.PlannerRepo.GetTermsList();

            if (initialList.Count == 0)
            {
                App.PlannerRepo.AddUser("admin", "test");
                App.PlannerRepo.AddTerm(1, "Term 1 - Winter 2021", new DateTime(2021, 1, 1, 0, 0, 0), new DateTime(2021, 6, 30, 0, 0, 0));
                App.PlannerRepo.AddCourse(1, "R101 - Intro to Women's Studies", new DateTime(2021, 1, 1, 0, 0, 0), new DateTime(2021, 2, 15, 0, 0, 0), false, "In progress", "Cody Burkholz", "206-353-8514", "cburk89@wgu.edu", "Study at least one chapter per day", "3.7");
                App.PlannerRepo.AddAssessment(1, "R101 OA", "Objective", new DateTime(2021, 2, 1, 0, 0, 0), new DateTime(2021, 2, 7, 0, 0, 0), false);
                App.PlannerRepo.AddAssessment(1, "R101 PA", "Performance", new DateTime(2021, 2, 8, 0, 0, 0), new DateTime(2021, 2, 15, 0, 0, 0), false);
            }
        }

        private async void LogInButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Check the inputs against the users in the database and assign the current user
                bool result = App.PlannerRepo.LoginChecker(userNameEntry.Text, passwordEntry.Text);

                if (result == true)
                {
                    await Navigation.PushAsync(new TermsPage());
                }

                else
                {
                    throw new InvalidCredentialsException();
                }
            }

            catch (InvalidCredentialsException)
            {
                await DisplayAlert("Alert", "Invalid user name or password", "OK");
            }
        }
    }
}