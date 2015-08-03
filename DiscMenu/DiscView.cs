using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Threading.Tasks;
using System.Threading;

namespace DiscMenu {
    public class DiscView : SurfaceView, ISurfaceHolderCallback, IRunnable {

        private bool IsRunning = false;
        private Java.Lang.Thread Thread = null;


        public DiscView(Context ctx) : base(ctx) {
            //不加这一句， Draw 的内容在 VS Emulator 里不显示。在 Android 自带的模拟器里正常。
            this.Holder.SetFormat(Format.Transparent);

            this.Holder.AddCallback(this);

            this.Focusable = true;
            this.FocusableInTouchMode = true;
        }

        private void Draw() {
            try {
                using (var canvas = this.Holder.LockCanvas()) {
                    if (canvas != null) {
                        this.DrawCenter(canvas);
                    }
                    this.Holder.UnlockCanvasAndPost(canvas);
                }
            } catch {
            }
        }

        private void DrawCenter(Canvas c) {
            var x = c.Width / 2;
            var y = c.Height / 2;
            var paint = new Paint() {
                Color = Color.White,
                AntiAlias = true
            };

            //线性渐变
            //var g = new LinearGradient(x - 30, y - 30, x + 30, y + 30, Color.Red, Color.Blue, Shader.TileMode.Mirror);

            // 角度渐变
            // 第一个,第二个参数表示渐变圆中心坐标
            //var g = new SweepGradient(x, y, Color.Red, Color.Blue);

            //环形渐变
            //第一个,第二个参数表示渐变圆中心坐标,第三个参数表示半径
            var g = new RadialGradient(x, y, 60, Color.Red, Color.Purple, Shader.TileMode.Mirror);
            paint.SetShader(g);


            c.DrawCircle(x, y, 60, paint);
            paint.Color = Color.Green;
            c.DrawCircle(x, y, 50, paint);
        }


        #region IRunnable
        public void Run() {
            while (this.IsRunning) {
                this.Draw();
            }
        }
        #endregion

        #region ISurfaceHolderCallback
        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height) {

        }

        public void SurfaceCreated(ISurfaceHolder holder) {
            this.IsRunning = true;
            this.Thread = new Java.Lang.Thread(this);
            this.Thread.Start();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder) {
            this.IsRunning = false;
        }

        #endregion

    }
}