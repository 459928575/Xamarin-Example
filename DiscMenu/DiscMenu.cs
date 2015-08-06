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
using Android.Animation;
using Android.Util;

namespace DiscMenu {
    /// <summary>
    /// http://blog.csdn.net/lmj623565791/article/details/43131133
    /// </summary>
    public class DiscMenu : ViewGroup, View.IOnTouchListener {

        /// <summary>
        /// 中心圆的标识
        /// </summary>
        private static readonly string CENTER_TAG = "C";

        /// <summary>
        /// 中心圆的半径
        /// </summary>
        private static readonly int CENTER_RADIUS = 60;

        /// <summary>
        /// 是否展开
        /// </summary>
        private bool Explanded = false;

        private Dictionary<int, AnimatorSet> Anims = new Dictionary<int, AnimatorSet>();


        public DiscMenu(Context ctx, Bitmap centerBmp) : base(ctx) {
            //使 OnDraw 可被调用
            //this.SetWillNotDraw(false);

            var c = new CenterView(ctx, centerBmp, CENTER_RADIUS);
            c.ShowType = CenterView.ShowTypes.Inner;
            c.Tag = CENTER_TAG;
            c.Click += C_Click;
            this.AddViewInLayout(c, 0, new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
            //c.SetZOrderMediaOverlay(true);

            this.SetOnTouchListener(this);
        }

        private void C_Click(object sender, EventArgs e) {
            var c = sender as CenterView;
            this.Explanded = !this.Explanded;

            if (this.Explanded) {
                c.Stop();
                this.Expand();
            } else {
                c.Start();
                this.Collopse();
            }
        }



        private void Expand() {
            for (var i = 0; i < this.ChildCount; i++) {
                this.Expand(i);
            }
        }

        private AnimatorSet GetAnimatorSet(int idx, bool expand) {
            AnimatorSet set = null;
            if (!this.Anims.ContainsKey(idx)) {
                var avgRadians = this.ToRadians(360 / (this.ChildCount - 1));
                var w = Math.Min(this.MeasuredWidth, this.MeasuredHeight) / 2;

                var cx = this.MeasuredWidth / 2;
                var cy = this.MeasuredHeight / 2;

                var c = this.GetChildAt(idx);
                var radians = idx * avgRadians;
                var hw = c.MeasuredWidth / 2;
                var hy = c.MeasuredHeight / 2;

                var tmp = (w - hw);
                var l = w + (int)Math.Round(tmp * Math.Cos(radians) - hw);
                var t = w + (int)Math.Round(tmp * Math.Sin(radians) - hw);

                //var duration = new Random(1000).Next(200, 1000);

                set = new AnimatorSet();
                set.SetTarget(c);
                set.SetDuration(500);

                float[] xs = new float[] { cx - hw, l };
                float[] ys = new float[] { cy - hy, t };
                float[] ss = new float[] { 0.1f, 1 };
                if (!expand) {
                    Array.Reverse(xs);
                    Array.Reverse(ys);
                    Array.Reverse(ss);
                }

                var aniX = ObjectAnimator.OfFloat(c, "X", xs);
                var aniY = ObjectAnimator.OfFloat(c, "Y", ys);
                var aniSX = ObjectAnimator.OfFloat(c, "ScaleX", ss);
                var aniSY = ObjectAnimator.OfFloat(c, "ScaleY", ss);

                set.PlayTogether(aniX, aniY, aniSX, aniSY);
                //this.Anims[idx] = set;
            } else
                set = this.Anims[idx];

            return set;
        }

        private View GetCanExpandCollopseChild(int idx) {
            var c = this.GetChildAt(idx);
            if (c != null && c.Tag != null && c.Tag.Equals(CENTER_TAG))
                return null;
            return c;
        }

        private void Expand(int idx) {
            //Animators may only be run on Looper threads
            //Animation 只能在 UI 线程上运行
            var c = this.GetCanExpandCollopseChild(idx);
            if (c == null)
                return;

            var set = this.GetAnimatorSet(idx, true);
            set.Cancel();
            set.Start();
        }

        private void Collopse() {
            for (var i = 0; i < this.ChildCount; i++) {
                this.Collopse(i);
            }
        }

        private void Collopse(int idx) {
            var c = this.GetCanExpandCollopseChild(idx);
            if (c == null)
                return;

            var set = this.GetAnimatorSet(idx, false);
            set.Cancel();
            set.Start();
        }





        protected override void OnLayout(bool changed, int l, int t, int r, int b) {

            for (var i = 0; i < this.ChildCount; i++) {
                var c = this.GetChildAt(i);
                if (c.Tag != null && c.Tag.Equals(CENTER_TAG)) {
                    //如果不分配空间 SurfaceView 的 SurfaceCreated 事件不会执行
                    c.Layout(0, 0, this.MeasuredWidth, this.MeasuredHeight);
                } else {
                    var cw = this.MeasuredWidth / 2;
                    var cy = this.MeasuredHeight / 2;
                    var left = cw - c.MeasuredWidth / 2;
                    var top = cy - c.MeasuredHeight / 2;
                    c.Layout(left, top, left + c.MeasuredWidth, top + c.MeasuredHeight);

                    //
                    c.ScaleX = 0.1f;
                    c.ScaleY = 0.1f;
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

        public void SetMenus(Dictionary<string, Drawable> txtAndResIDs) {
            foreach (var kv in txtAndResIDs) {
                var btn = new ImageButton(this.Context) {
                    Background = kv.Value,
                    Tag = kv.Key
                };
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


        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (disposing) {
                foreach (var s in this.Anims.Values) {
                    s.Dispose();
                }
            }
        }

    }
}