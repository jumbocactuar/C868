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
    public partial class AddCoursePage : ContentPage
    {
        public AddCoursePage()
        {
            InitializeComponent();
        }

        private async void OnSaveButton_Clicked(object sender, EventArgs e)
        {
            // Put the form inputs into forms acceptable by the AddCourse method
            int termID = App.PlannerRepo.SelectedTerm;
            string courseName = addCourseNameEntry.Text;
            DateTime start = addCourseStartDatePicker.Date;
            DateTime end = addCourseEndDatePicker.Date;
            bool notify = addCourseNotificationsSwitch.IsToggled == true ? true : false;
            object status = addCourseStatusPicker.SelectedItem;
            string instName = addCourseInstNameEntry.Text;
            string instPhone = addCourseInstPhoneEntry.Text;
            string instEmail = addCourseInstEmailEntry.Text;
            string notes = addCourseNotesEditor.Text;

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

                // Add the Course to the database
                App.PlannerRepo.AddCourse(termID, courseName, start, end, notify, statusString, instName, instPhone, instEmail, notes);

                // Set notifications if enabled
                if (notify == true)
                {
                    // Get the new list of courses
                    ObservableCollection<Course> courses = App.PlannerRepo.GetCoursesList();

                    // Get the ID of the assessment added above
                    int CourseID = courses.Last<Course>().CourseID;

                    // Add 1000 to the CourseID for course start date notification IDs
                    int startID = CourseID + 1000;

                    // Add 2000 to the CourseID for course end date notification IDs
                    int endID = CourseID + 2000;

                    CrossLocalNotifications.Current.Show("Course Start", $"{courseName} starts today", startID, start);
                    CrossLocalNotifications.Current.Show("Course End", $"{courseName} ends today", endID, end);
                }

                // Return to the Courses page
                await Navigation.PopAsync();
            }
        }

        private async void OnCancelButton_Clicked(object sender, EventArgs e)
        {
            // Return to the Courses page
            await Navigation.PopAsync();
        }
    }
}