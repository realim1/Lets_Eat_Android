
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

using Newtonsoft.Json;

namespace Lets_Eat_Android.Views.Customer
{
    [Activity(Label = "DishPage")]
    public class DishPage : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.DishPage);

            Views.Owner.Dish cur_dish = JsonConvert.DeserializeObject<Views.Owner.Dish>(Intent.GetStringExtra("Cur_Dish"));


            FindViewById<TextView>(Resource.Id.nameTextView).Text = cur_dish.Name;
            FindViewById<TextView>(Resource.Id.ingredientTextView).Text = cur_dish.Ingredients;
            FindViewById<TextView>(Resource.Id.descriptionTextView).Text = cur_dish.Description;
            FindViewById<TextView>(Resource.Id.priceTextView).Text = "$" + cur_dish.Price;

        }
    }
}
