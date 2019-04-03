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
        public static HttpClient client = new HttpClient();
        
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

            var content = new StringContent(JsonConvert.SerializeObject(bookingSystem), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);

            if (result.IsSuccessStatusCode)
            {

                return RedirectToAction("AllServices");

            }

            return View(bookingSystem);
        }

        [HttpGet]
        public async Task<ActionResult> ChoosenService(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var url = "http://localhost:60295/api/getbookingsystem/" + id;

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

            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("AllServices");
        }

        public async Task<ActionResult> GetAllBookingSystems(int? id)
        {
            if (id != null)
            {
                await Task.Run(() => DeleteSystem(id));
            }

            var listOfBookingSystems = await GetBookingSystems();

            return View(listOfBookingSystems);
        }

        [HttpGet]
        public async Task<List<BookingSystemEntity>> GetBookingSystems()
        {
            if (ModelState.IsValid)
            {
                var url = "http://localhost:60295/api/getallbookingsystems/";
                var response = await client.GetAsync(string.Format(url));
                string result = await response.Content.ReadAsStringAsync();

                var listOfAllBookingSystem = JsonConvert.DeserializeObject<List<BookingSystemEntity>>(result);

                if (response.IsSuccessStatusCode)
                {
                    return listOfAllBookingSystem;
                }

            }
            var fail = new List<BookingSystemEntity>();
            return fail;
        }

        [HttpGet]
        public async Task<ActionResult> ChoosenCity(string city)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var url = "http://localhost:60295/api/getbookingsystemsfromcity/" + city;

                    var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
                    var response = await client.GetAsync(string.Format(url, content));
                    string result = await response.Content.ReadAsStringAsync();

                    var bookingSystems = JsonConvert.DeserializeObject<List<BookingSystemEntity>>(result);
                    var sortedList = await SortListByServiceType(bookingSystems);

                    if (response.IsSuccessStatusCode)
                    {
                        return View(sortedList);
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
#pragma warning disable 1998
        public async Task<List<BookingSystemEntity>> SortListByServiceType(List<BookingSystemEntity> listOfBookingSystems)
        {
            listOfBookingSystems.OrderByDescending(x => (int)(x.ServiceType)).ToList();
            return listOfBookingSystems;
        }

        public async Task<BookingSystemEntity> GetCoordinatesAsync(BookingSystemEntity system)
        {

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

        [HttpDelete]
        public async void DeleteSystem(int? id)
        {
            var url = "http://localhost:60295/api/deletebookingsystem/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var result = await client.DeleteAsync(string.Format(url, content));

        }
    }
}