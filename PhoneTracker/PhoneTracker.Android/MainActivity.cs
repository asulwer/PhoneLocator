using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Android.Content;
using System.Threading;

namespace PhoneTracker.Droid
{
	[Activity (Label = "PhoneTracker", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

            MessagingCenter.Subscribe<MainPage>(this, "Start", (sender) => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<MainPage>(this, "Stop", (sender) => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StopService(intent);
            });
            
            global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new PhoneTracker.App ());
		}
    }

    [Service]
    public class LongRunningTaskService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (CrossGeolocator.Current.IsListening)
                return StartCommandResult.Sticky;

            CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true, new Plugin.Geolocator.Abstractions.ListenerSettings
            {
                ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = true,
                DeferralDistanceMeters = 1,
                DeferralTime = TimeSpan.FromSeconds(1),
                ListenForSignificantChanges = true,
                PauseLocationUpdatesAutomatically = false
            }).GetAwaiter();

            CrossGeolocator.Current.PositionChanged += (object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e) => {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Data d = new Data()
                    {
                        Position = e.Position
                    };

                    MessagingCenter.Send<Data>(d, "Position");
                });
            };

            return StartCommandResult.Sticky;
        }
    }
}

