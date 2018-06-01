
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
using Firebase.Database;

namespace Lets_Eat_Android.Views.Login
{
    [Activity(Label = "Registration")]
    public class Registration : Activity, IOnCompleteListener
    {

        Button register_btn;
        EditText name, phone_number, email, password, location;
        Spinner type_spinner;
        String selected_type;

        FirebaseAuth auth;
        FirebaseDatabase database;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Registration);

            auth = FirebaseAuth.GetInstance(MainActivity.app);

            database = FirebaseDatabase.GetInstance(MainActivity.app);

            register_btn = FindViewById<Button>(Resource.Id.register_button);
            name = FindViewById<EditText>(Resource.Id.user_name);
            phone_number = FindViewById<EditText>(Resource.Id.user_phone);
            email = FindViewById<EditText>(Resource.Id.user_email);
            password = FindViewById<EditText>(Resource.Id.user_password);
            location = FindViewById<EditText>(Resource.Id.user_location);

            Set_Entries();

            initSpinner();


            register_btn.Click += Register_User;
        }

        private void Register_User(object sender, EventArgs e)
        {
            if (selected_type == "Restaurant Owner" && (name.Text == "" || phone_number.Text == "" || email.Text == "" || password.Text == "" || location.Text == ""))
            {
                Toast.MakeText(this, "Please fill out all entries to register", ToastLength.Short).Show();
            }
            else if (selected_type == "Customer" && (name.Text == "" || phone_number.Text == "" || email.Text == "" || password.Text == ""))
            {
                Toast.MakeText(this, "Please fill out all entries to register", ToastLength.Short).Show();
            }

            else
            {
                SetEditing(false);
                CreateAccount(email.Text, password.Text);
            }
        }

        private void CreateAccount(string new_email, string new_password)
        {
            auth.CreateUserWithEmailAndPassword(new_email, new_password).AddOnCompleteListener(this);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {

                DatabaseReference reference = database.GetReference("users");

                reference.Child(auth.CurrentUser.Uid).Child("name").SetValue(name.Text);
                reference.Child(auth.CurrentUser.Uid).Child("phone_number").SetValue(phone_number.Text);
                reference.Child(auth.CurrentUser.Uid).Child("email").SetValue(email.Text);
                reference.Child(auth.CurrentUser.Uid).Child("password").SetValue(password.Text);
                reference.Child(auth.CurrentUser.Uid).Child("user_type").SetValue(selected_type);


                reference.Child(auth.CurrentUser.Uid).Child("address").SetValue(location.Text);

                Toast.MakeText(this, "Account has been Created", ToastLength.Short).Show();
                Finish();
            }
            else
            {
                Toast.MakeText(this, "E-mail or Password already exists", ToastLength.Short).Show();
                SetEditing(true);
            }

        }


        private void initSpinner()
        {
            type_spinner = FindViewById<Spinner>(Resource.Id.user_spinner);
            ArrayAdapter adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.user_types, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            type_spinner.Adapter = adapter;
            type_spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_Itemselected);
        }


        private void Spinner_Itemselected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            this.selected_type = spinner.GetItemAtPosition(e.Position).ToString();
            Set_Entries();
            Console.WriteLine("Current Spinner Item: " + this.selected_type);
        }


        private void SetEditing(bool enabled)
        {
            email.Enabled = enabled;
            password.Enabled = enabled;
            name.Enabled = enabled;
            phone_number.Enabled = enabled;
            location.Enabled = enabled;
            type_spinner.Enabled = enabled;

            if (enabled)
            {
                register_btn.Visibility = ViewStates.Visible;
                type_spinner.Visibility = ViewStates.Visible;
            }
            else
            {
                register_btn.Visibility = ViewStates.Gone;
                type_spinner.Visibility = ViewStates.Gone;
            }
        }

        private void Set_Entries()
        {
            if (selected_type == "customer")
            {
                email.Visibility = ViewStates.Visible;
                password.Visibility = ViewStates.Visible;
                name.Visibility = ViewStates.Visible;
                phone_number.Visibility = ViewStates.Visible;
                register_btn.Visibility = ViewStates.Visible;

                location.Visibility = ViewStates.Gone;
                location.Text = "";
            }
            else if (selected_type == "owner")
            {
                email.Visibility = ViewStates.Visible;
                password.Visibility = ViewStates.Visible;
                name.Visibility = ViewStates.Visible;
                phone_number.Visibility = ViewStates.Visible;
                register_btn.Visibility = ViewStates.Visible;
                location.Visibility = ViewStates.Visible;

                name.Hint = "Restaurant Name";

            }
            else
            {
                email.Visibility = ViewStates.Gone;
                password.Visibility = ViewStates.Gone;
                name.Visibility = ViewStates.Gone;
                phone_number.Visibility = ViewStates.Gone;
                register_btn.Visibility = ViewStates.Gone;
                location.Visibility = ViewStates.Gone;
            }
        }
    }
}
