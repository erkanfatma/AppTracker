using AU.AppTracker.Mobile.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AU.AppTracker.Mobile.Interfaces {
      public interface IAppStatistic {
            List<AppProcess> GetAppProcessesByTime(string time = "year");
      }
}
