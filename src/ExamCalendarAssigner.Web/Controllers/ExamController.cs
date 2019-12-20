using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamCalendarAssigner.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExamCalendarAssigner.Web.Controllers
{
    public class ExamController : Controller
    {
        public string a = "";
        int i = 0;
        public string Index()
        {
            string data = System.IO.File.ReadAllText(@"C:\Users\Hilmi\Desktop\aaBTU\ExamCalendarAssigner\src\ExamCalendarAssigner.Web\Models\Data.json");
            SınavTakvimModel ModelData = JsonConvert.DeserializeObject<SınavTakvimModel>(data);
            string tarih = System.IO.File.ReadAllText(@"C:\Users\Hilmi\Desktop\aaBTU\ExamCalendarAssigner\src\ExamCalendarAssigner.Web\Models\Tarih.json");
            TarihModel ModelTarih = JsonConvert.DeserializeObject<TarihModel>(tarih);
            //a=Convert.ToDateTime(ModelData.baslangic).DayOfWeek.ToString();
            DateTime baslangic = Convert.ToDateTime(ModelData.baslangic);
            List<TarihModel> tarihler = new List<TarihModel>();
            List<DersModel> dersler = new List<DersModel>();
            for (i = 0; i < 12; i++)
            {

                if (baslangic.AddDays(i).DayOfWeek.ToString() != "Saturday" && baslangic.AddDays(i).DayOfWeek.ToString() != "Sunday")
                {
                    tarihler.Add(new TarihModel
                    {
                        Tarih = Convert.ToDateTime(ModelData.baslangic).AddDays(i).Date.ToString("dd/M/yy"),
                        Gun = Convert.ToDateTime(ModelData.baslangic).AddDays(i).DayOfWeek.ToString()
                    });
                }
            }
            foreach (var item in ModelData.dersler)
            {

                dersler.Add(new DersModel { 
                kod= item.kod,
                program=item.program[0].gun.ToString()
                });

                try
                {
                    a = a + "\n" + item.program[0].gun.ToString() + "\t \t" + item.kod + "\t" + item.sinav.sure;

                    //string dersMode = ModelData.dersler[0].sinav.salonlar[0];
                }
                catch (Exception)
                {
                    continue;
                }
            }
             // a = ModelTarih.Tarih[0].ToString() + ModelTarih.Gun[0].ToString();
            //ModelData.dersler[0].kod.ToString() + ModelData.salonlar[0].kod.ToString() + ModelData.gozetmenler[0].isim.ToString();


            // a = newDate.ToShortDateString();

            if(ModelData.dersler[0].program[0].gun.to=tarihler[0].Gun.ToString())
            {
                foreach (var item in tarihler)
            {

                a = (a + item.Tarih + "\t" + item.Gun) + "\n";

            }
            }

            return a;
        }





        //public string a = "";

        //public string Index()
        //{       

        //    var gun = DateTime.Today;
        //    var ilk = gun.AddDays(1 - (int)gun.DayOfWeek);
        //    var son = dateTime.AddDays(5 - (int)gun.DayOfWeek);
        //    var son2 = gun.AddDays(5 - (int)gun.DayOfWeek);

        //    string json = System.IO.File.ReadAllText(@"C:\Users\Hilmi\Desktop\aaBTU\ExamCalendarAssigner\src\ExamCalendarAssigner.Web\Models\Data.json");
        //    SınavTakvimModel ModelData = JsonConvert.DeserializeObject<SınavTakvimModel>(json);
        //    DateTime dateTime = Convert.ToDateTime(ModelData.baslangic);

        //    foreach (var item in ModelData.dersler)
        //    {

        //        try
        //        {
        //            a = a + "\n" + item.program[0].gun.ToString() + "\t \t" + item.kod + "\t" + item.sinav.sure;

        //            //string dersMode = ModelData.dersler[0].sinav.salonlar[0];
        //        }
        //        catch (Exception)
        //        {
        //            continue;
        //        }
        //    }

        //    return a;
        //}
    }
}