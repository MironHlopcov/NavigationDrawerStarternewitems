using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using Java.Lang;
using MyFinToControl.Filters;
using MyFinToControl.Models;
using MyFinToControl.NoScrollExList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SearchView = AndroidX.AppCompat.Widget.SearchView;



namespace MyFinToControl.Fragments
{
    public class RightMenu : Fragment
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
        //private ExpandableListView expandableList;
        private NoScrollExListView expandableList;
        //private List<string> listHeaderData;
        private Dictionary<ExpandableGroupModel, List<ExpandableChildModel>> listChildData;
        //private List<ExpandableChildModel> groupData;
        #endregion


        private List<string> _FiltredList;
        public FilterItems FilredResultList { get; private set; }

        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler FiltersSet;
        public event EventHandler ClousFilter;

        protected virtual void OnSetFilters(object sender, EventArgs e)
        {
            EventHandler handler = FiltersSet;
            handler?.Invoke(this, e);
        }
        protected virtual void OnClousFilter(object sender, EventArgs e)
        {
            EventHandler handler = ClousFilter;
            handler?.Invoke(this, e);
        }

        public RightMenu()
        {
            _FiltredList = new List<string>();
            listChildData = new Dictionary<ExpandableGroupModel, List<ExpandableChildModel>>();
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetStyle(AndroidX.Fragment.App.DialogFragment.StyleNormal, Resource.Style.AppTheme_FullScreenDialog);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            #region SerchView
            View searchFragmt = inflater.Inflate(Resource.Layout.right_menu, container, false);
            sv1 = searchFragmt.FindViewById<SearchView>(Resource.Id.searchView);
            lv1 = searchFragmt.FindViewById<NoScrollListView>(Resource.Id.listViewToSearchView);
            lv1.Visibility = ViewStates.Invisible;

            //adp1 = new ArrayAdapter(this.Context, Android.Resource.Layout.SimpleListItem1, FiltredList);
            adp1 = new MyArrayAdapter(this.Context, Android.Resource.Layout.SimpleListItem1, FiltredList);

            lv1.Adapter = adp1;
            adp1.Filter.InvokeFilter("!@$#$^%&%^*&^(*&(*&)(*(&*&^(*^%&$&^#^%#&$"); //todo
            sv1.QueryTextChange += Sv1_QueryTextChange;

            sv1.FocusChange += FocusChange;
            lv1.ItemClick += Lv1_ItemClick;
            #endregion

            #region DateEdit
            date_text_edit1 = searchFragmt.FindViewById<EditText>(Resource.Id.text_edit1);
            date_text_edit2 = searchFragmt.FindViewById<EditText>(Resource.Id.text_edit2);
            //text_edit1.Text = DateTime.Now.ToShortDateString();
            //text_edit2.Text = DateTime.Now.ToShortDateString();
            date_text_edit1.TextChanged += Text_edit1_TextChanged;
            date_text_edit2.TextChanged += Text_edit2_TextChanged;
            date_text_edit1.FocusChange += FocusChange;
            date_text_edit2.FocusChange += FocusChange;

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
            // expandableList = searchFragmt.FindViewById(Resource.Id.expandList) as ExpandableListView;
            expandableList = searchFragmt.FindViewById(Resource.Id.expandList) as NoScrollExListView;
            listAdapter = new ExpandableListAdapter(Activity, listChildData);
            expandableList.SetAdapter(listAdapter);
            expandableList.GroupExpand += ExpandableList_Click;
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

        #region ExpandebleList
        private void ExpandableList_Click(object sender, EventArgs e)
        {
            lv1.Visibility = ViewStates.Gone;
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
        #endregion

        #region DateEdit
        private void Text_edit1_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            lv1.Visibility = ViewStates.Gone;

            if (((EditText)sender).Text != "")
                btn_clear_clear1.Visibility = ViewStates.Visible;
            else btn_clear_clear1.Visibility = ViewStates.Gone;
        }
        private void Text_edit2_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            lv1.Visibility = ViewStates.Gone;

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
            lv1.Visibility = ViewStates.Gone;
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
            lv1.Visibility = ViewStates.Gone;
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
        private void FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (sv1.IsFocused)
                lv1.Visibility = ViewStates.Visible;
            else
                lv1.Visibility = ViewStates.Invisible;
        }
        private void Sv1_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (sv1.Query.Count() > 0)
            {
                adp1.Filter.InvokeFilter(e.NewText);
                lv1.Visibility = ViewStates.Visible;
            }
            else
            {
                adp1.Filter.InvokeFilter("!@$#$^%&%^*&^(*&(*&)(*(&*&^(*^%&$&^#^%#&$"); //todo
            }
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
            string[] lvItems = new string[lv1.Adapter.Count];
            for (int i = 0; i < lv1.Adapter.Count; i++)
            {
                lvItems[i] = lv1.Adapter.GetItem(i).ToString();
            }

