# Privacy Policy:

## TUMonline:
Your TUMonline access token and id get stored in the apps [ApplicationData](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata), where by design no other applications have access to. 
All your other information (calendar, grades, lectures, ...) are stored in a local [SQLite-net](https://github.com/praeclarum/sqlite-net) database.
This database is also located in the apps [ApplicationData](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata) folder, where by design no other applications have access to.  
To access your personal information the app uses the requested TUMonline token via the TUMonline WEB-API.
If you don't want to give the app access to your personal information, you can disable this via the token manager in [TUMonline](https://campus.tum.de).

## Crash reporting
After a crash, the App will collect data about what happened and uploads this bundle to [App Center](https://appcenter.ms).  
Although it's not recommended, this can be disabled via `Settings` -> `Misc` -> `Disable crash reporting`.
The data collected this way only gets used for internal bug hunting and statistics. It will not be shared with others.

<details>
<summary>Example report:</summary>
<pre>
Incident Identifier: 9f87a925-2d28-40d1-9612-02b3c8cfc1d7
CrashReporter Key:   t7vgZ+qEyZITMCMsMVzbTvb7V0n6zB7UmdlztKvfoBk=
Hardware Model:      Z270-HD3P
Identifier:      TUM_CAMPUS_APP_UWP_
Version:         2.0.0.0

Date/Time:       2021-07-24T11:46:21.022Z
OS Version:      Windows 10.0.16299.125
Report Version:  104

Exception Type:  System.AggregateException
Crashed Thread:  2

Application Specific Information:
A Task's exception(s) were not observed either by Waiting on the Task or accessing its Exception property. As a result, the unobserved exception was rethrown by the finalizer thread. (Object reference not set to an instance of an object.)

Exception Stack:
unknown location
Data_Manager2.Classes.DBManager.ImageManager.<>c__DisplayClass9_0.<<contiuneAllDownloads>b__0>d.MoveNext()
</pre>
</details>

## Analytics
UWPX uses [AppCenter](https://appcenter.ms) [Analytics](https://docs.microsoft.com/en-us/appcenter/analytics/) to report basic information like session count, duration and which OS version you are running on.
Like Crash Reporting, Analytics can be disabled via `Settings` -> `Misc` -> `Disable analytics`.
The data collected this way only gets used for internal bug hunting and statistics. It will not be shared with others.  
More information: https://docs.microsoft.com/en-us/appcenter/sdk/data-collected
