using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFinToControl.Models
{
    public class ExpandableChildModel : Java.Lang.Object
    {
        public string Name { get; set; }
        public bool IsCheked { get; set; }

        public ExpandableChildModel()
        {
        }
        public ExpandableChildModel(string name)
        {
            Name = name;
            IsCheked = false;
        }
       
    }
}