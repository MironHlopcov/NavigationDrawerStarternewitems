using Android.OS;

using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using Google.Android.Material.Chip;
using Google.Android.Material.TextField;
using MyFinToControl.Filters;
using MyFinToControl.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace MyFinToControl.Fragments
{
    public class RightMenuNew : Fragment
    {
        public static readonly string TAG = "My:" + typeof(RightMenu).Name.ToUpper();

        private AndroidX.AppCompat.Widget.SearchView sv1;
        private ListView lv1;

        private EditText date_text_edit1;
        private EditText date_text_edit2;
        private Button btn_calendar1;
        private Button btn_calendar2;
        private Button btn_clear_clear1;
        private Button btn_clear_clear2;

        private Button btn_Ok;
        private Button btn_Clear;

        ArrayAdapter adp1;

        #region ExpandableList
        private ExpandableListAdapter listAdapter;
        private ExpandableListView expandableList;
        //private List<string> listHeaderData;
        private Dictionary<ExpandableGroupModel, List<ExpandableChildModel>> listChildData;
        //private List<ExpandableChildModel> groupData;
        #endregion


        private List<string> _FiltredList;
        public FilterItemsNew FilredResultList { get; private set; }

        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler SetFilters;
        public event EventHandler ClousFilter;

        protected virtual void OnSetFilters(object sender, EventArgs e)
        {
            EventHandler handler = SetFilters;
            handler?.Invoke(this, e);
        }
        protected virtual void OnClousFilter(object sender, EventArgs e)
        {
            EventHandler handler = ClousFilter;
            handler?.Invoke(this, e);
        }

        public RightMenuNew()
        {
            _FiltredList = new List<string>();
            listChildData = new Dictionary<ExpandableGroupModel, List<ExpandableChildModel>>();
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            #region SerchView
            View searchFragmt = inflater.Inflate(Resource.Layout.right_menu, container, false);
            sv1 = searchFragmt.FindViewById<SearchView>(Resource.Id.searchView);
            lv1 = searchFragmt.FindViewById<ListView>(Resource.Id.listViewToSearchView);

            adp1 = new ArrayAdapter(this.Context, Android.Resource.Layout.SimpleListItem1, FiltredList);
            lv1.Adapter = adp1;
            adp1.Filter.InvokeFilter("!@$#$^%&%^*&^(*&(*&)(*(&*&^(*^%&$&^#^%#&$"); //todo
            sv1.QueryTextChange += Sv1_QueryTextChange;
            sv1.FocusChange += Sv1_FocusChange;
            lv1.ItemClick += Lv1_ItemClick;
            #endregion

            #region DateEdit
            date_text_edit1 = searchFragmt.FindViewById<EditText>(Resource.Id.text_edit1);
            date_text_edit2 = searchFragmt.FindViewById<EditText>(Resource.Id.text_edit2);
            //text_edit1.Text = DateTime.Now.ToShortDateString();
            //text_edit2.Text = DateTime.Now.ToShortDateString();
            date_text_edit1.TextChanged += Text_edit1_TextChanged;
            date_text_edit2.TextChanged += Text_edit2_TextChanged;

            btn_calendar1 = searchFragmt.FindViewById<Button>(Resource.Id.btn_calendar1);
            btn_calendar2 = searchFragmt.FindViewById<Button>(Resource.Id.btn_calendar2);
            btn_clear_clear1 = searchFragmt.FindViewById<Button>(Resource.Id.btn_clear_clear1);
            btn_clear_clear2 = searchFragmt.FindViewById<Button>(Resource.Id.btn_clear_clear2);

            btn_calendar1.Click += Btn_calendar1_Click;
            btn_calendar2.Click += Btn_calendar2_Click;
            btn_clear_clear1.Click += Btn_clear_clear1_Click;
            btn_clear_clear2.Click += Btn_clear_clear2_Click;

            #endregion

            #region ExpandableList
            expandableList = searchFragmt.FindViewById(Resource.Id.expandList) as ExpandableListView;
            listAdapter = new ExpandableListAdapter(Activity, listChildData);
            expandableList.SetAdapter(listAdapter);
            #endregion

            #region OKClearButton
            btn_Ok = searchFragmt.FindViewById<Button>(Resource.Id.btn_Ok);
            btn_Clear = searchFragmt.FindViewById<Button>(Resource.Id.btn_Clear);

            btn_Ok.Click += Btn_Ok_Click;
            btn_Clear.Click += Btn_Clear_Click;
            #endregion

            // Use this to return your custom view for this Fragment
            return searchFragmt;
        }

        public void AddChekFilterItem(string groupName, List<string> childItems)
        {
            List<ExpandableChildModel> groupData = new List<ExpandableChildModel>();
            foreach (string item in childItems)
            {
                if (item != "0" && item != null) //to do
                    groupData.Add(new ExpandableChildModel(item));
            }
            listChildData.Add(new ExpandableGroupModel { Name = groupName, IsCheked = false }, groupData);

        }
        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

        }



        #region DateEdit
        private void Text_edit1_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (((EditText)sender).Text != "")
                btn_clear_clear1.Visibility = ViewStates.Visible;
            else btn_clear_clear1.Visibility = ViewStates.Gone;
        }
        private void Text_edit2_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (((EditText)sender).Text != "")
                btn_clear_clear2.Visibility = ViewStates.Visible;
            else btn_clear_clear2.Visibility = ViewStates.Gone;
        }
        private void Btn_clear_clear1_Click(object sender, EventArgs e)
        {
            date_text_edit1.Text = "";
        }
        private void Btn_clear_clear2_Click(object sender, EventArgs e)
        {
            date_text_edit2.Text = "";
        }
        private void Btn_calendar1_Click(object sender, EventArgs e)
        {
            new DatePickerFragment(delegate (DateTime time)
            {
                var _selectedDate = time;
                date_text_edit1.Text = "";
                date_text_edit1.Text = _selectedDate.ToLongDateString();
            })
          .Show(ParentFragmentManager, DatePickerFragment.TAG);
        }
        private void Btn_calendar2_Click(object sender, EventArgs e)
        {
            new DatePickerFragment(delegate (DateTime time)
            {
                var _selectedDate = time;
                date_text_edit2.Text = "";
                date_text_edit2.Text = _selectedDate.ToLongDateString();
            })
          .Show(ParentFragmentManager, DatePickerFragment.TAG);
        }
        #endregion

        #region SerchView
        private void Sv1_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            //if (sv1.IsFocused)
            //     sv1.Iconified = false;
            //else
            //     sv1.Iconified = true;
        }
        private void Sv1_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (sv1.Query != "")
            {
                // lv1.Visibility = ViewStates.Visible;
                adp1.Filter.InvokeFilter(e.NewText);
                //sv1.Iconified = false;
                lv1.Visibility = ViewStates.Visible;
            }
            else
            {
                adp1.Filter.InvokeFilter("!@$#$^%&%^*&^(*&(*&)(*(&*&^(*^%&$&^#^%#&$"); //todo
                //sv1.Iconified = true;
            }                                                                         //lv1.Visibility = ViewStates.Invisible;
        }
        private void Lv1_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            sv1.SetQuery(lv1.GetItemAtPosition(e.Position).ToString(), true);
            lv1.Visibility = ViewStates.Gone;
        }

        #endregion

        #region OnButtonClick
        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            FilredResultList = new FilterItemsNew(sv1.Query, new[] { date_text_edit1.Text, date_text_edit2.Text }, listAdapter);
            OnSetFilters(this, e);
        }
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            FilredResultList = null;

            sv1.SetQuery("", true);
            date_text_edit1.Text = "";
            date_text_edit2.Text = "";
            foreach (var item in listChildData)
            {
                item.Key.IsCheked = false;
                item.Value.ForEach(i => i.IsCheked = false);
            }
            listAdapter.NotifyDataSetChanged();
            FilredResultList = new FilterItemsNew(sv1.Query, new[] { date_text_edit1.Text, date_text_edit2.Text }, listAdapter);
            OnSetFilters(this, e);

        }
        #endregion
        public List<string> FiltredList
        {
            get
            {
                return _FiltredList;
            }
            set
            {
                _FiltredList?.Clear();
                _FiltredList.AddRange(value);
            }
        }

    }
    public class FilterItemsNew
    {
        public string SearchDiscriptions { get; private set; }
        public DateTime[] SearchDatas { get; private set; }
        public ExpandableListAdapter ExpandableListAdapter { get; private set; }

        public FilterItemsNew(string DiscriptionsSourse, string[] DatasSourse, ExpandableListAdapter expandableListAdapter)
        {
            SearchDiscriptions = DiscriptionsSourse;
            DateTime d1 = DateTime.TryParse(DatasSourse[0], out d1) ? d1 : default;
            DateTime d2;
            DateTime.TryParse(DatasSourse[1], out d2);
            SearchDatas = new[] { d1, d2 };
            ExpandableListAdapter = expandableListAdapter;
        }
    }
}