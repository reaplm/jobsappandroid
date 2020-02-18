using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
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
        private ViewPager viewPager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            tabLayout = (TabLayout)FindViewById(Resource.Id.tablayout);
            viewPager = (ViewPager)FindViewById(Resource.Id.viewpager);

            SetupViewPager(viewPager);
            tabLayout.SetupWithViewPager(viewPager);

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
        private AdapterFragment homeFragment;
        private AdapterFragment jobsFragment;

        public void SetupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            homeFragment = new AdapterFragment();
            jobsFragment = new AdapterFragment();

        adapter.AddFragment(homeFragment, "Home");
            adapter.AddFragment(jobsFragment, "Alerts");
            viewPager.Adapter = adapter;
        }

    }
}

