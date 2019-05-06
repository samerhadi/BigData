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
using DataLogic.Repository;
using Nito.AsyncEx;

namespace BigData.Controllers
{
    public class BookingController : BaseController
    {
        public static HttpClient client = new HttpClient();

        // GET: BookTime första gången den besöks
        public ActionResult BookTime(int id)
        {
            var findTimeModel = new FindTimeModel();
            findTimeModel.ArticleId = id;
            findTimeModel.BookingSystem = AsyncContext.Run(() => (new ArticleController().GetBookingSystemFromArticle(id)));
            findTimeModel.DateChoosen = false;
            return View(findTimeModel);
        }

        //GET: BookTime med en vald dag
        [HttpPost]
        public async Task<ActionResult> BookTime(FindTimeModel findTimeModel)
        {
            findTimeModel.DateChoosen = true;
            findTimeModel.ListOfTimes = await CreateListOfTimes(findTimeModel);

            return View(findTimeModel);
        }
        //hämtar alla bookingtables 
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
        public async Task<List<BookingTableEntity>> GetBookingTables()
        {
            var url = "http://localhost:60295/api/getallbookingtables/";

            var response = await client.GetAsync(string.Format(url));
            string result = await response.Content.ReadAsStringAsync();

            var listOfBookingTables = JsonConvert.DeserializeObject<List<BookingTableEntity>>(result);

            if (response.IsSuccessStatusCode)
            {

                return listOfBookingTables;
            }

            return listOfBookingTables;
        }

        //Returnerar en lista med alla tider för ett bokningssystem
        public async Task<List<Times>> CreateListOfTimes(FindTimeModel findTimeModel)
        {

            double timeLength = await GetArticleLength(findTimeModel.ArticleId);
            var listOfTimes = new List<Times>();

            DateTime startTime = SetStartTime(findTimeModel);
            DateTime endTime = startTime.AddMinutes(timeLength);
            var timesPerDay = SetTimesPerDay(timeLength);

            var listOfBookingTables = await GetBookingTables();

            for (int i = 0; i < timesPerDay; i++)
            {
                var time = new Times();

                time.StartTime = startTime;
                time.EndTime = endTime;

                findTimeModel.CheckTime = time;
                findTimeModel.CheckTime.TimeBooked = await new SuggestionRepo().CheckIfTimeIsBooked(findTimeModel, listOfBookingTables);

                listOfTimes.Add(findTimeModel.CheckTime);
                startTime = startTime.AddMinutes(timeLength);
                endTime = endTime.AddMinutes(timeLength);

            }
            return listOfTimes;
        }

        //Ska inte ligga här!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        [HttpGet]
        public async Task<double> GetArticleLength(int id)
        {
            var url = "http://localhost:60295/api/getarticlelength/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var response = await client.GetAsync(string.Format(url, content));
            string result = await response.Content.ReadAsStringAsync();

            var articleLength = JsonConvert.DeserializeObject<double>(result);

            if (response.IsSuccessStatusCode)
            {
                return articleLength;
            }

            return articleLength;
        }

        //sätter när en bookningssystem öppnar
        public DateTime SetStartTime(FindTimeModel findTimeModel)
        {
            DateTime startTime = findTimeModel.Time;
            DateTime time = DateTime.MinValue.Date.Add(new TimeSpan(08, 00, 00));
            startTime = startTime.Date.Add(time.TimeOfDay);

            return startTime;
        }

        //räknar ut hur många gånger per dag den valda tiden är
        public double SetTimesPerDay(double timeLength)
        {
            double openingTime = 8;
            double closingTime = 16;
            double timeOpen = closingTime - openingTime;
            double timeOpenMinutes = timeOpen * 60;
            double timesPerDay = timeOpenMinutes / timeLength;

            return timesPerDay;
        }

        //skickar in en model, med tiden man har valt.
        public async Task<ActionResult> TimeBooked(DateTime date, DateTime startTime, DateTime endTime, int id)
        {

            var bookingTable = new BookingTableEntity
            {
                ArticleId = id,
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
            var timeBookedModel = new TimeBookedModel();
            var url = "http://localhost:60295/api/getsuggestions/";

            var content = new StringContent(JsonConvert.SerializeObject(bookingTable), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();

            timeBookedModel = JsonConvert.DeserializeObject<TimeBookedModel>(result);

            if (response.IsSuccessStatusCode)
            {
                return timeBookedModel;
            }

            return timeBookedModel;
        }

        //Sparar en vald tid i databasen
        [HttpPost]
        public async void AddBooking(BookingTableEntity bookingTable)
        {
            var url = "http://localhost:60295/api/addbooking/";

            var content = new StringContent(JsonConvert.SerializeObject(bookingTable), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);

        }

        //tar bort en bokningtable
        [HttpDelete]
        public async void DeleteBooking(int? id)
        {
            var url = "http://localhost:60295/api/deletebooking/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var result = await client.DeleteAsync(string.Format(url, content));

        }
    }
}