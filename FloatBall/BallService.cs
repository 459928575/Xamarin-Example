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

namespace FloatBall {
    [Service]
    [IntentFilter(new string[] { "Xamarin.BallService" })]
    public class BallService : Service, View.IOnTouchListener, View.IOnClickListener {

        private FrameLayout FV = null;
        private ImageView Img = null;

        private AnimationSet AniSet = null;

        private IWindowManager WindowManager {
            get {
                return this.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            }
        }

        public BallService() {
            this.AniSet = new AnimationSet(true);
            var rotat = new RotateAnimation(0f, 360f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            rotat.RepeatMode = RepeatMode.Reverse;
            rotat.Duration = 500;
            rotat.AnimationStart += Ani_AnimationStart;
            rotat.AnimationEnd += Ani_AnimationEnd;
            rotat.FillBefore = true;
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
            this.FV.Background = new ShapeDrawable(shape);
            //this.FV.SetClipChildren(true);
            //this.FV.SetClipToPadding(true);



            this.FV.SetOnTouchListener(this);
            this.FV.SetOnClickListener(this);

            var img = new ImageView(this);
            img.SetImageResource(Resource.Drawable.Icon);
            this.Img = img;

            this.FV.AddView(img);
            var param = new WindowManagerLayoutParams();
            param.Width = WindowManagerLayoutParams.WrapContent;
            param.Height = WindowManagerLayoutParams.WrapContent;
            param.Gravity = GravityFlags.Top | GravityFlags.Left;////////
            param.Flags = WindowManagerFlags.NotFocusable;

            param.Type = WindowManagerTypes.Phone;//Must
            param.Format = Android.Graphics.Format.Transparent;

            this.WindowManager.AddView(this.FV, param);
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (disposing && this.FV != null) {
                this.WindowManager.RemoveView(this.FV);
                this.FV.Dispose();
            }
        }
    }
}