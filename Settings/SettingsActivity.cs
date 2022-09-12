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
using System.Text;
using static AndroidX.RecyclerView.Widget.RecyclerView;
using static MyFinToControl.MainActivity;
using ActionBar = AndroidX.AppCompat.App.ActionBar;

namespace MyFinToControl.Settings
{
    [Activity(Label = "SettingsActivity", Theme = "@style/AppTheme.NoActionBar")]
    public class SettingsActivity : AppCompatActivity, ISharedPreferencesOnSharedPreferenceChangeListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_settings);
            ActionBar actionBar = this.SupportActionBar;

            if (actionBar != null)
            {
                actionBar.SetDisplayHomeAsUpEnabled(true);
            }
            if (FindViewById(Resource.Id.idFrameLayout1) != null)
            {
                if (savedInstanceState != null)
                {
                    return;
                }
                // below line is to inflate our fragment.
                SupportFragmentManager.BeginTransaction().Add(Resource.Id.idFrameLayout1, new SettingsFragment()).Commit();

            }
        }
        
        protected override void OnResume()
        {
            base.OnResume();
            var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            prefs.RegisterOnSharedPreferenceChangeListener(this);
            
        }

        protected override void OnPause()
        {
            base.OnPause();
            var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            prefs.UnregisterOnSharedPreferenceChangeListener(this);
        }
        #region ISharedPreferencesOnSharedPreferenceChangeListener implementation
        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
           
        }
        #endregion
    }
}