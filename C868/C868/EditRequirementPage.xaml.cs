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
    public partial class EditRequirementPage : ContentPage
    {
        public int SelectedReq;

        public EditRequirementPage(Requirement requirement)
        {
            InitializeComponent();

            BindingContext = requirement;

            SelectedReq = requirement.ReqID;
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            // Prompt the user to confirm deletion, return to the previous page if confirmed
            bool answer = await DisplayAlert("Delete requirement?", "Are you sure?", "Yes", "Cancel");

            if (answer == true)
            {
                App.PlannerRepo.DeleteRequirement(SelectedReq);

                await Navigation.PopAsync();
            }
        }

        private async void EditRequirementSaveButton_Clicked(object sender, EventArgs e)
        {
            // Put the form inputs into forms acceptable by the UpdateRequirement method
            int paID = App.PlannerRepo.SelectedPA;
            string req = editRequirementRequirementEditor.Text;
            string notes = editRequirementNotesEditor.Text;
            bool satisfied = editRequirementSatisfiedCheckbox.IsChecked == true ? true : false;

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
                // Update the requirement in the database
                App.PlannerRepo.UpdateRequirement(paID, req, notes, satisfied);

                // Return to the PerformanceAssessment page
                await Navigation.PopAsync();
            }
        }
    }
}