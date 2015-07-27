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

namespace FlipView {
    public class UriImageSource {

        /// <summary>
        /// �Ƿ񻺴棬Ĭ�Ͽ���
        /// </summary>
        public bool EnableCache { get; set; }

        /// <summary>
        /// ����ʱ��, Ĭ��1��
        /// </summary>
        public TimeSpan CacheValidity { get; set; }


        private readonly static IsolatedStorageFile ISF = null;

        private readonly static string CacheDir = "ImageCache";

        static UriImageSource() {
            ISF = IsolatedStorageFile.GetUserStoreForApplication();
            if (!ISF.DirectoryExists(CacheDir)) {
                ISF.CreateDirectory(CacheDir);
            }
        }

        public UriImageSource() {
            this.CacheValidity = TimeSpan.FromDays(1);
            this.EnableCache = true;
        }

        public async Task<Stream> GetStream(string url) {
            var uri = new Uri(url);
            return await this.GetStream(uri);
        }

        /// <summary>
        /// ��ȡ�ļ���
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<Stream> GetStream(Uri uri) {
            Stream stm = null;
            if (!this.EnableCache) {
                stm = await this.GetStreamAsync(uri).ConfigureAwait(false);
            } else {
                var key = MD5(uri.AbsoluteUri);
                stm = await this.GetStreamFromCache(key).ConfigureAwait(false);
                if (stm == null) {
                    stm = await this.GetStreamAsync(uri).ConfigureAwait(false);
                    if (stm != null) {
                        await this.SaveCache(key, stm);
                    }
                }
            }

            return stm;
        }

        /// <summary>
        /// �������ȡ�ļ���
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private async Task<Stream> GetStreamAsync(System.Uri uri) {
            Stream stream = null;
            using (var client = new HttpClient())
            using (var rep = await client.GetAsync(uri)) {
                //WebClient
                //var bytes = await client.DownloadDataTaskAsync(uri);
                //stream = new MemoryStream(bytes);
                stream = await rep.Content.ReadAsStreamAsync();
            }
            return stream;
        }

        //public async Task<Stream> GetStreamAsync(System.Uri uri, CancellationToken cancellationToken) {
        //    Stream stream;
        //    using (HttpClient httpClient = new HttpClient()) {
        //        using (HttpResponseMessage async = await httpClient.GetAsync(uri, cancellationToken))
        //            stream = await async.Content.ReadAsStreamAsync();
        //    }
        //    return stream;
        //}

        /// <summary>
        /// �ӻ����ȡ�ļ���
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private async Task<Stream> GetStreamFromCache(string key) {
            Stream stm = null;
            var path = Path.Combine(CacheDir, key);
            if (!await this.IsExpire(path)) {
                stm = ISF.OpenFile(path, FileMode.Open, FileAccess.Read);
            }
            return stm;
        }

        /// <summary>
        /// �Ƿ����, ��������ڣ������ڴ���
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Task<bool> IsExpire(string path) {
            //����ļ������ڣ� ֱ���ǹ��ڴ���
            if (!ISF.FileExists(path))
                return Task.FromResult(true);

            var offset = ISF.GetLastWriteTime(path);
            return Task.FromResult(DateTime.Now - offset > this.CacheValidity);
        }

        /// <summary>
        /// ���滺��
        /// </summary>
        /// <param name="key"></param>
        /// <param name="stm"></param>
        /// <returns></returns>
        private Task SaveCache(string key, Stream stm) {
            var path = Path.Combine(CacheDir, key);
            using (var f = ISF.OpenFile(path, FileMode.Create, FileAccess.Write)) {
                return stm.CopyToAsync(f);
            }
        }

        private static string MD5(string input) {
            using (var md5Hasher = System.Security.Cryptography.MD5.Create()) {
                byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++) {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}