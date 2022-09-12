using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyFinToControl.Parsers
{
    public static class ParseStringToFloat
    {
        public static float GetFloat(string numberString)
        {
            // numberString = "1,234.56"; // en
            // var numberString = "1.234,56"; // de
            var cultureInfo = CultureInfo.InvariantCulture;
            // if the first regex matches, the number string is in us culture
            if (Regex.IsMatch(numberString, @"^(:?[\d,]+\.)*\d+$"))
            {
                cultureInfo = new CultureInfo("en-US");
            }
            // if the second regex matches, the number string is in de culture
            else if (Regex.IsMatch(numberString, @"^(:?[\d.]+,)*\d+$"))
            {
                cultureInfo = new CultureInfo("ru-RU");
            }
            float number;
            NumberStyles styles = NumberStyles.Float;
            bool isDouble = float.TryParse(numberString, styles, cultureInfo, out number);
            if (isDouble)
                return number;
            else
                return 0;
        }
    }
}