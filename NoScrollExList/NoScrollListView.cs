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
    [Register("com.companyname.MyFinToControl.NoScrollListView")]
    internal class NoScrollListView : ListView
    {
        public NoScrollListView(Context context) : base(context)
        {
        }

        public NoScrollListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public NoScrollListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public NoScrollListView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected NoScrollListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
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