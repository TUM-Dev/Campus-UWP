# Privacy Policy:
## TUMonline:
Your TUMonline access token and id get stored in the apps [ApplicationData](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata), where by design no other applications have access to.<br>
All your other information (calendar, grades, lectures, ...) are stored in a local [SQLite-net](https://github.com/praeclarum/sqlite-net) database.
This database is also located in the apps [ApplicationData](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata) folder, where by design no other applications have access to.<br>
To access your personal information the app uses the requested TUMonline token via the TUMonline WEB-API.
If you don't want to give the app access to your personal information, you can disable this via the token manager in [TUMonline](https://campus.tum.de).

## Crash reporting:
After a crash, the app will collect data about what happened and uploads this bundle to [App Center](https://appcenter.ms) or for versions lower than [1.7.0.0](https://github.com/COM8/UWP-TUM-Campus-App/releases/tag/1.7.0.0) to [HockeyApp](https://hockeyapp.net/features/crashreports/).<br>
Althoug it's not recommended, it can be disabled via `Settings` -> `Disable Crash Reports`.
Once disabled you might have to restart the app.

## GPS:
Your position only gets used to calculate the distance between you and the next canteen.
It's not necessary, but it's recommend, that you give the app access to your current position.
