
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

using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;

using Firebase.Auth;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

using Newtonsoft.Json;

namespace Lets_Eat_Android.Views.Owner
{
    public class Menu_Config : Android.Support.V4.App.Fragment
    {

        ListView mylist;
        List<Dish> listDishes = new List<Dish>();

        Button add_item_btn;

        private FirebaseAuth auth;
        private String FBURL = "https://fir-database-ec02e.firebaseio.com/";

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            auth = FirebaseAuth.GetInstance(MainActivity.app);

            await loadData();
            // Create your fragment here
        }

        public static Menu_Config NewInstance()
        {
            var frag1 = new Menu_Config { Arguments = new Bundle() };
            return frag1;
        }

        private async Task loadData()
        {

            var firebase = new FirebaseClient(FBURL);

            var items = await firebase
                .Child("menus")
                .Child(auth.CurrentUser.Uid)
                .OnceAsync<Dish>();

            foreach (var item in items)
            {
                Dish dish = new Dish();
                dish = item.Object;
                dish.Uid = item.Key;
                listDishes.Add(dish);

            }

            CustomDishListAdapter adapter = new CustomDishListAdapter(this, listDishes);
            adapter.NotifyDataSetChanged();

            mylist.Adapter = adapter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.Menu_Config, null);

            mylist = view.FindViewById<ListView>(Resource.Id.menulistView);

            add_item_btn = view.FindViewById<Button>(Resource.Id.add_item_button);

            add_item_btn.Click += delegate {

                Intent myIntent = new Intent();

                myIntent = new Intent(this.Activity, typeof(Add_Dish));

                StartActivity(myIntent);

            };

            mylist.ItemClick += Mylist_ItemClick;

            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            return view;
        }

        void Mylist_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this.Activity);
            Android.App.AlertDialog alert = dialog.Create();

            alert.SetTitle("Handle Dish Item");
            alert.SetMessage("How would you like to handle this dish item?");
            alert.SetButton("Remove", (c, ev) =>
            {
                var firebase = new FirebaseClient(FBURL);

                firebase
                    .Child("menus")
                    .Child(auth.CurrentUser.Uid)
                    .Child(listDishes[e.Position].Uid)
                    .DeleteAsync();

                listDishes.Remove(listDishes[e.Position]);
                CustomDishListAdapter adapter = new CustomDishListAdapter(this,listDishes);

                mylist.Adapter = adapter;

                Toast.MakeText(this.Activity, "Dish has been removed from your menu", ToastLength.Short).Show();

            });
            alert.SetButton2("Edit", (c, ev) =>
            {
                Intent myIntent = new Intent();

                myIntent = new Intent(this.Activity, typeof(Edit_Dish));

                myIntent.PutExtra("Dish", JsonConvert.SerializeObject(listDishes[e.Position]));

                StartActivity(myIntent);

            });
            alert.SetButton3("Ignore", (c, ev) =>
            {

            });
            alert.Show();


        }


    }
}
