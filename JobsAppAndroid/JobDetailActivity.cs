using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace JobsAppAndroid
{
    [Activity(Label = "JobDetailActivity")]
    public class JobDetailActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_list_detail);

            Intent intent = Intent;

            Title = intent.GetStringExtra("title");

            FindViewById<TextView>(Resource.Id.card_title_text).Text = intent.GetStringExtra("title");
            FindViewById<TextView>(Resource.Id.card_company_text).Text = intent.GetStringExtra("company");
           FindViewById<TextView>(Resource.Id.card_location_text).Text = intent.GetStringExtra("location");
            FindViewById<TextView>(Resource.Id.card_desc_text).Text = intent.GetStringExtra("description");
            FindViewById<TextView>(Resource.Id.footer_emp_typ_text).Text = intent.GetStringExtra("type");
            FindViewById<TextView>(Resource.Id.card_closing_text).Text = intent.GetStringExtra("closing");
            //FindViewById<TextView>(Resource.Id.card_posted_text).Text = intent.GetStringExtra("posted");
            var qualifications = "";
            var qArr = intent.GetStringArrayListExtra("qualifications");
            if (qArr != null)
            {
                foreach (var str in qArr)
                    qualifications += "\u2022 " + str + "\n";
            }
            

            var competencies = "";
            var cArr = intent.GetStringArrayListExtra("competencies");
            if(cArr != null)
            {
                foreach (var str in cArr)
                    competencies += "\u2022 " + str + "\n";
            }
            

            var contacts = "";
            var coArr = intent.GetStringArrayListExtra("contacts");
            if(coArr != null)
            {
                foreach (var str in coArr)
                    contacts += "\u2022 " + str + "\n";
            }

            FindViewById<TextView>(Resource.Id.card_qualif_text).Text = qualifications;
            FindViewById<TextView>(Resource.Id.card_competence_text).Text = competencies;
            FindViewById<TextView>(Resource.Id.footer_contact_text).Text = contacts;

        }
    }
}