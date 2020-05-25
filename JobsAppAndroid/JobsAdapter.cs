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

namespace JobsAppAndroid
{
    class JobsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<JobsAdapterClickEventArgs> ItemClick;
        public event EventHandler<JobsAdapterClickEventArgs> ItemLongClick;
        private List<Job> jobs;
        private RandomColorGenerator randomColorGenerator;

        public JobsAdapter(List<Job> data)
        {
            jobs = data;

            //Color Generator
            randomColorGenerator = new RandomColorGenerator();

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

            ShapeDrawable circle = new ShapeDrawable(new OvalShape());
            circle.Paint.Color = randomColorGenerator.GetColor();
            holder.CircularButton.Background = circle;
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

        public JobsAdapterViewHolder(View itemView, Action<JobsAdapterClickEventArgs> clickListener,
                            Action<JobsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.list_item_title);
            Company = itemView.FindViewById<TextView>(Resource.Id.list_item_company);
            Location = itemView.FindViewById<TextView>(Resource.Id.list_item_location);
            PostedDate = itemView.FindViewById<TextView>(Resource.Id.list_item_posted_date);
            ClosingDate = itemView.FindViewById<TextView>(Resource.Id.list_item_closing_date);
            CircularButton = itemView.FindViewById<Button>(Resource.Id.list_item_circle_btn);

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