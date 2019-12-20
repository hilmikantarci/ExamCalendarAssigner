using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExamCalendarAssigner.Web.Models;
using ExamCalendarAssigner.Data.Model;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;


namespace ExamCalendarAssigner.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string json = System.IO.File.ReadAllText(@"C:\Users\Hilmi\Desktop\data.json");
            SınavTakvimModel model = JsonConvert.DeserializeObject<SınavTakvimModel>(json);
            return View();
        }
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public string Upload(IFormFile files)
        {

            var content = string.Empty;

            using (var reader = new StreamReader(files.OpenReadStream()))
            {
                content = reader.ReadToEnd();
            }

            const string Datapath = @"C:\Users\Hilmi\Desktop\aaBTU\ExamCalendarAssigner\src\ExamCalendarAssigner.Web\Models\Data.json";

            SınavTakvimModel model = JsonConvert.DeserializeObject<SınavTakvimModel>(content);
            System.IO.File.WriteAllText(Datapath, "");
            System.IO.File.WriteAllText(Datapath, content);
            string json = System.IO.File.ReadAllText(Datapath);
            SınavTakvimModel ModelData = JsonConvert.DeserializeObject<SınavTakvimModel>(json);



            DersModel dersModel = ModelData.dersler[0];
            string dersMode = ModelData.dersler[0].sinav.salonlar[0];

            return dersMode;

        }
        //public async Task<IActionResult> Upload(List<IFormFile> files)
        //{
        //    //files.SaveAs(Server.MapPath())

        //    //return View("index");
        //    foreach (var file in files)
        //    {
        //        if (file.ContentLength > 0)
        //        {
        //            var fileName = Path.GetFileName(file.FileName);
        //            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
        //            file.SaveAs(path);
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
