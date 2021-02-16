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

            BindingContext = App.PlannerRepo.CurrentUser;
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

        private async void AddUserButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddUserPage());
        }

        private async void ChangePasswordButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }

        private async void OnAddTermButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTermPage());
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            if (courseSearchEntry.Text == null || courseSearchEntry.Text == "")
            {
                await DisplayAlert("Alert", "Please enter a search term.", "OK");
            }

            else
            {
                ObservableCollection<Course> courseList = App.PlannerRepo.GenerateSearchResults(courseSearchEntry.Text);

                if (courseList.Count > 0)
                {
                    await Navigation.PushAsync(new SearchResultsPage(courseList));
                }

                else
                {
                    await DisplayAlert("Alert", "No results found.", "OK");
                }
            }

        }

        private async void TermsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (Term)e.SelectedItem;
            App.PlannerRepo.SelectedTerm = item.TermID;

            await Navigation.PushAsync(new CoursesPage(item));
        }
    }
}
