using C868.Models;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace C868
{
    public partial class TermsPage : ContentPage
    {
        public TermsPage()
        {
            InitializeComponent();

            // If no data exists, add evaluation data
            ObservableCollection<Term> initialList = App.PlannerRepo.GetTermsList();

            if (initialList.Count == 0)
            {
                App.PlannerRepo.AddUser("admin", "test");
                App.PlannerRepo.AddTerm("Term 1 - Winter 2021", new DateTime(2021, 1, 1, 0, 0, 0), new DateTime(2021, 6, 30, 0, 0, 0));
                App.PlannerRepo.AddCourse(1, "C971 - Mobile Applications", new DateTime(2021, 1, 1, 0, 0, 0), new DateTime(2021, 2, 15, 0, 0, 0), false, "In progress", "Cody Burkholz", "206-353-8514", "cburk89@wgu.edu", "Study at least one chapter per day");
                App.PlannerRepo.AddAssessment(1, "C971 OA", "Objective", new DateTime(2021, 2, 1, 0, 0, 0), new DateTime(2021, 2, 7, 0, 0, 0), false);
                App.PlannerRepo.AddAssessment(1, "C971 PA", "Performance", new DateTime(2021, 2, 8, 0, 0, 0), new DateTime(2021, 2, 15, 0, 0, 0), false);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            termsList.ItemsSource = null;
            ObservableCollection<Term> terms = App.PlannerRepo.GetTermsList();
            termsList.ItemsSource = terms;

            // Display pending notifications
            ObservableCollection<Course> courses = App.PlannerRepo.GetAllCourses();
            ObservableCollection<Assessment> assessments = App.PlannerRepo.GetAllAssessments();

            foreach (Course course in courses)
            {
                if (course.Start == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Course Start", $"{course.CourseName} starts today");
                }

                if (course.End == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Course End", $"{course.CourseName} ends today");
                }
            }

            foreach (Assessment assessment in assessments)
            {
                if (assessment.Start == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Assessment Start", $"{assessment.Name} starts today");
                }

                if (assessment.End == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Assessment End", $"{assessment.Name} ends today");
                }
            }
        }

        private async void OnAddTermButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTermPage());
        }

        private async void TermsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (Term)e.SelectedItem;
            App.PlannerRepo.SelectedTerm = item.TermID;

            await Navigation.PushAsync(new CoursesPage(item));
        }
    }
}
