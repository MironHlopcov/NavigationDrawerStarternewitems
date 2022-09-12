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
    public class SelectItemDialog : AndroidX.Fragment.App.DialogFragment, View.IOnClickListener
    {
        public static string TAG = typeof(SelectItemDialog).Name;

        private AndroidX.AppCompat.Widget.Toolbar toolbar;
        private Android.Widget.AutoCompleteTextView autocompleteTVOperTyp;
        private TextInputLayout wrap_aut_comp_tv_OperationTyp;
        private TextInputEditText summOper;

        private Android.Widget.AutoCompleteTextView autCompTvOperationDiscription;
        private TextInputLayout aut_comp_tv_OperationDiscription;

        private Android.Widget.AutoCompleteTextView autCompTvOperationMccCode;
        private TextInputLayout aut_comp_tv_OperationMccCode;

        private Android.Widget.AutoCompleteTextView autCompTvOperationMccDiscription;
        private TextInputLayout aut_comp_tv_OperationMccDiscription;

        private TextInputLayout textfieldDateCheck;
        private TextInputEditText date_text_edit1;

        private TextInputLayout textfieldTimeCheck;
        private TextInputEditText date_text_edit2;

        private ChipGroup chipGroup;


        private DataItem selectedItem;

        public SelectItemDialog(DataItem dataItem)
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

            #region OperTyp
            wrap_aut_comp_tv_OperationTyp = view.FindViewById<TextInputLayout>(Resource.Id.wrap_aut_comp_tv_OperationTyp);
            wrap_aut_comp_tv_OperationTyp.EndIconVisible = false;
            autocompleteTVOperTyp = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationTyp);
            autocompleteTVOperTyp.Text = selectedItem.OperacionTyp.ToString();
            autocompleteTVOperTyp.Focusable = false;
            #endregion

            #region Summ
            summOper = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_OperationTSumm);
            //var asdf = string.Format("{0:F}", selectedItem.Sum);
            //summOper.SetText(string.Format("{0:F}", selectedItem.Sum).ToCharArray(), 0, string.Format("{0:F}", selectedItem.Sum.ToString()).Length);
            summOper.Text = string.Format("{0:F}", selectedItem.Sum);
            summOper.Focusable = false;
            #endregion

            #region DateEdit

            textfieldDateCheck = view.FindViewById<TextInputLayout>(Resource.Id.textfield_DateCheck);
            textfieldDateCheck.Tag = "textfieldDateCheck_Tag";

            date_text_edit1 = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_date);
            date_text_edit1.Text = selectedItem.Date.ToShortDateString();
            date_text_edit1.Focusable = false;
            #endregion

            #region TimeEdit

            textfieldTimeCheck = view.FindViewById<TextInputLayout>(Resource.Id.textfield_TimeCheck);
            textfieldTimeCheck.Tag = "textfieldTimeCheck_Tag";

            date_text_edit2 = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_time);
            date_text_edit2.Text = selectedItem.Date.ToLongTimeString();
            date_text_edit2.Focusable = false;

            #endregion

            #region OperationDiscription

            aut_comp_tv_OperationDiscription = view.FindViewById<TextInputLayout>(Resource.Id.wrap_tv_OperationDiscription);
            if (selectedItem.Descripton != "")
            {
                autCompTvOperationDiscription = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationDiscription);
                autCompTvOperationDiscription.Text = selectedItem.Descripton;
                autCompTvOperationDiscription.Focusable = false;
                aut_comp_tv_OperationDiscription.EndIconVisible = false;
            }
            else
                aut_comp_tv_OperationDiscription.Visibility = ViewStates.Gone;
            #endregion

            #region OperationMccCode
            aut_comp_tv_OperationMccCode = view.FindViewById<TextInputLayout>(Resource.Id.wrap_tv_OperationMccCode);
            if (selectedItem.MCC != 0)
            {
                autCompTvOperationMccCode = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationMccCode);
                autCompTvOperationMccCode.Text = selectedItem.MCC.ToString();
                autCompTvOperationMccCode.Focusable = false;
                aut_comp_tv_OperationMccCode.EndIconVisible = false;
            }
            else
                aut_comp_tv_OperationMccCode.Visibility = ViewStates.Gone;
            #endregion

            #region OperationMccDiscription
            aut_comp_tv_OperationMccDiscription = view.FindViewById<TextInputLayout>(Resource.Id.wrap_tv_OperationMccDiscription);
            if (selectedItem.MccDeskription != null)
            {
                autCompTvOperationMccDiscription = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationMccDiscription);
                autCompTvOperationMccDiscription.Text = selectedItem.MccDeskription;
                autCompTvOperationMccDiscription.Focusable = false;
                aut_comp_tv_OperationMccDiscription.EndIconVisible = false;
            }
            else
                aut_comp_tv_OperationMccDiscription.Visibility = ViewStates.Gone;
            #endregion

            #region ChipGroup
            //Распаршевать из строки разделенной пробелмами
            chipGroup = view.FindViewById<ChipGroup>(Resource.Id.chip_group_main);

            HashSet<string> tags = new HashSet<string>();
            var subTtags = DatesRepositorio.DataItems.Select(x => x.Title).OfType<String>().Select(x => x?.Split(" "));
            foreach (var tag in subTtags)
            {
                tags.UnionWith(tag);
            }
            var checkedChips = selectedItem.Title?.Split(" ");
            foreach (string tag in tags)
            {
                bool isChecked = checkedChips?.Contains(tag) ?? false;
                GreateChip(tag, isChecked, inflater);
            }
            #endregion

            #region Total
            var totalSummText = view.FindViewById<TextView>(Resource.Id.TotalSummTextView);
            var totalTransText = view.FindViewById<TextView>(Resource.Id.TotalTransactionTextView);
            var totalSummMccText = view.FindViewById<TextView>(Resource.Id.TotalMccCodeTextView);
            var totalTransMccText = view.FindViewById<TextView>(Resource.Id.TotalTransactionMccTextView);
            #endregion

            #region Recurring

            var recurringDiscrCount = DatesRepositorio.DataItems.Where(x => x.Descripton == selectedItem.Descripton).Count();
            var recurringDiscrSumm = DatesRepositorio.DataItems.Where(x => x.Descripton == selectedItem.Descripton).Select(x => x.Sum).Sum();
            var recurringMccCount = DatesRepositorio.DataItems.Where(x => x.MCC == selectedItem.MCC).Count();
            var recurringMccSumm = DatesRepositorio.DataItems.Where(x => x.MCC == selectedItem.MCC).Select(x => x.Sum).Sum();
            #endregion

            #region Share
            float shareOfTransactions = (float)recurringDiscrCount / DatesRepositorio.DataItems.Count * 100;
            float shareOfSumms = recurringDiscrSumm / DatesRepositorio.DataItems.Select(x => x.Sum).Sum() * 100;
            float shareOfMccTransactions = (float)recurringMccCount / DatesRepositorio.DataItems.Count * 100;
            float shareOfMccSumms = recurringMccSumm / DatesRepositorio.DataItems.Select(x => x.Sum).Sum() * 100;
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

            var plotView = view.FindViewById<PlotView>(Resource.Id.plot_view);
            plotView.Model = CreatePlotModel2(selectedItem.Descripton, shareOfSumms, shareOfTransactions, shareOfMccSumms, shareOfMccTransactions);

            return view;
        }

        private void GreateChip(string tag, bool isChecked, LayoutInflater inflater)
        {
            if (tag != "")
            {
                if (isChecked)
                {
                    var chip = (Chip)inflater.Inflate(Resource.Layout.chip_layot, null, false);
                    chip.CloseIconVisible = false;
                    chip.Checkable = true;
                    chip.Click += (sender, e) => { ((Chip)sender).Checked = true; };
                    chip.Text = tag;
                    chip.Checked = true;
                    chipGroup.AddView(chip);
                }
            }
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





        private int[] ColorSum { get; set; } = new int[3] { 220, 20, 60 };
        private int[] ColorTransCount { get; set; } = new int[3] { 0, 191, 255 };
        private int[] ColorSumMcc { get; set; } = new int[3] { 255, 140, 0 };
        private int[] ColorCountMcc { get; set; } = new int[3] { 46, 139, 87 };
        private int[] BackDarck { get; set; } = new int[3] { 64, 64, 64 };

        private PlotModel CreatePlotModel2(string diskr, float sum, float count, float sumMcc, float countMcc)
        {
            var plotModel1 = new PlotModel
            {
                TitlePadding = 2,
                Title = $"Аналитика",
                //Title = $"{diskr}",
                //plotModel1.Background = OxyColors.LightGray;
                DefaultColors = new List<OxyColor>
                {
                    OxyColors.WhiteSmoke,
                }
            };

            OxyColor shareColor;
            OxyColor textColor;
            Configuration config = this.Resources.Configuration;
            var ThemeMode = config.UiMode == (UiMode.NightYes | UiMode.TypeNormal);
            if (ThemeMode)
            {
                shareColor = OxyColor.FromRgb((byte)BackDarck[0], (byte)BackDarck[1], (byte)BackDarck[2]);
                textColor = OxyColors.WhiteSmoke;
            }
            else
            {
                textColor = OxyColor.FromRgb((byte)BackDarck[0], (byte)BackDarck[1], (byte)BackDarck[2]);
                shareColor = OxyColors.WhiteSmoke;
            }

            var plotModelWidth = plotModel1.Width;

            var pieSeriessumCountMcc = new CustomPieSeries();

            pieSeriessumCountMcc.Diameter = 1;
            pieSeriessumCountMcc.StartAngle = 60;
            pieSeriessumCountMcc.TextColor = textColor;
            pieSeriessumCountMcc.UnVisebleFillColors = shareColor;
            pieSeriessumCountMcc.Slices.Add(new PieSlice("", countMcc)
            {
                Fill = OxyColor.FromRgb((byte)ColorCountMcc[0], (byte)ColorCountMcc[1], (byte)ColorCountMcc[2])
            });
            pieSeriessumCountMcc.Slices.Add(new PieSlice("", 100 - countMcc) { Fill = pieSeriessumCountMcc.UnVisebleFillColors });

            var pieSeriesSumMcc = new CustomPieSeries();
            pieSeriesSumMcc.Diameter = 0.8;
            pieSeriesSumMcc.StartAngle = 40;
            pieSeriesSumMcc.TextColor = textColor;
            pieSeriesSumMcc.UnVisebleFillColors = shareColor;
            pieSeriesSumMcc.Slices.Add(new PieSlice("", sumMcc)
            {
                Fill = OxyColor.FromRgb((byte)ColorSumMcc[0], (byte)ColorSumMcc[1], (byte)ColorSumMcc[2])
            });
            pieSeriesSumMcc.Slices.Add(new PieSlice("", 100 - sumMcc) { Fill = pieSeriesSumMcc.UnVisebleFillColors });

            var pieSeriesCount = new CustomPieSeries();
            pieSeriesCount.Diameter = 0.5;
            pieSeriesCount.StartAngle = 20;
            pieSeriesCount.TextColor = textColor;
            pieSeriesCount.UnVisebleFillColors = shareColor;
            pieSeriesCount.Slices.Add(new PieSlice("", count)
            {
                Fill = OxyColor.FromRgb((byte)ColorTransCount[0], (byte)ColorTransCount[1], (byte)ColorTransCount[2])
            });
            pieSeriesCount.Slices.Add(new PieSlice("", 100 - count) { Fill = pieSeriesCount.UnVisebleFillColors });

            var pieSeriesSum = new CustomPieSeries();
            pieSeriesSum.StartAngle = 0;
            pieSeriesSum.TextColor = textColor;
            pieSeriesSum.UnVisebleFillColors = shareColor;
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
    }
}