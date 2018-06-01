
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

using Firebase.Auth;

using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace Lets_Eat_Android.Views.Owner
{
    [Activity(Label = "Add_Dish")]
    public class Add_Dish : Activity, View.IOnClickListener
    {
        private EditText name, ingredients, description, price;
        private Button save_btn;

        private FirebaseAuth auth;
        private String FBURL = "https://fir-database-ec02e.firebaseio.com/";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Add_Dish);

            auth = FirebaseAuth.GetInstance(MainActivity.app);

            name = FindViewById<EditText>(Resource.Id.add_dish_name);
            ingredients = FindViewById<EditText>(Resource.Id.add_dish_ingredients);
            description = FindViewById<EditText>(Resource.Id.add_dish_description);
            price = FindViewById<EditText>(Resource.Id.add_dish_price);

            save_btn = FindViewById<Button>(Resource.Id.save_add_item);

            save_btn.SetOnClickListener(this);

        }

        public async void OnClick(View v)
        {
            if (name.Text == "" || ingredients.Text == "" || description.Text == "" || price.Text == "")
            {
                Toast.MakeText(this, "Please fill out all dish information entries", ToastLength.Long).Show();
            }
            else
            {
                Dish new_dish = new Dish();
                new_dish.Name = name.Text;
                new_dish.Description = description.Text;
                new_dish.Ingredients = ingredients.Text;
                new_dish.Price = price.Text;

                var firebase = new FirebaseClient(FBURL);
                var item = await firebase
                    .Child("menus")
                    .Child(auth.CurrentUser.Uid)
                    .PostAsync<Dish>(new_dish);

                Toast.MakeText(this, "Dish has been saved", ToastLength.Long).Show();
                Finish();
            }
        }
    }
}
