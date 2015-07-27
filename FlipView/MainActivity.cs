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


            var imgs = new Dictionary<string, string>(){
                {"http://f.hiphotos.baidu.com/image/w%3D400/sign=cf3db92c0b24ab18e016e03705fbe69a/f703738da97739122e40d96bfa198618367ae252.jpg","彩虹@山"},
                {"http://e.hiphotos.baidu.com/image/w%3D400/sign=8b347e407c3e6709be0044ff0bc69fb8/e7cd7b899e510fb32396f5f0da33c895d0430ccd.jpg","礼物"},
                {"http://e.hiphotos.baidu.com/image/w%3D400/sign=9915223123a446237ecaa462a8237246/11385343fbf2b2116724dd05c98065380cd78e77.jpg","小鸟"}
            };

            var items = imgs.Select(i => {
                var img = new ImageView(this);
                //var client = new WebClient();
                //client.DownloadDataCompleted += Client_DownloadDataCompleted;
                //client.DownloadDataAsync(new Uri(i.Key), img);

                var source = new UriImageSource();
                var stm = source.GetStream(i.Key).Result;
                var bm = BitmapFactory.DecodeStreamAsync(stm).Result;
                img.SetImageBitmap(bm);
                return img;
            });

            var vp = new ViewPager(this);
            vp.Adapter = new FlipViewAdapter(items);
            this.AddContentView(vp, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 300));
        }

        //private void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {
        //    var img = e.UserState as ImageView;

        //    img.SetImageBitmap(BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length, new BitmapFactory.Options() {
        //        InSampleSize = 4
        //    }));
        //}
    }
}

