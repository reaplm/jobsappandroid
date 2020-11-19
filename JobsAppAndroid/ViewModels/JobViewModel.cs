using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Arch.Lifecycle;
using Android.Gms.Extensions;
using Android.Runtime;
using Firebase;
using Firebase.Firestore;
using JobsAppAndroid.Models;
using IEventListener = Firebase.Firestore.IEventListener;

namespace JobsAppAndroid.ViewModels
{
    public class JobViewModel : BaseViewModel
    { 
        
        public StartCommandFlags LoadJobsCommand { set; get; }


        public JobViewModel() 
        {
        

        }
     
        
    }
}