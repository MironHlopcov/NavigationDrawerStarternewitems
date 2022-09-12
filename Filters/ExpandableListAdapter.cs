using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyFinToControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFinToControl.Filters
{
    public class ExpandableListAdapter : BaseExpandableListAdapter
    {
        public Activity context;
        public List<string> headerList;
        public Dictionary<ExpandableGroupModel, List<ExpandableChildModel>> childList;

        public ExpandableListAdapter(Activity contextactivity, Dictionary<ExpandableGroupModel, List<ExpandableChildModel>> childlist)
        {
            context = contextactivity;
            childList = childlist;
            headerList = childlist.Keys.Select(x => x.Name).ToList<string>();
        }

        public override int GroupCount
        {
            get
            {
                return headerList.Count;
            }
        }

        public override bool HasStableIds
        {
            get
            {
                return false;
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            var groupName = headerList[groupPosition];
            var groupItem = childList.FirstOrDefault(x => x.Key.Name == groupName);
            return groupItem.Value[childPosition];
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            var groupName = headerList[groupPosition];
            var groupItem = childList.FirstOrDefault(x => x.Key.Name == groupName);
            return groupItem.Value.Count;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return headerList[groupPosition];
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {

            var groupName = headerList[groupPosition];
            var groupItem = childList.FirstOrDefault(x => x.Key.Name == groupName);
            ImageView imageSource;
            CheckBox chekBox;

            convertView = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.list_view_header, null);
            var txtView = (TextView)convertView.FindViewById(Resource.Id.txtHeaderText);
            imageSource = (ImageView)convertView.FindViewById(Resource.Id.imagearrow);
            chekBox = (CheckBox)convertView.FindViewById(Resource.Id.cb_group);
            txtView.Text = groupItem.Key.Name;
            chekBox.Checked = groupItem.Key.IsCheked;
            chekBox.Tag = groupName;
            chekBox.Click += ChekBox_Click;

            if (isExpanded == true)
                imageSource.SetImageResource(Resource.Drawable.up_arrow_one);
            else
                imageSource.SetImageResource(Resource.Drawable.down_arrow_one);

            chekBox.Visibility = chekBox.Checked ? ViewStates.Visible : ViewStates.Gone;
            return convertView;
        }

        private void ChekBox_Click(object sender, EventArgs e)
        {
            var chB = (CheckBox)sender;
            var grItem = childList.FirstOrDefault(x => x.Key.Name == chB.Tag.ToString());
            if (chB.Checked)
            {
                chB.Checked = false;
            }
            else
            {
                chB.Checked = false;
                grItem.Key.IsCheked = false;
                grItem.Value.ForEach(x => x.IsCheked = false);
                this.NotifyDataSetChanged();
            }
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            CheckBox chekBox;
            var groupName = headerList[groupPosition];
            var groupItem = childList.FirstOrDefault(x => x.Key.Name == groupName);
            var childItem = groupItem.Value[childPosition];

            convertView = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.expand_list_chlid, null);
            var txtHeader = (TextView)convertView.FindViewById(Resource.Id.txtChildText);
            var mtxtorImage = (TextView)convertView.FindViewById(Resource.Id.textorimage);
            var mImageortxt = (ImageView)convertView.FindViewById(Resource.Id.imageortext);
            chekBox = (CheckBox)convertView.FindViewById(Resource.Id.cb_child);
            chekBox.Checked = childItem.IsCheked;
            chekBox.Tag = groupName;

            chekBox.Click += Child_ChekBox_Click;

            txtHeader.Text = groupItem.Value[childPosition].Name;
            //txtHeader.SetTextColor(Android.Graphics.Color.ParseColor("#ff4891"));

           
            return convertView;
        }

        private void Child_ChekBox_Click(object sender, EventArgs e)
        {
            var chB = (CheckBox)sender;
            var grItem = childList.FirstOrDefault(x => x.Key.Name == chB.Tag.ToString());

            var parentView = (View)chB.Parent;
            var parentChildName = ((TextView)parentView.FindViewById(Resource.Id.txtChildText)).Text;
            var chItm = childList[grItem.Key].FirstOrDefault(x => x.Name == parentChildName);
            chItm.IsCheked = chB.Checked;

            if (childList[grItem.Key].Exists(x => x.IsCheked == true))
                grItem.Key.IsCheked = true;
            else
                grItem.Key.IsCheked = false;

           

            this.NotifyDataSetChanged();
        }

        public override void OnGroupExpanded(int groupPosition)
        {
            base.OnGroupExpanded(groupPosition);
        }


        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}