using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace JobsAppAndroid.Models
{
    public class User
    {
        public string Uuid { set; get; }
        public string Display { set; get; }
        public string FName { set; get; }
        public string LName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string PhotoUrl { set; get; }
        public bool IsEmailVerified { set; get; }
    }
}