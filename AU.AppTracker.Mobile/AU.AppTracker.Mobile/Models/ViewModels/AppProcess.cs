using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Linq;
using Xamarin.Forms;

namespace AU.AppTracker.Mobile.Models.ViewModels {
      public class AppProcess {
            public string AppName { get; set; }
            public string Icon { get; set; }
            public long TimeOfAppUsageInSeconds { get; set; }
            public long NetworkUsageReceived { get; set; }
            public long NetworkUsageTotal { get; set; }
            public long NetworkUsageSend { get; set; } 
            public string PackageName { get; set; }

            public string TimeAppUsageString {
                  get {
                        return TimeOfAppUsageInSeconds.ToString() + " mins";
                  }
            }
            public string NetworkAppUsageString {
                  get {
                        double gb = NetworkUsageTotal / 1048576D;
                        return String.Format("{0:0.##} {1}", gb, " GB");
                        //return gb.ToString() + " GB";
                  }
            }

      }
}
