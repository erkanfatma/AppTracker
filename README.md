# AppTracker

## Overview
Applications’ Control project, which is named “atracker”, is designed for measuring privacy risks in mobile apps on the Android operating system.

## Project Description
Privacy risks are related to applications that are installed on mobile phones and the Internet. This experiment analyzes which apps are used most and its network usage for Android devices. The experiment was tested with 3 different Android phones. All the applications in the phones accessed personal information in different ways. The main aim of the project is to control third-party applications on the Android operating system. Many third-party applications are trackers that include potentially privacy risks because there is no information about how they collect and store user data.

Application controls usage information of each app, how long it is used, and its network data usage in the device. Android provides developers to control the user’s usage of network resources. In the atracker application, it is been controlling how much data used when on Wifi and Mobile. With this approach, it is possible to report which application use the internet most depend on time. For instance, some applications can run on the background and send data to the application without any limitation. Atracker control how much data is used by different applications.

On the other hand, atracker application controls how long they are used. Android also gives an opportunity to that how long an application is used on the mobile phone. With using this issue, atracker is known that how much time a user spends in any application. A user can use any application without knowing how the application uses its data via a mobile application. Atracker determines whether an application is a third-party app or not.

## Similar Works
### App Usage Tracker
App Usage Tracker is an application for the Android operating system. The application tracks what apps are getting used on the device. It does not require the internet permission. It monitors which apps are used often. It cleans older usage data regularly. However, it tracks app usage when only the application working in the background. In addition to this, the application shows usage statistics after installation that means for the first launch of the application the usage report will be empty. Uninstalled apps do not be shown in the usage report.
### System Panel App
System Panel App was created to solve the battery life problem that enabling the monitoring apps that run on background. It also monitors which versions of the application that the user uses. On the other hand, it requires ‘Internet Access’ and ‘Telephony/Read phone State’ permissions. In addition to this, the application is not free. To use this app, you must pay 16.99 TL.
### Instant
The application named ‘Instant’ track phone and app uses both for Android and iOS devices. It includes phone usage time for days instead of looking at your time in individual apps. It requires a premium subscription and cost is $2 per month.

## Requirements
To build this application, there must be a platform for mobile application development. It is planned to use Microsoft’s Xamarin technology which is in Visual Studio.

Visual Studio 2019 will be used in this project. Microsoft published Visual Studio 2019 in April. However, there is not enough documentation for the 2019 edition. Microsoft Visual Studio is an IDE. It is used to develop websites, web apps, web services, and mobile applications. Visual Studio also supports different programming languages and allows developers to debug easily and quickly.

Xamarin is a platform developed by Miguel de Icaza, Nat Friedman, Joseph Hill. It provides to build IOS, Android, and Universal Windows Platform at the same time. It is possible to develop a native application. It is not necessary to learn Objective-C, Swift, or Java. Xamarin will be used with Entity Framework Core to avoid writing one code many times.

## Work Done
How to build a mobile app for the Android operating system to extract information of usage for all applications from the point of how long the application is used and application’s network data usage in the smartphone.

Xamarin Android includes system services to access device usage history and statistics about applications. To extract device history and how much data has been used by an application are available through system services.

Getting history applications on the smartphone requires to define permission of“ Package_Usage_Stats”. To add this requirement to the code manifest file is used as shown below.
```cs
<uses-permission android:name="android.permission.PACKAGE_USAGE_STATS" tools:ignore="ProtectedPermissions"/>
```

To check if the application has permission:
```cs
AppOpsManager appops = (AppOpsManager)context.GetSystemService(context.UsageStatsService);
var mode = AppOps.CheckOpNoThrow(AppOpsManager.OpstrGetUsageStats, Android.OS.Process.MyUid(), context.PackageName);
if(mode == AppOpsManager.ModeAllowed){
//Code here
}
```
This code statement invoke access screen to permission for the application.

- App Usage for Time
UsageStatsManager is a service that provides access to device statistics of applications. These statistics can be got depending on the day, week, month, and year.
```cs
UsageStatsManager mUsageStatsManager = (UsageStatsManager)context.GetSystemService(context.UsageStatsService);
IDictionary<String, UsageStats> lUsageStatsMap = mUsageStatsManager.QueryAndAggregateUsageStats(startMillis, endMillis);
```
‘QueryAndAggregateUsageStats’ is the function to query stats for the range of time as shown in the figure they are called as ‘startMillis’ and ‘endMillis’. Using IDictionary, map the package name, and UsageStats.

Getting usage time for an application with the package name, TotalTimeInForeground property is used.
```cs
foreach(var usageStats in lUsageStatsMap){
  packageName = usageStats.Key;
  totalTimeInMillis = UsageStats.Value.TotalTimeInForeground;
}
```

To compute start time and end time Calendar library is used as shown below.
```cs
Calendar calendar = Calendar.Instance;
long endMillis = calendar.TimeInMillis;
long startMillis;
startMillis = calendar.Add(Calendar.Year, -1);
```

- Network Usage for Time
System service includes ‘NetworkStatsManager’ to access network usage statistics and it is defined as below.
```cs
NetworkStatsManager networkStatsManager = (NetworkStatsManager)context.GetSystemService(Context.NetworkStatsService);
```

To query NetworkStats, some different methods placed. Here, Uid is used to query it and the Connectivitytype, which is known as network type, is selected for Wi-Fi and Mobile. The figure below shows how to use it with Wi-Fi.
```cs
PackageManager packageManager = Android.App.Application.Context.PackageManager;

ApplicationInfo info = packageManager.GetApplicationInfo(packageName, 0);
if(info != null){
  int uid = info.Uid;
  NetworkStats networkStatsWifi = networkStatsManager.QueryDetailsForUid(ConnectivityType.Wifi, null, startMillis, endMillis, uid);
}
```

Usage data collected of time named ‘Buckets’. This means buckets are iterated to get numbers of bytes received and transmitted for the interval as coded below.
```cs
NetworkStats.Bucket bucketWifi = new NetworkStats.Bucket();
while(networkStatsWifi.HasNextBucket){
  networkStatsWifi.GetNextBucket(bucketWifi);
  receivedWifi += bucketWifi.RxBytes;
  sentWifi += bucketWifi.TxBytes;
}
```
The sum of the receivedWifi and sentWifi gives the total data used by the app for a given time interval using Wi-Fi.

To reach data usage by Mobile, ConnectivityType is selected as Mobile.
```cs
NetworkStats networkStatsMobile = networkStatsManager.QueryDetailsForUid(ConnectivityType.Mobile, null, startMillis, endMillis, uid);
```




