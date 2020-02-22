

using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Grpc.Core;
using System;
using System.IO;

namespace JobsAppAndroid.Data
{
    public class Database
    {
        private FirestoreDb firestoreDb = null;

        public Database()
        {
            InitializeFirestore();
        }

        /// <summary>
        /// Create Firestore client using credentials loaded from a JSON file
        /// GOOGLE_APPLICATION_CREDENTIALS exception occurs when credentials are not set
        /// </summary>
        private void InitializeFirestore()
        {
            try
            {
                //Firestore setup
                var path = Path.Combine(
                       System.Environment.GetFolderPath(System.Environment.SpecialFolder
                       .ApplicationData), "jobsapp-c8100-7dd3e3e004b2.json");

                var credential = GoogleCredential
                    .FromFile(path);
                ChannelCredentials channelCredentials = credential.ToChannelCredentials();
                Channel channel = new Channel(FirestoreClient.DefaultEndpoint.ToString(), channelCredentials);
                FirestoreClient firestoreClient = FirestoreClient.Create(channel);
                firestoreDb = FirestoreDb.Create("jobsapp-c8100", firestoreClient);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception initializing Firestore Database " + ex);
            }

        }
        public FirestoreDb GetFirestoreDb()
        {
            if (firestoreDb == null)
                InitializeFirestore();

            return firestoreDb;
        }
    }
}