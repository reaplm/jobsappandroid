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

        private ProgressBar spinner;

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

            spinner = (ProgressBar)view.FindViewById(Resource.Id.rv_progress_bar);
            spinner.Visibility = ViewStates.Visible;

            jobs = new List<Job>();
            FetchCollection();
            jobsAdapter = new JobsAdapter(jobs);
            jobsAdapter.ItemClick += OnItemClick;

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.jobs_recycler_view);
            recyclerView.SetAdapter(jobsAdapter);

            DividerItemDecoration myDivider = new DividerItemDecoration(Context, DividerItemDecoration.Vertical);

            myDivider.SetDrawable(ContextCompat.GetDrawable(Context, Resource.Drawable.divider));
            recyclerView.AddItemDecoration(myDivider);

            return view;
        }
        public void OnItemClick(object sender, JobsAdapterClickEventArgs e)
        {
            var item = jobs[e.Position];

            Intent intent = new Intent(Context, typeof(JobDetailActivity));
            intent.PutExtra("title", item.Title);
            intent.PutExtra("company", item.Company);
            intent.PutExtra("location", item.Location);
            intent.PutExtra("description", item.Description);
            intent.PutExtra("type", item.Type);
            intent.PutStringArrayListExtra("qualifications", item.Qualifications);
            intent.PutStringArrayListExtra("competencies", item.Competencies);
            intent.PutStringArrayListExtra("contacts", item.Contacts);
            intent.PutExtra("closing", "Closes " + item.Closing.ToLongDateString());

            var timeSpan = TimeSpan.FromTicks(item.Posted.Ticks).Days;
            intent.PutExtra("posted", timeSpan.ToString() + " days ago");

            StartActivity(intent);

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
                    var dictionary = document.Data;

                    var job = ToObject(dictionary);
                    jobs.Add(job);
                }
                spinner.Visibility = ViewStates.Gone;

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
        /// <summary>
        /// Convert DocumentSnapshot to Object
        /// </summary>
        /// <param name="source">Dictionary</param>
        /// <returns>Object</returns>
        public Job ToObject(IDictionary<string, Java.Lang.Object> source)
        {
            var newObject = new Job();
            var newObjectType = newObject.GetType();

            foreach (var item in source)
            {
                if (newObjectType.GetProperty(item.Key) != null)
                {
                    if (item.Value.GetType().Equals(typeof(Java.Lang.String)))
                    {
                        //covert Java.Lang.String to System.string
                        newObjectType
                            .GetProperty(item.Key)
                            .SetValue(newObject, item.Value.ToString(), null);

                    }
                    if (item.Value.GetType().Equals(typeof(Java.Util.Date)))
                    {
                        //covert Java.Util.Date to DateTime
                        Java.Util.Date dt = (Java.Util.Date)(item.Value);
                        DateTime dateValue = new DateTime(1970, 1, 1).AddMilliseconds(dt.Time).ToLocalTime();

                        newObjectType
                            .GetProperty(item.Key)
                            .SetValue(newObject, dateValue, null);
                    }
                    if (item.Value.GetType().Equals(typeof(JavaList)))
                    {
                        //covert JavaList to List<>
                        var javaList = (JavaList)item.Value;
                        List<string> newList = new List<string>();

                        foreach (var listItem in javaList)
                        {
                            newList.Add(listItem.ToString());
                        }

                        newObjectType
                            .GetProperty(item.Key)
                            .SetValue(newObject, newList, null);
                    }
                }
            }

            return newObject;
        }
    }
}