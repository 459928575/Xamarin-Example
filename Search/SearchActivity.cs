using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Search {

    //http://developer.android.com/guide/topics/search/search-dialog.html
    //http://developer.xamarin.com/guides/android/advanced_topics/working_with_androidmanifest.xml/

    /// <summary>
    /// 
    /// </summary>
    [MetaData("android.app.searchable", Resource = "@xml/searchable")]
    [IntentFilter(new[] { Intent.ActionSearch })]
    [Activity(Label = "SearchActivity", Name = "searchExample.SearchA", LaunchMode = Android.Content.PM.LaunchMode.SingleTop)] // searchExample.SearchA 中的， searchExample 为包名， 必须为小写字母开头
    public class SearchActivity : Activity {

        private TextView T1 = null;
        private TextView T2 = null;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            var layout = new LinearLayout(this) {
                Orientation = Orientation.Vertical
            };
            this.AddContentView(layout, new ViewGroup.LayoutParams(-1, -1));

            this.T1 = new TextView(this);
            this.T2 = new TextView(this) {
                Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            };

            layout.AddView(this.T1);
            layout.AddView(this.T2);

            var btn = new Button(this) {
                Text = "Search"
            };

            btn.Click += Btn_Click;
            layout.AddView(btn);

            this.HandleIntent(this.Intent);
        }

        private void Btn_Click(object sender, EventArgs e) {
            this.OnSearchRequested();
        }

        protected override void OnNewIntent(Intent intent) {
            base.OnNewIntent(intent);

            this.HandleIntent(intent);
        }

        private void HandleIntent(Intent intent) {
            if (intent.Action.Equals(Intent.ActionSearch)) {
                var query = intent.GetStringExtra(SearchManager.Query);
                Toast.MakeText(this, query, ToastLength.Short).Show();

                if (this.T1 != null) {
                    this.T1.Text = query;
                }
            }
        }
    }
}