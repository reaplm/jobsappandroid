using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace JobsAppAndroid
{
    public class ViewPagerAdapter : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> fragments = new List<Android.Support.V4.App.Fragment>();
        private List<string> fragmentTitles = new List<string>();

        public ViewPagerAdapter(Android.Support.V4.App.FragmentManager manager) : base(manager)
        {
            //base.OnCreate(manager);
        }

        public override int Count
        {
            get
            {
                return fragments.Count;
            }
        }
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return fragments[position];
        }
        public override ICharSequence GetPageTitleFormatted(int position)
        {

            return new Java.Lang.String(fragmentTitles[position].ToLower());// display the title
            //return null;// display only the icon
        }

        public void AddFragment(Android.Support.V4.App.Fragment fragment, string title)
        {
            fragments.Add(fragment);
            fragmentTitles.Add(title);
        }

    }
}