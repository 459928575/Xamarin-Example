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

namespace DiscMenu {
    /// <summary>
    /// http://blog.csdn.net/lmj623565791/article/details/43131133
    /// </summary>
    public class DiscMenu : ViewGroup {

        public DiscMenu(Context ctx) : base(ctx) {
            //var dv = new DiscView(ctx);
            //this.AddView(dv, LayoutParams.MatchParent, LayoutParams.MatchParent);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b) {

            float sAng = 0;
            var ang = 360 / this.ChildCount;
            var w = Math.Min(this.MeasuredWidth, this.MeasuredHeight);

            for (var i = 0; i < this.ChildCount; i++) {
                sAng %= 360;
                var radians = this.ToRadians(sAng);
                var c = this.GetChildAt(i);

                var tmp = (w - c.MeasuredWidth) / 2;
                var left = w / 2 + (int)Math.Round(tmp * Math.Cos(radians) - c.MeasuredWidth / 2);
                var top = w / 2 + (int)Math.Round(tmp * Math.Sin(radians) - c.MeasuredWidth / 2);

                c.Layout(left, top, left + c.MeasuredWidth, top + c.MeasuredHeight);

                sAng += ang;
            }

        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            var w = MeasureSpec.GetSize(widthMeasureSpec);
            var h = MeasureSpec.GetSize(heightMeasureSpec);
            var r = Math.Min(w, h);

            for (var i = 0; i < this.ChildCount; i++) {
                var c = this.GetChildAt(i);
                var t = MeasureSpec.MakeMeasureSpec((int)(0.25 * r), MeasureSpecMode.Exactly);
                c.Measure(t, t);
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