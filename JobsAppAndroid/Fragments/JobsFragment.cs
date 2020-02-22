using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using JobsAppAndroid.Models;

namespace JobsAppAndroid.Fragments
{
    public class JobsFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView recyclerView;
        private JobsAdapter jobsAdapter;
        private List<Job> jobs;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.jobs_fragment, container, false);

            jobs = new List<Job>
            {
                new Job{ Id="1", Title="Driver", Company="BEC", Location="Gaborone", CreateTime=new DateTime(2020,01,10), Closing=new DateTime(2020,01,22)},
                new Job{ Id="2", Title="Software Developer", Company="Statistics Botswana", Location="Francistown", CreateTime=new DateTime(2020,02,01), Closing=new DateTime(2020,02,11)},
                new Job{ Id="3", Title="Database Administrator", Company="CIPA", Location="Kanye", CreateTime=new DateTime(2020,02,15), Closing=new DateTime(2020,02,18)},
                new Job{ Id="4", Title="Legal Manager", Company="BOPEU", Location="Tlokweng", CreateTime=new DateTime(2019,11,19), Closing=new DateTime(2019,11,29)},
                new Job{ Id="5", Title="Chef", Company="Masa Hotel", Location="Gaborone", CreateTime=new DateTime(2019,05,06), Closing=new DateTime(2019,05,16)}

            };

            jobsAdapter = new JobsAdapter(jobs);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.jobs_recycler_view);
            

            recyclerView.SetAdapter(jobsAdapter);

            DividerItemDecoration myDivider = new DividerItemDecoration(Context, DividerItemDecoration.Vertical);

            myDivider.SetDrawable(ContextCompat.GetDrawable(Context, Resource.Drawable.divider));
            recyclerView.AddItemDecoration(myDivider);

            return view;
        }
    }
}