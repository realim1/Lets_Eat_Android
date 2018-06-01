
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;

using System.Threading.Tasks;

using Firebase.Auth;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using System;

namespace Lets_Eat_Android.Views.Owner
{
    [Activity(Label = "Ownerpage", Theme = "@style/Theme.DesignDemo")]
    public class Ownerpage : AppCompatActivity
    {

        BottomNavigationView bottom_navigationView;

        private FirebaseAuth auth;
        private FirebaseUser user;
        private const string FBURL = "https://fir-database-ec02e.firebaseio.com/";
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Ownerpage);

            auth = FirebaseAuth.GetInstance(MainActivity.app);

            user = auth.CurrentUser;

            bottom_navigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            LoadFragment(Resource.Id.bottom_settings);

            bottom_navigationView.NavigationItemSelected += BottomNavigation_NavigationItemSelected;
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }


        void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.bottom_menu_config:
                    fragment = Menu_Config.NewInstance();
                    break;
                case Resource.Id.bottom_queue:
                    fragment = Queue.NewInstance();
                    break;
                case Resource.Id.bottom_tablelist:
                    fragment = Table_List.NewInstance();
                    break;
                case Resource.Id.bottom_reservations:
                    fragment = Reservations.NewInstance();
                    break;
                case Resource.Id.bottom_settings:
                    fragment = Settings.NewInstance();
                    break;
            }

            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

    }
}
