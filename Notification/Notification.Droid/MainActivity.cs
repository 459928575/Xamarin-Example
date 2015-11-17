using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Notification.Droid.Services;
using CN.Jpush.Android.Api;
using Xamarin.Forms.Platform.Android;
using Caliburn.Micro;

namespace Notification.Droid {

    [Activity(Label = "Notification", Theme = "@style/MyTheme", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            LoadApplication(new App(IoC.Get<SimpleContainer>()));

            JPushInterface.SetDebugMode(true);
            JPushInterface.Init(this);
        }

        protected override void OnResume() {
            base.OnResume();

            JPushInterface.OnResume(this);
        }

        protected override void OnPause() {
            base.OnPause();

            JPushInterface.OnPause(this);
        }
    }
}

