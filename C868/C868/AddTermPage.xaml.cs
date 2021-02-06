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
    public partial class AddTermPage : ContentPage
    {
        public AddTermPage()
        {
            InitializeComponent();
        }

        public async void OnSaveButton_Clicked(object sender, EventArgs e)
        {
            // Put the form inputs into forms acceptable by the AddTerm method
            string title = addTermNameEntry.Text;
            DateTime start = addTermStartDatePicker.Date;
            DateTime end = addTermEndDatePicker.Date;

            // Validate form inputs
            bool titleResult = App.PlannerRepo.EntryChecker(title);

            if (titleResult == false)
            {
                await DisplayAlert("Alert", "Title cannot be empty", "OK");
            }

            bool dateResult = App.PlannerRepo.DateChecker(start, end);

            if (dateResult == false)
            {
                await DisplayAlert("Alert", "Start must come before End", "OK");
            }

            if (titleResult == true && dateResult == true)
            {
                // Add the term to the database
                App.PlannerRepo.AddTerm(title, start, end);

                // Return to the Terms page
                await Navigation.PopAsync();
            }
        }

        private async void OnCancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}