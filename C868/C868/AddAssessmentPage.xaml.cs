using C868.Models;
using Plugin.LocalNotifications;
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
    public partial class AddAssessmentPage : ContentPage
    {
        public AddAssessmentPage()
        {
            InitializeComponent();
        }

        private async void AddAssessmentSaveButton_Clicked(object sender, EventArgs e)
        {
            // Put the form inputs into forms acceptable by the AddAssessment method
            int id = App.PlannerRepo.SelectedCourse;
            string name = addAssessmentNameEntry.Text;
            object type = addAssessmentTypePicker.SelectedItem;
            DateTime start = addAssessmentStartDatePicker.Date;
            DateTime end = addAssessmentEndDatePicker.Date;
            bool notify = addAssessmentNotificationsSwitch.IsToggled == true ? true : false;

            // Validate form inputs
            bool titleResult = App.PlannerRepo.EntryChecker(name);

            if (titleResult == false)
            {
                await DisplayAlert("Alert", "Assessment Name cannot be empty", "OK");
            }

            bool dateResult = App.PlannerRepo.DateChecker(start, end);

            if (dateResult == false)
            {
                await DisplayAlert("Alert", "Start must come before End", "OK");
            }

            bool typeResult = App.PlannerRepo.PickerChecker(type);

            if (typeResult == false)
            {
                await DisplayAlert("Alert", "Type cannot be empty", "OK");
            }

            bool typeCheck = App.PlannerRepo.AddAssessmentTypeChecker(type);

            if (typeCheck == false)
            {
                await DisplayAlert("Alert", "An assessment of the selected type already exists for this course", "OK");
            }

            if (titleResult == true && typeResult == true && typeCheck == true && dateResult == true)
            {
                // Convert the picker value to a string in preparation for storage
                string typeString = type.ToString();

                // Add the assessment to the database
                App.PlannerRepo.AddAssessment(id, name, typeString, start, end, notify);

                // Set notifications if enabled
                if (notify == true)
                {
                    // Get the new list of assessments
                    ObservableCollection<Assessment> assessments = App.PlannerRepo.GetAssessmentsList();

                    // Get the ID of the assessment added above
                    int AssessmentID = assessments.Last<Assessment>().AssessmentID;

                    // Add 3000 to the AssessmentID for assessment start date notification IDs
                    int startID = AssessmentID + 3000;

                    // Add 4000 to the AssessmentID for assessment end date notification IDs
                    int endID = AssessmentID + 4000;

                    CrossLocalNotifications.Current.Show("Assessment Start", $"{name} starts today", startID, start);
                    CrossLocalNotifications.Current.Show("Assessment End", $"{name} ends today", endID, end);
                }

                // Return to the Assessments page
                await Navigation.PopAsync();
            }
        }

        private async void OnCancelButton_Clicked(object sender, EventArgs e)
        {
            // Return to the previous page
            await Navigation.PopAsync();
        }
    }
}