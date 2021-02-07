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
    public partial class GradeReportPage : ContentPage
    {
        public GradeReportPage(Term term)
        {
            InitializeComponent();

            BindingContext = term;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            reportDateLabel.Text = $"Report Date: {DateTime.Now}";

            reportList.ItemsSource = null;
            ObservableCollection<Course> courses = App.PlannerRepo.GetCoursesList();
            reportList.ItemsSource = courses;
        }

        private async void ReportExitButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}