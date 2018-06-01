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


namespace Lets_Eat_Android.Views.Customer
{
    public class CustomRestaurantListAdapter : BaseAdapter<Restaurant>
    {
        List<Restaurant> restaurants;

        public CustomRestaurantListAdapter(List<Restaurant> restaurants)
        {
            this.restaurants = restaurants;
        }

        public override Restaurant this[int position]
        {
            get
            {
                return restaurants[position];
            }
        }

        public override int Count
        {
            get
            {
                return restaurants.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = restaurants[position];

            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RestaurantItem, parent, false);

                view.FindViewById<TextView>(Resource.Id.nameTextView).Text = item.name;
                if (item.cuisine != null)
                    view.FindViewById<TextView>(Resource.Id.cuisineTextView).Text = item.cuisine;
                if (item?.photoID != 0)
                    view.FindViewById<ImageView>(Resource.Id.photoImageView).SetImageResource(item.photoID);
            }
            else{
                view.FindViewById<TextView>(Resource.Id.nameTextView).Text = item.name;
                if (item.cuisine != null)
                    view.FindViewById<TextView>(Resource.Id.cuisineTextView).Text = item.cuisine;
                else{
                    view.FindViewById<TextView>(Resource.Id.cuisineTextView).Text = null;
                }
                if (item?.photoID != 0)
                    view.FindViewById<ImageView>(Resource.Id.photoImageView).SetImageResource(item.photoID);
            }


            return view;

        }
    }
}
