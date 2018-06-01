
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

using Firebase.Auth;
using Firebase.Database;

using Newtonsoft.Json;

namespace Lets_Eat_Android.Views.Owner
{
    [Activity(Label = "Edit_Dish")]
    public class Edit_Dish : Activity
    {

        private Dish cur_dish;

        private EditText name, ingredients, description, price;
        private Button save_btn;

        private FirebaseAuth auth;
        private FirebaseDatabase database;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Edit_Dish);

            auth = FirebaseAuth.GetInstance(MainActivity.app);
            database = FirebaseDatabase.GetInstance(MainActivity.app);

            name = FindViewById<EditText>(Resource.Id.edit_dish_name);
            ingredients = FindViewById<EditText>(Resource.Id.edit_dish_ingredients);
            description = FindViewById<EditText>(Resource.Id.edit_dish_description);
            price = FindViewById<EditText>(Resource.Id.edit_dish_price);

            save_btn = FindViewById<Button>(Resource.Id.save_edit_item);

            cur_dish = JsonConvert.DeserializeObject<Dish>(Intent.GetStringExtra("Dish"));

            name.Text = cur_dish.Name;
            ingredients.Text = cur_dish.Ingredients;
            description.Text = cur_dish.Description;
            price.Text = cur_dish.Price;

            save_btn.Click += delegate {
                DatabaseReference reference = database.GetReference("menus");

                reference.Child(auth.CurrentUser.Uid).Child(cur_dish.Uid).Child("Name").SetValue(name.Text);
                reference.Child(auth.CurrentUser.Uid).Child(cur_dish.Uid).Child("Ingredients").SetValue(ingredients.Text);
                reference.Child(auth.CurrentUser.Uid).Child(cur_dish.Uid).Child("Description").SetValue(description.Text);
                reference.Child(auth.CurrentUser.Uid).Child(cur_dish.Uid).Child("Price").SetValue(price.Text);

                Toast.MakeText(this, "Dish item was successfully edited", ToastLength.Short).Show();
                Finish();
            };
        }
    }
}
