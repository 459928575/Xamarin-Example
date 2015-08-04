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
            this.AddContentView(discView, new ViewGroup.LayoutParams(400, 400));

            var btn1 = new Button(this) {
                Text = "aaaa"
            };

            var btn2 = new Button(this) {
                Text = "bbbb"
            };

            var btn3 = new Button(this) {
                Text = "bbbb"
            };

            var btn4 = new Button(this) {
                Text = "bbbb"
            };

            var btn5 = new Button(this) {
                Text = "bbbb"
            };

            var btn6 = new Button(this) {
                Text = "bbbb"
            };

            discView.AddView(btn1);
            discView.AddView(btn2);
            discView.AddView(btn3);
            discView.AddView(btn4);
            discView.AddView(btn5);
            discView.AddView(btn6);
        }
    }
}

