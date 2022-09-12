using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace EfcToXamarinAndroid.Core
{
    public class DataItem : IEquatable<DataItem>
    {
        [Key]
        public int Id { get; set; }
        public long HashId { get; private set; }
        public OperacionTyps OperacionTyp { get; private set; }
        public float Balance { get; set; }
        public float Sum { get; set; }
        public float OldSum { get; set; }
        public int Karta { get; set; }
        public string? Title { get; set; }
        public string? Descripton { get; set; }
        public int MCC { get; set; }
        public string? MccDeskription { get; set; }
        public DateTime Date { get; set; }
        public bool IsParent { get; private set; }
        public int ParentId { get; set; }
        [XmlIgnore]
        public CategoryTyps DefaultCategoryTyps { get; private set; }
        [XmlIgnore]
        public CategoryTyps CastomCategoryTyps { get; private set; }
        [XmlIgnore]
        public List<SybCategory>? SubCategorys { get; private set; }
        public string? UnreachableText { get; private set; }
        public string? UnreachableOperacionTyp { get; set; }
        public string? SmsAdress { get; set; }
        [NotMapped]
        public bool IsNewDataItem { get; set; }



        public DataItem(OperacionTyps operacionTyp, DateTime dateTime)
        {
            Date = dateTime;
            OperacionTyp = operacionTyp;
            HashId = dateTime.AddMilliseconds(-(dateTime.Second * 1000)).Ticks;
            Descripton = OperacionTyp.ToString();
        }

        public DataItem()
        {

        }
        public void SetNewValues(DataItem dataItem)
        {
            OperacionTyp = dataItem.OperacionTyp;
            //Balance = dataItem.Balance;
            if (!IsParent) //если запись не редактировалась
                if (ParentId == 0) // и если у записи нет предков 
                    OldSum = Sum;
            Sum = dataItem.Sum;
            Karta = dataItem.Karta;
            Title = dataItem.Title;
            Descripton = dataItem.Descripton;
            MCC = dataItem.MCC;
            if (ParentId != 0) //если у записи есть предки, значит она пользовательская и можно изменять ее дату
            {
                Date = dataItem.Date;
                HashId = Date.AddMilliseconds(-(Date.Second * 1000)).Ticks;
            }
            //DefaultCategoryTyps = dataItem.DefaultCategoryTyps;
            //CastomCategoryTyps = dataItem.CastomCategoryTyps;
            //SubCategorys = dataItem.SubCategorys;
            IsParent = true; //помечаем запись как редактированную
        }
        public void SetOperTyp(OperacionTyps type)
        {
            if(OperacionTyp==OperacionTyps.UNREACHABLE)
                OperacionTyp = type;
        }
        public override string ToString()
        {
            return $"{Sum} {Descripton} {Date} ";
        }
        public override bool Equals(object obj) => this.Equals(obj as DataItem);
        public bool Equals(DataItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            if (this.OperacionTyp != other.OperacionTyp)
                return false;
            if (this.Date != other.Date)
                return false;
            if (this.Sum != other.Sum)
                return false;
            if (this.MccDeskription != other.MccDeskription)
                return false;
            if (this.Karta != other.Karta)
                return false;
            if (this.Title != other.Title)
                return false;
            if (this.Descripton != other.Descripton)
                return false;
            if (this.MCC != other.MCC)
                return false;




            return true;

        }





    }
}
