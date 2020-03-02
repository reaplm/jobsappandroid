using Android.App;
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
        private Button loginButton;
        private CheckBox rememberMeCheck;

        private FirebaseAuth firebaseAuth;
        private FirebaseApp app;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_login);

            //-----------Initiaize Firebase Authentication Service--------------
            app = FirebaseApp.GetInstance(GetString(Resource.String.app_name));
            firebaseAuth = FirebaseAuth.GetInstance(app);

            //check for null auth service

            email = FindViewById<EditText>(Resource.Id.email);
            password = FindViewById<EditText>(Resource.Id.password);
            loginButton = FindViewById<Button>(Resource.Id.login_button);
            rememberMeCheck = FindViewById<CheckBox>(Resource.Id.login_rememberme);

            //------Authenticate User------------------
            loginButton.Click += async (sender, e) =>
            {
                Intent result = new Intent();

                try
                {
                    //---- Attempt Login -----
                    await firebaseAuth.SignInWithEmailAndPasswordAsync(email.Text, password.Text);

                    if (firebaseAuth.CurrentUser != null)
                    {
                        result.PutExtra("message", "Login Successful!");
                        SetResult(Result.Ok, result);
                    }
                    else
                    {
                        result.PutExtra("message", "Login Failed");
                        SetResult(Result.Canceled, result);
                    }

                }
                catch (FirebaseException ex)
                {
                    FirebaseAuthException firebaseEx = ex as FirebaseAuthException;
                    //string exceptionMessage = firebaseEx..Reason.ToString();


                    result.PutExtra("message", ex.Message);
                    SetResult(Result.Canceled, result);
                }
                finally
                {
                    //---close the activity---
                    Finish();
                }
            };

        }
    }
}