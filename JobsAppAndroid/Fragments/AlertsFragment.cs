using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace JobsAppAndroid.Fragments
{
    public class AlertsFragment : Android.Support.V4.App.Fragment
    {
        private FloatingActionButton fab;
        private Dialog dialog;
        private Spinner frequencySpinner;
        private Spinner locationSpinner;
        private Spinner industrySpinner;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
             View view = inflater.Inflate(Resource.Layout.alerts_fragment, container, false);

            fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);           
            fab.Click += FabOnClick;

           

            return view;

            
        }
        /// <summary>
        /// FAB Onclick Method
        /// </summary>
        /// <param name="sender">fab</param>
        /// <param name="e">event</param>
        private void FabOnClick(object sender, EventArgs e)
        {
            //Open dialog form
            dialog = new Dialog(Context);
            dialog.SetContentView(Resource.Layout.new_alert_dialog);
            dialog.Window.SetSoftInputMode(SoftInput.AdjustResize);
            dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            //Create spinners
            frequencySpinner = dialog.FindViewById<Spinner>(Resource.Id.frequency_spinner);
            ArrayAdapter freqAdapter = ArrayAdapter.CreateFromResource(Context,
                Resource.Array.frequency_array, Resource.Layout.spinner_item);
            freqAdapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item);
            frequencySpinner.Adapter = freqAdapter;

            locationSpinner = dialog.FindViewById<Spinner>(Resource.Id.location_spinner);
            ArrayAdapter locationAdapter = ArrayAdapter.CreateFromResource(Context,
                Resource.Array.location_array, Resource.Layout.spinner_item);
            locationAdapter.SetDropDownViewResource(Resource.Layout.spinner_checked_dropdown);
            locationSpinner.Adapter = locationAdapter;

            industrySpinner = dialog.FindViewById<Spinner>(Resource.Id.industry_spinner);
            ArrayAdapter industryAdapter = ArrayAdapter.CreateFromResource(Context,
                Resource.Array.industry_array, Resource.Layout.spinner_item);
            industryAdapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item);
            industrySpinner.Adapter = industryAdapter;
            
            dialog.Show();
        }
    }
}