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
    public class DiskCache {

        private static readonly IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication();

        private string SubDir = null;

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

        private string GetPath(string file) {
            return Path.Combine(this.SubDir, MD5(file));
        }

        public Task<Stream> GetStream(string file) {
            var path = this.GetPath(file);

            if (ISF.FileExists(path)) {
                var stm = ISF.OpenFile(path, FileMode.Open, FileAccess.Read);
                return Task.FromResult<Stream>(stm);
            }
            return Task.FromResult<Stream>(null);
        }

        public async Task WriteStream(string file, Stream stm) {
            var path = this.GetPath(file);

            try {
                using (var fs = ISF.OpenFile(path, FileMode.OpenOrCreate, FileAccess.Write)) {
                    //await stm.CopyToAsync(fs); 如果不指定 buff size , 会报错
                    await stm.CopyToAsync(fs, 16384);
                }
            } catch {

            }
        }

        public DiskCache(string subDir) {
            this.SubDir = subDir;
            if (!ISF.DirectoryExists(subDir)) {
                ISF.CreateDirectory(subDir);
            }
        }
    }
}