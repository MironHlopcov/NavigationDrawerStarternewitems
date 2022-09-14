using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;

using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using AndroidX.Preference;
using AndroidX.ViewPager2.Widget;
using EfcToXamarinAndroid.Core;
using Google.Android.Material.Badge;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;
using Google.Android.Material.Tabs;
using MyFinToControl.Configs.ManagerCore;
using MyFinToControl.Fragments;
using MyFinToControl.Models;
using MyFinToControl.Parsers;
using MyFinToControl.Settings;
using Xamarin.Essentials;

namespace MyFinToControl
{
    [Android.App.Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public partial class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        DrawerLayout drawer;
        RightMenu _RightMenu;
        //RightMenuNew _RightMenuNew;
        //RightMenuT<DataItem> _RightMenuT;

        private static int[] tabIcons;

        TabLayout tabLayout;
        ViewPager2 pager;
        CustomViewPager2Adapter adapter;

        //private static List<BankConfiguration> smsFilters = new List<BankConfiguration>();

        private SmsReceiver smsBroadcastReceiver;

        protected override async void OnCreate(Bundle savedInstanceState)
        {

            Configuration config = this.Resources.Configuration;
            var ThemeMode = config.UiMode == (UiMode.NightYes | UiMode.TypeNormal);
            if (ThemeMode)
                this.SetTheme(Resource.Style.DarkTheme);
            else
                this.SetTheme(Resource.Style.LightTheme);



            //Configuration config = this.Resources.Configuration;
            //var ThemeMode = config.UiMode == (UiMode.NightYes | UiMode.TypeNormal);
            //if (ThemeMode)
            //    AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightYes;
            //else
            //    AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            savedInstanceState = null;
            base.OnCreate(savedInstanceState);
            //if (savedInstanceState == null)
            //{
            #region Stock
            //base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);


            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            drawer.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed, GravityCompat.End);

            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            #endregion

            #region ConfigManager
            ConfigurationManager configManager = ConfigurationManager.ConfigManager;
            var configuration = configManager.BankConfigurationFromJson;
            #endregion

            #region ShowDataFromDb
            await DatesRepositorio.SetDatasFromDB();
            #endregion

            #region Castom Tab

            tabLayout = FindViewById<TabLayout>(Resource.Id.tabLayout);
            tabLayout.InlineLabel = true;
            tabLayout.TabGravity = 0;



            pager = FindViewById<ViewPager2>(Resource.Id.pager);
            pager.OffscreenPageLimit = 3;//позволяет адакватно реагировать на нажатие кнопок
                                         //pager.SaveEnabled = false;

            tabLayout.TabSelected += (object sender, TabLayout.TabSelectedEventArgs e) =>
            {
                var tab = e.Tab;
                var layout = tab.View;

                var layoutParams = layout.LayoutParameters;// as AndroidX.AppCompat.Widget.LinearLayoutCompat.LayoutParams;

                tab.SetTabLabelVisibility(TabLayout.TabLabelVisibilityLabeled);

                layoutParams.Width = LinearLayoutCompat.LayoutParams.WrapContent;

                layout.LayoutParameters = layoutParams;
                var asdf = pager.ChildCount;
                //pager.ScrollY = 1;
            };
            tabLayout.TabUnselected += (object sender, TabLayout.TabUnselectedEventArgs e) =>
            {
                e.Tab.RemoveBadge();

                var tab = e.Tab;
                var layout = tab.View;
                tab.SetTabLabelVisibility(TabLayout.TabLabelVisibilityUnlabeled);
                // layoutParams.Width = LinearLayout.LayoutParams.WrapContent;
            };


            adapter = new CustomViewPager2Adapter(this.SupportFragmentManager, this.Lifecycle);
            tabIcons = new int[]
            {
                    Resource.Mipmap.ic_cash50,
                    Resource.Mipmap.ic_in_deposit50,
                    Resource.Mipmap.ic_cash_out111,
                    Resource.Mipmap.ic_error,

            };
            pager.Adapter = adapter;

            new TabLayoutMediator(tabLayout, pager, new CustomStrategy()).Attach();
            //              //adapter.NotifyDataSetChanged();

            #endregion

            #region ReadSmS
            //smsFilters.AddRange(configuration.Banks); //This operation took 5420
            //List<Sms> lst = await GetAllSmsAsync(smsFilters);// This operation took 1356
           // ParseSmsToDbAsync(configuration.Banks);///This operation took 56
            smsBroadcastReceiver = new SmsReceiver();
            #endregion
            //}
        }