            FilredResultList = new FilterItems(lvItems, new[] { date_text_edit1.Text, date_text_edit2.Text }, listAdapter);
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
            string[] lvItems = new string[lv1.Adapter.Count];
            for (int i = 0; i < lv1.Adapter.Count; i++)
            {
                lvItems[i] = lv1.Adapter.GetItem(i).ToString();
            }

            FilredResultList = new FilterItems(new string[] { }, new[] { date_text_edit1.Text, date_text_edit2.Text }, listAdapter);
            OnSetFilters(this, e);

        }
        #endregion

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

        }
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


    public class FilterItems
    {
        public string[] SearchDiscriptions { get; private set; }
        public DateTime[] SearchDatas { get; private set; }
        public ExpandableListAdapter ExpandableListAdapter { get; private set; }

        public FilterItems(string[] DiscriptionsSourse, string[] DatasSourse, ExpandableListAdapter expandableListAdapter)
        {
            SearchDiscriptions = DiscriptionsSourse;
            DateTime d1 = DateTime.TryParse(DatasSourse[0], out d1) ? d1 : default;
            DateTime d2;
            DateTime.TryParse(DatasSourse[1], out d2);
            SearchDatas = new[] { d1, d2 };
            ExpandableListAdapter = expandableListAdapter;
        }
    }

    public class MyArrayAdapter : ArrayAdapter<string>
    {
        private List<string> originalList;
        private MyAdapterFilter filter;

        public MyArrayAdapter(Context context, int textViewResourceId, List<string> objects) : base(context, textViewResourceId, objects)
        {
            originalList = objects;
        }
        public override Filter Filter
        {
            get
            {
                if (filter == null)
                {
                    filter = new MyAdapterFilter();
                    filter.originalList = originalList;
                    filter.FiltersSet += (sender, result) =>
                    {
                        this.Clear();
                        this.AddAll(result);
                        this.NotifyDataSetChanged();
                    };
                }
                return filter;
            }
        }
    }

    public class MyAdapterFilter : Filter
    {
        public List<string> originalList = new List<string>();
        public List<string> resultList = new List<string>();
        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            FilterResults result = new FilterResults();
            if (constraint != null && constraint.ToString().Length > 0)
            {
                List<string> filteredItems = new List<string>();
                for (int i = 0, l = originalList.Count; i < l; i++)
                {
                    string country = originalList[i];
                    if (country.ToLower().Contains(constraint.ToString().ToLower()))
                        filteredItems.Add(country);
                }
                resultList = filteredItems;
                //result.Count = filteredItems.Count;
                //result.Values = filteredItems.ToArray();
            }
            else
            {
                resultList = originalList;
                //result.Values = originalList.ToArray();
                //result.Count = originalList.Count;
            }
            return result;
        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {

            OnFiltersSet(this, resultList);
        }

        public delegate void EventHandler(object sender, List<string> results);
        public event EventHandler FiltersSet;
        protected virtual void OnFiltersSet(object sender, List<string> results)
        {
            EventHandler handler = FiltersSet;
            handler?.Invoke(this, results);
        }

        public string[] GetFiltredList()
        {
            return resultList.ToArray();
        }






    }


}
