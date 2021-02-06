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
    public partial class AssessmentDetailPage : ContentPage
    {
        public AssessmentDetailPage(Assessment assessment)
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
        }

        private async void EditAssessmentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditAssessmentPage(App.PlannerRepo.GetSelectedAssessment()));
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
    }
}