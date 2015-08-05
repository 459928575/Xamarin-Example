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
using Android.Graphics.Drawables;

namespace DiscMenu {
    public class CenterView : SurfaceView, ISurfaceHolderCallback, IRunnable {

        private bool IsRunning = false;
        private Java.Lang.Thread Thread = null;
        private Bitmap Center = null;
        private int Degree = 0;

        private static readonly int DEGREE_STEP = 3;

        private int R1 = 0, R2 = 0;

        public CenterView(Context ctx, Bitmap center) : base(ctx) {
            //不加这一句， Draw 的内容在 VS Emulator 里不显示。在 Android 自带的模拟器里正常。
            this.Holder.SetFormat(Format.Transparent);

            this.Holder.AddCallback(this);

            this.Focusable = true;
            this.FocusableInTouchMode = true;

            this.Center = center;

            this.R1 = (int)(15 * this.Context.Resources.DisplayMetrics.Density);
            this.R2 = (int)System.Math.Sqrt(System.Math.Pow(R1, 2) * 2) - 2;
        }

        private void Draw() {
            try {
                using (var canvas = this.Holder.LockCanvas()) {
                    if (canvas != null) {
                        //this.DrawCenter(canvas);
                        this.DrawBitmap(canvas, this.Center);
                        this.DrawRing(canvas);
                    }
                    this.Holder.UnlockCanvasAndPost(canvas);
                }
            } catch {
            }
        }

        private void DrawBitmap(Canvas c, Bitmap bmp) {
            var sc = c.Save();
            //每 100 毫秒转度一个角度
            //if (DateTime.Now.Ticks % 2 == 0) {
            this.Degree = this.Degree % 360 + DEGREE_STEP;
            //}
            //要先旋转，后画图才有效果
            c.Rotate(this.Degree, c.Width / 2, c.Height / 2);

            var paint = new Paint();
            var cx = c.Width / 2;
            var cy = c.Height / 2;

            var path = new Path();
            path.AddCircle(cx, cy, R2, Path.Direction.Cw);
            c.ClipPath(path);
            c.DrawColor(Color.White);

            var rect = new Rect(cx - R1, cy - R1, cx + R1, cy + R1);
            c.DrawBitmap(bmp, null, rect, paint);
            c.RestoreToCount(sc);
        }

        private void DrawRing(Canvas c) {
            var sc = c.Save();

            c.Rotate(-this.Degree, c.Width / 2, c.Height / 2);

            var cx = c.Width / 2;
            var cy = c.Height / 2;

            var pInner = new Path();
            pInner.AddCircle(cx, cy, this.R2 + 5, Path.Direction.Cw);
            var pOut = new Path();
            pOut.AddCircle(cx, cy, this.R2 + 10, Path.Direction.Cw);

            c.ClipPath(pOut);
            c.ClipPath(pInner, Region.Op.Difference);

            //var color = new Color((int)(DateTime.Now.Ticks % 0xFFFFFFFF));
            //c.DrawColor(color);

            var g = new SweepGradient(cx, cy, Color.Green, Color.Transparent);
            var paint = new Paint();
            paint.SetShader(g);
            c.DrawCircle(cx, cy, this.R2 + 10, paint);

            c.RestoreToCount(sc);
        }

        private double ToRadians(float degree) {
            return degree / 180f * System.Math.PI;
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