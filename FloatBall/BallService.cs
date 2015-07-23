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
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Views.Animations;
using Android.Graphics;

namespace FloatBall {
    [Service]
    [IntentFilter(new string[] { "Xamarin.BallService" })]
    public class BallService : Service, View.IOnTouchListener, View.IOnClickListener {

        private FrameLayout FV = null;
        private ImageView Img = null;

        private AnimationSet AniSet = null;

        private IWindowManager WindowManager {
            get {
                //不是简单的类型转换，一定要用 JavaCast
                return this.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            }
        }

        public BallService() {
            this.AniSet = new AnimationSet(true);
            var rotat = new RotateAnimation(0f, 360f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            rotat.RepeatMode = RepeatMode.Reverse;
            rotat.Duration = 500;
            //rotat.AnimationStart += Ani_AnimationStart;
            //rotat.AnimationEnd += Ani_AnimationEnd;
            rotat.FillBefore = true;
            //AccelerateDecelerateInterpolator  先加速后减速
            rotat.Interpolator = new AccelerateDecelerateInterpolator();
            this.AniSet.AddAnimation(rotat);
        }

        private void Ani_AnimationEnd(object sender, Animation.AnimationEndEventArgs e) {
            System.Diagnostics.Debug.WriteLine("Animation end");
        }

        private void Ani_AnimationStart(object sender, Animation.AnimationStartEventArgs e) {
            System.Diagnostics.Debug.WriteLine("Animation start");
        }

        public override IBinder OnBind(Intent intent) {
            return null;
        }

        public override void OnCreate() {
            base.OnCreate();

            this.CreateBallView();
        }

        public bool OnTouch(View v, MotionEvent e) {
            var parm = v.LayoutParameters as WindowManagerLayoutParams;
            if (parm != null) {
                parm.X = (int)e.RawX - v.MeasuredWidth / 2;
                parm.Y = (int)e.RawY - v.MeasuredHeight / 2;
                //System.Diagnostics.Debug.WriteLine("======>RawX : {0} RawY :{1} Width:{2} Height:{3} X:{4} Y:{5}", e.RawX, e.RawY, v.MeasuredWidth, v.MeasuredHeight, parm.X, parm.Y);
                this.WindowManager.UpdateViewLayout(v, parm);
            }
            // true 和 false 有啥区别？
            return false;
        }

        public void OnClick(View v) {
            Handler h = new Handler();
            h.Post(() => {
                this.Img.StartAnimation(this.AniSet);
            });
        }

        private void CreateBallView() {
            this.FV = new FrameLayout(this);
            var shape = new OvalShape();
            var dr = new ShapeDrawable(shape);
            dr.Paint.Color = Color.WhiteSmoke;
            dr.Paint.Alpha = 100;
            this.FV.Background = dr;

            //不懂如何裁剪。
            //this.FV.SetClipChildren(true);
            //this.FV.SetClipToPadding(true);


            this.FV.SetOnTouchListener(this);
            this.FV.SetOnClickListener(this);

            var img = new ImageView(this);
            img.SetImageResource(Resource.Drawable.Icon);
            this.Img = img;//动画对 FreameLayout 不起作用

            this.FV.AddView(img);
            var param = new WindowManagerLayoutParams();
            param.Width = WindowManagerLayoutParams.WrapContent;
            param.Height = WindowManagerLayoutParams.WrapContent;

            //我理解这个东西应该是 原点， 如果设置成其它值， 拖动的时候位置会有错。
            param.Gravity = GravityFlags.Top | GravityFlags.Left;
            param.Flags = WindowManagerFlags.NotFocusable;

            //必须设置， 不然会在 WindowManager.AddView 的时候报错。
            param.Type = WindowManagerTypes.Phone;
            param.Format = Android.Graphics.Format.Transparent;

            this.WindowManager.AddView(this.FV, param);
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (disposing && this.FV != null) {
                this.WindowManager.RemoveView(this.FV);
                this.FV.Dispose();
                this.Img.Dispose();
            }
        }
    }
}