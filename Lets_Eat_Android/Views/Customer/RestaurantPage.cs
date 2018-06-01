
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
using Android.Support.V7.App;

using Firebase.Auth;
using Firebase.Database;

using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

using Newtonsoft.Json;


namespace Lets_Eat_Android.Views.Customer
{
    [Activity(Label = "RestaurantPage")]
    public class RestaurantPage : Activity
    {
        private Restaurant cur_restaurant;

        private Button queue_btn, menu_btn, time1, time2, time3, time4, time5, time6, time7, time8;

        private FirebaseDatabase database;

        private FirebaseAuth auth;

        private int cur_queue_size;
        private EditText party_size;
        private bool already_queued;
        private string customer_name;
        private string date;

        public Button datepick_btn;
        public TextView reservation_text;
        public HorizontalScrollView horizontalScroll;


        private String FBURL = "https://fir-database-ec02e.firebaseio.com/";

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.RestaurantPage);

            database = FirebaseDatabase.GetInstance(MainActivity.app);
            auth = FirebaseAuth.GetInstance(MainActivity.app);

            cur_restaurant = JsonConvert.DeserializeObject<Restaurant>(Intent.GetStringExtra("Restaurant"));

            FindViewById<TextView>(Resource.Id.nameTextView).Text = cur_restaurant.name;
            FindViewById<TextView>(Resource.Id.cuisineTextView).Text = cur_restaurant.cuisine;
            FindViewById<TextView>(Resource.Id.addressdetail).Text = cur_restaurant.address;
            FindViewById<TextView>(Resource.Id.phonedetail).Text = cur_restaurant.phone_number;
            FindViewById<TextView>(Resource.Id.hoursdetail).Text = cur_restaurant.hours;
            FindViewById<TextView>(Resource.Id.descriptiondetail).Text = cur_restaurant.description;

            reservation_text = FindViewById<TextView>(Resource.Id.reservation);

            horizontalScroll = FindViewById<HorizontalScrollView>(Resource.Id.horizontalScrollView1);
            horizontalScroll.Visibility = ViewStates.Gone;

            datepick_btn = FindViewById<Button>(Resource.Id.schedule_button);

            queue_btn = FindViewById<Button>(Resource.Id.queueButton);
            menu_btn = FindViewById<Button>(Resource.Id.menuButton);

            datepick_btn.Click += DateSelect_OnClick;

            menu_btn.Click += delegate {
                Intent intent = new Intent(this, typeof(Views.Customer.View_Menu));
                intent.PutExtra("Cur_Restaurant", JsonConvert.SerializeObject(cur_restaurant));
                StartActivity(intent);
            };

            queue_btn.Click += Queue_Btn_Click;

