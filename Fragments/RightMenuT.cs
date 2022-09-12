using Android.OS;

using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using MyFinToControl.Filters;
using MyFinToControl.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace MyFinToControl.Fragments
{
    public class RightMenuT<T> : Fragment
    {
        public static readonly string TAG = "My:" + typeof(RightMenuT<T>).Name.ToUpper();

        private AndroidX.AppCompat.Widget.SearchView searchView;
        private ListView searchList;

        private EditText date_text_edit1;
        private EditText date_text_edit2;
        private Button btn_calendar1;
        private Button btn_calendar2;
        private Button btn_clear_clear1;
        private Button btn_clear_clear2;

        private Button btn_Ok;
        private Button btn_Clear;

        ArrayAdapter adp1;
        private List<T> _FiltredList;
        public FilterItems FilredResultList { get; private set; }

        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler SetFilters;




        //
        string SearchProperName;
        string DatePropName;
        string[] UnvisibleProps;
        //


        protected virtual void OnSetFilters(object sender, EventArgs e)
        {
            EventHandler handler = SetFilters;
            handler?.Invoke(this, e);
        }


        private ExpandableListAdapter listAdapter;
        private ExpandableListView expandableList;
        private List<string> listHeaderData;
        private Dictionary<ExpandableGroupModel, List<ExpandableChildModel>> listChildData;
        private List<ExpandableChildModel> groupData;

        public RightMenuT(string searchProperName, string datePropName, string[] unvisibleProps, List<T> filtredList)
        {
            _FiltredList = new List<T>();

            SearchProperName = searchProperName;
            DatePropName = datePropName;
            UnvisibleProps= unvisibleProps;
            FiltredList = filtredList;
           

            listChildData = new Dictionary<ExpandableGroupModel, List<ExpandableChildModel>>();
            List<ExpandableChildModel> chldrnList = new List<ExpandableChildModel>();

            var propertis = typeof(T).GetProperties();
            foreach (var prop in propertis)
            {
                if(UnvisibleProps.Contains(prop.Name))
                    continue;
                

                var propName = typeof(T).GetProperty(prop.Name);
                var propValues = FiltredList.Select(x => x.GetType().GetProperty(prop.Name).GetValue(x, null).ToString()).Distinct();

                foreach (var child in propValues)
                {
                    chldrnList.Add(new ExpandableChildModel(child));
                }

                listChildData.Add(new ExpandableGroupModel(prop.Name), chldrnList);

                //var dfgh = FiltredList.GetType().GetProperty(prop.Name);
                //var asdfasdf = _FiltredList.Select(x=>x.GetType().GetProperty(prop.Name)).ToList();
                //var sdfg = _FiltredList.Select(x => typeof(T).GetProperty(prop.Name));
                //chldrnList.AddRange(typeof(T));

                //listChildData.Add(new ExpandableGroupModel(prop.Name), new  )
            }

            
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            #region SerchView
            View searchFragmt = inflater.Inflate(Resource.Layout.right_menu, container, false);
            searchView = searchFragmt.FindViewById<SearchView>(Resource.Id.searchView);
            searchList = searchFragmt.FindViewById<ListView>(Resource.Id.listViewToSearchView);

           
            var sprop = FiltredList.Select(x => x.GetType().GetProperty(SearchProperName).GetValue(x, null)).Distinct().ToList();
            adp1 = new ArrayAdapter(this.Context, Android.Resource.Layout.SimpleListItem1, sprop);
            searchList.Adapter = adp1;
            adp1.Filter.InvokeFilter("!@$#$^%&%^*&^(*&(*&)(*(&*&^(*^%&$&^#^%#&$"); //todo
            searchView.QueryTextChange += Sv1_QueryTextChange;
            searchView.FocusChange += Sv1_FocusChange;
            searchList.ItemClick += Lv1_ItemClick;
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

            #region OKClearButton
            btn_Ok = searchFragmt.FindViewById<Button>(Resource.Id.btn_Ok);
            btn_Clear = searchFragmt.FindViewById<Button>(Resource.Id.btn_Clear);

            btn_Ok.Click += Btn_Ok_Click;
            btn_Clear.Click += Btn_Clear_Click;
            #endregion

            // Use this to return your custom view for this Fragment
            return searchFragmt;
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
            date_text_edit1.Text = "";
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
            if (searchView.Query != "")
            {
                // lv1.Visibility = ViewStates.Visible;
                adp1.Filter.InvokeFilter(e.NewText);
                //sv1.Iconified = false;
            }
            else
            {
                adp1.Filter.InvokeFilter("!@$#$^%&%^*&^(*&(*&)(*(&*&^(*^%&$&^#^%#&$"); //todo
                //sv1.Iconified = true;
            }                                                                         //lv1.Visibility = ViewStates.Invisible;
        }
        private void Lv1_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            searchView.SetQuery(searchList.GetItemAtPosition(e.Position).ToString(), true);
            searchList.Visibility = ViewStates.Gone;
        }
      
        #endregion

        #region OKClearButton
        private void Btn_Ok_Click(object sender, EventArgs e)
        {
          //  FilredResultList = new FilterItems(searchView.Query, new[] { date_text_edit1.Text, date_text_edit2.Text }, listAdapter);
            OnSetFilters(this, e);
        }
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            FilredResultList = null;

            searchView.SetQuery("", true);
            date_text_edit1.Text = "";
            date_text_edit2.Text = "";

            OnSetFilters(this, e);

        }
        #endregion



        public List<T> FiltredList
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
    public class FilterItemsT
    {
        public string SearchDiscriptions { get; private set; }
        public string[] SearchDatas { get; private set; }

        public FilterItemsT(SearchView DiscriptionsSourse, EditText[] DatasSourse)
        {
            SearchDiscriptions = DiscriptionsSourse.Query;
            SearchDatas = new[] { DatasSourse[0].Text, DatasSourse[1].Text };
        }
    }
}
