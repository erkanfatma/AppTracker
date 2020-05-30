using AU.AppTracker.Mobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AU.AppTracker.Mobile {
      public partial class App : Application {
            public App() {
                  InitializeComponent();

                  MainPage = new AppTabbedPage();
            }

            protected override void OnStart() {
            }

            protected override void OnSleep() {
            }

            protected override void OnResume() {
            }
      }
}
