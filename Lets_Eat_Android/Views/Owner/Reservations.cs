
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

using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace Lets_Eat_Android.Views.Owner
{
    public class Reservations : Android.Support.V4.App.Fragment
    {

        ListView mylist;
        List<Reservation_Class> listReservations = new List<Reservation_Class>();

        private FirebaseAuth auth;
        private String FBURL = "https://fir-database-ec02e.firebaseio.com/";

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            auth = FirebaseAuth.GetInstance(MainActivity.app);

            await loadData();

            // Create your fragment here
        }

        public static Reservations NewInstance()
        {
            var frag1 = new Reservations { Arguments = new Bundle() };
            return frag1;
        }

        private async Task loadData()
        {

            var firebase = new FirebaseClient(FBURL);

            var items = await firebase
                .Child("reservations")
                .Child(auth.CurrentUser.Uid)
                .OnceAsync<Reservation_Class>();

            foreach (var item in items)
            {
                Reservation_Class reservation = new Reservation_Class();
                reservation = item.Object;
                reservation.account_uid = item.Key;
                listReservations.Add(reservation);

            }

            listReservations.Sort(delegate (Reservation_Class c1, Reservation_Class c2) { return c1.name.CompareTo(c2.name); });

            CustomReservationListAdapter adapter = new CustomReservationListAdapter(this, listReservations);
            adapter.NotifyDataSetChanged();

            mylist.Adapter = adapter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.Reservations, null);

            mylist = view.FindViewById<ListView>(Resource.Id.reservationslistView);

            mylist.ItemClick += Mylist_ItemClick;

            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            return view;
        }

        void Mylist_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this.Activity);
            Android.App.AlertDialog alert = dialog.Create();

            alert.SetTitle("Remove Reservation");
            alert.SetMessage("Are you sure you want to remove this reservation?");
            alert.SetButton("Yes", (c, ev) =>
            {
                var firebase = new FirebaseClient(FBURL);

                firebase
                    .Child("reservations")
                    .Child(auth.CurrentUser.Uid)
                    .Child(listReservations[e.Position].account_uid)
                    .DeleteAsync();

                listReservations.Remove(listReservations[e.Position]);
                CustomReservationListAdapter adapter = new CustomReservationListAdapter(this, listReservations);

                mylist.Adapter = adapter;

                Toast.MakeText(this.Activity, "Reservation has been removed", ToastLength.Short).Show();

            });
            alert.SetButton2("No", (c, ev) =>
            {

            });
            alert.Show();
        }
    }
}
