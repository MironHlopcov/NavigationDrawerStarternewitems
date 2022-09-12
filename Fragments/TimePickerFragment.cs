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

namespace MyFinToControl.Fragments
{
    public class TimePickerFragment : AndroidX.Fragment.App.DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        public static readonly string TAG = "My:" + typeof(TimePickerFragment).Name.ToUpper();

        Action<DateTime> _dateTimeSelectedHandler = delegate { };

        public TimePickerFragment(Action<DateTime> onDateSelected)
        {
            _dateTimeSelectedHandler = onDateSelected;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime now = DateTime.Now;
            return new TimePickerDialog(Activity,
                this,
               now.Hour,
               now.Minute,
               true);
        }

        void TimePickerDialog.IOnTimeSetListener.OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            DateTime now = DateTime.Now;
            DateTime selectedTime = new DateTime(now.Year, now.Month, now.Day, hourOfDay, minute, now.Second);
            _dateTimeSelectedHandler(selectedTime);
        }
    }
}