using C868.Models;
using Plugin.LocalNotifications;
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
    public partial class EditAssessmentPage : ContentPage
    {
        public EditAssessmentPage(Assessment assessment)
        {
            InitializeComponent();

            BindingContext = assessment;
        }

        private async void EditAssessmentSaveButton_Clicked(object sender, EventArgs e)
        {
            // Put form inputs into forms acceptable by the UpdateAssessment method
            int id = App.PlannerRepo.SelectedAssessment;
            string name = editAssessmentNameEntry.Text;
            object type = editAssessmentTypePicker.SelectedItem;
            DateTime start = editAssessmentStartDatePicker.Date;
            DateTime end = editAssessmentEndDatePicker.Date;
            bool notify = editAssessmentNotificationsSwitch.IsToggled == true ? true : false;

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

            bool typeCheck = App.PlannerRepo.EditAssessmentTypeChecker(type);

            if (typeCheck == false)
            {
                await DisplayAlert("Alert", "An assessment of the selected type already exists for this course", "OK");
            }

            if (titleResult == true && typeResult == true && typeCheck == true && dateResult == true)
            {
                // Convert the picker value to a string in preparation for storage
                string typeString = type.ToString();

                // Update the assessment in the database
                App.PlannerRepo.UpdateAssessment(id, name, typeString, start, end, notify);

                // Add 3000 to the AssessmentID for assessment start date notification IDs
                int startID = id + 3000;

                // Add 4000 to the AssessmentID for assessment end date notification IDs
                int endID = id + 4000;

                // Set notifications if enabled
                if (notify == true)
                {
                    CrossLocalNotifications.Current.Show("Assessment Start", $"{name} starts today", startID, start);
                    CrossLocalNotifications.Current.Show("Assessment End", $"{name} ends today", endID, end);
                }

                // Cencel notifications if disabled
                if (notify == false)
                {
                    CrossLocalNotifications.Current.Cancel(startID);
                    CrossLocalNotifications.Current.Cancel(endID);
                }

                // Return to the AssessmentDetail page
                await Navigation.PopAsync();
            }
        }

        private async void OnCancelButton_Clicked(object sender, EventArgs e)
        {
            // Return to the AssessmentDetail page
            await Navigation.PopAsync();
        }
    }
}