        private async void SetSettings()
        {
            _ = await CheckAndRequestSmsPermission();
            _ = await CheckAndRequestStorageWritePermission();
            _ = await CheckAndRequestStorageReadPermission();


            var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            object showunreashables;
            prefs.All.TryGetValue("key_showunreashables", out showunreashables);
            switch (showunreashables)
            {
                case "1":
                    adapter.TabCount = 4;
                    break;
                case "2":
                    adapter.TabCount = 3;
                    break;
                //case "3":
                //    if (DatesRepositorio.GetUnreachable(DatesRepositorio.NewDataItems)?.Count > 0)
                //        adapter.TabCount = 4;
                //    else
                //        adapter.TabCount = 3;
                //    break;
                default:
                    break;
            };

        }

        #region ViewLifecucle
        protected override void OnStart()
        {
            SetSettings();
            base.OnStart();
            for (int i = 0; i < tabLayout.TabCount; i++)
                tabLayout.GetTabAt(i).View.LayoutParameters.Width = LinearLayoutCompat.LayoutParams.WrapContent;
        }
        protected override void OnResume()
        {
            base.OnResume();
            
            ConfigurationManager configManager = ConfigurationManager.ConfigManager;
            var configuration = configManager.BankConfigurationFromJson;
            ParseSmsToDbAsync(configuration.Banks);

            RegisterReceiver(smsBroadcastReceiver, new IntentFilter("android.provider.Telephony.SMS_RECEIVED"));

        }
        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(smsBroadcastReceiver);
        }
        protected override void OnStop()
        {
            base.OnStop();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            //outState.PutInt("click_count", _counter);
            //Log.Debug(GetType().FullName, "Activity A - Saving instance state");
            // always call the base implementation!
            base.OnSaveInstanceState(outState);

            var tabSelectedPosition = tabLayout.SelectedTabPosition;
            outState.PutInt("selectedTabPosition", tabSelectedPosition);



        }
        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            //base.OnRestoreInstanceState(savedInstanceState);
        }
        #endregion


        private async Task ParseSmsToDbAsync(List<BankConfiguration> bankConfigurations)
        {
            SmsReader smsReader = new SmsReader(this);
            List<Sms> smsList = await smsReader.GetAllSmsAsync(bankConfigurations);

            Parser parserBelarusbank = new Parser(smsList, bankConfigurations);//This operation took 3558
            var data = await parserBelarusbank.GetData();
            if (data != null)
            {
                await DatesRepositorio.AddDatas(data);//This operation took 10825
                                                      //               UpdateBadgeToTabs();
            }
        }

        
        async Task<FileResult> PickAndShowFromPdf(PickOptions options)
        {

            #region ConfigManager
            ConfigurationManager configManager = ConfigurationManager.ConfigManager;
            var configuration = configManager.BankConfigurationFromJson;
            #endregion
            string message;
            try
            {

                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    Parser parserBelarusbank = new Parser(result.FullPath, configuration.Banks);//This operation took 3558
                    var data = await parserBelarusbank.GetDataFromPdf();
                    await DatesRepositorio.AddDatas(data);//This operation took 10825
                                                          //adapter.AddNewItemToFragments();
                                                          //adapter.UpdateFragments();
                                                          //adapter.NotifyDataSetChanged();
                                                          //                    UpdateBadgeToTabs();
                    message = "Файл обработан.";

                }
                return result;
            }
            catch (Exception ex)
            {
                message = "Произошла ошибка";
                // The user canceled or something went wrong
                return null;
            }
            Android.Widget.Toast.MakeText(this, message, Android.Widget.ToastLength.Short).Show();
            return null;
        }
        async Task<FileResult> PickAndShowFromFile(PickOptions options)
        {
            string message;
            try
            {
                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    bool isReadonly = Android.OS.Environment.MediaMountedReadOnly.Equals(Android.OS.Environment.ExternalStorageState);
                    bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);

                    SerializarionToXml serializer = new SerializarionToXml();
                    var data = serializer.DeserializeFile(result.FullPath);
                    await DatesRepositorio.AddDatas(data.ToList());//This operation took 10825
                    message = "Файл обработан.";                                             //                    UpdateBadgeToTabs();

                }
                return result;
            }
            catch (Exception ex)
            {
                message = "Произошла ошибка";
                Android.Widget.Toast.MakeText(this, message, Android.Widget.ToastLength.Short).Show();
                return null;
            }
        }

        #region Stock
        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            #region FilterBtnClic
            if (id == Resource.Id.action_openRight)
            {
                if (_RightMenu == null)
                {
                    _RightMenu = new RightMenu();

                    var filterFragmentTransaction = SupportFragmentManager.BeginTransaction();
                    filterFragmentTransaction.Add(Resource.Id.MenuFragmentFrame, _RightMenu, "MENU");
                    filterFragmentTransaction.Commit();
                    _RightMenu.FiltredList = DatesRepositorio.DataItems.Select(x => x.Descripton).Distinct().ToList<string>();

                    _RightMenu.AddChekFilterItem("Карта", DatesRepositorio.DataItems.Select(x => x.Karta.ToString()).Distinct().OrderBy(x => x).ToList());
                    //_RightMenu.AddChekFilterItem("Категория по умолчанию", DatesRepositorio.DataItems.Select(x => x.DefaultCategoryTyps.ToString()).Distinct().ToList());
                    //_RightMenu.AddChekFilterItem("Пользовательская категория", DatesRepositorio.DataItems.Select(x => x.CastomCategoryTyps.ToString()).Distinct().ToList());

                    _RightMenu.AddChekFilterItem("MCC код", DatesRepositorio.DataItems.Select(x => x.MCC.ToString()).Distinct().OrderBy(x=>x).ToList());
                    _RightMenu.AddChekFilterItem("MCC описание", DatesRepositorio.DataItems.Select(x => x.MccDeskription?.ToString()).Distinct().OrderBy(x => x).ToList());

                   //// Dictionary<string,int> tagsWithMass = new Dictionary<string,int>();

                   ////// HashSet<string> tags = new HashSet<string>();
                   //// var subTtags = DatesRepositorio.DataItems.Select(x => x.Title).OfType<String>().Where(x => x != "").Select(x => x.Split(" "));
                   //// foreach (var tags in subTtags)
                   //// {
                   ////     foreach (var tag in tags)
                   ////     {
                   ////         if (!tagsWithMass.TryAdd(tag, 1))
                   ////             tagsWithMass[tag]++;
                   ////     }
                   //// }
                    _RightMenu.AddChekFilterItem("Тег", DatesRepositorio.GetTags());

                    drawer.OpenDrawer(GravityCompat.End);

                    _RightMenu.FiltersSet += (object sender, EventArgs e) =>
                    {

                        var filter = ((RightMenu)sender).FilredResultList;

                        var fltr = DatesRepositorio.MFilter;
                        fltr.GetResult(x => filter.SearchDiscriptions.Length == 0 ? true : filter.SearchDiscriptions.Contains(x.Descripton));
                        fltr.GetResult(x => filter.SearchDatas[0] == default ?
                                                                        x.Date.Date > DateTime.MinValue : (
                                                                        filter.SearchDatas[1] == default ?
                                                                        x.Date.Date == filter.SearchDatas[0] :
                                                                        x.Date.Date >= filter.SearchDatas[0] &&
                                                                        x.Date.Date <= filter.SearchDatas[1]));
                        var chLi = filter.ExpandableListAdapter.childList;
                        foreach (var item in chLi)
                        {
                            if (!item.Key.IsCheked)
                                continue;

                            switch (item.Key.Name)
                            {
                                case "Карта":
                                    fltr.GetResult(q => item.Value.Where(r => r.IsCheked).Select(w => w.Name).Contains(q.Karta.ToString())).ToArray();
                                    break;
                                case "Категория по умолчанию":
                                    fltr.GetResult(q => item.Value.Where(r => r.IsCheked).Select(w => w.Name).Contains(q.DefaultCategoryTyps.ToString())).ToArray();
                                    break;
                                case "Пользовательская категория":
                                    fltr.GetResult(q => item.Value.Where(r => r.IsCheked).Select(w => w.Name).Contains(q.CastomCategoryTyps.ToString())).ToArray();
                                    break;
                                case "MCC код":
                                    fltr.GetResult(q => item.Value.Where(r => r.IsCheked).Select(w => w.Name).Contains(q.MCC.ToString())).ToArray();
                                    break;
                                case "MCC описание":
                                    fltr.GetResult(q => item.Value.Where(r => r.IsCheked).Select(w => w.Name).Contains(q.MccDeskription)).ToArray();
                                    break;
                                case "Тег":
                                    var a1 = item.Value.Where(r => r.IsCheked).Select(w => w.Name).ToArray();
                                    //var a4 = DatesRepositorio.DataItems.Where(q=> q.Title != null && a1.Any(x => q.Title.Contains(x))).ToArray();
                                    //var a41 = DatesRepositorio.DataItems.Where(q=> q.Title != null && a1.Where(x => q.Title.Contains(x)).ToArray().Length==a1.Length).ToArray();
                                    //var a23 = DatesRepositorio.DataItems.Where(q => q.Title != null && q.Title.Split(' ').Intersect(a1).ToArray().Length==a1.Length).ToArray();
                                    //var a24 = DatesRepositorio.DataItems.Where(q => q.Title != null).Where(q => item.Value.Where(r => r.IsCheked).Select(w => w.Name).ToArray().Intersect(q.Title.Split(' ')).ToArray().Length== item.Value.Where(r => r.IsCheked).Select(w => w.Name).ToArray().Length).ToArray();
                                    //var a234 = DatesRepositorio.DataItems.Where(q => q.Title != null).Where(q => q.Title.Split(' ').Intersect(item.Value.Where(r => r.IsCheked).Select(w => w.Name)).ToArray().Length== item.Value.Where(r => r.IsCheked).Select(w => w.Name).ToArray().Length).ToArray();
                                    //var a5 = DatesRepositorio.DataItems.Where(q=>q.Title!=null).Where(q => item.Value.Where(r => r.IsCheked).Select(w => w.Name).Any(x =>q.Title.Contains(x))).ToArray();
                                    //var a3 = a1.Where(x=>DatesRepositorio.DataItems.Any(q => q.Title.Contains(x)));                                    //var a2 = DatesRepositorio.DataItems.Select(q => a1.Intersect(q.Title.Split(' ')).Count>0);

                                    //fltr.GetResult(q => item.Value.Where(r => r.IsCheked).Select(w => w.Name).Contains(q.Title)).ToArray(); //искать полные соответвия тегов
                                    //fltr.GetResult(q => q.Title != null && item.Value.Where(r => r.IsCheked).Select(w => w.Name).Any(x => q.Title.Contains(x))).ToArray(); //искать хотябы одно соответствие тега
                                    fltr.GetResult(q => q.Title != null && a1.Where(x => q.Title.Contains(x)).ToArray().Length == a1.Length).ToArray(); //искать полные соответвия тегов
                                    break;
                            }
                        }
                        drawer.CloseDrawer(GravityCompat.End);
                        //                        adapter.UpdateFragments();
                    };
                    return true;
                }
                else
                {
                    drawer.OpenDrawer(GravityCompat.End);
                }
            }
            #endregion

            return base.OnOptionsItemSelected(item);
        }


        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            var dialog = new AddItemDialog();
            dialog.AddedItem += (sender, e) =>
            {

                //                UpdateBadgeToTabs();
                dialog.Dismiss();
            };
            dialog.Display(SupportFragmentManager);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_import)
            {
                var options = new PickOptions
                {
                    PickerTitle = "@string/select_pdf_report"
                    //FileTypes = customFileType,
                };

                //PickAndShowFromPdf(options);
                PickAndShowFromPdf(options);
                return false;
            }
            if (id == Resource.Id.nav_db_clear)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);

                builder.SetTitle("Очистка базы данных");
                builder.SetMessage("Данное действие приведет к полной очистке базы данных приложения. " +
                    "Для сохранения возможности востановить данные воспользуйтесь функцией экспорат в файл. " +
                    "Вы действительно ходите продолжить?");

                builder.SetCancelable(false);
                builder.SetPositiveButton("Очистить", async (c, ev) =>
                {
                    string message;
                    message = await DatesRepositorio.DeleteAllItems() ? "База данных очищена." : "Произошла ошибка очистки базы данных.";
                    Android.Widget.Toast.MakeText(this, message, Android.Widget.ToastLength.Short).Show();
                    //                    UpdateBadgeToTabs();
                });
                builder.SetNegativeButton("Отмена", (c, ev) =>
                {
                    return;
                });
                builder.Create();
                builder.Show();
                drawer.CloseDrawer(GravityCompat.Start);
                return false;
            }

            if (id == Resource.Id.nav_upload)
            {

                bool isReadonly = Android.OS.Environment.MediaMountedReadOnly.Equals(Android.OS.Environment.ExternalStorageState);
                bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);

                SerializarionToXml serializer = new SerializarionToXml();

                string documentsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
                string localFilename = $"FinReport{DateTime.Now.ToString().Replace('.', '_').Replace(':', '_')}.xml";
                string localPath = Path.Combine(documentsPath, localFilename);

                var message = serializer.SaveToFile(localPath) ?? "Произошла ошибка";
                Android.Widget.Toast.MakeText(this, message, Android.Widget.ToastLength.Short).Show();

            }
            if (id == Resource.Id.nav_restore)
            {
                var options = new PickOptions
                {
                    PickerTitle = "@string/select_pdf_report"
                    //FileTypes = customFileType,
                };
                PickAndShowFromFile(options);
                drawer.CloseDrawer(GravityCompat.Start);
                return false;
            }
            if (id == Resource.Id.nav_dubl_search)
            {
                DuplicatesSearcher searcher = new DuplicatesSearcher(DatesRepositorio.DataItems);
                var duplicates = searcher.SearcDuplicates();
            }
            if (id == Resource.Id.nav_manage)
            {
                Intent intent = new Intent(this, typeof(SettingsActivity));
                StartActivity(intent);
                drawer.CloseDrawer(GravityCompat.Start);
                return false;
            }


            drawer.CloseDrawer(GravityCompat.Start);

            return false;
        }

        public void CheckPermission()
        {
            
        }
        public async Task<PermissionStatus> CheckAndRequestSmsPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Sms>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.Sms>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.Sms>();

            return status;
        }
        public async Task<PermissionStatus> CheckAndRequestStorageReadPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.StorageRead>();

            return status;
        }
        public async Task<PermissionStatus> CheckAndRequestStorageWritePermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.StorageWrite>();

            return status;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(@base);

        }
        #endregion



    }
}

