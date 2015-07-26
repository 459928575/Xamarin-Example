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
using Android.Support.V4.View;
using Java.Lang;
using System.Diagnostics;

namespace FlipView {
    public class FlipViewAdapter : PagerAdapter {

        private IEnumerable<View> Items;

        public override int Count {
            get {
                return this.Items.Count() * 2;
            }
        }

        public FlipViewAdapter(IEnumerable<View> items) {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Count() == 0)
                throw new ArgumentException("items is empty", "items");

            this.Items = items;
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object objectValue) {
            return view.Equals(objectValue);
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position) {
            position %= this.Items.Count();
            var item = this.Items.ElementAt(position);
            container.AddView(item, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            return item;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue) {
            //var item = this.Items.ElementAt(position);
            //container.RemoveView(item);
            container.RemoveView((View)objectValue);
        }

        public override void FinishUpdate(ViewGroup container) {
            var vp = container as ViewPager;
            var pos = vp.CurrentItem;
            //System.Diagnostics.Debug.WriteLine("=====>before {0}", pos);
            if (pos == 0) {
                pos = this.Items.Count();/////////////
                vp.SetCurrentItem(pos, false);
            } else if (pos == this.Count - 1) {
                pos = this.Items.Count() - 1;///////////////
                vp.SetCurrentItem(pos, false);
            }
            //System.Diagnostics.Debug.WriteLine("=====>after {0}", pos);
        }
    }
}