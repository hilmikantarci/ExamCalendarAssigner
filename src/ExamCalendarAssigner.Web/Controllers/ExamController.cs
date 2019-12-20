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
        public IActionResult Index()
        {
            string data = System.IO.File.ReadAllText(@"C:\Users\Hilmi\Desktop\aaBTU\ExamCalendarAssigner\src\ExamCalendarAssigner.Web\Models\Data.json");
            SınavTakvimModel ModelData = JsonConvert.DeserializeObject<SınavTakvimModel>(data);
            string tarih = System.IO.File.ReadAllText(@"C:\Users\Hilmi\Desktop\aaBTU\ExamCalendarAssigner\src\ExamCalendarAssigner.Web\Models\Tarih.json");
            TarihModel ModelTarih = JsonConvert.DeserializeObject<TarihModel>(tarih);
            //a=Convert.ToDateTime(ModelData.baslangic).DayOfWeek.ToString();
            DateTime baslangic = Convert.ToDateTime(ModelData.baslangic);
            List<TarihModel> tarihList = new List<TarihModel>();
            List<TakvimModel> takvimList = new List<TakvimModel>();            
            string[] saatler = { "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00" };

            for (int i = 0; i < 12; i++)
            {

                if (baslangic.AddDays(i).DayOfWeek.ToString() != "Saturday" && baslangic.AddDays(i).DayOfWeek.ToString() != "Sunday")
                {
                    tarihList.Add(new TarihModel
                    {
                        Tarih = Convert.ToDateTime(ModelData.baslangic).AddDays(i).Date.ToString("dd/M/yy"),
                        Gun = Convert.ToDateTime(ModelData.baslangic).AddDays(i).DayOfWeek.ToString()
                    });
                }
            }

            List<DersModel> dersler = new List<DersModel>();
            List<ProgramModel> program1 = new List<ProgramModel>();
            string[] gunler = new string[ModelData.dersler.Count];
            for (int i = 0; i < ModelData.dersler.Count; i++)
            {
                gunler[i] = ModelData.dersler[i].program[0].gun;
            }

            for (int i = 0; i < ModelData.dersler.Count; i++)
            {
                Random rastgelesayi = new Random();
                int no = rastgelesayi.Next(0, saatler.Length);
                int no2 = rastgelesayi.Next(0, 1);
                int no3 = rastgelesayi.Next(0, ModelData.gozetmenler.Count);
                foreach (var item in tarihList)
                {
                    if (gunler[i] == item.Gun)
                    {
                        TakvimModel takvim = new TakvimModel
                        {
                            Tarih = new List<TarihModel>
                            {
                                new TarihModel
                                {
                                    Tarih = item.Tarih,
                                    Gun = item.Gun
                                }
                            },
                            Saat = saatler[no],
                            dersKodu = ModelData.dersler[i].kod,
                            dersAdi = ModelData.dersler[i].baslik,
                            Salon = "B204",
                            gozetmen = "deneme"
                        };
                        takvimList.Add(takvim);
                        break;
                    }
                }
            }
            return Ok(JsonConvert.SerializeObject(tarihList));
        }
    }
}