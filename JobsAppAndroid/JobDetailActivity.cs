using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using JobsAppAndroid.Models;
using JobsAppAndroid.Services;
using Newtonsoft.Json;

namespace JobsAppAndroid
{
    [Activity(Label = "JobDetailActivity")]
    public class JobDetailActivity : AppCompatActivity
    {
        private Job job;

        private ISharedPreferences preferences;
        private AppPreferences appPreferences;

        private FirebaseApp app;
        private FirebaseAuth auth;

        private CheckBox faveCheck;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_list_detail);

            preferences = PreferenceManager.GetDefaultSharedPreferences(this);
            appPreferences = new AppPreferences(preferences);

            //setup firebase
            app = FirebaseApp.Instance;
            auth = FirebaseAuth.GetInstance(app);

            Intent intent = Intent;
            job = JsonConvert.DeserializeObject<Job>(intent.GetStringExtra("job"));

            Title = job.Title;

            FindViewById<TextView>(Resource.Id.card_title_text).Text = job.Title;
            FindViewById<TextView>(Resource.Id.card_company_text).Text = job.Company;
            FindViewById<TextView>(Resource.Id.card_location_text).Text = job.Location;
            FindViewById<TextView>(Resource.Id.card_desc_text).Text = job.Description;
            FindViewById<TextView>(Resource.Id.footer_emp_typ_text).Text = job.Type;
            FindViewById<TextView>(Resource.Id.card_closing_text).Text = job.Closing.ToLongDateString();

            faveCheck = FindViewById<CheckBox>(Resource.Id.card_fav_button);
            faveCheck.Checked = appPreferences.IsLiked(job);
            faveCheck.CheckedChange += FaveCheck_CheckedChange;

            var qualifications = "";
            var qArr = job.Qualifications;
            if (qArr != null)
            {
                foreach (var str in qArr)
                    qualifications += "\u2022 " + str + "\n";
            }
            

            var competencies = "";
            var cArr = job.Competencies;
            if(cArr != null)
            {
                foreach (var str in cArr)
                    competencies += "\u2022 " + str + "\n";
            }
            

            var contacts = "";
            var coArr = job.Contacts;
            if(coArr != null)
            {
                foreach (var str in coArr)
                    contacts += "\u2022 " + str + "\n";
            }

            FindViewById<TextView>(Resource.Id.card_qualif_text).Text = qualifications;
            FindViewById<TextView>(Resource.Id.card_competence_text).Text = competencies;
            FindViewById<TextView>(Resource.Id.footer_contact_text).Text = contacts;

        }
        /// <summary>
        /// Checkbox changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FaveCheck_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;

            if (auth.CurrentUser != null)
            {
                //Add to likes
                if (checkbox.Checked)
                {
                    //Save to shared preferences
                    job.Liked = true;
                    appPreferences.SaveLike(job);

                }
                else
                {
                    //delete from likes
                    job.Liked = false;
                    appPreferences.DeleteLike(job);
                }

            }
        }
    }
}