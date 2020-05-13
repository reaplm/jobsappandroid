using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Firestore;
using Java.Lang;
using Java.Util;
using JobsAppAndroid.Models;

namespace JobsAppAndroid.Fragments
{
    public class JobsFragment : Android.Support.V4.App.Fragment, IOnSuccessListener, IOnFailureListener
    {
        private RecyclerView recyclerView;
        private JobsAdapter jobsAdapter;
        private List<Job> jobs;

        private FirebaseApp app;
        private FirebaseFirestore db;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            app = FirebaseApp.GetInstance(GetString(Resource.String.app_name));

            db = FirebaseFirestore.GetInstance(app);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.jobs_fragment, container, false);

            jobs = new List<Job>();

            FetchCollection();

            jobsAdapter = new JobsAdapter(jobs);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.jobs_recycler_view);
            

            recyclerView.SetAdapter(jobsAdapter);

            DividerItemDecoration myDivider = new DividerItemDecoration(Context, DividerItemDecoration.Vertical);

            myDivider.SetDrawable(ContextCompat.GetDrawable(Context, Resource.Drawable.divider));
            recyclerView.AddItemDecoration(myDivider);

            return view;
        }
        /// <summary>
        /// Fetch collection from firestore database
        /// </summary>
        private void FetchCollection()
        {
            try
            {
                CollectionReference collection = db.Collection("jobs");

                collection.Get().AddOnSuccessListener(this).AddOnFailureListener(this);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error in FindAllAsync: " + ex.Message);
            }

        }
        /// <summary>
        /// On success Listener
        /// </summary>
        /// <param name="result">List of documents</param>
        public void OnSuccess(Java.Lang.Object result)
        {
            try
            {
                var documents = (QuerySnapshot)result;

                foreach (DocumentSnapshot document in documents.Documents)
                {
                    Java.Util.Date dt = document.GetDate("Closing");
                    DateTime closingDt = dt != null ? new DateTime(1970, 1, 1).AddMilliseconds(dt.Time).ToLocalTime() : new DateTime();
                    DateTime postedDt = dt != null ? new DateTime(1970, 1, 1).AddMilliseconds(dt.Time).ToLocalTime() : new DateTime();

                    jobs.Add(new Job
                    {
                        Closing = closingDt,
                        Company = document.GetString("Company"),
                        Description = document.GetString("Description"),
                        Location = document.GetString("Location"),
                        CreateTime = postedDt,
                        Title = document.GetString("Title"),
                        Source = document.GetString("Source")
                    });
                }

                jobsAdapter.NotifyDataSetChanged(); //for updating adapter
            }
            catch(Java.Lang.Exception e)
            {
                Console.WriteLine("Jobs fragment failure: " + e.StackTrace);
            }
        }
        /// <summary>
        /// On failure listener
        /// </summary>
        /// <param name="e">Firestore Exception</param>
        public void OnFailure(Java.Lang.Exception e)
        {
            Console.WriteLine("Jobs fragment failure: " + e.StackTrace);
        }

    }
}