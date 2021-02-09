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
    public partial class CoursesPage : ContentPage
    {
        public CoursesPage(Term term)
        {
            InitializeComponent();

            BindingContext = term;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh the BindingContext and Term info
            BindingContext = null;
            BindingContext = App.PlannerRepo.GetSelectedTerm();

            courseList.ItemsSource = null;
            ObservableCollection<Course> courses = App.PlannerRepo.GetCoursesList();
            courseList.ItemsSource = courses;

            // Maximum six courses can be added per term
            if (courses.Count >= 6)
            {
                addCourseButton.IsEnabled = false;
            }

            else
            {
                addCourseButton.IsEnabled = true;
            }
        }

        private async void OnAddCourseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCoursePage());
        }

        private async void CourseList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (Course)e.SelectedItem;
            App.PlannerRepo.SelectedCourse = item.CourseID;

            await Navigation.PushAsync(new AssessmentsPage(item));
        }

        private async void EditTermButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditTermPage(App.PlannerRepo.GetSelectedTerm()));
        }

        private async void DeleteTermButton_Clicked(object sender, EventArgs e)
        {
            // Prompt the user to confirm deletion, return to the previous page if confirmed
            bool answer = await DisplayAlert("Delete term?", "Are you sure?", "Yes", "Cancel");

            if (answer == true)
            {
                ObservableCollection<Course> deleteCourses = App.PlannerRepo.GetCoursesList();

                try
                {
                    if (deleteCourses.Count > 0)
                    {
                        throw new TermDeletionException();
                    }

                    else
                    {
                        int id = App.PlannerRepo.SelectedTerm;

                        App.PlannerRepo.DeleteTerm(id);

                        await Navigation.PopAsync();
                    }
                }

                catch (TermDeletionException)
                {
                    await DisplayAlert("Unable to delete", "This term cannot be deleted because it is associated with one or more courses.", "OK");
                }
            }
        }

        private async void GradeReportButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GradeReportPage(App.PlannerRepo.GetSelectedTerm()));
        }
    }
}