using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using JobsAppAndroid.Models;

namespace JobsAppAndroid
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : AppCompatActivity, IOnCompleteListener
    {
        private EditText fName;
        private EditText lName;
        private EditText phone;
        private EditText email;
        private EditText password;
        private Button registerButton;
        private CheckBox rememberMeCheck;

        private FirebaseApp app;
        private FirebaseAuth auth;
        //private FirebaseFirestore firestore;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_register);

            Title = "Register";

            fName = FindViewById<EditText>(Resource.Id.register_fname);
            lName = FindViewById<EditText>(Resource.Id.register_lname);
            phone = FindViewById<EditText>(Resource.Id.register_phone);
            email = FindViewById<EditText>(Resource.Id.register_email);
            password = FindViewById<EditText>(Resource.Id.register_password);
            phone = FindViewById<EditText>(Resource.Id.register_phone);
            registerButton = FindViewById<Button>(Resource.Id.register_button);
            rememberMeCheck = FindViewById<CheckBox>(Resource.Id.register_rememberme);

            registerButton.Click += RegisterButton_Click;

            //Initiaize Firebase Authentication Service
            app = FirebaseApp.Instance;
            auth = FirebaseAuth.GetInstance(app);
        }
        /// <summary>
        /// Firebase Registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            Intent result = new Intent();
            try
            {

               

                // Attempt registration and login
                await auth.CreateUserWithEmailAndPasswordAsync(email.Text, password.Text);

                if (auth.CurrentUser != null)//Success
                {
                    
                    result.PutExtra("isLoggedIn", true);
                    result.PutExtra("message", "Registration Successful!");
                    SetResult(Result.Ok, result);

                }
                else //failed registration
                {
                    result.PutExtra("isLoggedIn", false);
                    result.PutExtra("message", "Registration falied");
                    SetResult(Result.Ok, result);

                }

            }
            catch (Exception ex)
            {
                result.PutExtra("isLoggedIn", false);
                result.PutExtra("message", ex.Message);
                SetResult(Result.Ok, result);

            }
            finally
            {
                //Close Activity 
                Finish();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
               //Log this 
            }
            else
            {

            }
        }
    }
}