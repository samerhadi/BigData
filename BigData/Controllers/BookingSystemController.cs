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
using DataLogic.Repository;

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

        [HttpGet]
        public async Task<ActionResult> ChoosenService(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var url = "http://localhost:60295/api/getbookingsystem/" + id;

                    using (var client = new HttpClient())
                    {

                        var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                        var response = await client.GetAsync(string.Format(url, content));
                        string result = await response.Content.ReadAsStringAsync();

                        var bookingSystem = JsonConvert.DeserializeObject<BookingSystemEntity>(result);

                        if (response.IsSuccessStatusCode)
                        {
                            return View(bookingSystem);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("AllServices");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBookingSystems()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var url = "http://localhost:60295/api/getallbookingsystems/";

                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(string.Format(url));
                        string result = await response.Content.ReadAsStringAsync();

                        var listOfAllBookingSystem = JsonConvert.DeserializeObject<List<BookingSystemEntity>>(result);

                        if (response.IsSuccessStatusCode)
                        {
                            return View(listOfAllBookingSystem);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("AllServices");
        }

        [HttpGet]
        public async Task<ActionResult> ChoosenCity(string city)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var url = "http://localhost:60295/api/getbookingsystemsfromcity/" + city;

                    using (var client = new HttpClient())
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
                        var response = await client.GetAsync(string.Format(url, content));
                        string result = await response.Content.ReadAsStringAsync();

                        var bookingSystems = JsonConvert.DeserializeObject<List<BookingSystemEntity>>(result);
                        var sortedList = SortListByServiceType(bookingSystems);

                        if (response.IsSuccessStatusCode)
                        {
                            return View(sortedList);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("AllServices");
        }

        // GET: AllServices
        public ActionResult AllServices()
        {
            return View();
        }

        public List<BookingSystemEntity> SortListByServiceType(List<BookingSystemEntity> listOfBookingSystems)
        {
            listOfBookingSystems.OrderByDescending(x => (int)(x.ServiceType)).ToList();
            return listOfBookingSystems;
        }

        public async Task<BookingSystemEntity> GetCoordinatesAsync(BookingSystemEntity system)
        {
            var client = new HttpClient();

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