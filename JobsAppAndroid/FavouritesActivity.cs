using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using JobsAppAndroid.Adapters;
using JobsAppAndroid.Models;
using JobsAppAndroid.Services;
using Newtonsoft.Json;

namespace JobsAppAndroid
{
    [Activity(Label = "Favourite Jobs")]
    public class FavouritesActivity : AppCompatActivity, IEventListener
    {
        private RecyclerView recyclerView;
        private LikesAdapter likesAdapter;
        private List<Job> likes;

        private FirebaseApp app;
        private FirebaseFirestore db;
        private FirebaseAuth auth;

        private SwipeRefreshLayout swipeRefreshLayout;
        private bool reload = false;

        private ISharedPreferences preferences;
        private AppPreferences appPreferences;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_favourites);

            //Setup firebase
            app = FirebaseApp.Instance;
            db = FirebaseFirestore.GetInstance(app);
            auth = FirebaseAuth.GetInstance(app);            

            swipeRefreshLayout = (SwipeRefreshLayout)FindViewById(Resource.Id.refreshView);
            swipeRefreshLayout.Refresh += delegate (object sender, System.EventArgs e)
            {
                reload = true;
                //FetchCollection();
            };

            swipeRefreshLayout.Post(() =>
            {
                swipeRefreshLayout.Refreshing = false;
                recyclerView.Clickable = false;
            });

            reload = true;

            //GetLikesFromPreferences();
            likesAdapter = new LikesAdapter(this);
            likesAdapter.ItemClick += OnItemClick;

            recyclerView = FindViewById<RecyclerView>(Resource.Id.faves_recycler_view);
            recyclerView.SetAdapter(likesAdapter);


            DividerItemDecoration myDivider = new DividerItemDecoration(this, DividerItemDecoration.Vertical);

            myDivider.SetDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.divider));
            recyclerView.AddItemDecoration(myDivider);
        }

        private void OnItemClick(object sender, LikesAdapterClickEventArgs e)
        {
            
        }
        /// <summary>
        /// Fetch likes from 
        /// </summary>
        private void GetLikesFromPreferences()
        {
            likes = appPreferences.GetLikes();

            swipeRefreshLayout.Post(() =>
            {
                swipeRefreshLayout.Refreshing = false;
                recyclerView.Clickable = true;
            });

        }
        /// <summary>
        /// Fetch likes from firestore
        /// </summary>
        private void FetchCollection()
        {
            try
            {
                CollectionReference collection = db.Collection("likes");
                collection.WhereEqualTo("Uid", auth.Uid).AddSnapshotListener(this);
                // collection.Get().AddOnSuccessListener(this).AddOnFailureListener(this);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error in FindLikes: " + ex.Message);
            }
        }
        /// <summary>
        /// Firestore document fetch complete event
        /// </summary>
        /// <param name="value"></param>
        /// <param name="error"></param>
        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            if (error != null)
            {
                //Log.w("TAG", "listen:error", error);
                swipeRefreshLayout.Post(() =>
                {
                    swipeRefreshLayout.Refreshing = false;
                });
                reload = false;

                Toast.MakeText(this, "Error fetching you likes...", ToastLength.Long).Show();
                return;
            }

            var snapshots = (QuerySnapshot)value;
            foreach (DocumentSnapshot document in snapshots.Documents)
            {
                var dictionary = document.Data;
                dictionary.Add("Id", document.Id);
                var job = ToObject(dictionary);

                likes.Add(job);
            }
            if (reload)
            {
                likesAdapter.NotifyDataSetChanged(); //for updating adapter
                reload = false;
            }
            swipeRefreshLayout.Post(() =>
            {
                swipeRefreshLayout.Refreshing = false;
                recyclerView.Clickable = true;
            });

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