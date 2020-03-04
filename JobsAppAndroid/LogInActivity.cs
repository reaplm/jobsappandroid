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

            email = FindViewById<EditText>(Resource.Id.email);
            password = FindViewById<EditText>(Resource.Id.password);
            loginButton = FindViewById<Button>(Resource.Id.login_button);
            rememberMeCheck = FindViewById<CheckBox>(Resource.Id.login_rememberme);

            loginButton.Click += LoginButton_Click;

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

                    //Initiaize Firebase Authentication Service
                    app = FirebaseApp.GetInstance(GetString(Resource.String.app_name));
                    firebaseAuth = FirebaseAuth.GetInstance(app);

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

    }
}