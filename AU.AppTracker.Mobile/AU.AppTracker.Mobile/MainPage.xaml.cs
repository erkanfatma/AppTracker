using AU.AppTracker.Mobile.Interfaces;
using AU.AppTracker.Mobile.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace AU.AppTracker.Mobile {
      [DesignTimeVisible(false)]
      public partial class MainPage : ContentPage {
            List<AppProcess> apps = new List<AppProcess>();
            public MainPage() {
                  InitializeComponent();
                  LoadData();
            }

            private void LoadData() {
                   apps = DependencyService.Get<IAppStatistic>().GetAppProcessesByTime();
                  lstApps.ItemsSource = apps;
            }

            private void AppSearchBar_TextChanged(object sender, TextChangedEventArgs e) {  
                  var searchedApps = apps.Where(x => x.AppName.ToLower().Contains(appSearchBar.Text.ToLower())).ToList();
                  lstApps.ItemsSource = searchedApps;
            }
      }
}
