using DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLogic.Entities;
using DataLogic.Context;
using static DataLogic.Models.GoogleGeocodingModel;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace BigData.Controllers
{
    public class BookingSystemController : BaseController
    {

        // GET: CreateBookingSystem
        public ActionResult CreateBookingSystem()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateBookingSystem(BookingSystemEntity bookingSystem)
        {

            bookingSystem = await GetCoordinatesAsync(bookingSystem);

            var url = "http://localhost:60295/api/addbookingsystem";

            using (var client = new HttpClient())
            {

                var content = new StringContent(JsonConvert.SerializeObject(bookingSystem), Encoding.UTF8, "application/json");

                var result = await client.PostAsync(url, content);

                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("AllServices");

                }

                return View(bookingSystem);

            }

        }

        ////Skapar ett nytt bokningssystem
        //[HttpPost]
        //public async Task<ActionResult> CreateBookingSystem(BookingSystemEntity system)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            system = await GetCoordinatesAsync(system);
        //            db.BookingSystems.Add(system);
        //            db.SaveChanges();
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return View();
        //}

        // GET: AllServices
        public ActionResult AllServices()
        {

            return View();
        }

        // GET: ChoosenService
        public ActionResult ChoosenService(int id)
        {
            var bookingSystem = db.BookingSystems.Find(id);

            return View(bookingSystem);
        }

        public ActionResult ChooseCityOrebro()
        {
            var bookingSystems = db.BookingSystems.Where(model => model.City == "Örebro").ToList();
            var sortedList = bookingSystems.OrderByDescending(x => (int)(x.ServiceType)).ToList();
            return View(sortedList);

        }

        public ActionResult ChooseCityStockholm()
        {
            var bookingSystems = db.BookingSystems.Where(model => model.City == "Stockholm").ToList();
            var sortedList = bookingSystems.OrderByDescending(x => (int)(x.ServiceType)).ToList();
            return View(sortedList);
        }

        public async Task<BookingSystemEntity> GetCoordinatesAsync(BookingSystemEntity system)
        {
            var client = new HttpClient();
            var location = new Location();

            string cityName = system.City;
            string streetName = system.Adress;

            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={cityName}+{streetName}&key=AIzaSyAxzPnxjGlRXDkjvVNamfloAAx1eMYqyBw";
            var response = await client.GetAsync(string.Format(url, cityName));
            string result = await response.Content.ReadAsStringAsync();
            RootObject root = JsonConvert.DeserializeObject<RootObject>(result);

            foreach (var item in root.results)
            {
                system.Latitude = item.geometry.location.lat;
                system.Longitude = item.geometry.location.lng;
            }

            return system;
        }

    }
}