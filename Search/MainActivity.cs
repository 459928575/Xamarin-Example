using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Search {

    [MetaData("android.app.default_searchable", Value = "searchExample.SearchA")]
    [Activity(Label = "Search", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            var layout = new LinearLayout(this);
            this.AddContentView(layout, new ViewGroup.LayoutParams(-1, -1));

            var btn = new Button(this) {
                Text = "Search"
            };
            layout.AddView(btn);
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, EventArgs e) {
            this.OnSearchRequested();
        }
    }
}

