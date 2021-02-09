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
using Xamarin.Essentials;

namespace C868
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentsPage : ContentPage
    {
        public AssessmentsPage(Course course)
        {
            InitializeComponent();

            BindingContext = course;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh the binding context and assessments list
            BindingContext = null;
            BindingContext = App.PlannerRepo.GetSelectedCourse();

            assessmentList.ItemsSource = null;
            ObservableCollection<Assessment> assessments = App.PlannerRepo.GetAssessmentsList();
            assessmentList.ItemsSource = assessments;

            // Maximum two assessments per course
            if (assessments.Count >= 2)
            {
                addAssessmentButton.IsEnabled = false;
            }

            else
            {
                addAssessmentButton.IsEnabled = true;
            }
        }

        private async void AddAssessmentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAssessmentPage());
        }

        private async void AssessmentList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (Assessment)e.SelectedItem;
            App.PlannerRepo.SelectedAssessment = item.AssessmentID;

            await Navigation.PushAsync(new AssessmentDetailPage(item));
        }

        private async void EditCourseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditCourse(App.PlannerRepo.GetSelectedCourse()));
        }

        private async void ShareNotesButton_Clicked(object sender, EventArgs e)
        {
            var course = App.PlannerRepo.GetSelectedCourse();

            string notes = course.Notes;

            // If notes is null  or empty, add placeholder text so the share request has something to send
            if (notes == null || notes == "")
            {
                notes = "none";
            }

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = notes,
                Title = "Share Notes"
            });
        }

        private async void DeleteCourseButton_Clicked(object sender, EventArgs e)
        {
            // Prompt the user to confirm deletion, return to the previous page if confirmed
            bool answer = await DisplayAlert("Delete course?", "Are you sure?", "Yes", "Cancel");

            if (answer == true)
            {
                ObservableCollection<Assessment> deleteAssessments = App.PlannerRepo.GetAssessmentsList();

                try
                {
                    if (deleteAssessments.Count > 0)
                    {
                        throw new CourseDeletionException();
                    }

                    else
                    {
                        int id = App.PlannerRepo.SelectedCourse;
                        int notifStartID = id + 1000;
                        int notifEndID = id + 2000;

                        // Delete the course and cancel its notifications
                        App.PlannerRepo.DeleteCourse(id);

                        CrossLocalNotifications.Current.Cancel(notifStartID);
                        CrossLocalNotifications.Current.Cancel(notifEndID);

                        await Navigation.PopAsync();
                    }
                }

                catch (CourseDeletionException)
                {
                    await DisplayAlert("Unable to delete", "This course cannot be deleted because it is associated with one or more assessments.", "OK");
                }
            }
        }
    }
}