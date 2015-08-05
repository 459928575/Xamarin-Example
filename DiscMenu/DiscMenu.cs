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
    public class DiscMenu : ViewGroup, View.IOnTouchListener {

        private static readonly string CENTER_TAG = "C";
        private static readonly int CENTER_RADIUS = 50;

        private bool Explanded = false;


        public DiscMenu(Context ctx, Bitmap centerBmp) : base(ctx) {
            //使 OnDraw 可被调用
            this.SetWillNotDraw(false);

            var c = new CenterView(ctx, centerBmp, CENTER_RADIUS);
            c.ShowType = CenterView.ShowTypes.Inner;
            c.Tag = CENTER_TAG;
            c.Click += C_Click;
            this.AddViewInLayout(c, 0, new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));

            this.SetOnTouchListener(this);
        }

        private void C_Click(object sender, EventArgs e) {
            this.Explanded = !this.Explanded;
            this.Invalidate();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b) {

            float sAng = 0;
            var ang = 360 / (this.ChildCount - 1);
            var w = Math.Min(this.MeasuredWidth, this.MeasuredHeight) / 2;

            for (var i = 0; i < this.ChildCount; i++) {
                var c = this.GetChildAt(i);
                if (c.Tag != null && c.Tag.Equals(CENTER_TAG)) {
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
                if (c.Tag != null && c.Tag.Equals(CENTER_TAG))
                    continue;

                var t = MeasureSpec.MakeMeasureSpec((int)(0.25 * r), MeasureSpecMode.Exactly);
                c.Measure(t, t);
            }
        }

        protected override void OnDraw(Canvas canvas) {
            base.OnDraw(canvas);

            var cx = this.MeasuredWidth / 2;
            var cy = this.MeasuredHeight / 2;

            if (!this.Explanded) {
                var path = new Path();
                path.AddCircle(cx, cy, CENTER_RADIUS, Path.Direction.Cw);
                canvas.ClipPath(path);
            } else {

            }
        }


        public void SetMenus(Dictionary<string, Drawable> txtAndResIDs) {
            foreach (var kv in txtAndResIDs) {
                var btn = new ImageButton(this.Context) {
                    Background = kv.Value,
                    Tag = kv.Key
                };
                //var btn = new CenterView(this.Context, (kv.Value as BitmapDrawable).Bitmap, 40) {
                //    Tag = kv.Key
                //};
                btn.Click += Btn_Click;
                this.AddView(btn);
            }
        }

        private void Btn_Click(object sender, EventArgs e) {
            var btn = sender as ImageButton;
            var txt = (string)btn.Tag;
            Toast.MakeText(this.Context, txt, ToastLength.Short).Show();
        }

        /// <summary>
        /// 度数转为弧度
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        private double ToRadians(float degree) {
            return degree / 180f * Math.PI;
        }

        public bool OnTouch(View v, MotionEvent e) {
            //var x = (int)e.RawX - v.MeasuredWidth / 2;
            //var y = (int)e.RawY - v.MeasuredHeight / 2;
            v.SetX(e.RawX);
            v.SetY(e.RawY);
            return false;
        }
    }
}