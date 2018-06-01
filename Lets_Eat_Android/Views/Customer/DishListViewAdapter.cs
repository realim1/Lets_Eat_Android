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
    public class DishListViewAdapter : BaseAdapter<Dish>
    {
        List<Dish> dishes;

        public DishListViewAdapter(List<Dish> dishes)
        {
            this.dishes = dishes;
        }

        public override Dish this[int position]
        {
            get
            {
                return dishes[position];
            }
        }

        public override int Count
        {
            get
            {
                return dishes.Count;
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
            var item = dishes[position];

            var view = convertView;

            if (view == null)
            {
                
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.DishItem, parent, false);

                view.FindViewById<TextView>(Resource.Id.nameTextView).Text = item.Name;
                if (item.Price != null)
                    view.FindViewById<TextView>(Resource.Id.priceTextView).Text = "$" + item.Price;
                if (item?.PhotoID != 0)
                    view.FindViewById<ImageView>(Resource.Id.photoImageView).SetImageResource(item.PhotoID);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.nameTextView).Text = item.Name;
                if (item.Price != null)
                    view.FindViewById<TextView>(Resource.Id.priceTextView).Text = "$" + item.Price;
                else
                {
                    view.FindViewById<TextView>(Resource.Id.priceTextView).Text = "No Price Set";
                }
                if (item?.PhotoID != 0)
                    view.FindViewById<ImageView>(Resource.Id.photoImageView).SetImageResource(item.PhotoID);
            }


            return view;

        }
    }
}