            var firebase = new FirebaseClient(FBURL);
            customer_name = await firebase
                .Child("users")
                .Child(auth.CurrentUser.Uid)
                .Child("name")
                .OnceSingleAsync<string>();

        }

        private async void Queue_Btn_Click(object sender, EventArgs e)
        {
            await get_Queue_Size();

            if (already_queued)
            {
                Toast.MakeText(this, "You are already in the Queue for " + cur_restaurant.name, ToastLength.Short).Show();
            }
            else
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View view = layoutInflater.Inflate(Resource.Layout.Queue_dialog_box, null);
                Android.Support.V7.App.AlertDialog.Builder alertbuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertbuilder.SetView(view);

                party_size = view.FindViewById<EditText>(Resource.Id.party_size);
                view.FindViewById<TextView>(Resource.Id.positionTextView).Text += cur_queue_size;

                alertbuilder.SetCancelable(false).SetPositiveButton("Queue Up", QueueUp)
                .SetNegativeButton("Cancel", delegate
                {
                    alertbuilder.Dispose();
                });
                Android.Support.V7.App.AlertDialog dialog = alertbuilder.Create();
                dialog.Show();
            }

        }

        private async void QueueUp(object sender, DialogClickEventArgs e)
        {
            await get_Queue_Size();

            DatabaseReference reference = database.GetReference("queues");

            reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("name").SetValue(customer_name);
            reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("party_size").SetValue(party_size.Text);
            reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("queue_position").SetValue(cur_queue_size + 1);

            Toast.MakeText(this, "Your Party of " + party_size.Text + " was Succesfully Queued! Your Position in Queue is: " + (cur_queue_size + 1), ToastLength.Long).Show();
        }


        private async Task get_Queue_Size(){

            var firebase = new FirebaseClient(FBURL);

            var items = await firebase
                .Child("queues")
                .Child(cur_restaurant.uid)
                .OnceAsync<Owner.Queue_Class>();

            cur_queue_size = 0;
            already_queued = false;

            foreach (var item in items)
            {
                if (item.Key == auth.CurrentUser.Uid && !already_queued)
                    already_queued = true;
                
                cur_queue_size++;
            }

        }

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)

            {
                date = time.ToLongDateString();
                reservation_text.Text += " for " + date;

                datepick_btn.Visibility = ViewStates.Gone;

                horizontalScroll.Visibility = ViewStates.Visible;

                string hour_time = DateTime.Now.ToString("HH");
                int hour = Int32.Parse(hour_time) + 1;

                time1 = FindViewById<Button>(Resource.Id.timeButton1);
                time2 = FindViewById<Button>(Resource.Id.timeButton2);
                time3 = FindViewById<Button>(Resource.Id.timeButton3);
                time4 = FindViewById<Button>(Resource.Id.timeButton4);
                time5 = FindViewById<Button>(Resource.Id.timeButton5);
                time6 = FindViewById<Button>(Resource.Id.timeButton6);
                time7 = FindViewById<Button>(Resource.Id.timeButton7);
                time8 = FindViewById<Button>(Resource.Id.timeButton8);

                Set_Reservation_Time(hour++, time1);
                Set_Reservation_Time(hour++, time2);
                Set_Reservation_Time(hour++, time3);
                Set_Reservation_Time(hour++, time4);
                Set_Reservation_Time(hour++, time5);
                Set_Reservation_Time(hour++, time6);
                Set_Reservation_Time(hour++, time7);
                Set_Reservation_Time(hour++, time8);

                DatabaseReference reference = database.GetReference("reservations");
                time1.Click += delegate {
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("date").SetValue(date);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("time").SetValue(time1.Text);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("name").SetValue(customer_name);
                };

                time1.Click += delegate {
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("date").SetValue(date);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("time").SetValue(time1.Text);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("name").SetValue(customer_name);
                    Toast.MakeText(this, "Successfully created a reservation for " + date + ", " + time1.Text, ToastLength.Short).Show();
                };
                time2.Click += delegate {
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("date").SetValue(date);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("time").SetValue(time2.Text);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("name").SetValue(customer_name);
                    Toast.MakeText(this, "Successfully created a reservation for " + date + ", " + time2.Text, ToastLength.Short).Show();
                };
                time3.Click += delegate {
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("date").SetValue(date);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("time").SetValue(time3.Text);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("name").SetValue(customer_name);
                    Toast.MakeText(this, "Successfully created a reservation for " + date + ", " + time3.Text, ToastLength.Short).Show();
                };
                time4.Click += delegate {
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("date").SetValue(date);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("time").SetValue(time4.Text);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("name").SetValue(customer_name);
                    Toast.MakeText(this, "Successfully created a reservation for " + date + ", " + time4.Text, ToastLength.Short).Show();
                };
                time5.Click += delegate {
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("date").SetValue(date);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("time").SetValue(time5.Text);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("name").SetValue(customer_name);
                    Toast.MakeText(this, "Successfully created a reservation for " + date + ", " + time5.Text, ToastLength.Short).Show();
                };
                time6.Click += delegate {
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("date").SetValue(date);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("time").SetValue(time6.Text);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("name").SetValue(customer_name);
                    Toast.MakeText(this, "Successfully created a reservation for " + date + ", " + time6.Text, ToastLength.Short).Show();
                };
                time7.Click += delegate {
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("date").SetValue(date);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("time").SetValue(time7.Text);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("name").SetValue(customer_name);
                    Toast.MakeText(this, "Successfully created a reservation for " + date + ", " + time7.Text, ToastLength.Short).Show();
                };
                time8.Click += delegate {
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("date").SetValue(date);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("time").SetValue(time8.Text);
                    reference.Child(cur_restaurant.uid).Child(auth.CurrentUser.Uid).Child("name").SetValue(customer_name);
                    Toast.MakeText(this, "Successfully created a reservation for " + date + ", " + time8.Text, ToastLength.Short).Show();
                };



            });

            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void Set_Reservation_Time(int time, Button button){
            bool AM = false;
            string myString;
            int temp = time - 12;

            if (time > 24)
                time = time % 24;

            if (time < 12)
                AM = true;
            else
                AM = false;

            if (time == 12)
                myString = time.ToString() + ":00 PM";
            else
                if (time == 24)
            {

                myString = temp.ToString() + ":00 AM";
            }
            else
                    if (AM)
                myString = time.ToString() + ":00 AM";
            else
                myString = temp.ToString() + ":00 PM";

            button.Text = myString;

        }


    }

    public class DatePickerFragment : DialogFragment,
    DatePickerDialog.IOnDateSetListener
    {

        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerFragment frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity, this, currently.Year, currently.Month - 1, currently.Day);
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {

            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);

            _dateSelectedHandler(selectedDate);

        }
    }
}
