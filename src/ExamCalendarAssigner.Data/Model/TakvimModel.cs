using System;
using System.Collections.Generic;
using System.Text;


namespace ExamCalendarAssigner.Data.Model
{
    public class TakvimModel
    {         
        public List<TarihModel> Tarih { get; set; }
        public string Saat { get; set; }
        public string dersKodu { get; set; }
        public string dersAdi { get; set; }
        public string Salon { get; set; }
        public string gozetmen { get; set; }
    }
}
