using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamCalendarAssigner.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExamCalendarAssigner.Web.Controllers
{
    public class ExamController : Controller
    {
        private readonly ILogger<ExamController> _logger;
        private readonly IHostEnvironment _env;

        public ExamController(ILogger<ExamController> logger,
            IHostEnvironment env
            )
        {
            _logger = logger;
            _env = env;
        }


        public List<TakvimModel> Index()
        {
            string datapath = $"{_env.ContentRootPath}\\Models\\Data.json";
            string data = System.IO.File.ReadAllText(datapath);
            SınavTakvimModel modelData = JsonConvert.DeserializeObject<SınavTakvimModel>(data);
            List<TakvimModel> takvimList = new List<TakvimModel>();
            int day = (modelData.bitis - modelData.baslangic).Days + 1;
            for (int i = 0; i < day; i++)
            {
                DateTime sinavGunu = modelData.baslangic.AddDays(i);
                if (modelData.tatil.Exists(t => t.Year == sinavGunu.Year && t.Month == sinavGunu.Month && t.Day == sinavGunu.Day)) //Eğer tatil günü ise sıradaki güne geç
                    continue;
                var bugunDersleri = modelData.dersler.Where(t => t.program.FirstOrDefault().gun.ToLowerInvariant() == sinavGunu.DayOfWeek.ToString().ToLowerInvariant() && t.Atandi == false);
                foreach (var item in bugunDersleri)
                {
                    TakvimModel takvim = new TakvimModel();
                    takvim.Tarih = sinavGunu;
                    takvim.Saat = new TimeSpan(9,0,0);
                    //takvim.Saat = new DateTime().;
                    takvim.dersKodu = item.kod;
                    takvim.dersAdi = item.baslik;
                    takvim.Salon = item.sinav.salonlar.FirstOrDefault();
                    takvim.gozetmen = "default";
                    item.Atandi = true;
                    takvimList.Add(takvim);
                }

                //}
                //TakvimModel takvim = new TakvimModel();
                //takvim.Tarih = sinavGunu;
                //takvim.Saat = new TimeSpan(9, 0, 0);

                //takvimList.Add(takvim);

            }


            ///taanamayanları kontorl et
         //   var atanmayanDersler = modelData.dersler.Where(t => t.Atandi ==false);


            return takvimList;

            //  string tarih = System.IO.File.ReadAllText(@"C:\Users\Hilmi\Desktop\aaBTU\ExamCalendarAssigner\src\ExamCalendarAssigner.Web\Models\Tarih.json");
            //   TarihModel ModelTarih = JsonConvert.DeserializeObject<TarihModel>(tarih);
            //a=Convert.ToDateTime(ModelData.baslangic).DayOfWeek.ToString();
            //DateTime baslangic = Convert.ToDateTime(ModelData.baslangic);
            //List<TarihModel> tarihList = new List<TarihModel>();
            //List<TarihModel> tarihList2 = new List<TarihModel>();
            //List<TakvimModel> takvimList = new List<TakvimModel>();

            //string[] saatler = { "09:00", "09:30", "10:00", "10:30", "11:00" , "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00" };

            ////for (int i = 0; i < 12; i++)
            ////{

            ////    if (baslangic.AddDays(i).DayOfWeek.ToString() != "Saturday" && baslangic.AddDays(i).DayOfWeek.ToString() != "Sunday")
            ////    {
            ////        tarihList.Add(new TarihModel
            ////        {
            ////            Tarih = Convert.ToDateTime(ModelData.baslangic).AddDays(i).Date.ToString("dd/M/yy"),
            ////            Gun = Convert.ToDateTime(ModelData.baslangic).AddDays(i).DayOfWeek.ToString()
            ////        });
            ////    }
            ////}
            //for (int i = 0; i < 6; i++)
            //{

            //    if (baslangic.AddDays(i).DayOfWeek.ToString() != "Saturday" && baslangic.AddDays(i).DayOfWeek.ToString() != "Sunday")
            //    {
            //        tarihList.Add(new TarihModel
            //        {
            //            Tarih = Convert.ToDateTime(ModelData.baslangic).AddDays(i).Date.ToString("dd/M/yy"),
            //            Gun = Convert.ToDateTime(ModelData.baslangic).AddDays(i).DayOfWeek.ToString()
            //        });
            //    }
            //}
            //for (int i = 6; i < 12; i++)
            //{

            //    if (baslangic.AddDays(i).DayOfWeek.ToString() != "Saturday" && baslangic.AddDays(i).DayOfWeek.ToString() != "Sunday")
            //    {
            //        tarihList2.Add(new TarihModel
            //        {
            //            Tarih = Convert.ToDateTime(ModelData.baslangic).AddDays(i).Date.ToString("dd/M/yy"),
            //            Gun = Convert.ToDateTime(ModelData.baslangic).AddDays(i).DayOfWeek.ToString()
            //        });
            //    }
            //}

            //List<DersModel> dersler = new List<DersModel>();
            //List<ProgramModel> program1 = new List<ProgramModel>();
            //string[] gunler = new string[ModelData.dersler.Count];
            //for (int i = 0; i < ModelData.dersler.Count; i++)
            //{
            //    gunler[i] = ModelData.dersler[i].program[0].gun;//sırasıyla ders günlerini attık
            //}

            //for (int i = 0; i < ModelData.dersler.Count; i++)
            //{
            //    Random rastgelesayi = new Random();
            //    int no = rastgelesayi.Next(0, saatler.Length);
            //    int no2 = rastgelesayi.Next(0, 2);
            //    int no3 = rastgelesayi.Next(0, ModelData.gozetmenler.Count);
            //    int no4 = rastgelesayi.Next(0, ModelData.gozetmenler.Count);
            //    if (no2 == 0)
            //    {
            //        foreach (var item in tarihList)
            //        {

            //            if (gunler[i] == item.Gun)
            //            {

            //                TakvimModel takvim = new TakvimModel
            //                {

            //                    Tarih = new List<TarihModel>
            //                {
            //                    new TarihModel
            //                    {
            //                       Tarih=item.Tarih,

            //                        Gun = item.Gun
            //                    }
            //                },
            //                    Saat = saatler[no],
            //                    dersKodu = ModelData.dersler[i].kod,
            //                    dersAdi = ModelData.dersler[i].baslik,
            //                    Salon = ModelData.dersler[i].sinav.salonlar[0],
            //                    gozetmen = ModelData.gozetmenler[no3].isim
            //                };
            //                for (int j = 0; j < takvimList.Count; j++)
            //                {
            //                    if (takvimList[j].Tarih[0].Tarih == takvim.Tarih[0].Tarih)
            //                    {
            //                        if (takvim.Saat == takvimList[j].Saat)
            //                        {
            //                            no = rastgelesayi.Next(0, saatler.Length);
            //                            takvim.Saat = saatler[no];
            //                            break;
            //                        }

            //                    }
            //                }
            //                takvimList.Add(takvim);
            //                break;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        foreach (var item in tarihList2)
            //        {

            //            if (gunler[i] == item.Gun)
            //            {

            //                TakvimModel takvim = new TakvimModel
            //                {

            //                    Tarih = new List<TarihModel>
            //                {
            //                    new TarihModel
            //                    {
            //                       Tarih=item.Tarih,

            //                        Gun = item.Gun
            //                    }
            //                },
            //                    Saat = saatler[no],
            //                    dersKodu = ModelData.dersler[i].kod,
            //                    dersAdi = ModelData.dersler[i].baslik,
            //                    Salon = ModelData.dersler[i].sinav.salonlar[0],
            //                    gozetmen = ModelData.gozetmenler[no3].isim
            //                };
            //                for (int j = 0; j < takvimList.Count; j++)
            //                {
            //                    if (takvimList[j].Tarih[0].Tarih == takvim.Tarih[0].Tarih)
            //                    {
            //                        if (takvim.Saat == takvimList[j].Saat)
            //                        {
            //                            no = rastgelesayi.Next(0, saatler.Length);
            //                            takvim.Saat = saatler[no];
            //                            j = 0;
            //                        }

            //                    }
            //                }
            //                takvimList.Add(takvim);
            //                break;
            //            }
            //        }
            //    }
            //}
            //return takvimList;



            //return Ok(JsonConvert.SerializeObject(tarihList));
        }
    }
}