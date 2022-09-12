using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EfcToXamarinAndroid.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MyFinToControl.Parsers
{
    public class SerializarionToXml
    {

        XmlSerializer xmlFormat = new XmlSerializer(typeof(DataItem[]));
        public string SaveToFile(string filename)
        {

            // сохранение массива в файл


            //using (var stringwriter = new System.IO.StringWriter())
            //{
            //    var serializer = new XmlSerializer(typeof(DataItem));
            //    var ggg = DatesRepositorio.DataItems.ToArray();
            //    serializer.Serialize(stringwriter, ggg[2]);
            //    var sdf  =  stringwriter.ToString();
            //}
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(DataItem[]));
                    xmlFormat.Serialize(fs, DatesRepositorio.DataItems.ToArray());
                }
                return filename;
            }
            catch
            {
                return null;
            }


        }
        public DataItem[] DeserializeFile(string filename)
        {

            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                DataItem[]? dataItems = xmlFormat.Deserialize(fs) as DataItem[];
                return dataItems;
            }
        }
    }
}

