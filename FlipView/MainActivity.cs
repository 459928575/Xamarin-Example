using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Android.Graphics;
using Java.Net;
using System.Threading.Tasks;

namespace FlipView {
    [Activity(Label = "FlipView", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {
        //int count = 1;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            //var imgs = new List<string>() {
            //    "http://photocdn.sohu.com/20150416/mp11097112_1429164315587_2.png",
            //    "http://s1.sinaimg.cn/mw690/0024SuDBgy6RoZf6lRqe4&690",
            //    //"http://att.x2.hiapk.com/forum/201204/03/1243289i9kxtoxihioootz.jpg",
            //    "http://b.hiphotos.baidu.com/image/pic/item/c8ea15ce36d3d539ff0934ce3987e950352ab082.jpg"
            //};

            var imgs = new List<string>() { "A", "B", "C" };

            var items = imgs.Select(i => {
                //var img = new ImageView(this);
                //var client = new WebClient();
                //client.DownloadDataCompleted += Client_DownloadDataCompleted;
                //client.DownloadDataAsync(new Uri(i), img);
                //return img;
                var txt = new TextView(this);
                txt.Text = i;
                return txt;
            });

            var vp = new ViewPager(this);
            vp.Adapter = new FlipViewAdapter(items);
            this.AddContentView(vp, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 300));
        }

        private void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {
            var img = e.UserState as ImageView;

            img.SetImageBitmap(BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length, new BitmapFactory.Options() {
                InSampleSize = 4
            }));
        }
    }
}

