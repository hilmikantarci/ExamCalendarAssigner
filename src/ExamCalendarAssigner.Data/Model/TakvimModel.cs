using System;
using System.Collections.Generic;
using System.Text;


namespace ExamCalendarAssigner.Data.Model
{
    public class TakvimModel
    {         
        public DateTime Tarih { get; set; }
        public TimeSpan Saat { get; set; }
        public string dersKodu { get; set; }
        public string dersAdi { get; set; }
        public string Salon { get; set; }
        public string gozetmen { get; set; }
    }
}
