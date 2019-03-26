using DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLogic.Entities;
using DataLogic.Context;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using BigData.APIController;
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

        // GET: ChoosenService
        public async Task<ActionResult> ChoosenService(int id)
        {
            var url = "http://localhost:60295/api/findbookingsystem";

            using (var client = new HttpClient())
            {

                var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

                var result = await client.PostAsync(url, content);

                if (result.IsSuccessStatusCode)
                {
                    var bookingSystem = db.BookingSystems.Find(id);
                    return View(bookingSystem);

                }

                return RedirectToAction("AllServices");

            }
        }

        // GET: AllServices
        public ActionResult AllServices()
        {
            var listOfAllServices = db.BookingSystems.ToList();
            return View(listOfAllServices);
        }

        //// GET: ChoosenService
        //public ActionResult ChoosenService(int id)
        //{
        //    var bookingSystem = new BookingSystemRepo().GetBookingSystem(id);
        //    return View(bookingSystem);
        //}
    }

}