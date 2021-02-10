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
    public partial class AddFlashCardPage : ContentPage
    {
        public AddFlashCardPage()
        {
            InitializeComponent();
        }

        private async void AddFlashCardSaveButton_Clicked(object sender, EventArgs e)
        {
            // Put the form inputs into forms acceptable by the AddFlashCard method
            int assessmentID = App.PlannerRepo.SelectedAssessment;
            string question = addFlashCardQuestionEditor.Text;
            string answer = addFlashCardAnswerEditor.Text;
            object confidence = addFlashCardConfidencePicker.SelectedItem;

            // Validate form inputs
            bool questionResult = App.PlannerRepo.EntryChecker(question);

            if (questionResult == false)
            {
                await DisplayAlert("Alert", "Question cannot be empty", "OK");
            }

            bool answerResult = App.PlannerRepo.EntryChecker(answer);

            if (answerResult == false)
            {
                await DisplayAlert("Alert", "Answer cannot be empty", "OK");
            }

            bool confidenceResult = App.PlannerRepo.PickerChecker(confidence);

            if (confidenceResult == false)
            {
                await DisplayAlert("Alert", "Confidence cannot be empty", "OK");
            }

            if (questionResult == true && answerResult == true && confidenceResult == true)
            {
                // Convert the picker value to a string for storage
                string confidenceString = confidence.ToString();

                // Add the flash card to the database
                App.PlannerRepo.AddFlashCard(assessmentID, question, answer, confidenceString);

                // Return to the ObjectiveAssessment page
                await Navigation.PopAsync();
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}