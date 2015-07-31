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
using Android.Graphics.Drawables;

namespace FlipView {
    public class UriImageSource {

        private string SubDir {
            get {
                return "ImageCache";
            }
        }

        private DiskLRUCache Cache = null;

        public bool EnableCache { get; set; }

        public TimeSpan CacheTime { get; set; }

        public UriImageSource() {
            this.EnableCache = true;
            this.CacheTime = TimeSpan.FromDays(1);
            this.Cache = new DiskLRUCache(this.SubDir);
        }

        public async Task<Drawable> GetDrable(string url) {
            var stm = await this.GetStream(url);
            return new BitmapDrawable(stm);
        }

        public async Task<Bitmap> GetBitmap(string url) {
            var stm = await this.GetStream(url);
            return await BitmapFactory.DecodeStreamAsync(stm);
        }

        private async Task<Stream> GetStream(string url) {
            Stream stm = null;

            if (this.EnableCache) {
                stm = await this.GetStreamFromCache(url);
            }

            if (!this.EnableCache || stm == null || stm.Length == 0) {
                stm = await this.GetStreamFromWeb(url);

                if (this.EnableCache && stm != null && stm.Length > 0) {
                    await this.SaveToCache(url, stm);
                }
            }

            return stm;
        }

        private async Task<Stream> GetStreamFromWeb(string url) {
            Stream stream = null;
            using (var client = new HttpClient())
            using (var rep = await client.GetAsync(url)) {
                stream = await rep.Content.ReadAsStreamAsync();
            }
            return stream;
        }

        private async Task<Stream> GetStreamFromCache(string url) {
            return await this.Cache.GetStream(url);
        }

        private async Task SaveToCache(string url, Stream stm) {
            await this.Cache.WriteStream(url, stm);
        }
    }
}