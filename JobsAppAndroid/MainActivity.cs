using System;
using Android.App;
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

namespace JobsAppAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private TabLayout tabLayout;
        private DrawerLayout drawerLayout;
        private ViewPager viewPager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.drawer_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            

            //Drawer
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar,Resource.String.drawer_open, Resource.String.drawer_close);
            drawerLayout.AddDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            tabLayout = (TabLayout)FindViewById(Resource.Id.tablayout);
            viewPager = (ViewPager)FindViewById(Resource.Id.viewpager);

            
            SetupViewPager(viewPager);
            tabLayout.SetupWithViewPager(viewPager);

            tabLayout.SetTabTextColors(Color.ParseColor("#ffffff"), Color.ParseColor("#ffffff"));
            SetupTabIcons();

            var container = FindViewById<FrameLayout>(Resource.Id.container);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            //navigationView.Inflate(Resource.Menu.drawer_menu);
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
        private AdapterFragment homeFragment;
        private AdapterFragment alertsFragment;
        private JobsFragment jobsFragment;

        public void SetupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            homeFragment = new AdapterFragment();
            alertsFragment = new AdapterFragment();
            jobsFragment = new JobsFragment();

            adapter.AddFragment(homeFragment, "Home");
            adapter.AddFragment(alertsFragment, "Alerts");
            adapter.AddFragment(jobsFragment, "Jobs");
            viewPager.Adapter = adapter;
        }
        private void SetupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                drawerLayout.CloseDrawers();
            };
        }
        private void SetupTabIcons()
        {
            tabLayout.GetTabAt(0).SetIcon(Resource.Mipmap.outline_home);
            tabLayout.GetTabAt(1).SetIcon(Resource.Mipmap.outline_notification);
            tabLayout.GetTabAt(2).SetIcon(Resource.Mipmap.outline_work);
        }
    }
}

