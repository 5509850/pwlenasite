using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Pw.Lena.Slave.Droid.Screens.Adapters.Models;
using Pw.Lena.Slave.Droid.UI.ViewHolders;

namespace Pw.Lena.Slave.Droid.Screens.Adapters
{
    public class AppMenuItemAdapter : BaseAdapter<AppMenuItem>
    {
        private readonly IList<AppMenuItem> items;
        private readonly LayoutInflater inflater;

        public AppMenuItemAdapter(IList<AppMenuItem> items, LayoutInflater inflater)
        {
            this.items = items;
            this.inflater = inflater;
        }

        public override int Count => items.Count;

        public override AppMenuItem this[int position] => items[position];

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            AppMenuItemViewHolder viewHolder = null;

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.app_menu_item, parent, false);

                viewHolder = new AppMenuItemViewHolder(convertView);

                viewHolder.AppMenuItemButton.Click += AppMenuItemButton_Click;
                viewHolder.AppMenuItemText.Click += AppMenuItemText_Click;

                convertView.Tag = viewHolder;
            }

            if (viewHolder == null)
            {
                viewHolder = (AppMenuItemViewHolder)convertView.Tag;
            }

            SetItem(position, viewHolder);

            return convertView;
        }

        private void SetItem(int position, AppMenuItemViewHolder viewHolder)
        {
            var item = items[position];

            viewHolder.AppMenuItemText.SetText(item.LeftSubItem.ResourceId);
            viewHolder.AppMenuItemText.Tag = position;

            if (item.RightSubItem != null)
            {
                viewHolder.AppMenuItemButton.SetImageResource(item.RightSubItem.ResourceId);
                viewHolder.AppMenuItemButton.Visibility = ViewStates.Visible;
                viewHolder.AppMenuItemButton.Tag = position;
            }
            else
            {
                viewHolder.AppMenuItemButton.Visibility = ViewStates.Gone;
            }
        }

        private void AppMenuItemText_Click(object sender, EventArgs e)
        {
            var textView = (UI.TextView)sender;

            var position = (int)textView.Tag;

            items[position].LeftSubItem?.Action?.Invoke();
        }

        private void AppMenuItemButton_Click(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;

            var position = (int)button.Tag;

            items[position].RightSubItem?.Action?.Invoke();
        }
    }
}