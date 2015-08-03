using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Graphics;

namespace DiscMenu {
    [Activity(Label = "DiscMenu", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            var discView = new DiscMenu(this);
            this.AddContentView(discView, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));

            var btn = new Button(this) {
                Text = "aaaa"
            };

            discView.AddView(btn, 200, 100);
        }
    }
}

