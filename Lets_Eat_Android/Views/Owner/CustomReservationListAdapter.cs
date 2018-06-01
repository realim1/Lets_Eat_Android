using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Views;


namespace Lets_Eat_Android.Views.Owner
{
    public class CustomReservationListAdapter : BaseAdapter<Reservation_Class>
    {
        List<Reservation_Class> reservations;
        Android.Support.V4.App.Fragment fragment;
        LayoutInflater inflater;

        public CustomReservationListAdapter(Android.Support.V4.App.Fragment fragment, List<Reservation_Class> reservations)
        {
            this.fragment = fragment;
            this.reservations = reservations;
        }

        public override Reservation_Class this[int position]
        {
            get
            {
                return reservations[position];
            }
        }

        public override int Count
        {
            get
            {
                return reservations.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = reservations[position];

            var view = convertView;

            if (view == null)
            {
                inflater = Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
                view = inflater.Inflate(Resource.Layout.ReservationItem, parent, false);

                view.FindViewById<TextView>(Resource.Id.nameTextView).Text = item.name;
                if (item?.party_size != null)
                    view.FindViewById<TextView>(Resource.Id.party_sizeTextView).Text += item.party_size;
                if (item.date != null)
                    view.FindViewById<TextView>(Resource.Id.dateTextView).Text += item.date;
                if (item.time != null)
                    view.FindViewById<TextView>(Resource.Id.timeTextView).Text += item.time;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.nameTextView).Text = item.name;
                if (item?.party_size != null)
                    view.FindViewById<TextView>(Resource.Id.party_sizeTextView).Text += item.party_size;
                if (item.date != null)
                    view.FindViewById<TextView>(Resource.Id.dateTextView).Text += item.date;
                if (item.time != null)
                    view.FindViewById<TextView>(Resource.Id.timeTextView).Text += item.time;
            }


            return view;

        }
    }
}
