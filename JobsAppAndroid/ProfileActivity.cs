using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Org.Apache.Http.Authentication;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace JobsAppAndroid
{
    [Activity(Label = "Account")]
    public class ProfileActivity : AppCompatActivity, IOnSuccessListener
    {
        private TextView name;
        private TextView email;
        private TextView phone;
        private TextView resetPassword;
        private TextView deleteAccount;
        private ImageButton editName;
        private ImageButton editPhone;

        private FirebaseApp app;
        private FirebaseAuth auth;
        private FirebaseFirestore db;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_profile);

            name = FindViewById<TextView>(Resource.Id.profile_name_text);
            email = FindViewById<TextView>(Resource.Id.profile_email_text);
            phone = FindViewById<TextView>(Resource.Id.profile_phone_text);
            resetPassword = FindViewById<TextView>(Resource.Id.profile_reset_password);
            editName = FindViewById<ImageButton>(Resource.Id.profile_name_edit);
            editPhone = FindViewById<ImageButton>(Resource.Id.profile_phone_edit);

            editName.Click += EditName_Click;
            editPhone.Click += EditPhone_Click;

            resetPassword.Click += ResetPassword_Click;

            //get current user
            app = FirebaseApp.Instance;
            auth = FirebaseAuth.GetInstance(app);
            db = FirebaseFirestore.GetInstance(app);

            var user = auth.CurrentUser;
            if(user != null)
            {
                name.Text = "Pearl Molefe";
                email.Text = user.Email;
                phone.Text = "71406569";
            }
            



        }
        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPassword_Click(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// Dialog for editing phone number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPhone_Click(object sender, EventArgs e)
        {


        }
        /// <summary>
        /// Dialog for editing first and last name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditName_Click(object sender, EventArgs e)
        {
           
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            throw new NotImplementedException();
        }
    }
}