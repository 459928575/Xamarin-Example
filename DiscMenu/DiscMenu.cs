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

            for (var i = 0; i < this.ChildCount; i++) {
                var c = this.GetChildAt(i);
                c.Layout(l, t, c.MeasuredWidth, c.MeasuredHeight);
            }

        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            for (var i = 0; i < this.ChildCount; i++) {
                var c = this.GetChildAt(i);
                //var t = MeasureSpec.MakeMeasureSpec(1 / 4, MeasureSpecMode.Exactly);
                c.Measure( widthMeasureSpec, heightMeasureSpec );
            }
        }

        protected override void MeasureChild(View child, int parentWidthMeasureSpec, int parentHeightMeasureSpec) {
            base.MeasureChild(child, parentWidthMeasureSpec, parentHeightMeasureSpec);
        }
    }
}