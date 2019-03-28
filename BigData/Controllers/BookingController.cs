using DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLogic.Entities;
using DataLogic.Context;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

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
        public async Task<ActionResult> BookTime(FindTimeModel findTimeModel)
        {
            findTimeModel.BookingSystem = db.BookingSystems.Find(findTimeModel.BookingSystem.BookningSystemId);
            findTimeModel.DateChoosen = true;
            findTimeModel.ListOfTimes = await CreateListOfTimes(findTimeModel);

            return View(findTimeModel);
        }

        //Returnerar en lista med alla tider för ett bokningssystem
        public async Task<List<Times>> CreateListOfTimes(FindTimeModel findTimeModel)
        {
            var listOfTimes = new List<Times>();
            int startTime = 8;
            int endTime = startTime + 1;

            for (int i = 0; i < 8; i++)
            {
                var times = new Times();
                times.StartTime = startTime;
                times.EndTime = endTime;
                times.TimeBooked = await CheckIfTimeIsBooked(findTimeModel, times);
                listOfTimes.Add(times);
                startTime++;
                endTime++;
            }
            return listOfTimes;
        }

        //Returnerar en bool beroende på om en tid är bokad eller inte
        public async Task<bool> CheckIfTimeIsBooked(FindTimeModel findTimeModel, Times times)
        {
            var timeBooked = false;

            var bookingTableEntity = new BookingTableEntity
            {
                StartTime = times.StartTime,
                Date = findTimeModel.Time,
                BookingSystem = findTimeModel.BookingSystem
            };

            var url = "http://localhost:60295/api/getallbookings/";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(string.Format(url));
                string result = await response.Content.ReadAsStringAsync();

                var listOfBookingTables = JsonConvert.DeserializeObject<List<BookingTableEntity>>(result);

             
                if (response.IsSuccessStatusCode)
                {
                    foreach (var item in listOfBookingTables)
                    {
                        
                        if (item.Date == bookingTableEntity.Date && item.StartTime == bookingTableEntity.StartTime && item.BookingSystem == bookingTableEntity.BookingSystem)
                        {
                            timeBooked = true;
                        }
                    }
                }
            }

            return timeBooked;
        }

        //GET: TimeBooked
        public async Task<ActionResult> TimeBooked(DateTime date, int startTime, int endTime, int id)
        {

            var bookingTable = new BookingTableEntity
            {
                BookingSystem = db.BookingSystems.Find(id),
                Date = date,
                StartTime = startTime,
                EndTime = endTime
            };

            AddBooking(bookingTable);

            var listOfBookingSystem = new SuggestionController().ListOfServicesInSameCity(bookingTable.BookingSystem);
      

            var listOfBookingSystemInRadius = new SuggestionController().ListOfAllSystemsInRadius(listOfBookingSystem, bookingTable);

            var timeBookedModel = new TimeBookedModel();
            timeBookedModel = await new SuggestionController().FindTimesForListOfBookingSystems(bookingTable, listOfBookingSystemInRadius);
            timeBookedModel.BookingTableEntity = bookingTable;
            
            return View(timeBookedModel);
        }
        //Sparar en vald tid i databasen
        [HttpPost]
        public async void AddBooking(BookingTableEntity bookingTable)
        {
         
           
            var url = "http://localhost:60295/api/addbooking/";

            using (var client = new HttpClient())
            {
              
                var content = new StringContent(JsonConvert.SerializeObject(bookingTable), Encoding.UTF8, "application/json");

                var result = await client.PostAsync(url, content);
            }
        }
    }
}