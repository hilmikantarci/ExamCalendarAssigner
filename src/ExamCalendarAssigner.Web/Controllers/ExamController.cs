using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamCalendarAssigner.Data.Model;
using Microsoft.AspNetCore.Http;
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

        List<TakvimModel> takvimList = new List<TakvimModel>();
        public IActionResult Index()
        {
            string datapath = $"{_env.ContentRootPath}\\Models\\Data.json";
            string data = System.IO.File.ReadAllText(datapath);
            SınavTakvimModel modelData = JsonConvert.DeserializeObject<SınavTakvimModel>(data);

            int day = (modelData.bitis - modelData.baslangic).Days + 1;
            List<string> hariciDersler = new List<string>();

            foreach (var item in modelData.harici_dersler)
            {
                hariciDersler.Add(item);

            }
            // tarihList.Add(new TarihModel
            ////        {
            ////            Tarih = Convert.ToDateTime(ModelData.baslangic).AddDays(i).Date.ToString("dd/M/yy"),
            ////            Gun = Convert.ToDateTime(ModelData.baslangic).AddDays(i).DayOfWeek.ToString()
            ////        });
            ///
            foreach (var ders in modelData.dersler)
            {
                if (modelData.harici_dersler.Exists(t => t == ders.kod))
                    ders.Atandi = true;
            }

            for (int i = 0; i < day; i++)
            {
                DateTime sinavGunu = modelData.baslangic.AddDays(i);
                if (modelData.tatil.Exists(t => t.Year == sinavGunu.Year && t.Month == sinavGunu.Month && t.Day == sinavGunu.Day)) //Eğer tatil günü ise sıradaki güne geç
                    continue;

                var baslangicSaati = 9;
                var ogleSaati = 12;
                var bitisSaati = 16;

                DersModel oncekiDers = null;
                TakvimModel oncekiTakvim = null;

                while (true)
                {
                    var bugunDersleri = modelData.dersler.Where(t => t.program.FirstOrDefault().gun.ToLowerInvariant() == sinavGunu.DayOfWeek.ToString().ToLowerInvariant() && t.Atandi == false).ToList();
                    if (bugunDersleri.Count() == 0)
                        break;
                    Random random = new Random();
                    int rastgelesayi = random.Next(0, bugunDersleri.Count());
                    var ders = bugunDersleri[rastgelesayi];
                    TakvimModel takvim = new TakvimModel();
                    takvim.Tarih = sinavGunu;
                    if (oncekiDers == null)
                        takvim.Saat = new TimeSpan(baslangicSaati, 0, 0);
                    else
                    {
                        takvim.Saat = oncekiTakvim.Saat.Add(new TimeSpan(0, oncekiDers.sinav.sure, 0));
                        if (takvim.Saat.Hours == ogleSaati)
                        {
                            if (takvim.Tarih.DayOfWeek != DayOfWeek.Friday)
                                takvim.Saat = new TimeSpan(ogleSaati + 1, 0, 0);
                            else
                                takvim.Saat = new TimeSpan(ogleSaati + 1, 30, 0);
                        }
                        else if ((takvim.Saat.Hours == 11) && (takvim.Saat.Minutes > 0))
                        {
                            if (takvim.Tarih.DayOfWeek != DayOfWeek.Friday)
                                takvim.Saat = new TimeSpan(ogleSaati + 1, 30, 0);
                            else
                                takvim.Saat = new TimeSpan(ogleSaati + 2, 0, 0);
                        }
                        else if (takvim.Saat.Minutes == 0)
                            takvim.Saat = new TimeSpan(takvim.Saat.Hours, 0, 0);
                        else if (takvim.Saat.Minutes <= 30)
                            takvim.Saat = new TimeSpan(takvim.Saat.Hours, 30, 0);
                        else
                            takvim.Saat = new TimeSpan(takvim.Saat.Hours + 1, 0, 0);
                    }
                    takvim.dersKodu = ders.kod;
                    takvim.dersAdi = ders.baslik;
                    takvim.Salonlar = ders.sinav.salonlar.ToList();
                    takvim.gozetmen = "default";
                    takvimList.Add(takvim);
                    ders.Atandi = true;
                    oncekiDers = ders;
                    oncekiTakvim = takvim;
                  
                    if (takvim.Saat.Hours >= bitisSaati)
                        break;
                    
                }
            }
            var atanmayanDersler = modelData.dersler.Where(t => t.Atandi == false);
            foreach (var item in atanmayanDersler)
            {
                var baslangicSaati = 9;
                DersModel oncekiDers = null;
                TakvimModel oncekiTakvim = null;
                TakvimModel takvim = new TakvimModel();
                takvim.Tarih = modelData.bitis.AddDays(1);
                if (oncekiDers == null)
                    takvim.Saat = new TimeSpan(baslangicSaati, 0, 0);
                else
                {
                    takvim.Saat = oncekiTakvim.Saat.Add(new TimeSpan(0, oncekiDers.sinav.sure, 0));

                }
                takvim.dersKodu = item.kod;
                takvim.dersAdi = item.baslik;
                takvim.Salonlar = item.sinav.salonlar;
                takvim.gozetmen = "default";
                takvimList.Add(takvim);
                item.Atandi = true;
                oncekiDers = item;
                oncekiTakvim = takvim;

            }

            return View(takvimList);
        //    string json = JsonConvert.SerializeObject(takvimList.ToString());

        //    string jsonString = JsonSerializer.Serialize(, takvimList.ToString());
        //    File.WriteAllText(fileName, jsonString);
        //    string content = takvimList.ToString();
        //    string json = System.IO.File.ReadAllText(@"C:\Users\Hilmi\Desktop\modelFirst.json");
        //    SınavTakvimModel model = JsonConvert.DeserializeObject<SınavTakvimModel>(json);


        //    SınavTakvimModel modela = JsonConvert.DeserializeObject<SınavTakvimModel>(content);
        //    System.IO.File.WriteAllText(json, "");
        //    System.IO.File.WriteAllText(json, takvimList.ToString());
        //    string jsona = System.IO.File.ReadAllText(json);
        //}
        //TakvimModel takvim = new TakvimModel();
        //takvim.Tarih = sinavGunu;
        //    takvim.Saat = new TimeSpan(9, 0, 0);

        //takvimList.Add(takvim);

        }

    }

    ///taanamayanları kontorl et


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
