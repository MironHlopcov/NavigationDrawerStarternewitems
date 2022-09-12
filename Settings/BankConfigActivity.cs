using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using AndroidX.DrawerLayout.Widget;
using AndroidX.Preference;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Google.Android.Material.Tabs;
using MyFinToControl.Configs.ManagerCore;
using MyFinToControl.Fragments;
using Org.Xmlpull.V1.Sax2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static Android.Icu.Text.Transliterator;
using static AndroidX.RecyclerView.Widget.RecyclerView;
using static MyFinToControl.MainActivity;
using ActionBar = AndroidX.AppCompat.App.ActionBar;

namespace MyFinToControl.Settings
{
    [Activity(Label = "Банки")]
    public class BankConfigActivity : AppCompatActivity, ListView.IOnItemClickListener, ListView.IOnItemLongClickListener
    {
        ListView listView;
        private List<BankConfiguration> dataItems = new List<BankConfiguration>();
        private static CustomBankConfigAdapter adapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_bank_config);
            ActionBar actionBar = this.SupportActionBar;
            if (actionBar != null)
            {
                actionBar.SetDisplayHomeAsUpEnabled(true);
            }

            #region ConfigManager
            ConfigurationManager configManager = ConfigurationManager.ConfigManager;
            var configuration = configManager.BankConfigurationFromJson;
            #endregion

            dataItems.AddRange(configuration.Banks);

            listView = (ListView)FindViewById(Resource.Id.bank_config_dateslistView);
            adapter = new CustomBankConfigAdapter(dataItems, this.ApplicationContext);
            listView.SetAdapter(adapter);

            listView.OnItemClickListener =this;
            listView.OnItemLongClickListener =this;
        }
        protected override void OnResume()
        {
            base.OnResume();
          
        }

        protected override void OnPause()
        {
            base.OnPause();
           
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            //var dialog = new SelectItemDialog(dataItems[position]);
            //dialog.EditItemChange += (sender, e) =>
            //{
            //    dialog.Dismiss();
            //};
            //dialog.Display(Activity.SupportFragmentManager);
        }

        public bool OnItemLongClick(AdapterView parent, View view, int position, long id)
        {
            return true;
        }
    }
}