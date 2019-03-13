using DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLogic.Entities;
using DataLogic.Context;

namespace BigData.Controllers
{
    public class BookingSystemController : BaseController
    {

        // GET: CreateBookingSystem
        public ActionResult CreateBookingSystem()
        {
            return View();
        }

        //Skapar ett nytt bokningssystem
        [HttpPost]
        public ActionResult CreateBookingSystem(BookingSystemEntity system)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.BookingSystems.Add(system);
                    db.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }

        // GET: AllServices
        public ActionResult AllServices()
        {
            var listOfAllServices = db.BookingSystems.ToList();

            return View(listOfAllServices);
        }

        // GET: ChoosenService
        public ActionResult ChoosenService(int id)
        {
            var bookingSystem = db.BookingSystems.Find(id);
            return View(bookingSystem);
        }

    }

}