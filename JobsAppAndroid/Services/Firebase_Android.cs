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
using Google.Cloud.Firestore;
using JobsAppAndroid.Data;
using JobsAppAndroid.Models;

namespace JobsAppAndroid.Services
{
    public class Firebase_Android : IDatabase
    {
        FirestoreDb firestoreDb = null;

        public Firebase_Android()
        {
            firestoreDb = new Database().GetFirestoreDb();

        }

        /// <summary>
        /// Fetch all jobs from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Job>> FindAllJobs()
        {
            List<Job> jobs = new List<Job>();

            try
            {

                CollectionReference collection = firestoreDb.Collection("jobs");

                QuerySnapshot documents = await collection.GetSnapshotAsync();



                foreach (DocumentSnapshot document in documents)
                {
                    jobs.Add(document.ConvertTo<Job>());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FindAllJobsAsync: " + ex.Message);
            }

            return jobs;
        }
    }
}