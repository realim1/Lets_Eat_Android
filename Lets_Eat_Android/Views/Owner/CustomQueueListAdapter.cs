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
    public class CustomQueueListAdapter : BaseAdapter<Queue_Class>
    {
        List<Queue_Class> queue;
        Android.Support.V4.App.Fragment fragment;
        LayoutInflater inflater;

        public CustomQueueListAdapter(Android.Support.V4.App.Fragment fragment, List<Queue_Class> queue)
        {
            this.fragment = fragment;
            this.queue = queue;
        }

        public override Queue_Class this[int position]
        {
            get
            {
                return queue[position];
            }
        }

        public override int Count
        {
            get
            {
                return queue.Count;
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
            var item = queue[position];

            var view = convertView;

            if (view == null)
            {
                inflater = Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
                view = inflater.Inflate(Resource.Layout.QueueItem, parent, false);

                view.FindViewById<TextView>(Resource.Id.nameTextView).Text = item.name;
                if (item.party_size != null)
                    view.FindViewById<TextView>(Resource.Id.party_sizeTextView).Text += item.party_size;
                if (item.queue_position != 0)
                    view.FindViewById<TextView>(Resource.Id.positionTextView).Text += item.queue_position;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.nameTextView).Text = item.name;
                if (item.party_size != null)
                    view.FindViewById<TextView>(Resource.Id.party_sizeTextView).Text += item.party_size;
                if (item.queue_position != 0)
                    view.FindViewById<TextView>(Resource.Id.positionTextView).Text += item.queue_position;
            }


            return view;

        }
    }
}
