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
    public partial class SearchResultsPage : ContentPage
    {
        public SearchResultsPage(ObservableCollection<Course> courses)
        {
            InitializeComponent();

            BindingContext = courses;

            courseSearchList.ItemsSource = courses;
        }

        private void CourseSearchList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}