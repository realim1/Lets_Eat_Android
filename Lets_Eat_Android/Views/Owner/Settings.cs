
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Firebase.Auth;
using Firebase.Database;

using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Lets_Eat_Android.Views.Customer;

namespace Lets_Eat_Android.Views.Owner
{
    public class Settings : Android.Support.V4.App.Fragment
    {
        private Button save_btn;

        private EditText restaurant_name, cuisine, description, address, contact, hours;

        private String FBURL = "https://fir-database-ec02e.firebaseio.com/";
        private FirebaseAuth auth;
        private FirebaseDatabase database;
        private Restaurant cur_restaurant = new Restaurant();

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            auth = FirebaseAuth.GetInstance(MainActivity.app);
            database = FirebaseDatabase.GetInstance(MainActivity.app);

            await loadData();

        }

        public static Settings NewInstance()
        {
            var frag1 = new Settings { Arguments = new Bundle() };
            return frag1;
        }

        private async Task loadData()
        {

            var firebase = new FirebaseClient(FBURL);

            cur_restaurant = await firebase
                .Child("users")
                .Child(auth.CurrentUser.Uid)
                .OnceSingleAsync<Restaurant>();

            restaurant_name.Text = cur_restaurant.name;
            cuisine.Text = cur_restaurant.cuisine;
            description.Text = cur_restaurant.description;
            address.Text = cur_restaurant.address;
            contact.Text = cur_restaurant.phone_number;
            hours.Text = cur_restaurant.hours;

            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.Settings, null);

            restaurant_name = view.FindViewById<EditText>(Resource.Id.setting_restaurant_name);
            cuisine = view.FindViewById<EditText>(Resource.Id.setting_cuisine);
            description = view.FindViewById<EditText>(Resource.Id.setting_description);
            address = view.FindViewById<EditText>(Resource.Id.setting_address);
            contact = view.FindViewById<EditText>(Resource.Id.setting_phone_num);
            hours = view.FindViewById<EditText>(Resource.Id.setting_hours);

            save_btn = view.FindViewById<Button>(Resource.Id.save_restaurant_info);

            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            save_btn.Click += delegate {
                SetEditing(false);
                Save_Info();

                Toast.MakeText(this.Activity, "Information was successfully saved", ToastLength.Short).Show();

            };

            return view;
        }

        private void SetEditing(bool enabled){

            restaurant_name.Enabled = enabled;
            cuisine.Enabled = enabled;
            description.Enabled = enabled;
            address.Enabled = enabled;
            contact.Enabled = enabled;
            hours.Enabled = enabled;
            if (enabled)
            {
                save_btn.Visibility = ViewStates.Visible;
            }
            else
            {
                save_btn.Visibility = ViewStates.Gone;
            }

        }

        private void Save_Info(){
            DatabaseReference reference = database.GetReference("users");

            reference.Child(auth.CurrentUser.Uid).Child("address").SetValue(address.Text);
            reference.Child(auth.CurrentUser.Uid).Child("cuisine").SetValue(cuisine.Text);
            reference.Child(auth.CurrentUser.Uid).Child("description").SetValue(description.Text);
            reference.Child(auth.CurrentUser.Uid).Child("hours").SetValue(hours.Text);
            reference.Child(auth.CurrentUser.Uid).Child("name").SetValue(restaurant_name.Text);
            reference.Child(auth.CurrentUser.Uid).Child("phone_number").SetValue(contact.Text);

            SetEditing(true);
        }
    }
}
