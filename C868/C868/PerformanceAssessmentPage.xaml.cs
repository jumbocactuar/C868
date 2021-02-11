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
    public partial class PerformanceAssessmentPage : ContentPage
    {
        public PerformanceAssessmentPage(Assessment assessment)
        {
            InitializeComponent();

            BindingContext = assessment;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh the assessment info
            BindingContext = null;
            BindingContext = App.PlannerRepo.GetSelectedAssessment();

            reqsList.ItemsSource = null;
            ObservableCollection<Requirement> reqs = App.PlannerRepo.GetRequirementsList();
            reqsList.ItemsSource = reqs;
        }

        private async void AddReqButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddRequirementPage());
        }

        private async void DeleteAssessmentButton_Clicked(object sender, EventArgs e)
        {
            // Prompt the user to confirm deletion, return to the previous page if confirmed
            bool answer = await DisplayAlert("Delete assessment?", "Are you sure?", "Yes", "Cancel");

            if (answer == true)
            {
                int id = App.PlannerRepo.SelectedAssessment;
                int notifStartID = id + 3000;
                int notifEndID = id + 4000;

                // Delete the assessment and cancel its notifications
                App.PlannerRepo.DeleteAssessment(id);

                CrossLocalNotifications.Current.Cancel(notifStartID);
                CrossLocalNotifications.Current.Cancel(notifEndID);

                await Navigation.PopAsync();
            }
        }

        private async void EditAssessmentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditAssessmentPage(App.PlannerRepo.GetSelectedAssessment()));
        }

        private async void ReqsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (Requirement)e.SelectedItem;
            App.PlannerRepo.SelectedPA = item.ReqID;

            await Navigation.PushAsync(new EditRequirementPage(item));
        }
    }
}