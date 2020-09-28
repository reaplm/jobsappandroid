using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using JobsAppAndroid.Adapters;
using JobsAppAndroid.Models;
using Org.Apache.Http.Authentication;
using Square.Picasso;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using File = Java.IO.File;

namespace JobsAppAndroid
{
    [Activity(Label = "Account")]
    public class ProfileActivity : AppCompatActivity, IOnSuccessListener, IOnFailureListener, IDialogInterfaceOnClickListener
    {
        private TextView name;
        private TextView phone;
        private ImageView profileImage;

        private ListView listView;

        private FirebaseApp app;
        private FirebaseAuth auth;
        private FirebaseFirestore db;

        const int TAKE_PHOTO_REQ = 100;
        const int GALLERY_PHOTO_REQ = 200;

        private File imageFolder = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_profile);

            //Setup Firebase
            //get current user
            app = FirebaseApp.Instance;
            auth = FirebaseAuth.GetInstance(app);
            db = FirebaseFirestore.GetInstance(app);

            //Profile image location
            var sdpath = Application.Context.GetExternalFilesDir(null).AbsolutePath;
            imageFolder = new File(sdpath + File.Separator + "Profile Images");

            if (!imageFolder.Exists())
            {
                imageFolder.Mkdirs();
            }

            File image = new File(imageFolder.AbsolutePath, auth.CurrentUser.Uid + ".jpg");

            //ListView
            listView = FindViewById<ListView>(Resource.Id.profile_listview);
            List<ListItem> listData = new List<ListItem>
            {
                new ListItem { Heading = "NAME", SubHeading = "Pearl Molefe" },
                new ListItem { Heading = "EMAIL", SubHeading = auth.CurrentUser.Email },
                new ListItem { Heading = "PHONE", SubHeading = "71406569" },
                new ListItem { Heading = "ACCOUNT", SubHeading = "FREE" }
            };
            listView.Adapter = new ListAdapter(this, listData);
            listView.AddFooterView(new View(this));

            profileImage = FindViewById<ImageView>(Resource.Id.profile_pic);
            profileImage.Click += ProfileImage_Click;

