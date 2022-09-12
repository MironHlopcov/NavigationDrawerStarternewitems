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
using System.Text;

namespace MyFinToControl.Parsers
{
    internal class DuplicatesSearcher
    {
        public List<DataItem> DataItems { get; private set; }
        public List<DataItem> DuplicateItems { get; private set; }
      //  public List<DataItem> DuplicatedItems { get; private set; }
        public int SearchAccuracyByMinutes { get; set; } = 5;
        public DuplicatesSearcher(List<DataItem> dataItems)
        {
            DataItems = dataItems;
        }
        public List<DataItem> SearcDuplicates()
        {
            DuplicateItems = new List<DataItem>();
            foreach (var item in DataItems)
            {
                //var duplicates = DataItems.Where(x => x.Sum == item.Sum | x.OldSum == item.Sum).Where(x =>
                //item.Date.AddMinutes(-SearchAccuracyByMinutes) < x.Date &&
                //item.Date.AddMinutes(-SearchAccuracyByMinutes) > x.Date).ToList();
                if (DuplicateItems.Contains(item))
                    continue;
                var duplicates = DataItems
                    .Where(x => x.Id != item.Id)
                    .Where(x => x.Sum == item.Sum | x.OldSum == item.Sum)
                //    .Where(x => item.Date.AddMinutes(-SearchAccuracyByMinutes) < x.Date
                //    &&
                //item.Date.AddMinutes(SearchAccuracyByMinutes) > x.Date)
                    .ToList();
                if (duplicates.Count > 0)
                {
                    DuplicateItems.AddRange(duplicates);
                    DuplicateItems.Add(item);
                }
            }


            SelectDuplicate();
            return DuplicateItems;
        }

        private void SelectDuplicate()
        {
            DuplicateItems.ForEach(x => DataItems.FirstOrDefault(y => y.Id == x.Id).IsNewDataItem = true);
        }
    }
}