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
            findTimeModel.ListOfTimes = CreateListOfTimes(findTimeModel);
            
            return View(findTimeModel);
        }

        //Returnerar en lista med alla tider
        public List<Times> CreateListOfTimes(FindTimeModel findTimeModel)
        {
            var listOfTimes = new List<Times>();
            int startTime = 8;
            int endTime = startTime + 1;

            for (int i = 0; i < 8; i++)
            {
                var times = new Times();
                times.StartTime = startTime;
                times.EndTime = endTime;
                times.TimeBooked = CheckIfTimeIsBooked(findTimeModel, times);
                listOfTimes.Add(times);
                startTime++;
                endTime++;
            }
            return listOfTimes;
        }

        public List<Times> CreateListOfTimes(FindTimeModel findTimeModel, BookingTableEntity bookingTable)
        {
            var listOfTimes = new List<Times>();
            int startTime = bookingTable.StartTime - 1;
            int endTime = startTime + 1;

            for (int i = 0; i < 3; i++)
            {
                if(startTime != bookingTable.StartTime && startTime >= 8 && startTime < 16)
                {
                    var times = new Times();
                    times.StartTime = startTime;
                    times.EndTime = endTime;
                    times.TimeBooked = CheckIfTimeIsBooked(findTimeModel, times);

                    if (!times.TimeBooked)
                    {
                        listOfTimes.Add(times);
                    }
                }
                startTime++;
                endTime++;
            }
            return listOfTimes;
        }

        //Returnerar en bool beroende på om en tid är bokad eller inte
        public bool CheckIfTimeIsBooked(FindTimeModel findTimeModel, Times times)
        {
            var timeBooked = false;

            var bookingTableEntity = new BookingTableEntity
            {
                StartTime = times.StartTime,
                Date = findTimeModel.Time,
                BookingSystem = findTimeModel.BookingSystem
            };

            var databaseBookingTableList = new List<BookingTableEntity>();
            databaseBookingTableList = db.BookingTabels.ToList();

            foreach (var item in databaseBookingTableList)
            {
                if(item.Date == bookingTableEntity.Date && item.StartTime == bookingTableEntity.StartTime && item.BookingSystem == bookingTableEntity.BookingSystem)
                {
                    timeBooked = true;
                }
            }
            
            return timeBooked;
        }

        //GET: TimeBooked
        public ActionResult TimeBooked(DateTime date, int startTime, int endTime, int id)
        {
            
            var bookingTable = new BookingTableEntity
            {
                BookingSystem = db.BookingSystems.Find(id),
                Date = date,
                StartTime = startTime,
                EndTime = endTime
            };

            SaveBookedTime(bookingTable);

            var listOfBookingSystem = new List<BookingSystemEntity>();
            listOfBookingSystem = ServicesSuggestionList(bookingTable.BookingSystem);

            var listOfBookingSystemInRadius = new SuggestionController().AllSystemsInRadius(listOfBookingSystem, bookingTable);

            var timeBookedModel = new TimeBookedModel();
            timeBookedModel = FindTimesForAllBookingSystems(bookingTable, listOfBookingSystemInRadius);
            timeBookedModel.BookingTableEntity = bookingTable;

            return View(timeBookedModel);
        }

        public TimeBookedModel FindTimesForAllBookingSystems(BookingTableEntity bookingTable, List<BookingSystemEntity> listOfBookingSystem)
        {
            var timeBookedModel = new TimeBookedModel();

            foreach (var item in listOfBookingSystem)
            {
                var findTimeModel = new FindTimeModel();
                findTimeModel.BookingSystem = item;
                findTimeModel.Time = bookingTable.Date;

                var time = new Times
                {
                    StartTime = bookingTable.StartTime,
                    EndTime = bookingTable.EndTime
                };

                findTimeModel.ChoosenTime = time;
                findTimeModel.ListOfTimes = CreateListOfTimes(findTimeModel, bookingTable);
                var listOfFindTimeModels = new List<FindTimeModel>();

                if (findTimeModel.ListOfTimes.Count() > 0)
                {
                    listOfFindTimeModels.Add(findTimeModel);
                }

                timeBookedModel.ListOfFindTimeModels = listOfFindTimeModels;
            }

            return timeBookedModel;
        }

        //Sparar en vald tid i databasen
        public void SaveBookedTime(BookingTableEntity bookingTable)
        {
            db.BookingTabels.Add(bookingTable);
            db.SaveChanges();

        }

        public List<BookingSystemEntity> ServicesSuggestionList(BookingSystemEntity bookingSystem)
        {
            var listOfSuggestedServices = new List<BookingSystemEntity>();
            listOfSuggestedServices = db.BookingSystems.Where(b => b.City == bookingSystem.City && b.BookningSystemId != bookingSystem.BookningSystemId).ToList();

            return listOfSuggestedServices;
        }
    }
}