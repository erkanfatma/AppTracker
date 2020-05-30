using AU.AppTracker.Mobile.Interfaces;
using AU.AppTracker.Mobile.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AU.AppTracker.Mobile.Views {
      [XamlCompilation(XamlCompilationOptions.Compile)]
      public partial class AppUsagePage : ContentPage {
            List<AppProcess> apps = new List<AppProcess>();
            public AppUsagePage() {
                  InitializeComponent();
                  LoadData();
            }

            private void LoadData(string time = "year") {
                  
                  apps = DependencyService.Get<IAppStatistic>().GetAppProcessesByTime(time);
                  lstApps.ItemsSource = apps;
            }

            private void AppSearchBar_TextChanged(object sender, TextChangedEventArgs e) {
                  var searchedApps = apps.Where(x => x.AppName.ToLower().Contains(appSearchBar.Text.ToLower())).ToList();
                  lstApps.ItemsSource = searchedApps;
            }

            private void AtLeastButton_Clicked(object sender, System.EventArgs e) {
                  AtMostButton.IsVisible = true;
                  AtLeastButton.IsVisible = false;
                  lstApps.ItemsSource = apps.OrderBy(x => x.TimeOfAppUsageInSeconds).ToList();
            }

            private void AtMostButton_Clicked(object sender, System.EventArgs e) {
                  AtMostButton.IsVisible = false;
                  AtLeastButton.IsVisible = true;
                  lstApps.ItemsSource = apps.OrderByDescending(x => x.TimeOfAppUsageInSeconds).ToList();
            }

            private void OrderByTimeButton_Clicked(object sender, System.EventArgs e) {
                  string buttonText = (sender as Button).Text;
                  if(buttonText == "TODAY") {
                        LoadData("day");
                  } else if(buttonText == "WEEK") {
                        LoadData("week");
                  } else if(buttonText == "MONTH") {
                        LoadData("month");
                  } else if(buttonText == "YEAR") {
                        LoadData("year");
                  }
            }
      }
}