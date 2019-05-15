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
using static DataLogic.Models.BookingSystemViewModel;

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
        //skapar ett bookingsystem

        public async Task<ActionResult> AddBookingSystemAsync(CreateBookingSystemModel createBookingSystemModel)
        {
            createBookingSystemModel.BookingSystem.ServiceType = Convert.ToInt32(createBookingSystemModel.ServiceType);

            await Task.Run(() => AddBookingSystem(createBookingSystemModel.BookingSystem));

            return RedirectToAction("GetAllBookingSystemsView");
        }

        [HttpPost]
        public async void AddBookingSystem(BookingSystemEntity bookingSystem)
        {

            bookingSystem = await GetCoordinatesAsync(bookingSystem);

            var url = "http://localhost:60295/api/addbookingsystem";

            var content = new StringContent(JsonConvert.SerializeObject(bookingSystem), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);
        }

        [HttpGet]
        public async Task<ActionResult> ChoosenBookingSystem(int id)
        {
            var bookingSystemInformationModel = new BookingSystemInformationModel();

            bookingSystemInformationModel.BookingSystem = await GetBookingSystem(id);
            bookingSystemInformationModel.ListOFArticles = await new ArticleController().GetArticlesFromBookingSystem(id);

            return View(bookingSystemInformationModel);
        }

        [HttpGet]
        public async Task<BookingSystemEntity> GetBookingSystem(int id)
        {
            var url = "http://localhost:60295/api/getbookingsystem/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var response = await client.GetAsync(string.Format(url, content));
            string result = await response.Content.ReadAsStringAsync();

            var bookingSystem = JsonConvert.DeserializeObject<BookingSystemEntity>(result);

            if (response.IsSuccessStatusCode)
            {
                return bookingSystem;
            }

            return bookingSystem;
        }

        //hämtar alla bookingsystem
        public async Task<ActionResult> GetAllBookingSystemsView(int? id)
        {
            if (id != null)
            {
                await Task.Run(() => DeleteSystem(id));
            }

            var listOfBookingSystems = await GetAllBookingSystems();


            return View(listOfBookingSystems);
        }

        [HttpGet]
        public async Task<List<BookingSystemEntity>> GetAllBookingSystems()
        {
            var url = "http://localhost:60295/api/getallbookingsystems/";
            var response = await client.GetAsync(string.Format(url));
            string result = await response.Content.ReadAsStringAsync();

            var listOfAllBookingSystem = JsonConvert.DeserializeObject<List<BookingSystemEntity>>(result);

            if (response.IsSuccessStatusCode)
            {
                return listOfAllBookingSystem;
            }

            return listOfAllBookingSystem;
        }

        //hämtar alla bookningsystem som ligger i samma stad
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

            return RedirectToAction("ChooseCity");
        }

        // GET: AllServices
        public ActionResult ChooseCity()
        {
            return View();
        }
#pragma warning disable 1998
        //sorterar system efter servicetype
        public async Task<List<BookingSystemEntity>> SortListByServiceType(List<BookingSystemEntity> listOfBookingSystems)
        {
            var sortedlist = listOfBookingSystems.OrderByDescending(x => (int)(x.ServiceType)).ToList();
            return sortedlist;
        }
        //hämtar kordinater för en adress
        public async Task<BookingSystemEntity> GetCoordinatesAsync(BookingSystemEntity system)
        {

            string cityName = system.City;
            string streetName = system.Adress;

            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={cityName}+{streetName}&key=AIzaSyAxzPnxjGlRXDkjvVNamfloAAx1eMYqyBw";
            var response = await client.GetAsync(string.Format(url));
            string result = await response.Content.ReadAsStringAsync();

            RootObject root = JsonConvert.DeserializeObject<RootObject>(result);

            foreach (var item in root.results)
            {
                system.Latitude = item.geometry.location.lat;
                system.Longitude = item.geometry.location.lng;
            }

            return system;
        }
        //tar bort ett system
        [HttpDelete]
        public async void DeleteSystem(int? id)
        {
            var url = "http://localhost:60295/api/deletebookingsystem/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var result = await client.DeleteAsync(string.Format(url, content));

        }

        [HttpGet]
        public async Task<int> GetBookingSystemServiceType(int id)
        {
            var url = "http://localhost:60295/api/getbookingsystemservicetype/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var response = await client.GetAsync(string.Format(url, content));
            string result = await response.Content.ReadAsStringAsync();

            var serviceType = JsonConvert.DeserializeObject<int>(result);

            if (response.IsSuccessStatusCode)
            {
                return serviceType;
            }

            return serviceType;
        }
    }
}