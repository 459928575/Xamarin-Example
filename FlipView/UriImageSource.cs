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
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using AO = Android.OS;
using System.Net.Http;
using Android.Graphics;

namespace FlipView {
    public class UriImageSource : DiskLRUCache {

        public override string SubDir {
            get {
                return "ImageCache";
            }
        }

        public async Task<ImageView> Get(Context ctx, string url) {
            var stm = await this.GetStream(url);
            var bm = await BitmapFactory.DecodeStreamAsync(stm);
            var img = new ImageView(ctx);
            img.SetImageBitmap(bm);
            return img;
        }

        public async void Attach(ImageView img, string url) {
            var stm = await this.GetStream(url);
            var bm = await BitmapFactory.DecodeStreamAsync(stm);
            img.SetImageBitmap(bm);
        }
    }
}