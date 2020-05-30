using Android.App;
using Android.App.Usage;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.Net;
using Android.Provider;
using Android.Widget;
using AU.AppTracker.Mobile.Droid.Dependencies;
using AU.AppTracker.Mobile.Interfaces;
using AU.AppTracker.Mobile.Models.ViewModels;
using Java.Util;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppsDependency))]
namespace AU.AppTracker.Mobile.Droid.Dependencies {
      public class AppsDependency : IAppStatistic {
            List<AppProcess> appProcesses = new List<AppProcess>();
            private static Context context = Android.App.Application.Context;
            public static int MY_PERMISSIONS_REQUEST_PACKAGE_USAGE_STATS = 1;

            [Obsolete]
            public List<AppProcess> GetAppProcessesByTime(string time = "year") {
                  try {

                        #region AppStatistics 
                        PackageManager packageManager = context.PackageManager;
                        AppOpsManager appOps = (AppOpsManager)context.GetSystemService(Context.AppOpsService);

                        var mode = appOps.CheckOpNoThrow(AppOpsManager.OpstrGetUsageStats, Android.OS.Process.MyUid(), context.PackageName);

                        if(mode == AppOpsManager.ModeAllowed) {
                              UsageStatsManager mUsageStatsManager = (UsageStatsManager)context.GetSystemService(Context.UsageStatsService);

                              NetworkStatsManager networkStatsManager = (NetworkStatsManager)context.GetSystemService(Context.NetworkStatsService);

                              Calendar calendar = Calendar.Instance;
                              long endMillis = calendar.TimeInMillis;
                              long startMillis;
                              if(time == "day") {
                                    calendar.Add(Calendar.DayOfWeekInMonth, -1);
                              } else if(time == "month") {
                                    calendar.Add(Calendar.Month, -1);
                              } else if(time == "week") {
                                    calendar.Add(Calendar.WeekOfMonth, -1);
                              } else {
                                    calendar.Add(Calendar.Year, -1);
                              }
                              startMillis = calendar.TimeInMillis;

                              IDictionary<String, UsageStats> lUsageStatsMap = mUsageStatsManager.QueryAndAggregateUsageStats(startMillis, endMillis);

                              foreach(var usageStats in lUsageStatsMap) {
                                    long totalTimeInMillis = 0;
                                    long totalTimeInSeconds = 0;
                                    string packageName = "";
                                    string appName = "";
                                    long receivedWifi = 0;
                                    long sentWifi = 0;
                                    long receivedMobile = 0;
                                    long sentMobile = 0;
                                    DateTime lastUsage = new DateTime();

                                    packageName = usageStats.Key;
                                    totalTimeInMillis = usageStats.Value.TotalTimeInForeground;
                                    totalTimeInSeconds = totalTimeInMillis / 1000 / 60;
                                    appName = AppNameByPackageName(packageName);

                                    ApplicationInfo info = packageManager.GetApplicationInfo(packageName, 0);
                                    if(info != null) {
                                          int uid = info.Uid;
                                          NetworkStats networkStatsWifi = networkStatsManager.QueryDetailsForUid(ConnectivityType.Wifi, null, startMillis, endMillis, uid);

                                          NetworkStats.Bucket bucketWifi = new NetworkStats.Bucket();
                                          while(networkStatsWifi.HasNextBucket) {
                                                networkStatsWifi.GetNextBucket(bucketWifi);
                                                receivedWifi += bucketWifi.RxBytes;
                                                sentWifi += bucketWifi.TxBytes;
                                          }
                                          NetworkStats networkStatsMobile = networkStatsManager.QueryDetailsForUid(ConnectivityType.Mobile, null, startMillis, endMillis, uid);
                                          NetworkStats.Bucket bucketMobile = new NetworkStats.Bucket();
                                          while(networkStatsMobile.HasNextBucket) {
                                                networkStatsMobile.GetNextBucket(bucketMobile);
                                                receivedMobile += bucketMobile.RxBytes;
                                                sentMobile += bucketMobile.TxBytes;
                                          }
                                    }

                                    appProcesses.Add(new AppProcess { PackageName = packageName, TimeOfAppUsageInSeconds = totalTimeInSeconds, AppName = appName, NetworkUsageTotal = receivedWifi + sentWifi + receivedMobile + sentMobile, NetworkUsageSend = sentWifi + sentMobile, NetworkUsageReceived = receivedWifi + receivedMobile });
                              }
                        } else {
                              // When needed permission
                              var activity = (MainActivity)Forms.Context;
                              activity.StartActivityForResult(new Intent(Settings.ActionUsageAccessSettings), MY_PERMISSIONS_REQUEST_PACKAGE_USAGE_STATS);
                        }
                  } catch(Exception ex) {
                        throw new Exception(ex.Message);
                  }
                  #endregion  
                  return appProcesses;
            }

            public string AppNameByPackageName(string packageName) {
                  string appName = "";
                  PackageManager pm = Android.App.Application.Context.PackageManager;
                  ApplicationInfo ai;
                  try {
                        ai = pm.GetApplicationInfo(packageName, PackageInfoFlags.MetaData);
                        if(ai != null)
                              appName = pm.GetApplicationLabel(ai);
                  } catch(Exception ex) {
                        appName = "Unknown";
                  }
                  return appName;
            }

            public List<AppProcess> InstalledApps() {
                  List<AppProcess> result = new List<AppProcess>();

                  #region InstalledAppList
                  var apps = Android.App.Application.Context.PackageManager.GetInstalledApplications(PackageInfoFlags.MatchAll);

                  foreach(var item in apps) {
                        string appname = item.LoadLabel(Android.App.Application.Context.PackageManager);
                        string packageName = item.PackageName;
                        Drawable appIcon = item.LoadIcon(Android.App.Application.Context.PackageManager);
                        result.Add(new AppProcess { AppName = appname, PackageName = packageName, Icon = appIcon.ToString() });
                  }
                  #endregion

                  return result;
            }

      }
}