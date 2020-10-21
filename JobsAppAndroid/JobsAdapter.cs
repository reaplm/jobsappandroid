using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using JobsAppAndroid.Models;
using System.Collections.Generic;
using Android.Support.V4.Graphics.Drawable;
using Android.Support.V4.Content;
using Android.Graphics.Drawables;
using Android.Support.V4.Content.Res;
using Android.Graphics.Drawables.Shapes;
using Android.Graphics;
using Firebase;
using Firebase.Firestore;
using Firebase.Auth;
using Android.Runtime;
using Android.Content;
using Android.Preferences;
using Newtonsoft.Json;
using JobsAppAndroid.Services;

namespace JobsAppAndroid
{
    class JobsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<JobsAdapterClickEventArgs> ItemClick;
        public event EventHandler<JobsAdapterClickEventArgs> ItemLongClick;
        private List<Job> jobs;
        private RandomColorGenerator randomColorGenerator;

        private FirebaseApp app;
        private FirebaseFirestore db;
        private FirebaseAuth auth;

        Context context;
        private ISharedPreferences preferences;
        private AppPreferences appPreferences;

        public JobsAdapter(Context context, List<Job> data)
        {
            jobs = data;
            this.context = context;

            //Color Generator
            randomColorGenerator = new RandomColorGenerator();

            //Setup firebase
            app = FirebaseApp.Instance;
            db = FirebaseFirestore.GetInstance(app);
            auth = FirebaseAuth.GetInstance(app);

            preferences = PreferenceManager.GetDefaultSharedPreferences(context);
            appPreferences = new AppPreferences(preferences);

        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            var id = Resource.Layout.jobs_list_item;
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new JobsAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = jobs[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as JobsAdapterViewHolder;
            holder.Title.Text = jobs[position].Title;
            holder.Company.Text = jobs[position].Company;
            holder.Location.Text = jobs[position].Location;
            holder.PostedDate.Text = jobs[position].Posted.ToShortDateString();
            holder.ClosingDate.Text = "Closing date: " + jobs[position].Closing.ToShortDateString();
            holder.CircularButton.Text = jobs[position].Title.Substring(0, 1).ToUpper();

            jobs[position].Liked = appPreferences.IsLiked(jobs[position]);
            holder.FaveCheckbox.Checked = jobs[position].Liked;
            

            ShapeDrawable circle = new ShapeDrawable(new OvalShape());
            circle.Paint.Color = randomColorGenerator.GetColor();
            holder.CircularButton.Background = circle;

            

            holder.FaveCheckbox.CheckedChange += delegate
            {
                if(auth.CurrentUser != null)
                {
                    //Add to likes
                    if (holder.FaveCheckbox.Checked)
                    {
                        //Save to shared preferences
                        jobs[holder.AdapterPosition].Liked = true;
                        appPreferences.SaveLike(jobs[holder.AdapterPosition]);

                        //Save to db
                        //var data = ToDictionary(item);
                        //data.Add("Uid", auth.CurrentUser.Uid);
                        //db.Collection("likes").Add(data);
                    }
                    else
                    {
                        //delete from preferences
                        jobs[holder.AdapterPosition].Liked = false;
                        appPreferences.DeleteLike(jobs[holder.AdapterPosition]);

                        //delete from db
                        //db.Collection("likes").Document(item.Id).Delete();
                    }

                }
            };

        }
        /// <summary>
        /// Convert object to dictionary
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IDictionary<string, Java.Lang.Object> ToDictionary(Job source)
        {
            var dictionary = new Dictionary<string, Java.Lang.Object>();
            var properties = source.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(source) != null) { 
                    if (property.PropertyType.Equals(typeof(string))) {
                        dictionary.Add(property.Name, property.GetValue(source).ToString());
                    }
                    else if (property.PropertyType.Equals(typeof(DateTime)))
                    {
                        DateTime date = (DateTime)property.GetValue(source);
                        var ticksAtEpoch = new DateTime(1970, 1, 1).ToLocalTime().Ticks;
                        var ticksPerMilliSec = 10000;

                        //DateTime dateValue = new DateTime(1970, 1, 1).AddMilliseconds(dt.Time).ToLocalTime();
                        Java.Util.Date dt = new Java.Util.Date((date.ToLocalTime().Ticks-ticksAtEpoch)/ticksPerMilliSec);

                        dictionary.Add(property.Name, dt);
                    }
                    else if (property.PropertyType.Equals(typeof(List<string>)))
                    {
                        //covert to JavaList
                        var javaList = new JavaList<string>();

                        foreach (var listItem in (List<string>)property.GetValue(source))
                        {
                            javaList.Add(listItem.ToString());
                        }

                        dictionary.Add(property.Name, javaList);
                    }
                }
            }

            return dictionary;
        }

        public override int ItemCount => jobs.Count;

        void OnClick(JobsAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(JobsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class JobsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Title { get; set; }
        public TextView Company { get; set; }
        public TextView Location { get; set; }
        public TextView PostedDate { get; set; }
        public TextView ClosingDate { get; set; }
        public Button CircularButton { set; get; }
        public CheckBox FaveCheckbox { set; get; }

        public JobsAdapterViewHolder(View itemView, Action<JobsAdapterClickEventArgs> clickListener,
                            Action<JobsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.list_item_title);
            Company = itemView.FindViewById<TextView>(Resource.Id.list_item_company);
            Location = itemView.FindViewById<TextView>(Resource.Id.list_item_location);
            PostedDate = itemView.FindViewById<TextView>(Resource.Id.list_item_posted_date);
            ClosingDate = itemView.FindViewById<TextView>(Resource.Id.list_item_closing_date);
            CircularButton = itemView.FindViewById<Button>(Resource.Id.list_item_circle_btn);
            FaveCheckbox = itemView.FindViewById<CheckBox>(Resource.Id.list_item_fav_button);

            itemView.Click += (sender, e) => clickListener(new JobsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new JobsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class JobsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}