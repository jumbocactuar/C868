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
    public partial class EditCourse : ContentPage
    {
        public EditCourse(Course course)
        {
            InitializeComponent();

            BindingContext = course;
        }

        private async void EditCourseSaveButton_Clicked(object sender, EventArgs e)
        {
            // Put the form inputs into forms acceptable by the UpdateCourse method
            int id = App.PlannerRepo.SelectedCourse;
            string courseName = editCourseNameEntry.Text;
            DateTime start = editCourseStartDatePicker.Date;
            DateTime end = editCourseEndDatePicker.Date;
            bool notify = editCourseNotificationsSwitch.IsToggled == true ? true : false;
            object status = editCourseStatusPicker.SelectedItem;
            string instName = editCourseInstNameEntry.Text;
            string instPhone = editCourseInstPhoneEntry.Text;
            string instEmail = editCourseInstEmailEntry.Text;
            string notes = editCourseNotesEditor.Text;

            // Validate form inputs
            bool titleResult = App.PlannerRepo.EntryChecker(courseName);

            if (titleResult == false)
            {
                await DisplayAlert("Alert", "Course Name cannot be empty", "OK");
            }

            bool dateResult = App.PlannerRepo.DateChecker(start, end);

            if (dateResult == false)
            {
                await DisplayAlert("Alert", "Start must come before End", "OK");
            }

            bool statusResult = App.PlannerRepo.PickerChecker(status);

            if (statusResult == false)
            {
                await DisplayAlert("Alert", "Status cannot be empty", "OK");
            }

            bool instNameResult = App.PlannerRepo.EntryChecker(instName);

            if (instNameResult == false)
            {
                await DisplayAlert("Alert", "Instructor Name cannot be empty", "OK");
            }

            bool instPhoneResult = App.PlannerRepo.PhoneChecker(instPhone);

            if (instPhoneResult == false)
            {
                await DisplayAlert("Alert", "Instructor Phone must be in a valid format", "OK");
            }

            bool instEmailResult = App.PlannerRepo.EmailChecker(instEmail);

            if (instEmailResult == false)
            {
                await DisplayAlert("Alert", "Instructor Email must be in a valid format", "OK");
            }

            if (notes == null)
            {
                notes = "";
            }

            if (titleResult == true && dateResult == true && statusResult == true && instNameResult == true && instPhoneResult == true && instEmailResult == true)
            {
                // Convert the picker value to a string in preparation for storage
                string statusString = status.ToString();

                // Update the Course record in the database
                App.PlannerRepo.UpdateCourse(id, courseName, start, end, notify, statusString, instName, instPhone, instEmail, notes);

                // Add 1000 to the CourseID for course start date notification IDs
                int startID = id + 1000;

                // Add 2000 to the CourseID for course end date notification IDs
                int endID = id + 2000;

                // Set notifications if enabled
                if (notify == true)
                {
                    CrossLocalNotifications.Current.Show("Course Start", $"{courseName} starts today", startID, start);
                    CrossLocalNotifications.Current.Show("Course End", $"{courseName} ends today", endID, end);
                }

                // Cencel notifications if disabled
                if (notify == false)
                {
                    CrossLocalNotifications.Current.Cancel(startID);
                    CrossLocalNotifications.Current.Cancel(endID);
                }

                // Return to the Courses page
                await Navigation.PopAsync();
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}