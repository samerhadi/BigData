using DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLogic.Entities;
using DataLogic.Context;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Web.Mvc;

namespace BigData.Controllers
{
    public class BookingController : BaseController
    {
        public static HttpClient client = new HttpClient();
        
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

        public async Task<ActionResult> GetAllBookingTables(int? id)
        {
            if (id != null)
            {
                await Task.Run(() => DeleteBooking(id));
            }

            var listOfBookingSystems = await GetBookingTables();
            UpdateModel(listOfBookingSystems);
            return View(listOfBookingSystems);
        }

        [HttpGet]
        public async Task<List<BookingTableEntity>>GetBookingTables()
        {
           
                var url = "http://localhost:60295/api/getallbookings/";

                var response = await client.GetAsync(string.Format(url));
                string result = await response.Content.ReadAsStringAsync();

                var bookingTables = JsonConvert.DeserializeObject<List<BookingTableEntity>>(result);

                if (response.IsSuccessStatusCode)
                {

                    return bookingTables;
                }
                var fail = new List<BookingTableEntity>();
                return fail;
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

        //public async Task<List<Times>> CreateListOfTimes(FindTimeModel findTimeModel)
        //{
        //    if(findTimeModel.TimeLength == 1)
        //    {
        //        var listOfTimes = new List<Times>();
        //        int startTime = 8;
        //        int endTime = startTime + 1;

        //        for (int i = 0; i < 8; i++)
        //        {
        //            var times = new Times();
        //            times.StartTime = startTime;
        //            times.EndTime = endTime;
        //            times.TimeBooked = await CheckIfTimeIsBooked(findTimeModel, times);
        //            listOfTimes.Add(times);
        //            startTime++;
        //            endTime++;
        //        }
        //    }

        //    if(findTimeModel.TimeLength == 0.30)
        //    {
        //        var listOfTimes = new List<Times>();
        //        int startTime = 8;
        //        int endTime = startTime + 0.5;

        //        for (int i = 0; i < 8; i++)
        //        {
        //            var times = new Times();
        //            times.StartTime = startTime;
        //            times.EndTime = endTime;
        //            times.TimeBooked = await CheckIfTimeIsBooked(findTimeModel, times);
        //            listOfTimes.Add(times);
        //            startTime + 0.5;
        //            endTime + 0.5;
        //        }
        //    }
        //}

        //Returnerar en bool beroende på om en tid är bokad eller inte
        public async Task<bool> CheckIfTimeIsBooked(FindTimeModel findTimeModel, Times times)
        {
            var timeBooked = false;

            var bookingTableEntity = new BookingTableEntity
            {
                StartTime = times.StartTime,
                Date = findTimeModel.Date,
                BookingSystemId = findTimeModel.BookingSystem.BookningSystemId
            };

            var url = "http://localhost:60295/api/getallbookings/";
            var response = await client.GetAsync(string.Format(url));
            string result = await response.Content.ReadAsStringAsync();

            var listOfBookingTables = JsonConvert.DeserializeObject<List<BookingTableEntity>>(result);

            if (response.IsSuccessStatusCode)
            {
                foreach (var item in listOfBookingTables)
                {
                    if (item.Date == bookingTableEntity.Date && item.StartTime == bookingTableEntity.StartTime && item.BookingSystemId == bookingTableEntity.BookingSystemId)
                    {
                        timeBooked = true;
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
                BookingSystemId = id,
                Date = date,
                StartTime = startTime,
                EndTime = endTime
            };

            await Task.Run(() => AddBooking(bookingTable));

            var timeBookedModel = await GetSuggestion(bookingTable);

            return View(timeBookedModel);
        }

        [HttpPost]
        public async Task<TimeBookedModel> GetSuggestion(BookingTableEntity bookingTable)
        {
            var failtimeBookedModel = new TimeBookedModel();
            var url = "http://localhost:60295/api/getsuggestions/";

            var content = new StringContent(JsonConvert.SerializeObject(bookingTable), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();

            var timeBookedModel = JsonConvert.DeserializeObject<TimeBookedModel>(result);

            if (response.IsSuccessStatusCode)
            {
                return timeBookedModel;
            }

            return failtimeBookedModel;
        }


        //Sparar en vald tid i databasen
        [HttpPost]
        public async void AddBooking(BookingTableEntity bookingTable)
        {
            var url = "http://localhost:60295/api/addbooking/";

            var content = new StringContent(JsonConvert.SerializeObject(bookingTable), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);

        }

   

        [HttpDelete]
        public async void DeleteBooking(int? id)
        {
            var url = "http://localhost:60295/api/deletebooking/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var result = await client.DeleteAsync(string.Format(url, content));

        }
    }
}