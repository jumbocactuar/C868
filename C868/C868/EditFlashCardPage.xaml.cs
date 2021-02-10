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
    public partial class EditFlashCardPage : ContentPage
    {
        public int SelectedFlashCard;

        public EditFlashCardPage(FlashCard flashCard)
        {
            InitializeComponent();

            BindingContext = flashCard;

            SelectedFlashCard = flashCard.CardID;
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            // Prompt the user to confirm deletion, return to the previous page if confirmed
            bool answer = await DisplayAlert("Delete flash card?", "Are you sure?", "Yes", "Cancel");

            if (answer == true)
            {
                App.PlannerRepo.DeleteFlashCard(SelectedFlashCard);

                await Navigation.PopAsync();
            }
        }

        private async void EditFlashCardSaveButton_Clicked(object sender, EventArgs e)
        {
            // Put the form inputs into forms acceptable by the UpdateFlashCard method
            int oaID = App.PlannerRepo.SelectedOA;
            string question = editFlashCardQuestionEditor.Text;
            string answer = editFlashCardAnswerEditor.Text;
            object confidence = editFlashCardConfidencePicker.SelectedItem;

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

                // Update the flash card in the database
                App.PlannerRepo.UpdateFlashCard(oaID, question, answer, confidenceString);

                // Return to the ObjectiveAssessment page
                await Navigation.PopAsync();
            }
        }
    }
}