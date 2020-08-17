using System;
namespace DekorStone_API.Models.Structs
{
    public class DailySumStruct
    {
        public long ID { get; set; }
        public DateTime cDate {get;set;}
        public double dayStartMoney { get; set; }
        public double dayEndMoney { get; set; }
        public double income { get; set; }
        public double outcome { get; set; }
    }
}
