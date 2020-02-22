using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using JobsAppAndroid.Models;
using System.Collections.Generic;

namespace JobsAppAndroid
{
    class JobsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<JobsAdapterClickEventArgs> ItemClick;
        public event EventHandler<JobsAdapterClickEventArgs> ItemLongClick;
        private List<Job> jobs; 

        public JobsAdapter(List<Job> data)
        {
            jobs = data;
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
            holder.CreatedDate.Text = jobs[position].CreateTime.ToLongDateString();
            holder.ClosingDate.Text = jobs[position].Closing.ToShortDateString();
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
        public TextView CreatedDate { get; set; }
        public TextView ClosingDate { get; set; }

        public JobsAdapterViewHolder(View itemView, Action<JobsAdapterClickEventArgs> clickListener,
                            Action<JobsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.list_item_title);
            Company = itemView.FindViewById<TextView>(Resource.Id.list_item_company);
            Location = itemView.FindViewById<TextView>(Resource.Id.list_item_location);
            CreatedDate = itemView.FindViewById<TextView>(Resource.Id.list_item_created_date);
            ClosingDate = itemView.FindViewById<TextView>(Resource.Id.list_item_closing_date);

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