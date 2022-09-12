using Android.App;
using Android.Content;
using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Java.Lang;
using AndroidX.Preference;
using MyFinToControl.Settings;

namespace MyFinToControl.Fragments
{
    public class SettingsFragment : AndroidX.Preference.PreferenceFragmentCompat
    {
        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            AddPreferencesFromResource(Resource.Xml.settings);
        }
        public override bool OnPreferenceTreeClick(Preference preference)
        {
            var key = preference.Key;
            if (key=="key_start_View")
            {
                Intent intent = new Intent(this.Context, typeof(BankConfigActivity));
                StartActivity(intent);
            }
            return false;
        }


    }
}