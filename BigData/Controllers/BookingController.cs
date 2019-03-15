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
        // GET: BookTime första gången den besöks
        public ActionResult BookTime(int id)
        {
            var findTimeModel = new FindTimeModel();
            findTimeModel.BookingSystem = db.BookingSystems.Find(id);
            findTimeModel.DateChoosen = false;
            return View(findTimeModel);
        }

        //GET: BookTime med en vald dag
        [HttpPost]
        public ActionResult BookTime(FindTimeModel findTimeModel)
        {
            findTimeModel.BookingSystem = db.BookingSystems.Find(findTimeModel.BookingSystem.BookningSystemId);
            findTimeModel.DateChoosen = true;
            findTimeModel.ListOfTimes = CreateListOfTimes();
            return View(findTimeModel);
        }

        //Returnerar en lista med alla tider
        public List<string> CreateListOfTimes()
        {
            var listOfTimes = new List<string>();
            int startTid = 8;
            string skapaTid;
            for (int i = 0; i <= 8; i++)
            {
                skapaTid = Convert.ToString(startTid) + ":00-" + Convert.ToString(startTid + 1) + ":00";
                listOfTimes.Add(skapaTid);
                startTid++;
            }
            return listOfTimes;
        }

        //GET: TimeBooked
        public ActionResult TimeBooked(FindTimeModel findTimeModel, string time, int id)
        {
            findTimeModel.BookingSystem = db.BookingSystems.Find(id);
            findTimeModel.ChoosenTime = time;
            SaveBookedTime(findTimeModel);
            return View();
        }

        //Sparar en vald tid i databasen
        public void SaveBookedTime(FindTimeModel findTimeModel)
        {

        }
    }
}