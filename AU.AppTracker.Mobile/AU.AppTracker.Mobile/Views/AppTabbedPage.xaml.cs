using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AU.AppTracker.Mobile.Views {
      [XamlCompilation(XamlCompilationOptions.Compile)]
      public partial class AppTabbedPage : TabbedPage {
            public AppTabbedPage() {
                  InitializeComponent();
                  this.BarBackgroundColor = Color.FromHex("#584153");
                  this.BarTextColor = Color.FromHex("#faf2f2");
            }
      }
}