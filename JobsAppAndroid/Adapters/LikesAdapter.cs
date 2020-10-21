using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using JobsAppAndroid.Models;
using JobsAppAndroid.Services;
using Newtonsoft.Json;

namespace JobsAppAndroid.Adapters
{
    class LikesAdapter : RecyclerView.Adapter
    {
        public event EventHandler<LikesAdapterClickEventArgs> ItemClick;
        public event EventHandler<LikesAdapterClickEventArgs> ItemLongClick;
        private List<Job> likes;
        private RandomColorGenerator randomColorGenerator;

        private Context context;
        private ISharedPreferences preferences;
        private AppPreferences appPreferences;

        public LikesAdapter(Context context)
        {
            this.context = context;

            //Color Generator
            randomColorGenerator = new RandomColorGenerator();

            preferences = PreferenceManager.GetDefaultSharedPreferences(context);
            appPreferences = new AppPreferences(preferences);

            likes = appPreferences.GetLikes();

        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            var id = Resource.Layout.jobs_list_item;
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new LikesAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = likes[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as LikesAdapterViewHolder;
            holder.Title.Text = likes[position].Title;
            holder.Company.Text = likes[position].Company;
            holder.Location.Text = likes[position].Location;
            holder.PostedDate.Text = likes[position].Posted.ToShortDateString();
            holder.ClosingDate.Text = "Closing date: " + likes[position].Closing.ToShortDateString();
            holder.CircularButton.Text = likes[position].Title.Substring(0, 1).ToUpper();
            holder.FaveCheckbox.Checked = likes[position].Liked;

            ShapeDrawable circle = new ShapeDrawable(new OvalShape());
            circle.Paint.Color = randomColorGenerator.GetColor();
            holder.CircularButton.Background = circle;

            holder.FaveCheckbox.Click += delegate
            {
                    //Add to likes
                    if (!holder.FaveCheckbox.Checked)
                    {

                        DeleteFromPreferences(item);
                    }

            };

        }
        /// <summary>
        /// Remove item from liked items
        /// </summary>
        /// <param name="item">Item to remove</param>
        private void DeleteFromPreferences(Job item)
        {
            //delete from preferences
            appPreferences.DeleteLike(item);

            if (likes != null)
            {
                likes.Remove(item);
                NotifyDataSetChanged();
            }

        }
        public override int ItemCount => likes.Count;

        void OnClick(LikesAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(LikesAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class LikesAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Title { get; set; }
        public TextView Company { get; set; }
        public TextView Location { get; set; }
        public TextView PostedDate { get; set; }
        public TextView ClosingDate { get; set; }
        public Button CircularButton { set; get; }
        public CheckBox FaveCheckbox { set; get; }

        public LikesAdapterViewHolder(View itemView, Action<LikesAdapterClickEventArgs> clickListener,
                            Action<LikesAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.list_item_title);
            Company = itemView.FindViewById<TextView>(Resource.Id.list_item_company);
            Location = itemView.FindViewById<TextView>(Resource.Id.list_item_location);
            PostedDate = itemView.FindViewById<TextView>(Resource.Id.list_item_posted_date);
            ClosingDate = itemView.FindViewById<TextView>(Resource.Id.list_item_closing_date);
            CircularButton = itemView.FindViewById<Button>(Resource.Id.list_item_circle_btn);
            FaveCheckbox = itemView.FindViewById<CheckBox>(Resource.Id.list_item_fav_button);
            FaveCheckbox.Checked = true;

            itemView.Click += (sender, e) => clickListener(new LikesAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new LikesAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class LikesAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
