using System;
using System.Collections.Generic;


namespace EfcToXamarinAndroid.MicrationsHelper
{
    
    public class DataItem
    {
        [Key]
        public int Id { get; set; }
        public long HashId { get; private set; }
        public OperacionTyps OperacionTyp { get; private set; }
        public float Balance { get; set; }
        public float Sum { get; set; }
        public int Karta { get; set; }
        public string Title { get; set; }
        public string Descripton { get; set; }
        public int MCC { get; set; }
        public DateTime Date { get; private set; }
        public CategoryTyps DefaultCategoryTyps { get; private set; }
        public CategoryTyps CastomCategoryTyps { get; private set; }
        public List<SybCategory> SubCategorys { get; private set; }

        public DataItem()
        {
          
        }


        public DataItem(OperacionTyps operacionTyp, DateTime dateTime )
        {
            Date = dateTime;
            OperacionTyp = operacionTyp;
            //////not overflov to 16/11/3169 09:46:40
            HashId = (long)((int)operacionTyp * 1000000000000000000) + dateTime.Ticks;
            


        }

        public override string ToString()
        {
            return $"{Sum} {Descripton} {Date} ";
        }


    }
}
