using System;
using System.Collections.Generic;
namespace ExamCalendarAssigner.Data.Model
{
    public class SınavTakvimModel
    {
        public DateTime baslangic { get; set; }
        public DateTime bitis { get; set; }
        public List<DateTime> tatil { get; set; }
        public List<string> harici_dersler { get; set; }
        public int gunluk_max_ayni_sinif_sinav_sayisi { get; set; }
        public List<DersModel> dersler { get; set; }
        public List<GozetmenModel> gozetmenler { get; set; }
        public List<SalonModel> salonlar { get; set; }
    }
}