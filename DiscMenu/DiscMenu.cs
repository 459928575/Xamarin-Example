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
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Threading.Tasks;

namespace DiscMenu {
    /// <summary>
    /// http://blog.csdn.net/lmj623565791/article/details/43131133
    /// </summary>
    public class DiscMenu : ViewGroup {

        private static readonly string CenterTag = "C";

        public DiscMenu(Context ctx, Bitmap centerBmp) : base(ctx) {
            //使 OnDraw 可被调用
            //this.SetWillNotDraw(false);

            var c = new CenterView(ctx, centerBmp);
            c.Tag = CenterTag;
            this.AddViewInLayout(c, 0, new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b) {

            float sAng = 0;
            var ang = 360 / (this.ChildCount - 1);
            var w = Math.Min(this.MeasuredWidth, this.MeasuredHeight) / 2;

            for (var i = 0; i < this.ChildCount; i++) {
                var c = this.GetChildAt(i);
                if (c.Tag != null && c.Tag.Equals(CenterTag)) {
                    //如果不分配空间 SurfaceView 的 SurfaceCreated 事件不会执行
                    c.Layout(0, 0, this.MeasuredWidth, this.MeasuredHeight);
                } else {
                    sAng %= 360;
                    var radians = this.ToRadians(sAng);
                    var cw = c.MeasuredWidth / 2;

                    var tmp = (w - cw);
                    var left = w + (int)Math.Round(tmp * Math.Cos(radians) - cw);
                    var top = w + (int)Math.Round(tmp * Math.Sin(radians) - cw);

                    c.Layout(left, top, left + c.MeasuredWidth, top + c.MeasuredHeight);

                    sAng += ang;
                }
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            var w = MeasureSpec.GetSize(widthMeasureSpec);
            var h = MeasureSpec.GetSize(heightMeasureSpec);
            var r = Math.Min(w, h);

            for (var i = 0; i < this.ChildCount; i++) {
                var c = this.GetChildAt(i);
                if (c.Tag != null && c.Tag.Equals(CenterTag))
                    continue;

                var t = MeasureSpec.MakeMeasureSpec((int)(0.25 * r), MeasureSpecMode.Exactly);
                c.Measure(t, t);
            }
        }

        protected override void OnDraw(Canvas canvas) {
            base.OnDraw(canvas);

            //var w = Math.Min(this.Width, this.Height) / 2;
            //var paint = new Paint() {
            //    Color = Color.White,
            //    //Alpha = 180,
            //    AntiAlias = true
            //};

            //var p = new Path();
            //p.AddCircle(w, w, w, Path.Direction.Cw);//闭合
            //var p2 = new Path();
            //p2.AddCircle(w, w, w / 2, Path.Direction.Cw);
            //var p3 = new Path();
            //p3.AddCircle(w, w, w / 3, Path.Direction.Cw);

            //canvas.ClipPath(p);
            //canvas.ClipPath(p2, Region.Op.Difference);
            //canvas.ClipPath(p3, Region.Op.Union);
            //canvas.DrawPath(p, paint);
        }


        public void SetMenus(Dictionary<string, Drawable> txtAndResIDs) {
            foreach (var kv in txtAndResIDs) {
                var btn = new ImageView(this.Context) {
                    Background = kv.Value
                };
                this.AddView(btn);
            }
        }

        /// <summary>
        /// 度数转为弧度
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        private double ToRadians(float degree) {
            return degree / 180f * Math.PI;
        }
    }
}