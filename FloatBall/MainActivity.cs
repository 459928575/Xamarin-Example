using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace FloatBall {
    [Activity(Label = "FloatBall", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            this.Finish();

            //var intent = new Intent(Application.Context, typeof(BallService));
            var intent = new Intent("Xamarin.BallService");
            this.StartService(intent);

            //Button button = FindViewById<Button>(Resource.Id.MyButton);
            //button.Click += Button_Click;
        }
    }
}

