
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

using Lets_Eat_Android.Views.Owner;

using Firebase.Auth;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

using Newtonsoft.Json;

namespace Lets_Eat_Android.Views.Customer
{
    [Activity(Label = "View_Menu")]
    public class View_Menu : Activity
    {

        ListView mylist;
        List<Dish> listDishes = new List<Dish>();

        private FirebaseAuth auth;
        private String FBURL = "https://fir-database-ec02e.firebaseio.com/";

        private Restaurant cur_restaurant;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.View_Menu);

            auth = FirebaseAuth.GetInstance(MainActivity.app);

            mylist = FindViewById<ListView>(Resource.Id.menulistView);

            cur_restaurant = JsonConvert.DeserializeObject<Restaurant>(Intent.GetStringExtra("Cur_Restaurant"));

            await loadData();

            mylist.ItemLongClick += Mylist_ItemLongClick;

        }

        void Mylist_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Intent myIntent;

            myIntent = new Intent(this, typeof(DishPage));

            myIntent.PutExtra("Cur_Dish", JsonConvert.SerializeObject(listDishes[e.Position]));

            StartActivity(myIntent); 
        }


        private async Task loadData()
        {

            var firebase = new FirebaseClient(FBURL);

            var items = await firebase
                .Child("menus")
                .Child(cur_restaurant.uid)
                .OnceAsync<Dish>();

            foreach (var item in items)
            {
                Dish dish = new Dish();
                dish = item.Object;
                dish.Uid = item.Key;
                listDishes.Add(dish);

            }

            DishListViewAdapter adapter = new DishListViewAdapter(listDishes);
            adapter.NotifyDataSetChanged();

            mylist.Adapter = adapter;
        }
    }
}
