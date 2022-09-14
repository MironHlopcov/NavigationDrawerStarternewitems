using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;

using AndroidX.AppCompat.Widget;
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
    public class AddItemDialog : AndroidX.Fragment.App.DialogFragment, View.IOnClickListener
    {
        public static string TAG = typeof(AddItemDialog).Name;

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

        //  private List<AddedItemRow> AddedRows = new List<AddedItemRow>();

        public AddItemDialog()
        {

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
            //View view = inflater.Inflate(Resource.Layout.addItem_dialog, container, false);
            View view = inflater.Inflate(Resource.Layout.testItem_dialog, container, false);
            toolbar = (Toolbar)view.FindViewById(Resource.Id.toolbar);

            #region OperTyp
            wrap_aut_comp_tv_OperationTyp = view.FindViewById<TextInputLayout>(Resource.Id.wrap_aut_comp_tv_OperationTyp);
            var operTypList = typeof(OperacionTyps).GetEnumNames().SkipLast(1).ToArray();
            var operTypAdapter = new Android.Widget.ArrayAdapter<string>(this.Context, (int)Resource.Layout.dropdown_item, operTypList);
            autocompleteTVOperTyp = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationTyp);
            autocompleteTVOperTyp.Adapter = operTypAdapter;
            #endregion

            #region Summ
            wrap_texstInput_OperationTSumm = view.FindViewById<TextInputLayout>(Resource.Id.wrap_texstInput_OperationTSumm);
            summOper = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_OperationTSumm);
            #endregion

            #region DateEdit
            textfieldDateCheck = view.FindViewById<TextInputLayout>(Resource.Id.textfield_DateCheck);
            textfieldDateCheck.Tag = "textfieldDateCheck_Tag";

            textfieldDateCheck.SetStartIconOnClickListener(this);
            textfieldDateCheck.SetEndIconOnClickListener(this);

            date_text_edit1 = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_date);
            date_text_edit1.Text = DateTime.Now.ToShortDateString();
            #endregion

            #region TimeEdit
            textfieldTimeCheck = view.FindViewById<TextInputLayout>(Resource.Id.textfield_TimeCheck);
            textfieldTimeCheck.Tag = "textfieldTimeCheck_Tag";

            textfieldTimeCheck.SetStartIconOnClickListener(this);
            textfieldTimeCheck.SetEndIconOnClickListener(this);

            date_text_edit2 = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_time);
            date_text_edit2.Text = DateTime.Now.ToShortTimeString();

            #endregion

            #region OperationDiscription
            autCompTvOperationDiscription = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationDiscription);
            var operationDiscriptionAdapter = DatesRepositorio.DataItems==null ? null :
               new Android.Widget.ArrayAdapter<string>(this.Context, (int)Resource.Layout.dropdown_item, DatesRepositorio.DataItems.
               Select(x => x.Descripton).Where(x => x != null).Distinct().ToArray()); 
            autCompTvOperationDiscription.Adapter = operationDiscriptionAdapter;
            #endregion

            #region OperationMccCode
            autCompTvOperationMccCode = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationMccCode);
            var operationMccCodeAdapter = DatesRepositorio.DataItems == null ? null :
               new Android.Widget.ArrayAdapter<int>(this.Context, (int)Resource.Layout.dropdown_item, DatesRepositorio.DataItems.
               Select(x => x.MCC).Where(x => x != null).Distinct().ToArray());
            autCompTvOperationMccCode.Adapter = operationMccCodeAdapter;
            #endregion

            #region OperationMccDiscription
            autCompTvOperationMccDiscription = view.FindViewById<Android.Widget.AutoCompleteTextView>(Resource.Id.aut_comp_tv_OperationMccDiscription);
            var operationMccDiscriptionAdapter = DatesRepositorio.DataItems == null ? null :
               new Android.Widget.ArrayAdapter<string>(this.Context, (int)Resource.Layout.dropdown_item, DatesRepositorio.DataItems.
               Select(x => x.MccDeskription).Where(x => x != null).Distinct().ToArray());
            autCompTvOperationMccDiscription.Adapter = operationMccDiscriptionAdapter;
            #endregion

            #region CreateChipInput
            texstInput_CreateChip = view.FindViewById<TextInputEditText>(Resource.Id.texstInput_CreateChip);
            texstInput_CreateChip.TextChanged += Tv_CreateChip_TextChanged;
            texstInput_CreateChip.EditorAction += TexstInput_CreateChip_EditorAction;
            #endregion

            #region ChipGroup

            chipGroup = view.FindViewById<ChipGroup>(Resource.Id.chip_group_main);

            //            HashSet<string> tags = new HashSet<string>();
            //            var subTtags = DatesRepositorio.DataItems == null ? null : DatesRepositorio.DataItems.Select(x => x.Title).OfType<String>().Select(x => x?.Split(" "));
            //            if (subTtags != null)
            //            {
            //                foreach (var tag in subTtags)
            //                {
            //                   tags.UnionWith(tag);
            //               }
            var tags = DatesRepositorio.GetTags();
            if (tags != null)
            {
                foreach (string tag in tags)
                {
                    GreateChip(tag, inflater);
                }
            }
            //            }

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
                GreateChip(tag, inflater);
            }
            texstInput_CreateChip.Text = "";

            InputMethodManager imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(texstInput_CreateChip.WindowToken, 0);
        }
        private void GreateChip(string tag, LayoutInflater inflater)
        {
            if (tag != "")
            {
                var chip = (Chip)inflater.Inflate(Resource.Layout.chip_layot, null, false);
                chip.Checkable = true;
                chip.Text = tag;
                chip.SetOnCloseIconClickListener(this);
                chipGroup.AddView(chip);
            }
        }
        private void Tv_CreateChip_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            toolbar.SetNavigationOnClickListener(this);
            toolbar.Title = "Новая транзакция";
            toolbar.InflateMenu(Resource.Menu.addItem_dialog);
            toolbar.MenuItemClick += Toolbar_MenuItemClick;
        }
        private void Toolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
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
            if (ParseStringToFloat.GetFloat(summOper.Text) == 0)
            {
                wrap_texstInput_OperationTSumm.Error = "Сумма операции должна быть больше 0 и не может превышать " +
                    "340 млрд.";
                isError = true;
            }
            else
                wrap_aut_comp_tv_OperationTyp.ErrorEnabled = false;

            DateTime itDateTime = DateTime.Parse(date_text_edit1.Text + " " + date_text_edit2.Text);
            if (DatesRepositorio.DataItems != null)
            {
                while (DatesRepositorio.DataItems.Any(x => x?.Date == itDateTime))
                {
                    if (itDateTime.Second == 59)
                        itDateTime = itDateTime.AddSeconds(-59);
                    else
                        itDateTime = itDateTime.AddSeconds(1);
                }
            }

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
            if (itDateTime.Date > DateTime.Now.Date)
            {
                textfieldDateCheck.Error = "Нельзя создавать транзакции будущих периодов!";
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

            if (itDateTime.TimeOfDay > DateTime.Now.TimeOfDay)
            {
                textfieldTimeCheck.Error = "Нельзя создавать транзакции будущих периодов!";
                isError = true;
            }
            else
                textfieldTimeCheck.ErrorEnabled = false;

            #endregion
            if (isError)
                return;

            DataItem item = new DataItem(Enum.Parse<OperacionTyps>(autocompleteTVOperTyp.Text), itDateTime);

            item.Sum = ParseStringToFloat.GetFloat(summOper.Text);
            // item.Sum = float.TryParse(summOper.Text, out float outSumm) ? outSumm : default;
            item.Descripton = autCompTvOperationDiscription.Text;
            item.MCC = int.TryParse(autCompTvOperationMccCode.Text, out int outMcc) ? outMcc : default;
            item.MccDeskription = autCompTvOperationMccDiscription.Text;
            item.ParentId = -1; //-1 для записей созданных пользователем
            var checkedTipsId = chipGroup.CheckedChipIds;
            var chipsText = new List<string>();
            foreach (var id in checkedTipsId)
            {
                Chip chip = (Chip)chipGroup.FindViewById(id.IntValue());
                chipsText.Add(chip.Text);
            }

            item.Title = String.Join(" ", chipsText.ToArray());

            DatesRepositorio.AddDatas(new List<DataItem> { item });

            OnAddedItem(this, e);
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
                    var fragment = (AndroidX.Fragment.App.DialogFragment)FragmentManager.FindFragmentByTag(typeof(AddItemDialog).Name);
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

        public event EventHandler AddedItem;

        protected void OnAddedItem(object sender, EventArgs e)
        {
            EventHandler handler = AddedItem;
            handler?.Invoke(this, e);
        }
    }



}