            //Load profile image
            Picasso.Get().Load(image)
                .Placeholder(Resource.Drawable.profile_image)
                .Error(Resource.Drawable.profile_image)
                .Into(FindViewById<ImageView>(Resource.Id.profile_pic));

            



        }

        private void ProfileImage_Click(object sender, EventArgs e)
        {
            //Open Dialog
            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(this);

            alertBuilder.SetTitle("Add Photo");
            alertBuilder.SetItems(Resource.Array.upload_photo, this);

            AlertDialog alertDialog = alertBuilder.Create();
            alertDialog.Show();
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPassword_Click(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// Dialog for editing phone number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPhone_Click(object sender, EventArgs e)
        {
            LayoutInflater inflater = LayoutInflater.From(this);
            View view = inflater.Inflate(Resource.Layout.dialog_profile_phone, null);

            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(this);
            alertBuilder.SetView(view);

            var editPhone = view.FindViewById<EditText>(Resource.Id.dialog_edit_phone);
            editPhone.Text = phone.Text;

            alertBuilder.SetTitle("Edit Phone")
                .SetPositiveButton("Submit", delegate
                {
                    Toast.MakeText(this, "You clicked Submit!", ToastLength.Short).Show();

                })
                .SetNegativeButton("Cancel", delegate
                {
                    alertBuilder.Dispose();

                });

            AlertDialog alertDialog = alertBuilder.Create();
            alertDialog.Show();

        }
        /// <summary>
        /// Dialog for editing first and last name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditName_Click(object sender, EventArgs e)
        {
            LayoutInflater inflater = LayoutInflater.From(this);
            View view = inflater.Inflate(Resource.Layout.dialog_profile_name, null);

            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(this);
            alertBuilder.SetView(view);

            var fName = view.FindViewById<EditText>(Resource.Id.dialog_edit_fname);
            var lName = view.FindViewById<EditText>(Resource.Id.dialog_edit_lname);

            string[] fullName = name.Text.Split(" ");
            fName.Text = fullName[0];
            lName.Text = fullName[1];

            alertBuilder.SetTitle("Edit Name")
                .SetPositiveButton("Submit", delegate
                {
                    try
                    {
                        //get current user
                        if (auth.CurrentUser != null)
                        {
                            var document = db.Collection("users").Document(auth.CurrentUser.Uid);
                            var data = new Dictionary<string, Java.Lang.Object>();
                            data.Add("FName", fName.Text);
                            data.Add("LName", lName.Text);

                            document.Update((IDictionary<string, Java.Lang.Object>)data);

                            Toast.MakeText(this, "Done!", ToastLength.Long).Show();


                        }
                        else { Toast.MakeText(this, "Something went wrong. Sorry.", ToastLength.Long).Show(); }
                    }
                    catch (System.Exception ex)
                    {
                        Toast.MakeText(this, "Failed to update. Sorry.", ToastLength.Long).Show();
                    }


                })
                .SetNegativeButton("Cancel", delegate
                {
                    alertBuilder.Dispose();

                });

            AlertDialog alertDialog = alertBuilder.Create();
            alertDialog.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public void OnSuccess(Java.Lang.Object result)
        {
            Toast.MakeText(this, "Profile changed successfully!", ToastLength.Long).Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void OnFailure(Java.Lang.Exception e)
        {
            Console.WriteLine("Failed to update profile image for user - " + auth.CurrentUser.Uid + " - " + e.InnerException.Message);
            Toast.MakeText(this, "Failed to update profile image", ToastLength.Long).Show();
        }
        /// <summary>
        /// Change profile image dialog onClick event handler
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="which"></param>
        public void OnClick(IDialogInterface dialog, int which)
        {
            switch (which)
            {
                case 0:
                    //Upload from camera
                    UploadFromCamera();
                    break;
                case 1:
                    //Upload from gallery
                    UploadFromGallery();
                    break;
                case 2:
                    dialog.Dismiss();
                    break;
            }
        }
        /// <summary>
        /// Upload profile image from gallery
        /// </summary>
        private void UploadFromGallery()
        {
            Intent intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(intent, GALLERY_PHOTO_REQ);
        }

        /// <summary>
        /// Start camera activity
        /// </summary>
        private void UploadFromCamera()
        {
            try
            {
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                StartActivityForResult(intent, TAKE_PHOTO_REQ);

            }

            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error opening camera: " + ex);
            }
        }
        /// <summary>
        /// Callback for camera result and gallery picker
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                //Capture from camera
                if (requestCode == TAKE_PHOTO_REQ && data != null)
                {

                    try
                    {
                        Bitmap srcBitmap = (Bitmap)data.Extras.Get("data");

                        var sdpath = Application.Context.GetExternalFilesDir(null).AbsolutePath;

                        File folder = new File(sdpath + File.Separator + "Profile Images");

                        if (!folder.Exists())
                        {
                            folder.Mkdirs();
                        }

                        File file = new File(folder.AbsolutePath, auth.CurrentUser.Uid + ".jpg");


                        var stream = new FileStream(file.AbsolutePath, FileMode.Create);
                        srcBitmap.Compress(Bitmap.CompressFormat.Png, 80, stream);

                        //Upload to firestore
                        var reference = db.Collection("users").Document(auth.CurrentUser.Uid);
                        reference.Update("PhotoUrl", file.AbsolutePath)
                            .AddOnSuccessListener(this)
                            .AddOnFailureListener(this);

                        profileImage.SetImageBitmap(srcBitmap);
                        stream.Flush();
                        stream.Close();


                    }
                    catch (System.Exception ex)
                    {
                        System.Console.WriteLine("Error uploading profile image: " + ex);
                    }
                }
                //Select from gallery
                if (requestCode == GALLERY_PHOTO_REQ && data != null)
                {

                    try
                    {
                        var imageUri = data.Data;
                        Bitmap srcImage = MediaStore.Images.Media.GetBitmap(this.ContentResolver, imageUri);

                        var sdpath = Application.Context.GetExternalFilesDir(null).AbsolutePath;

                        File folder = new File(sdpath + File.Separator + "Profile Images");

                        if (!folder.Exists())
                        {
                            folder.Mkdirs();
                        }

                        File file = new File(folder.AbsolutePath, auth.CurrentUser.Uid + ".jpg");


                        var stream = new FileStream(file.AbsolutePath, FileMode.Create);
                        srcImage.Compress(Bitmap.CompressFormat.Png, 100, stream);

                        //Upload to firestore
                        var reference = db.Collection("users").Document(auth.CurrentUser.Uid);
                        reference.Update("PhotoUrl", file.AbsolutePath)
                            .AddOnSuccessListener(this)
                            .AddOnFailureListener(this);

                        profileImage.SetImageBitmap(srcImage);
                        stream.Flush();
                        stream.Close();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error choosing from gallery for user - " + auth.CurrentUser.Uid + " - :" + ex);
                    }
                }
            }
        }

       
    }
}