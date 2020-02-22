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
using Google.Cloud.Firestore;

namespace JobsAppAndroid.Models
{
    public class Job
    {
        [FirestoreDocumentId]
        public string Id { set; get; }
        [FirestoreProperty]
        public string Title { set; get; }
        [FirestoreProperty]
        public string Description { set; get; }
        [FirestoreProperty]
        public string Source { set; get; }
        [FirestoreProperty]
        public string Type { set; get; }
        [FirestoreProperty]
        public string Location { set; get; }
        [FirestoreProperty]
        public string Company { set; get; }
        [FirestoreProperty]
        public DateTime Closing { set; get; }
        [FirestoreProperty]
        public DateTime CreateTime { set; get; }
        [FirestoreProperty]
        public List<string> Contacts { set; get; }
        [FirestoreProperty]
        public List<string> Competencies { set; get; }
        [FirestoreProperty]
        public List<string> Qualifications { set; get; }
    }
}
       
