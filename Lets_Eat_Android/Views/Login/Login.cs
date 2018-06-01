
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
using Android.Gms.Tasks;

using Firebase.Auth;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

using Lets_Eat_Android.Views.Owner;

namespace Lets_Eat_Android.Views.Login
{
    [Activity(Label = "Login")]
    public class Login : Activity, IOnCompleteListener
    {

        Button login_btn;
        TextView text_register;
        EditText password, email;
        private FirebaseAuth auth;

        private string FBURL = "https://fir-database-ec02e.firebaseio.com/";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.Login);

            auth = FirebaseAuth.GetInstance(MainActivity.app);

            login_btn = FindViewById<Button>(Resource.Id.login_button);
            text_register = FindViewById<TextView>(Resource.Id.create_account);
            password = FindViewById<EditText>(Resource.Id.password);
            email = FindViewById<EditText>(Resource.Id.email);

            text_register.Click += Go_registerpage;
            login_btn.Click += Attempt_Login;
            SetEditing(true);

        }

        private void Go_registerpage(object sender, EventArgs e)
        {
            StartActivity(typeof(Views.Login.Registration));
        }

        private void Attempt_Login(object sender, EventArgs e)
        {
            if (email.Text == "" || password.Text == "")
            {
                Toast.MakeText(this, "Please Enter your E-mail and Password", ToastLength.Short).Show();
            }
            else
            {
                Login_User(email.Text, password.Text);
                SetEditing(false);
            }
        }


        private void Login_User(string check_email, string check_password)
        {
            auth.SignInWithEmailAndPassword(check_email, check_password).AddOnCompleteListener(this);

        }

        public async void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {

                var firebase = new FirebaseClient(FBURL);

                string type = await firebase
                    .Child("users")
                    .Child(auth.CurrentUser.Uid)
                    .Child("user_type")
                    .OnceSingleAsync<string>();

                if (type == "customer")
                {
                    StartActivity(typeof(Views.Customer.MainPage_Customer));
                    Finish();
                    Toast.MakeText(this, "Login Successful", ToastLength.Short).Show();
                }
                else
                {
                    StartActivity(typeof(Views.Owner.Ownerpage));
                    Finish();
                    Toast.MakeText(this, "Login Successful", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Login Failed", ToastLength.Short).Show();
                SetEditing(true);
            }

        }



        private void SetEditing(bool enabled)
        {
            email.Enabled = enabled;
            password.Enabled = enabled;
            if (enabled)
            {
                login_btn.Visibility = ViewStates.Visible;
                text_register.Visibility = ViewStates.Visible;
            }
            else
            {
                login_btn.Visibility = ViewStates.Gone;
                text_register.Visibility = ViewStates.Gone;
            }
        }
    }

}

