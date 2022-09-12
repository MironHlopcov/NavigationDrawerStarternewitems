using Android;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using EfcToXamarinAndroid.Core;
using Google.Android.Material.Badge;
using Google.Android.Material.Tabs;
using MyFinToControl.Fragments;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFinToControl
{
    public partial class MainActivity
    {
        class ViewPage2Fragment : AndroidX.Fragment.App.Fragment, ListView.IOnItemClickListener, ListView.IOnItemLongClickListener
        {

            public int Index { get; private set; }
            public List<DataItem> ListData { get; set; }
            public DataAdapter DataAdapter { get; private set; } //tested
            private int[] ColorSum { get; set; } = new int[3] { 220, 20, 60 };
            private int[] ColorTransCount { get; set; } = new int[3] { 0, 191, 255 };
            private int[] ColorSumMcc { get; set; } = new int[3] { 255, 140, 0 };
            private int[] ColorCountMcc { get; set; } = new int[3] { 46, 139, 87 };

            public ViewPage2Fragment(int index, List<DataItem> listData)
            {
                Index = index;
                ListData = listData;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                var ViewAD = LayoutInflater.Inflate(Resource.Layout.tab_layout, container, false);
                ListView listViewItems = ViewAD.FindViewById<ListView>(Resource.Id.dateslistView);

                listViewItems.OnItemClickListener = this;
                listViewItems.OnItemLongClickListener = this;

                var listAdapter = new DataAdapter(this, Index);
                DataAdapter = listAdapter; //test
                listViewItems.Adapter = listAdapter;

                //listAdapter.OnDataSetChanged += (AndroidX.Fragment.App.Fragment context) =>
                //{
                //    TabLayout tabLayout = (TabLayout)Activity.FindViewById(Resource.Id.tabLayout);
                //    var tab = tabLayout.GetTabAt(Index);
                //    int oldCount = 0;
                //    int.TryParse(tab.Text.Split(":")[0], out oldCount);
                //    tab.SetText($"{ListData.Count}: Sum={ListData.Select(x => x.Sum).Sum()}");
                //    UpdateBadgeToTabs(tabLayout, Index);
                //};
                listAdapter.OnDataSetChanged += ListAdapter_OnDataSetChanged;

                TabLayout listView = (TabLayout)Activity.FindViewById(Resource.Id.tabLayout);
                listView.GetTabAt(Index).SetText($"{ListData.Count}: Sum={ListData.Select(x => x.Sum).Sum()}");
                if (ListData.Count > 0)
                   UpdateBadgeToTabs((TabLayout)Activity.FindViewById(Resource.Id.tabLayout), Index);
                return ViewAD;
            }

            private void ListAdapter_OnDataSetChanged(AndroidX.Fragment.App.Fragment context)
            {
                TabLayout tabLayout = (TabLayout)Activity.FindViewById(Resource.Id.tabLayout);
                var tab = tabLayout.GetTabAt(Index);
                int oldCount = 0;
                int.TryParse(tab.Text.Split(":")[0], out oldCount);
                tab.SetText($"{ListData.Count}: Sum={ListData.Select(x => x.Sum).Sum()}");
                UpdateBadgeToTabs(tabLayout, Index);
            }

            public override void OnDestroyView()
            {
                this.DataAdapter.OnDataSetChanged-= ListAdapter_OnDataSetChanged;
                base.OnDestroyView();
            }

            private void UpdateBadgeToTabs(TabLayout tabLayout, int index)
            {
                if (DatesRepositorio.NewDataItems != null)
                {
                    int newItemsCount = 0;
                    BadgeDrawable badge;
                    switch (index)
                    {
                        case 0:
                            newItemsCount = DatesRepositorio.GetPayments(DatesRepositorio.NewDataItems).Count;
                            if (newItemsCount < 1)
                                break;
                            badge = tabLayout.GetTabAt(index).OrCreateBadge;
                            badge.Number += newItemsCount;
                            badge.BadgeGravity = BadgeDrawable.TopStart;
                            break;
                        case 1:
                            newItemsCount = DatesRepositorio.GetDeposits(DatesRepositorio.NewDataItems).Count;
                            if (newItemsCount < 1)
                                break;
                            badge = tabLayout.GetTabAt(index).OrCreateBadge;
                            badge.Number += newItemsCount;
                            badge.BadgeGravity = BadgeDrawable.TopStart;
                            break;
                        case 2:
                            newItemsCount = DatesRepositorio.GetCashs(DatesRepositorio.NewDataItems).Count;
                            if (newItemsCount < 1)
                                break;
                            badge = tabLayout.GetTabAt(index).OrCreateBadge;
                            badge.Number += newItemsCount;
                            badge.BadgeGravity = BadgeDrawable.TopStart;
                            break;
                        case 3:
                            newItemsCount = DatesRepositorio.GetUnreachable(DatesRepositorio.NewDataItems).Count;
                            if (newItemsCount < 1)
                                break;
                            badge = tabLayout.GetTabAt(index).OrCreateBadge;
                            badge.Number += newItemsCount;
                            badge.BadgeGravity = BadgeDrawable.TopStart;
                            break;
                    }
                }
            }

            public void OnItemClickDeletAfter(AdapterView parent, View view, int position, long id)
            {
                #region PlotView
                LayoutInflater inflater = Activity.LayoutInflater;
                var plot_layout = inflater.Inflate(Resource.Layout.plot_layout, null);
                var plotView = plot_layout.FindViewById<PlotView>(Resource.Id.plot_view);
                #endregion
                #region Total
                var totalSummText = plot_layout.FindViewById<TextView>(Resource.Id.TotalSummTextView);
                var totalTransText = plot_layout.FindViewById<TextView>(Resource.Id.TotalTransactionTextView);
                var totalSummMccText = plot_layout.FindViewById<TextView>(Resource.Id.TotalMccCodeTextView);
                var totalTransMccText = plot_layout.FindViewById<TextView>(Resource.Id.TotalTransactionMccTextView);
                #endregion
                #region Recurring
                var selectedItem = ListData[position];
                var recurringDiscrCount = ListData.Where(x => x.Descripton == selectedItem.Descripton).Count();
                var recurringDiscrSumm = ListData.Where(x => x.Descripton == selectedItem.Descripton).Select(x => x.Sum).Sum();
                var recurringMccCount = ListData.Where(x => x.MCC == selectedItem.MCC).Count();
                var recurringMccSumm = ListData.Where(x => x.MCC == selectedItem.MCC).Select(x => x.Sum).Sum();
                #endregion
                #region Share
                float shareOfTransactions = (float)recurringDiscrCount / ListData.Count * 100;
                float shareOfSumms = recurringDiscrSumm / ListData.Select(x => x.Sum).Sum() * 100;
                float shareOfMccTransactions = (float)recurringMccCount / ListData.Count * 100;
                float shareOfMccSumms = recurringMccSumm / ListData.Select(x => x.Sum).Sum() * 100;
                #endregion
                #region Color
                totalSummText.SetTextColor(Android.Graphics.Color.Rgb(ColorSum[0], ColorSum[1], ColorSum[2]));
                totalTransText.SetTextColor(Android.Graphics.Color.Rgb(ColorTransCount[0], ColorTransCount[1], ColorTransCount[2]));
                totalSummMccText.SetTextColor(Android.Graphics.Color.Rgb(ColorSumMcc[0], ColorSumMcc[1], ColorSumMcc[2]));
                totalTransMccText.SetTextColor(Android.Graphics.Color.Rgb(ColorCountMcc[0], ColorCountMcc[1], ColorCountMcc[2]));
                #endregion
                #region SetTextView
                totalSummText.Text = $"Сумма транзакций \"{selectedItem.Descripton}\": \r{recurringDiscrSumm} ({(shareOfSumms > 0.099 ? string.Format("{0:N0}", shareOfSumms) : "<0,01")}%)";
                totalTransText.Text = $"Количество - {recurringDiscrCount} ({(shareOfTransactions > 0.099 ? string.Format("{0:N0}", shareOfTransactions) : "<0,01")}%)";
                totalSummMccText.Text = $"Сумма транзакций по категории \"{selectedItem.MccDeskription}\": \r{recurringMccSumm} ({(shareOfMccSumms > 0.099 ? string.Format("{0:N0}", shareOfMccSumms) : "<0,01")}%)";
                totalTransMccText.Text = $"Количество транзакций по данной категории - {recurringMccCount} ({(shareOfMccTransactions > 0.099 ? string.Format("{0:N0}", shareOfMccTransactions) : "<0,01")}%)";
                #endregion

                plotView.Model = CreatePlotModel2(selectedItem.Descripton, shareOfSumms, shareOfTransactions, shareOfMccSumms, shareOfMccTransactions);
                AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                builder.SetCancelable(false);
                builder.SetIcon(Resource.Drawable.abc_ic_go_search_api_material);
                builder.SetView(plot_layout);
                builder.SetNegativeButton("OK", (c, ev) =>
                {
                    builder.Dispose();
                });
                builder.Create();
                builder.Show();
            }
            public void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                var dialog = new SelectItemDialog(ListData[position]);
                dialog.EditItemChange += (sender, e) =>
                {
                    //////                    DataAdapter.NotifyDataSetChanged();
                    dialog.Dismiss();
                };
                dialog.Display(Activity.SupportFragmentManager);
            }
            public bool OnItemLongClick(AdapterView parent, View view, int position, long id)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                builder.SetTitle(ListData[position].Descripton);
                builder.SetMessage(" Выберите действие");
                builder.SetCancelable(false);
                builder.SetPositiveButton("Редактировать", (c, ev) =>
                {
                    var dialog = new EditItemDialog(ListData[position]);
                    dialog.EditItemChange += (sender, e) =>
                    {
                        ////////                       DataAdapter.NotifyDataSetChanged();
                        dialog.Dismiss();
                    };
                    dialog.Display(Activity.SupportFragmentManager);
                    builder.Dispose();
                });
                builder.SetNegativeButton("Удалить", (c, ev) =>
                {
                    DatesRepositorio.DeleteItem(ListData[position]);
                    ///////                   DataAdapter.NotifyDataSetChanged();
                });
                builder.SetNeutralButton("Отмена", (c, ev) =>
                {
                    builder.Dispose();
                });
                builder.Create();
                builder.Show();
                return true;
            }
            private PlotModel CreatePlotModel2(string diskr, float sum, float count, float sumMcc, float countMcc)
            {
                var plotModel1 = new PlotModel
                {
                    TitlePadding = 2,
                    Title = $"{diskr}",
                    //plotModel1.Background = OxyColors.LightGray;
                    DefaultColors = new List<OxyColor>{
                    OxyColors.WhiteSmoke,
                }
                };

                var plotModelWidth = plotModel1.Width;

                var pieSeriessumCountMcc = new CustomPieSeries();
                pieSeriessumCountMcc.Diameter = 1;
                pieSeriessumCountMcc.StartAngle = 60;
                pieSeriessumCountMcc.UnVisebleFillColors = OxyColors.WhiteSmoke;
                pieSeriessumCountMcc.Slices.Add(new PieSlice("", countMcc)
                {
                    Fill = OxyColor.FromRgb((byte)ColorCountMcc[0], (byte)ColorCountMcc[1], (byte)ColorCountMcc[2])
                });
                pieSeriessumCountMcc.Slices.Add(new PieSlice("", 100 - countMcc) { Fill = pieSeriessumCountMcc.UnVisebleFillColors });

                var pieSeriesSumMcc = new CustomPieSeries();
                pieSeriesSumMcc.Diameter = 0.8;
                pieSeriesSumMcc.StartAngle = 40;
                pieSeriesSumMcc.UnVisebleFillColors = OxyColors.WhiteSmoke;
                pieSeriesSumMcc.Slices.Add(new PieSlice("", sumMcc)
                {
                    Fill = OxyColor.FromRgb((byte)ColorSumMcc[0], (byte)ColorSumMcc[1], (byte)ColorSumMcc[2])
                });
                pieSeriesSumMcc.Slices.Add(new PieSlice("", 100 - sumMcc) { Fill = pieSeriesSumMcc.UnVisebleFillColors });

                var pieSeriesCount = new CustomPieSeries();
                pieSeriesCount.Diameter = 0.5;
                pieSeriesCount.StartAngle = 20;
                pieSeriesCount.UnVisebleFillColors = OxyColors.WhiteSmoke;
                pieSeriesCount.Slices.Add(new PieSlice("", count)
                {
                    Fill = OxyColor.FromRgb((byte)ColorTransCount[0], (byte)ColorTransCount[1], (byte)ColorTransCount[2])
                });
                pieSeriesCount.Slices.Add(new PieSlice("", 100 - count) { Fill = pieSeriesCount.UnVisebleFillColors });

                var pieSeriesSum = new CustomPieSeries();
                pieSeriesSum.StartAngle = 0;
                pieSeriesSum.UnVisebleFillColors = OxyColors.WhiteSmoke;
                pieSeriesSum.Diameter = 0.2;

                pieSeriesSum.Slices.Add(new PieSlice("", sum)
                {
                    Fill = OxyColor.FromRgb((byte)ColorSum[0], (byte)ColorSum[1], (byte)ColorSum[2])
                });
                pieSeriesSum.Slices.Add(new PieSlice("", 100 - sum) { Fill = pieSeriesSum.UnVisebleFillColors });

                plotModel1.Series.Add(pieSeriessumCountMcc);
                plotModel1.Series.Add(pieSeriesSumMcc);
                plotModel1.Series.Add(pieSeriesCount);
                plotModel1.Series.Add(pieSeriesSum);



                return plotModel1;




            }

            public override void OnSaveInstanceState(Bundle outState)
            {
                base.OnSaveInstanceState(outState);
                //outState.PutInt("MainActivity", act);
            }
            public override void SetInitialSavedState(SavedState state)
            {
                base.SetInitialSavedState(state);
            }
            
        }
    }


}

