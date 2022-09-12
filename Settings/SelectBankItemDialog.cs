using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using AndroidX.Fragment.App;
using EfcToXamarinAndroid.Core;
using Google.Android.Material.Chip;
using Google.Android.Material.TextField;
using MyFinToControl.Models;
using MyFinToControl.Parsers;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFinToControl.Fragments
{
    public class SelectBankItemDialog : AndroidX.Fragment.App.DialogFragment, View.IOnClickListener
    {
        public static string TAG = typeof(SelectBankItemDialog).Name;

        private AndroidX.AppCompat.Widget.Toolbar toolbar;
 
        private DataItem selectedItem;

        public SelectBankItemDialog(DataItem dataItem)
        {
            selectedItem = dataItem;
        }

        public void Display(AndroidX.Fragment.App.FragmentManager fragmentManager)
        {
            this.Show(fragmentManager, TAG);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(AndroidX.Fragment.App.DialogFragment.StyleNormal, Resource.Style.AppTheme_FullScreenDialog);
            this.Activity.Window.SetSoftInputMode(SoftInput.AdjustPan | SoftInput.AdjustResize);
        }

        public override void OnStart()
        {
            base.OnStart();
            Dialog dialog = Dialog;
            if (dialog != null)
            {
                int width = ViewGroup.LayoutParams.MatchParent;
                int height = ViewGroup.LayoutParams.MatchParent;
                dialog.Window.SetLayout(width, height);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.selectitem_dialog, container, false);
            toolbar = (AndroidX.AppCompat.Widget.Toolbar)view.FindViewById(Resource.Id.toolbar);


            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            toolbar.SetNavigationOnClickListener(this);
            toolbar.Title = selectedItem.Descripton;
        }

        private void Toolbar_MenuItemClick(object sender, AndroidX.AppCompat.Widget.Toolbar.MenuItemClickEventArgs e)
        {
            OnEditItemChange(this, e);
        }

        private void Toolbar_NavigationClick(object sender, AndroidX.AppCompat.Widget.Toolbar.NavigationClickEventArgs e)
        {
            Dismiss();
        }

        public void OnClick(View v)
        {
            v.Dispose();
            v = null;
            var fragment = (AndroidX.Fragment.App.DialogFragment)FragmentManager.FindFragmentByTag(typeof(SelectItemDialog).Name);
            fragment?.Dismiss();
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            dialog.Dismiss();
        }

        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler EditItemChange;
        protected void OnEditItemChange(object sender, EventArgs e)
        {
            EventHandler handler = EditItemChange;
            handler?.Invoke(this, e);
        }
    }
}