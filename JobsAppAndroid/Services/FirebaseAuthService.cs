using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using JobsAppAndroid.ViewModels;

namespace JobsAppAndroid.Services
{
    public class FirebaseAuthService
    {
        private Context context;
        private FirebaseAuthProvider authProvider;

        public FirebaseAuthService(Context _context)
        {
            context = _context;
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(
                context.GetString(Resource.String.google_api_key)));
        }
        public async Task<User> SingInWithEmailPassword(string email, string password)
        {
            try
            {
                var auth = await authProvider
                .SignInWithEmailAndPasswordAsync(email, password);

                return auth.User;
            }
           catch(Exception ex)
            {
                Console.WriteLine("Exception in SingInWithEmailPassword - " + ex);
                return null;
            }

        }
    }
}