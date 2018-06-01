
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

using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;

using Firebase.Auth;
using Firebase.Database;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

using Newtonsoft.Json;

namespace Lets_Eat_Android.Views.Customer
{
    [Activity(Label = "MainPage_Customer", Theme = "@style/Theme.DesignDemo")]
    public class MainPage_Customer : AppCompatActivity
    {
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        FirebaseAuth auth;
        ListView mylist;
        List<Restaurant> listRestaurants = new List<Restaurant>();

        FirebaseDatabase database;

        private String FBURL = "https://fir-database-ec02e.firebaseio.com/";

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.MainPage_Customer);

            database = FirebaseDatabase.GetInstance(MainActivity.app);

            mylist = FindViewById<ListView>(Resource.Id.listView);

            await loadData();

            auth = FirebaseAuth.GetInstance(MainActivity.app);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            SetSupportActionBar(toolbar);

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.hamburger_drawer);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            Setup_Nav();

            mylist.ItemClick += Mylist_ItemClick;
        }

        void Mylist_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Views.Customer.RestaurantPage));
            intent.PutExtra("Restaurant", JsonConvert.SerializeObject(listRestaurants[e.Position]));
            StartActivity(intent);
        }


        private async Task loadData()
        {

            var firebase = new FirebaseClient(FBURL);

            var items = await firebase
                .Child("users")
                .OnceAsync<Restaurant>();

            foreach (var item in items)
            {
                if (item.Object.user_type == "owner")
                {
                    Restaurant restaurant = new Restaurant();
                    restaurant = item.Object;
                    restaurant.uid = item.Key;
                    listRestaurants.Add(restaurant);
                }
            }

            CustomRestaurantListAdapter adapter = new CustomRestaurantListAdapter(listRestaurants);

            mylist.Adapter = adapter;
        }


        private void Setup_Nav()
        {

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            navigationView.NavigationItemSelected += (sender, e) =>
            {

                e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.action_home:
                        Toast.MakeText(this, "Go Home", ToastLength.Short).Show();
                        break;

                    case Resource.Id.action_logout:
                        auth.SignOut();
                        StartActivity(typeof(Views.Login.Login));
                        Finish();
                        Toast.MakeText(this, "Successfully Signed Out", ToastLength.Short).Show();
                        break;


                    default:
                        break;


                }

                drawerLayout.CloseDrawers();
            };

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer((int)GravityFlags.Left);
                    return true;

            }
            return base.OnOptionsItemSelected(item);
        }
    }
}
