
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

using Newtonsoft.Json;

namespace Lets_Eat_Android.Views.Owner
{
    public class Queue : Android.Support.V4.App.Fragment
    {
        ListView mylist;
        List<Queue_Class> listQueue = new List<Queue_Class>();

        Button remove_party_btn;

        private FirebaseAuth auth;
        private String FBURL = "https://fir-database-ec02e.firebaseio.com/";

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            auth = FirebaseAuth.GetInstance(MainActivity.app);

            await loadData();

            // Create your fragment here
        }

        public static Queue NewInstance()
        {
            var frag1 = new Queue { Arguments = new Bundle() };
            return frag1;
        }

        private async Task loadData()
        {

            var firebase = new FirebaseClient(FBURL);

            var items = await firebase
                .Child("queues")
                .Child(auth.CurrentUser.Uid)
                .OnceAsync<Queue_Class>();

            foreach (var item in items)
            {
                Queue_Class party = new Queue_Class();
                party = item.Object;
                party.account_uid = item.Key;
                listQueue.Add(party);
            }

            listQueue.Sort(delegate (Queue_Class c1, Queue_Class c2) { return c1.queue_position.CompareTo(c2.queue_position); });

            CustomQueueListAdapter adapter = new CustomQueueListAdapter(this, listQueue);
            adapter.NotifyDataSetChanged();

            mylist.Adapter = adapter;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.Queue, null);

            mylist = view.FindViewById<ListView>(Resource.Id.queuelistView);

            remove_party_btn = view.FindViewById<Button>(Resource.Id.remove_party_button);

            remove_party_btn.Click += Remove_Party_Btn_Click;

            mylist.ItemClick += Mylist_ItemClick;

            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            return view;
        }

        private async void Remove_Party_Btn_Click(object sender, EventArgs e)
        {
            var firebase = new FirebaseClient(FBURL);

            await firebase
                .Child("queues")
                .Child(auth.CurrentUser.Uid)
                .Child(listQueue[0].account_uid)
                .DeleteAsync();

            listQueue.Remove(listQueue[0]);

            CustomQueueListAdapter adapter = new CustomQueueListAdapter(this, listQueue);

            mylist.Adapter = adapter;

            Toast.MakeText(this.Activity, "Queue has moved", ToastLength.Short).Show();

        }

        void Mylist_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this.Activity);
            Android.App.AlertDialog alert = dialog.Create();

            alert.SetTitle("Remove Party from Queue");
            alert.SetMessage("Are you sure you want to remove this party from the queue?");
            alert.SetButton("Yes", (c, ev) =>
            {
                var firebase = new FirebaseClient(FBURL);

                firebase
                    .Child("queues")
                    .Child(auth.CurrentUser.Uid)
                    .Child(listQueue[e.Position].account_uid)
                    .DeleteAsync();

                listQueue.Remove(listQueue[e.Position]);
                CustomQueueListAdapter adapter = new CustomQueueListAdapter(this, listQueue);

                mylist.Adapter = adapter;

                Toast.MakeText(this.Activity, "Party has been removed from the Queue", ToastLength.Short).Show();

            });
            alert.SetButton2("No", (c, ev) =>
            {

            });
            alert.Show();
        }

    }
}
