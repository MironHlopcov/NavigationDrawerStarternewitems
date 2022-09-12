using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using EfcToXamarinAndroid.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFinToControl.Settings
{
    internal class CustomBankConfigAdapter : BaseAdapter<BankConfiguration>
    {
        private readonly List<BankConfiguration> dataItems;
        Context mContext;
        View mView;

        public CustomBankConfigAdapter(List<BankConfiguration> dataItems, Context mContext)
        {
            this.dataItems = dataItems;
            this.mContext = mContext;
        }

        #region Override
        public override BankConfiguration this[int position]
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
                if (dataItems == null)
                    return 0;
                return dataItems.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = LayoutInflater.From(mContext);
            var view = inflater.Inflate(Resource.Layout.bank_list_item, parent, false);
            var bankName = view.FindViewById<TextView>(Resource.Id.bank_name_TextView);
            var bankSmsNumber = view.FindViewById<TextView>(Resource.Id.sms_number_TextView);
            bankName.Text = dataItems[position].Name;
            bankSmsNumber.Text = dataItems[position].SmsNumber;
            return view;
        }
        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();
        }
        #endregion
    }
}