using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EfcToXamarinAndroid.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFinToControl.Filters
{
    public class MFilter
    {
        public IEnumerable<DataItem> InDataItems { private get; set; }
        public List<DataItem> OutDataItems { get; private set; }
        private DataItem FilterModel { get; set; }
        public int Count { get; }

        public MFilter(List<DataItem> inDataItems)
        {
            InDataItems = inDataItems;
        }

        public List<DataItem> GetResult(Expression<Func<DataItem, bool>> dataItemFilter)
        {
            IEnumerable<DataItem> query;
            if (OutDataItems == null)
                query = InDataItems.AsQueryable().Where(dataItemFilter);
            else
                query = OutDataItems.AsQueryable().Where(dataItemFilter);
            OutDataItems = query.ToList();
            OnFiltredClose(this);
            return OutDataItems;
        }
        public List<DataItem> GetResult(Expression<Func<DataItem, bool>> dataItemFilter, Expression<Func<DataItem, bool>> dataItemFilter2)
        {
            IEnumerable<DataItem> query;
            if (OutDataItems == null)
                query = InDataItems.AsQueryable().Where(dataItemFilter);
            else
                query = OutDataItems.AsQueryable().Where(dataItemFilter);
            OutDataItems = query.ToList();
            OnFiltredClose(this);
            return OutDataItems;
        }

        public delegate void EventHandler(object sender);
        public event EventHandler FiltredClose;
        protected virtual void OnFiltredClose(object sender)
        {
            EventHandler handler = FiltredClose;
            handler?.Invoke(this);
        }


    }
}