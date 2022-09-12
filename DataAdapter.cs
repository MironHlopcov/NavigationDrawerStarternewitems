using Android;
using Android.Views;
using Android.Widget;
using EfcToXamarinAndroid.Core;
using Google.Android.Material.Badge;
using Google.Android.Material.Tabs;
using MyFinToControl.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MyFinToControl
{
    public class DataAdapter : BaseAdapter<DataItem>
    {
        private readonly AndroidX.Fragment.App.Fragment _fragment;
        private readonly List<DataItem> dataItems;

        public delegate void DataAdapterHandler(AndroidX.Fragment.App.Fragment context);
        public event DataAdapterHandler? OnDataSetChanged;

        public DataAdapter(AndroidX.Fragment.App.Fragment fragment, List<DataItem> dataItems)
        {
            this._fragment = fragment;
            this.dataItems = dataItems;
        }

        public DataAdapter(AndroidX.Fragment.App.Fragment fragment, int position)
        {
            this._fragment = fragment;
            switch (position)
            {
                case 0:
                    this.dataItems = DatesRepositorio.Payments;
                    DatesRepositorio.PaymentsChanged += (s, e) => { 
                        NotifyDataSetChanged(); 
                    };
                    break;
                case 1:
                    this.dataItems = DatesRepositorio.Deposits;
                    DatesRepositorio.DepositsChanged += (s, e) => { 
                        NotifyDataSetChanged(); 
                    };
                    break;
                case 2:
                    this.dataItems = DatesRepositorio.Cashs;
                    DatesRepositorio.CashsChanged += (s, e) => { 
                        NotifyDataSetChanged(); 
                    };
                    break;
                case 3:
                    this.dataItems = DatesRepositorio.Unreachable;
                    DatesRepositorio.UnreachableChanged += (s, e) =>
                    {
                        NotifyDataSetChanged();
                    };
                    break;
            }
        }

        #region Override
        public override DataItem this[int position]
        {
            get
            {
                return dataItems[position];
            }
        }

        public override int Count
        {
            get
            {
                return dataItems.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return dataItems[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _fragment.LayoutInflater.Inflate(Resource.Layout.list_item, parent, false);// inflate the xml for each item

            var txtSum = view.FindViewById<TextView>(Resource.Id.sum_TextView);
            var txtDeskr = view.FindViewById<TextView>(Resource.Id.deskription_TextView);
            var txtMcc = view.FindViewById<TextView>(Resource.Id.mcc_code_TextView); 
            var txtDate = view.FindViewById<TextView>(Resource.Id.data_TextView);
            var txtTime = view.FindViewById<TextView>(Resource.Id.time_TextView);
            var redDot = view.FindViewById<ImageView>(Resource.Id.data_is_new_item);

            txtSum.Text = string.Format("{0:F}", dataItems[position].Sum);
            //txtSum.Text = dataItems[position].OldSum.ToString() +":"+ string.Format("{0:F}", dataItems[position].Sum);
            //txtSum.Text = dataItems[position].Sum.ToString(CultureInfo.InvariantCulture);
            txtDeskr.Text = dataItems[position].Descripton;
            //txtDate.Text = dataItems[position].Date.ToShortDateString();
            if(dataItems[position].Date.Year==DateTime.Now.Year)
                txtDate.Text = dataItems[position].Date.ToString("dd.MM");
            else
                txtDate.Text = dataItems[position].Date.ToString("dd.MM.yyyy");
            txtTime.Text = dataItems[position].Date.ToLongTimeString();
            txtMcc.Text = dataItems[position].MCC == 0 ? "" : $"{dataItems[position].MCC}: {dataItems[position].MccDeskription}";

            if (dataItems[position].IsNewDataItem)
                redDot.Visibility = ViewStates.Visible;
           
              
            return view;
        }
        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();
            OnDataSetChanged?.Invoke(_fragment);
        }
        #endregion

        public MFilter MFilter
        {
            get
            {
                MFilter mFilter = new MFilter(dataItems);
                return mFilter;
            }
        }
    }
}