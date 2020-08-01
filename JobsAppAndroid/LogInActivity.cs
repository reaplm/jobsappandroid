﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

using Firebase;
using Firebase.Auth;
using System;

namespace JobsAppAndroid
{
    [Activity(Label = "LogInActivity")]
    public class LogInActivity : Activity
    {
        private EditText email;
        private EditText password;
        private TextView registerLink;
        private Button loginButton;
        private CheckBox rememberMeCheck;

        private FirebaseAuth firebaseAuth;
        private FirebaseApp app;

        private const int REG_REQUEST_ID = 1001;

        static Result RESULT_OK = Result.Ok;
        static Result RESULT_CANCELED = Result.Canceled;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_login);

            email = FindViewById<EditText>(Resource.Id.email);
            password = FindViewById<EditText>(Resource.Id.password);
            registerLink = FindViewById<TextView>(Resource.Id.register_link);
            loginButton = FindViewById<Button>(Resource.Id.login_button);
            rememberMeCheck = FindViewById<CheckBox>(Resource.Id.login_rememberme);

            registerLink.Click += RegisterLink_Click;
            loginButton.Click += LoginButton_Click;

            //Initiaize Firebase Authentication Service
            app = FirebaseApp.Instance;
            firebaseAuth = FirebaseAuth.GetInstance(app);

        }
        /// <summary>
        /// Start registration activity     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterLink_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));

            StartActivityForResult(intent, REG_REQUEST_ID);
        }


        /// <summary>
        /// Login Button Click Event
        /// Authenticates user using firebase authentication service
        /// </summary>
        /// <param name="sender">Login Button</param>
        /// <param name="e">Event</param>
        protected async void LoginButton_Click(object sender, EventArgs e)
        {
                Intent result = new Intent();

                try
                {

                   

                    // Attempt Login 
                    await firebaseAuth.SignInWithEmailAndPasswordAsync(email.Text, password.Text);

                    if (firebaseAuth.CurrentUser != null) 
                    {
                        result.PutExtra("isLoggedIn", true);
                        result.PutExtra("message", "Login Successful!");
                        SetResult(Result.Ok, result);
                    }
                    else //failed login
                    {
                        result.PutExtra("isLoggedIn", false);
                        result.PutExtra("message", "Login Failed");
                        SetResult(Result.Ok, result);
                    }

                }
                catch (Exception ex)
                {
                    //FirebaseAuthException firebaseEx = ex as FirebaseAuthException;

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
        /// Intent result callback
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //RegistrationActivity
            if (requestCode == REG_REQUEST_ID)
            {
                if (resultCode == RESULT_OK)
                {
                    bool isLoggedIn = data.GetBooleanExtra("isLoggedIn", false);

                    if (isLoggedIn)
                    {
                        Toast.MakeText(this, data.GetStringExtra("message"), ToastLength.Long).Show();

                    }
                    else
                    {
                        Toast.MakeText(this, data.GetStringExtra("message"), ToastLength.Long).Show();
                    }
                }
                else if (resultCode == RESULT_CANCELED)
                {
                    ;
                }
            }
        }
    }
}