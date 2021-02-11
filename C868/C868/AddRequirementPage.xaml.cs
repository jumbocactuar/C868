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
    public partial class AddRequirementPage : ContentPage
    {
        public AddRequirementPage()
        {
            InitializeComponent();
        }

        private async void AddRequirementSaveButton_Clicked(object sender, EventArgs e)
        {
            // Put the form inputs into forms acceptable by the AddRequirement method
            int assessmentID = App.PlannerRepo.SelectedAssessment;
            string req = addRequirementRequirementEditor.Text;
            string notes = addRequirementNotesEditor.Text;
            bool satisfied = addRequirementSatisfiedCheckbox.IsChecked == true ? true : false;

            // Validate form inputs
            bool reqResult = App.PlannerRepo.EntryChecker(req);

            if (reqResult == false)
            {
                await DisplayAlert("Alert", "Requirement cannot be empty", "OK");
            }

            bool notesResult = App.PlannerRepo.EntryChecker(notes);

            if (notesResult == false)
            {
                await DisplayAlert("Alert", "Answer cannot be empty", "OK");
            }

            if (reqResult == true && notesResult == true)
            {
                // Add the requirement to the database
                App.PlannerRepo.AddRequirement(assessmentID, req, notes, satisfied);

                // Return to the PerformanceAssessment page
                await Navigation.PopAsync();
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}