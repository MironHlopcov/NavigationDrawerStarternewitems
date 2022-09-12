using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFinToControl.NoScrollExList
{
    [Register("com.companyname.MyFinToControl.NoScrollExListView")]
    internal class NoScrollExListView : ExpandableListView
    {
        public NoScrollExListView(Context context) : base(context)
        {
        }

        public NoScrollExListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public NoScrollExListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public NoScrollExListView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected NoScrollExListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
           int heightMeasureSpec_custom = MeasureSpec.MakeMeasureSpec(
               int.MaxValue >> 2, MeasureSpecMode.AtMost);
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec_custom);
            var param = this.LayoutParameters;
               param.Height = MeasuredHeight;
                
        }
    }
}