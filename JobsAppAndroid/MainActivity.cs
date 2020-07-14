using System;
using Android.App;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Android.Gms.Common.Apis;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using JobsAppAndroid.Fragments;
using Android.Gms.Common;
using Android.Content;
using Firebase.Auth;
using Firebase;
using Firebase.Firestore;

namespace JobsAppAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private TabLayout tabLayout;
        private DrawerLayout drawerLayout;
        private ViewPager viewPager;

        private FirebaseAuth firebaseAuth;
        private FirebaseApp app;

        static Result RESULT_OK = Result.Ok;
        static Result RESULT_CANCELED = Result.Canceled;

        //TabLayout Fragments
        private AdapterFragment homeFragment;
        private AlertsFragment alertsFragment;
        private JobsFragment jobsFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.drawer_main);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);

                //Setup Navigation Drawer 
                drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
                var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
                drawerLayout.AddDrawerListener(drawerToggle);
                drawerToggle.SyncState();

                //Setup Navigation View
                SetNavigationViewListener();

                // Setup TabLayout 
                tabLayout = (TabLayout)FindViewById(Resource.Id.tablayout);
                viewPager = (ViewPager)FindViewById(Resource.Id.viewpager);

                SetupViewPager(viewPager);
                tabLayout.SetupWithViewPager(viewPager);

                //Setup TabLayout 
                SetupTabIcons();

                //[START Initialize Firebase]

                var options = new Firebase.FirebaseOptions.Builder()
                .SetApplicationId(GetString(Resource.String.google_application_id))
                .SetApiKey(GetString(Resource.String.google_api_key))
                .SetDatabaseUrl(GetString(Resource.String.google_firebase_url))
                .SetProjectId(GetString(Resource.String.google_project_id))
                .Build();

                app = FirebaseApp.InitializeApp(this, options, GetString(Resource.String.app_name));
                firebaseAuth = FirebaseAuth.GetInstance(app);

                //[END Initialize Firebase]

                //Initialize UI 
                UpdateUI();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error Msg: " + ex);
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
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
        private void SetNavigationViewListener()
        {
            NavigationView navigationView = drawerLayout.FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
        }
        /// <summary>
        /// Setup TabLayout ViewPager
        /// </summary>
        /// <param name="viewPager"></param>
        public void SetupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);

            //Create fragments for each tab
            homeFragment = new AdapterFragment();
            alertsFragment = new AlertsFragment();
            jobsFragment = new JobsFragment();

            adapter.AddFragment(homeFragment, "Home");
            adapter.AddFragment(alertsFragment, "Alerts");
            adapter.AddFragment(jobsFragment, "Jobs");
            viewPager.Adapter = adapter;
        }
        /// <summary>
        /// Add tab icons to TabLayout
        /// </summary>
        private void SetupTabIcons()
        {
            tabLayout.GetTabAt(0).SetIcon(Resource.Mipmap.home_icon);
            tabLayout.GetTabAt(1).SetIcon(Resource.Mipmap.notification_icon);
            tabLayout.GetTabAt(2).SetIcon(Resource.Mipmap.suitcase_icon);
        }
        /// <summary>
        /// DrawerLayout Navigation Menu
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            switch (menuItem.ItemId)
            {
                case Resource.Id.action_login:
                    if (firebaseAuth.CurrentUser == null) { SignIn(); }
                    else SignOut();
                    break;
            }
            drawerLayout.CloseDrawer(GravityCompat.Start, true);
            return true;
        }
        /// <summary>
        /// Sign out
        /// </summary>
        private void SignOut()
        {
            firebaseAuth.SignOut();
            Toast.MakeText(this, "Sign Out Successful!", ToastLength.Long).Show();
            UpdateUI();
        }

        /// <summary>
        /// Sign In
        /// Starts LoginActivity
        /// </summary>
        void SignIn()
        {
            Intent intent = new Intent(this, typeof(LogInActivity));

            StartActivityForResult(intent, 1);
        }
        /// <summary>
        /// On return from activity
        /// 1: LoginActivity
        /// </summary>
        /// <param name="requestCode">Code to identify activity</param>
        /// <param name="resultCode">Success/Fail code</param>
        /// <param name="data">Returned Data</param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            
            //LoginActivity
            if (requestCode == 1)
            {
                if (resultCode == RESULT_OK)
                {
                    bool isLoggedIn = data.GetBooleanExtra("isLoggedIn", false);

                    if (isLoggedIn)
                    {
                        UpdateUI();
                        Toast.MakeText(this, data.GetStringExtra("message"), ToastLength.Long).Show();
                    }
                    else
                    {
                        //UpdateUI();
                        Toast.MakeText(this, data.GetStringExtra("message"), ToastLength.Long).Show();
                    }
                }
                else if (resultCode == RESULT_CANCELED)
                {
                    ;
                }
            }
        }
        /// <summary>
        /// Update user interface on login or logout
        /// </summary>
        private void UpdateUI()
        {
            NavigationView navigationView = drawerLayout.FindViewById<NavigationView>(Resource.Id.nav_view);

            IMenu menu = navigationView.Menu;
            IMenuItem menuItem = menu.FindItem(Resource.Id.action_login);

            View navHeader = navigationView.GetHeaderView(0);

            if (firebaseAuth.CurrentUser != null)
            {
                menuItem.SetTitle("Log Out");
                navHeader.FindViewById<TextView>(Resource.Id.nav_header_text).Text = firebaseAuth.CurrentUser.DisplayName;
            }
            else
            {
                menuItem.SetTitle("Log In");
                navHeader.FindViewById<TextView>(Resource.Id.nav_header_text).Text = "";
            }
        }
    }
}

