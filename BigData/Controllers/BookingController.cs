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
    public class BookingController : BaseController
    {
        // GET: Booking
        public ActionResult BookTime()
        {
            return View();
        }
    }
}