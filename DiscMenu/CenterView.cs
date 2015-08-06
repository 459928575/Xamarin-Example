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

        /// <summary>
        /// 旋转步长
        /// </summary>
        private static readonly int DEGREE_STEP = 3;

        /// <summary>
        /// 圆环宽
        /// </summary>
        private static readonly int RING_WIDTH = 10;

        /// <summary>
        /// 圆环与线之前的空间
        /// </summary>
        private static readonly int SPACE = 10;

        /// <summary>
        /// 图片半径
        /// </summary>
        public int BitmapRadius { get; private set; }

        /// <summary>
        /// 半径
        /// </summary>
        public int Radius {
            get;
            private set;
        }


        /// <summary>
        /// 外切长宽
        /// </summary>
        public int OWH { get; private set; }

        /// <summary>
        /// 内接长宽
        /// </summary>
        public int IWH { get; private set; }

        /// <summary>
        /// 图片是内接还是外切
        /// </summary>
        public ShowTypes ShowType { get; set; }

        public CenterView(Context ctx, Bitmap center, int radius) : base(ctx) {
            //不加这一句， Draw 的内容在 VS Emulator 里不显示。在 Android 自带的模拟器里正常。
            this.Holder.SetFormat(Format.Transparent);

            this.Holder.AddCallback(this);

            this.Focusable = true;
            this.FocusableInTouchMode = true;

            this.Center = center;

            //解析度
            var density = 1;// this.Context.Resources.DisplayMetrics.Density;

            this.BitmapRadius = radius - (SPACE + RING_WIDTH) / 2;
            this.Radius = radius;
            this.OWH = (int)(radius * 2 * density);
            this.IWH = (int)(System.Math.Sqrt(System.Math.Pow(radius, 2) * 2) * density);
        }

        private void Draw() {
            try {
                using (var canvas = this.Holder.LockCanvas()) {
                    if (canvas != null) {
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

            this.Degree = this.Degree % 360 + DEGREE_STEP;
            //要先旋转，后画图才有效果
            c.Rotate(this.Degree, c.Width / 2, c.Height / 2);

            //中心点
            var cx = c.Width / 2;
            var cy = c.Height / 2;

            //限定显示范围, 指定半径的圆
            var path = new Path();
            path.AddCircle(cx, cy, this.BitmapRadius, Path.Direction.Cw);
            c.ClipPath(path);
            c.DrawColor(Color.White);

            //图片的绘制范围
            var w = (this.ShowType == ShowTypes.Inner ? this.IWH : this.OWH) / 2;
            var rect = new Rect(cx - w, cy - w, cx + w, cy + w);

            //绘制图片
            var paint = new Paint();
            //src为null, 将把整个图片做为源
            c.DrawBitmap(bmp, null, rect, paint);

            c.RestoreToCount(sc);
        }

        private void DrawRing(Canvas c) {
            var sc = c.Save();

            c.Rotate(-this.Degree, c.Width / 2, c.Height / 2);

            //中心点
            var cx = c.Width / 2;
            var cy = c.Height / 2;

            //用两个圆 Path 构造一个 圆环显示区域, 即挖空内圆
            var pInner = new Path();
            pInner.AddCircle(cx, cy, this.BitmapRadius + SPACE / 2, Path.Direction.Cw);
            var pOut = new Path();
            pOut.AddCircle(cx, cy, this.Radius, Path.Direction.Cw);

            c.ClipPath(pOut);
            c.ClipPath(pInner, Region.Op.Difference);

            //var color = new Color((int)(DateTime.Now.Ticks % 0xFFFFFFFF));
            //c.DrawColor(color);

            //用角度渐变填充外圆的范围
            var g = new SweepGradient(cx, cy, Color.Green, Color.Transparent);
            var paint = new Paint();
            paint.SetShader(g);
            c.DrawCircle(cx, cy, this.Radius, paint);

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

        public void Start() {
            this.IsRunning = true;
        }

        public void Stop() {
            this.IsRunning = false;
        }

        #region IRunnable
        public void Run() {
            while (true) {
                if (this.IsRunning)
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


        public enum ShowTypes {
            /// <summary>
            /// 内接, 完整显示
            /// </summary>
            Inner,
            /// <summary>
            /// 外接, 边接不显示
            /// </summary>
            Out
        }

    }
}