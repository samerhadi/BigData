using BigData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigData.Controllers
{
    public class BookingSystemController : BaseController
    {
        // GET: BookingSystem
        public ActionResult BookingSystems()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BookingSystems(BookingSystem system)
        {
            try
            {
                BookingSystem booking = new BookingSystem();
                if (ModelState.IsValid)
                {
                    booking.Adress = system.Adress;
                    booking.SystemName = system.SystemName;
                    booking.Website = system.Website;
                    booking.SystemDescription = system.SystemDescription;
                    booking.PostaICode = system.PostaICode;
                    booking.PhoneNumber = system.PhoneNumber;
                    booking.LatitudeAndLongitude = system.LatitudeAndLongitude;
                    booking.Email = system.Email;
                    booking.City = system.City;

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

    }
}