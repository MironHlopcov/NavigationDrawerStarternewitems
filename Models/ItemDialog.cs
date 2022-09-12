using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;

using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using AndroidX.Fragment.App;
using EfcToXamarinAndroid.Core;
using Google.Android.Material.Chip;
using Google.Android.Material.TextField;
using MyFinToControl.Models;
using MyFinToControl.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFinToControl.Fragments
{
    public class ItemDialog : AndroidX.Fragment.App.DialogFragment, View.IOnClickListener
    {
        public static string TAG = typeof(ItemDialog).Name;

        private Toolbar toolbar;
        private Android.Widget.AutoCompleteTextView autocompleteTVOperTyp;
        private TextInputLayout wrap_aut_comp_tv_OperationTyp;
        private TextInputEditText summOper;
        private TextInputLayout wrap_texstInput_OperationTSumm;
        private Android.Widget.AutoCompleteTextView autCompTvOperationDiscription;
        private Android.Widget.AutoCompleteTextView autCompTvOperationMccCode;
        private Android.Widget.AutoCompleteTextView autCompTvOperationMccDiscription;

        private TextInputLayout textfieldDateCheck;
        private TextInputEditText date_text_edit1;

        private TextInputLayout textfieldTimeCheck;
        private TextInputEditText date_text_edit2;

        private TextInputEditText texstInput_CreateChip;
        private ChipGroup chipGroup;

        private List<AddedItemRow> AddedRows = new List<AddedItemRow>();

        private DataItem selectedItem;

        public ItemDialog(DataItem dataItem)
        {
            selectedItem = dataItem;
        }

        public void Display(AndroidX.Fragment.App.FragmentManager fragmentManager)
        {

            //AddItemDialog exampleDialog = new AddItemDialog();
            this.Show(fragmentManager, TAG);
            //return this;
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
                //dialog.Window.SetWindowAnimations(Resource.Style.AppTheme_Slide);
            }
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            // View view = inflater.Inflate(Resource.Layout.edititem_dialog, container, false);
            View view = inflater.Inflate(Resource.Layout.testItem_dialog, container, false);
            toolbar = (Toolbar)view.FindViewById(Resource.Id.toolbar);

            #region OperTyp
            wrap_aut_comp_tv_OperationTyp = view.FindViewById<TextInputLayout>(Resource.Id.wrap_aut_comp_tv_OperationTyp);
            var operTypList = typeof(OperacionTyps).GetEnumNames();
            var operTypAdapter = new Android.Widget.ArrayAdapter<string>(this.Context, (int)Resource.Layout.dropdown_item, operTypList);
            autocompleteTVOperTyp = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationTyp);
            autocompleteTVOperTyp.Text = selectedItem.OperacionTyp.ToString();
            autocompleteTVOperTyp.Adapter = operTypAdapter;

            #endregion

            #region Summ
            wrap_texstInput_OperationTSumm = view.FindViewById<TextInputLayout>(Resource.Id.wrap_texstInput_OperationTSumm);
            summOper = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_OperationTSumm);
            //summOper.SetText(string.Format("{0:F}", selectedItem.Sum.ToString()).ToCharArray(), 0, string.Format("{0:F}", selectedItem.Sum.ToString()).Length);
            summOper.Text = string.Format("{0:F}", selectedItem.Sum);
            #endregion

            #region DateEdit

            textfieldDateCheck = view.FindViewById<TextInputLayout>(Resource.Id.textfield_DateCheck);
            textfieldDateCheck.Tag = "textfieldDateCheck_Tag";

            textfieldDateCheck.SetStartIconOnClickListener(this);
            textfieldDateCheck.SetEndIconOnClickListener(this);

            date_text_edit1 = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_date);
            date_text_edit1.Text = selectedItem.Date.ToShortDateString();

            if (selectedItem.ParentId == 0)
                date_text_edit1.Enabled = false;
            #endregion

            #region TimeEdit

            textfieldTimeCheck = view.FindViewById<TextInputLayout>(Resource.Id.textfield_TimeCheck);
            textfieldTimeCheck.Tag = "textfieldTimeCheck_Tag";

            textfieldTimeCheck.SetStartIconOnClickListener(this);
            textfieldTimeCheck.SetEndIconOnClickListener(this);

            date_text_edit2 = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_time);
            date_text_edit2.Text = selectedItem.Date.ToLongTimeString();

            if (selectedItem.ParentId == 0)
                date_text_edit2.Enabled = false;
            #endregion

            #region OperationDiscription
            autCompTvOperationDiscription = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationDiscription);
            var operationDiscriptionAdapter =
               new Android.Widget.ArrayAdapter<string>(this.Context, (int)Resource.Layout.dropdown_item, DatesRepositorio.DataItems.
               Select(x => x.Descripton).Where(x => x != null).Distinct().ToArray());
            autCompTvOperationDiscription.Adapter = operationDiscriptionAdapter;
            autCompTvOperationDiscription.Text = selectedItem.Descripton;
            #endregion

            #region OperationMccCode
            autCompTvOperationMccCode = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationMccCode);
            var operationMccCodeAdapter =
               new Android.Widget.ArrayAdapter<int>(this.Context, (int)Resource.Layout.dropdown_item, DatesRepositorio.DataItems.
               Select(x => x.MCC).Where(x => x != null).Distinct().ToArray());
            autCompTvOperationMccCode.Adapter = operationMccCodeAdapter;
            autCompTvOperationMccCode.Text = selectedItem.MCC.ToString();
            #endregion

            #region OperationMccDiscription
            autCompTvOperationMccDiscription = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationMccDiscription);
            var operationMccDiscriptionAdapter =
               new Android.Widget.ArrayAdapter<string>(this.Context, (int)Resource.Layout.dropdown_item, DatesRepositorio.DataItems.
               Select(x => x.MccDeskription).Where(x => x != null).Distinct().ToArray());
            autCompTvOperationMccDiscription.Adapter = operationMccDiscriptionAdapter;
            autCompTvOperationMccDiscription.Text = selectedItem.MccDeskription;
            #endregion

            #region CreateChipInput
            texstInput_CreateChip = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_CreateChip);

            texstInput_CreateChip.EditorAction += TexstInput_CreateChip_EditorAction;
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

            return view;
        }

        private void TexstInput_CreateChip_EditorAction(object sender, Android.Widget.TextView.EditorActionEventArgs e)
        {
            var tags = texstInput_CreateChip.Text.Trim(' ').Split(" ");
            var inflater = LayoutInflater.From(this.Context);

            var chipsText = new List<string>();
            for (int i = 0; i < chipGroup.ChildCount; i++)
            {
                chipsText.Add(((Chip)chipGroup.GetChildAt(i)).Text);
            }
            foreach (string tag in tags)
            {

                if (chipsText.Any(x => x == tag))
                {
                    Android.Widget.Toast.MakeText(this.Context, $"Тег {tag} уже существует", Android.Widget.ToastLength.Short).Show();
                    continue;
                }
                GreateChip(tag, false, inflater);
            }
            texstInput_CreateChip.Text = "";
            InputMethodManager imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(texstInput_CreateChip.WindowToken, 0);

        }
        private void GreateChip(string tag, bool isChecked, LayoutInflater inflater)
        {
            if (tag != "")
            {
                var chip = (Chip)inflater.Inflate(Resource.Layout.chip_layot, null, false);
                chip.Checkable = true;
                chip.Text = tag;
                chip.SetOnCloseIconClickListener(this);
                if (isChecked)
                    chip.Checked = true;
                chipGroup.AddView(chip);
            }
        }


        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            toolbar.SetNavigationOnClickListener(this);
            //toolbar.Title = "Редактировать";
            toolbar.InflateMenu(Resource.Menu.addItem_dialog);
            toolbar.MenuItemClick += Toolbar_MenuItemClick;
        }
        private bool IsInputCorrect()
        {
            bool isError = false;
            if (autocompleteTVOperTyp.Text == "")
            {
                wrap_aut_comp_tv_OperationTyp.Error = "Поле обязательно для заполнения";
                isError = true;
            }
            else
                wrap_aut_comp_tv_OperationTyp.ErrorEnabled = false;

            if (summOper.Text == "")
            {
                wrap_texstInput_OperationTSumm.Error = "Поле обязательно для заполнения";
                isError = true;
            }
            else
                wrap_aut_comp_tv_OperationTyp.ErrorEnabled = false;
            #region DateCheck
            if (date_text_edit1.Text == "")
            {
                textfieldDateCheck.Error = "Поле обязательно для заполнения";
                isError = true;
            }
            else
                textfieldDateCheck.ErrorEnabled = false;

            if (!DateTime.TryParse(date_text_edit1.Text, out DateTime dateResult))
            {
                textfieldDateCheck.Error = "Не удается преобразовать значение к требуемому формату";
                isError = true;
            }
            else
                textfieldDateCheck.ErrorEnabled = false;
            #endregion
            #region TimeCheck
            if (date_text_edit2.Text == "")
            {
                textfieldTimeCheck.Error = "Поле обязательно для заполнения";
                isError = true;
            }
            else
                textfieldTimeCheck.ErrorEnabled = false;

            if (!DateTime.TryParse(date_text_edit2.Text, out DateTime timeResult))
            {
                textfieldTimeCheck.Error = "Не удается преобразовать значение к требуемому формату";
                isError = true;
            }
            else
                textfieldTimeCheck.ErrorEnabled = false;
            #endregion
            if (isError)
                return false;
            return true;
        }
        private void Toolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {

            if (!IsInputCorrect())
                return;

            DataItem item = new DataItem(Enum.Parse<OperacionTyps>(autocompleteTVOperTyp.Text), DateTime.Parse(date_text_edit1.Text + " " + date_text_edit2.Text));

            item.Sum = ParseStringToFloat.GetFloat(summOper.Text);
            // item.Sum = float.TryParse(summOper.Text, out float outSumm) ? outSumm : default;
            item.Descripton = autCompTvOperationDiscription.Text;
            item.MCC = int.TryParse(autCompTvOperationMccCode.Text, out int outMcc) ? outMcc : default;
            item.MccDeskription = autCompTvOperationMccDiscription.Text;

            var checkedTipsId = chipGroup.CheckedChipIds;
            var chipsText = new List<string>();
            foreach (var id in checkedTipsId)
            {
                Chip chip = (Chip)chipGroup.FindViewById(id.IntValue());
                chipsText.Add(chip.Text);
            }

            item.Title = String.Join(" ", chipsText.ToArray());

            if (item.Equals(selectedItem))
            {
                Dismiss();
                return;
            }
            DatesRepositorio.UpdateItemValue(selectedItem.Id, item);
            OnEditItemChange(this, e);
        }

        private void Toolbar_NavigationClick(object sender, Toolbar.NavigationClickEventArgs e)
        {
            Dismiss();
        }

        public void OnClick(View v)
        {
            if (v is Chip)
            {
                chipGroup.RemoveView(v);
                return;
            }
            var viewPar = (ViewGroup)v.Parent.Parent.Parent;
            switch (v.Id)
            {
                case Resource.Id.text_input_start_icon:
                    if (viewPar.Tag.ToString() == "textfieldDateCheck_Tag")
                    {
                        new DatePickerFragment(delegate (DateTime datetime)
                        {
                            var _selectedDate = datetime;
                            date_text_edit1.Text = "";
                            date_text_edit1.Text = _selectedDate.ToLongDateString();
                        })
                       .Show(ParentFragmentManager, DatePickerFragment.TAG);
                        textfieldDateCheck.EndIconVisible = true;
                    }
                    if (viewPar.Tag.ToString() == "textfieldTimeCheck_Tag")
                    {
                        new TimePickerFragment(delegate (DateTime datetime)
                        {
                            var _selectedDate = datetime;
                            date_text_edit2.Text = "";
                            date_text_edit2.Text = _selectedDate.ToShortTimeString();
                        })
                        .Show(ParentFragmentManager, TimePickerFragment.TAG);
                        textfieldTimeCheck.EndIconVisible = true;
                    }
                    break;
                case Resource.Id.text_input_end_icon:
                    if (((View)(viewPar.Parent)).Tag.ToString() == "textfieldDateCheck_Tag")
                    {
                        date_text_edit1.Text = "";
                        textfieldDateCheck.EndIconVisible = false;
                    }
                    if (((View)(viewPar.Parent)).Tag.ToString() == "textfieldTimeCheck_Tag")
                    {
                        date_text_edit2.Text = "";
                        textfieldTimeCheck.EndIconVisible = false;
                    }
                    break;

                default:
                    v.Dispose();
                    v = null;
                    var fragment = (AndroidX.Fragment.App.DialogFragment)FragmentManager.FindFragmentByTag(typeof(EditItemDialog).Name);
                    fragment?.Dismiss();
                    break;

            }
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            dialog.Dismiss();
            // RequireActivity().OnBackPressed();


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