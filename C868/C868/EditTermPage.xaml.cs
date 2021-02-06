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
    public partial class EditTermPage : ContentPage
    {
        public EditTermPage(Term term)
        {
            InitializeComponent();

            BindingContext = term;
        }

        private async void EditTermSaveButton_Clicked(object sender, EventArgs e)
        {
            // Put the form inputs into forms acceptable by the UpdateTerm method
            int id = App.PlannerRepo.SelectedTerm;
            string title = editTermTitle.Text;
            DateTime start = editTermStart.Date;
            DateTime end = editTermEnd.Date;

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
                // Update the Term in the database
                App.PlannerRepo.UpdateTerm(id, title, start, end);

                // Return to the Courses page
                await Navigation.PopAsync();
            }
        }

        private async void EditTermCancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}