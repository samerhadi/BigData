﻿using BigData.Models;
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
        public ActionResult CreateBookingSystem()
        {
            return View();
        }

        //Skapar ett nytt bokningssystem
        [HttpPost]
        public ActionResult CreateBookingSystem(BookingSystem system)
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

    }
}