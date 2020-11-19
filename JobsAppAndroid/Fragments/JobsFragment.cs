using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Android.Arch.Lifecycle;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Firestore;
using JobsAppAndroid.Models;
using JobsAppAndroid.ViewModels;
using Newtonsoft.Json;
using static Android.Support.V4.Widget.SwipeRefreshLayout;
using IEventListener = Firebase.Firestore.IEventListener;

namespace JobsAppAndroid.Fragments
{
    public class JobsFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView recyclerView;
        private SwipeRefreshLayout swipeRefreshLayout;

        private JobsAdapter jobsAdapter;

        private JobViewModel viewModel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.jobs_fragment, container, false);

            swipeRefreshLayout = (SwipeRefreshLayout)view.FindViewById(Resource.Id.refreshView);
            
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.jobs_recycler_view);

            // setup ViewModel
            ViewModelProvider viewModelProvider = new ViewModelProvider(this, new ViewModelProvider.NewInstanceFactory());
            viewModel = viewModelProvider.Get(Java.Lang.Class.FromType(typeof(JobViewModel))) as JobViewModel;
            //ViewModel Events
            viewModel.Jobs.CollectionChanged += Jobs_CollectionChanged;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            //Jobs = viewModel.Jobs;
            jobsAdapter = new JobsAdapter(Context, viewModel.Jobs);
            jobsAdapter.ItemClick += OnItemClick;

            recyclerView.SetAdapter(jobsAdapter);

        }
        /// <summary>
        /// View Item OnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnItemClick(object sender, JobsAdapterClickEventArgs e)
        {
            var item = viewModel.Jobs[e.Position];

            //Details Activity
            Intent intent = new Intent(Context, typeof(JobDetailActivity));
            intent.PutExtra("job", JsonConvert.SerializeObject(item));

            StartActivity(intent);
        }
        #region PropertChangedEvents
            /// <summary>
            /// ViewModel Propert Changed Event
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                
            }
            /// <summary>
            /// Collection Changed Event
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Jobs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                //Update adapter
                jobsAdapter.NotifyDataSetChanged();
            }
        #endregion



    }
}