using Android.Arch.Lifecycle;
using Android.Runtime;
using Firebase;
using Firebase.Firestore;
using JobsAppAndroid.Models;
using JobsAppAndroid.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace JobsAppAndroid.ViewModels
{
    public class BaseViewModel : ViewModel, INotifyPropertyChanged, IEventListener
    {

        private bool isBusy = false;
        public bool IsBusy
        {
            set
            {
                SetProperty(ref isBusy, value);
            }
            get { return isBusy; }
        }
        public ObservableCollection<Job> Jobs { set; get; }

        private FirebaseApp app;
        private FirebaseFirestore db;

        public BaseViewModel()
        {
            app = FirebaseApp.Instance;
            db = FirebaseFirestore.GetInstance(app);

            Jobs = new ObservableCollection<Job>();
            FetchCollection();
        }
        

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;

            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        #endregion
        #region Firebase Service
        /// <summary>
        /// Fetch collection from firestore database
        /// </summary>
        public void FetchCollection()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                CollectionReference collection = db.Collection("jobs");
                collection.AddSnapshotListener(this);
                // collection.Get().AddOnSuccessListener(this).AddOnFailureListener(this);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error in FetchCollection: " + ex.Message);
            }

        }
        /// <summary>
        /// Snapshot listener that is trigger by DB changes (added, modified, deleted)
        /// </summary>
        /// <param name="value">New Snapshot</param>
        /// <param name="error">Firebase exception object</param>
        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            if (error != null)
            {
                //Log.w("TAG", "listen:error", error);
                return;
            }

            var snapshots = (QuerySnapshot)value;
            foreach (DocumentChange dc in snapshots.DocumentChanges)
            {
                var dictionary = dc.Document.Data;
                var job = ToObject(dictionary);
                job.Id = dc.Document.Id;



                switch (dc.GetType().ToString())
                {
                    case "ADDED":
                        //Log.d("TAG", "New Msg: " + dc.getDocument().toObject(Message.class));
                        Jobs.Add(job);

                        break;
                    case "MODIFIED":
                        //find modified item and replace with new document
                        var item = Jobs.FirstOrDefault(x => x.Id == job.Id);
                        int index = Jobs.IndexOf(item);

                        Jobs.Remove(item);
                        Jobs.Insert(index, job);

                        break;
                    case "REMOVED":
                        //Remove item
                        var itemToRemove = Jobs.FirstOrDefault(x => x.Id == job.Id);
                        Jobs.Remove(itemToRemove);
                        break;
                }
            }
            IsBusy = false;
            OnPropertyChanged("Jobs");
        }
        /// <summary>
        /// Convert DocumentSnapshot to Object
        /// </summary>
        /// <param name="source">Dictionary</param>
        /// <returns>Object</returns>
        public Job ToObject(IDictionary<string, Java.Lang.Object> source)
        {
            var newObject = new Job();
            var newObjectType = newObject.GetType();

            foreach (var item in source)
            {
                if (newObjectType.GetProperty(item.Key) != null)
                {
                    if (item.Value.GetType().Equals(typeof(Java.Lang.String)))
                    {
                        //covert Java.Lang.String to System.string
                        newObjectType
                            .GetProperty(item.Key)
                            .SetValue(newObject, item.Value.ToString(), null);

                    }
                    if (item.Value.GetType().Equals(typeof(Java.Util.Date)))
                    {
                        //covert Java.Util.Date to DateTime
                        Java.Util.Date dt = (Java.Util.Date)(item.Value);
                        DateTime dateValue = new DateTime(1970, 1, 1).AddMilliseconds(dt.Time).ToLocalTime();

                        newObjectType
                            .GetProperty(item.Key)
                            .SetValue(newObject, dateValue, null);
                    }
                    if (item.Value.GetType().Equals(typeof(JavaList)))
                    {
                        //covert JavaList to List<>
                        var javaList = (JavaList)item.Value;
                        List<string> newList = new List<string>();

                        foreach (var listItem in javaList)
                        {
                            newList.Add(listItem.ToString());
                        }

                        newObjectType
                            .GetProperty(item.Key)
                            .SetValue(newObject, newList, null);
                    }
                }
            }

            return newObject;
        }

        #endregion
    }

}