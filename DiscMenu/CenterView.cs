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
        /// ��ת����
        /// </summary>
        private static readonly int DEGREE_STEP = 3;

        /// <summary>
        /// Բ����
        /// </summary>
        private static readonly int RING_WIDTH = 10;

        /// <summary>
        /// Բ������֮ǰ�Ŀռ�
        /// </summary>
        private static readonly int SPACE = 10;

        /// <summary>
        /// ͼƬ�뾶
        /// </summary>
        public int BitmapRadius { get; private set; }

        /// <summary>
        /// �뾶
        /// </summary>
        public int Radius {
            get;
            private set;
        }


        /// <summary>
        /// ���г���
        /// </summary>
        public int OWH { get; private set; }

        /// <summary>
        /// �ڽӳ���
        /// </summary>
        public int IWH { get; private set; }

        /// <summary>
        /// ͼƬ���ڽӻ�������
        /// </summary>
        public ShowTypes ShowType { get; set; }

        public CenterView(Context ctx, Bitmap center, int radius) : base(ctx) {
            //������һ�䣬 Draw �������� VS Emulator �ﲻ��ʾ���� Android �Դ���ģ������������
            this.Holder.SetFormat(Format.Transparent);

            this.Holder.AddCallback(this);

            this.Focusable = true;
            this.FocusableInTouchMode = true;

            this.Center = center;

            //������
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
            //Ҫ����ת����ͼ����Ч��
            c.Rotate(this.Degree, c.Width / 2, c.Height / 2);

            //���ĵ�
            var cx = c.Width / 2;
            var cy = c.Height / 2;

            //�޶���ʾ��Χ, ָ���뾶��Բ
            var path = new Path();
            path.AddCircle(cx, cy, this.BitmapRadius, Path.Direction.Cw);
            c.ClipPath(path);
            c.DrawColor(Color.White);

            //ͼƬ�Ļ��Ʒ�Χ
            var w = (this.ShowType == ShowTypes.Inner ? this.IWH : this.OWH) / 2;
            var rect = new Rect(cx - w, cy - w, cx + w, cy + w);

            //����ͼƬ
            var paint = new Paint();
            //srcΪnull, ��������ͼƬ��ΪԴ
            c.DrawBitmap(bmp, null, rect, paint);

            c.RestoreToCount(sc);
        }

        private void DrawRing(Canvas c) {
            var sc = c.Save();

            c.Rotate(-this.Degree, c.Width / 2, c.Height / 2);

            //���ĵ�
            var cx = c.Width / 2;
            var cy = c.Height / 2;

            //������Բ Path ����һ�� Բ����ʾ����, ���ڿ���Բ
            var pInner = new Path();
            pInner.AddCircle(cx, cy, this.BitmapRadius + SPACE / 2, Path.Direction.Cw);
            var pOut = new Path();
            pOut.AddCircle(cx, cy, this.Radius, Path.Direction.Cw);

            c.ClipPath(pOut);
            c.ClipPath(pInner, Region.Op.Difference);

            //var color = new Color((int)(DateTime.Now.Ticks % 0xFFFFFFFF));
            //c.DrawColor(color);

            //�ýǶȽ��������Բ�ķ�Χ
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

            //���Խ���
            //var g = new LinearGradient(x - 30, y - 30, x + 30, y + 30, Color.Red, Color.Blue, Shader.TileMode.Mirror);

            // �ǶȽ���
            // ��һ��,�ڶ���������ʾ����Բ��������
            //var g = new SweepGradient(x, y, Color.Red, Color.Blue);

            //���ν���
            //��һ��,�ڶ���������ʾ����Բ��������,������������ʾ�뾶
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
            /// �ڽ�, ������ʾ
            /// </summary>
            Inner,
            /// <summary>
            /// ���, �߽Ӳ���ʾ
            /// </summary>
            Out
        }

    }
}