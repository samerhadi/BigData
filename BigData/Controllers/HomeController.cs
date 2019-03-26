using DataLogic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static DataLogic.Models.GoogleGeocoding;

namespace BigData.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public async Task<ActionResult> Contact()
        {
            ViewBag.Message = "Your contact page.";

            await GetCoordinatesAsync();
            return View();
        }

        public async Task<Location> GetCoordinatesAsync()
        {
            var client = new HttpClient();
            var location = new Location();

            string cityName = "Örebro";
            string streetName = "Skolgatan 42";
            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={cityName}+{streetName}&key=AIzaSyAxzPnxjGlRXDkjvVNamfloAAx1eMYqyBw";
            var response = await client.GetAsync(string.Format(url, cityName));
            string result = await response.Content.ReadAsStringAsync();
            RootObject root = JsonConvert.DeserializeObject<RootObject>(result);

            foreach(var item in root.results)
            {
                location.lat = item.geometry.location.lat;
                location.lng = item.geometry.location.lng;
            }

            return location;
        }
    }
}