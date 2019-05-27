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
using static DataLogic.Models.SuggestionViewModels;

namespace BigData.Controllers
{
    public class BookingController : BaseController
    {
        public static HttpClient client = new HttpClient();

        // GET: BookTime första gången den besöks
        public ActionResult BookTime(int id)
        {
            var bookTimeModel = new BookTimeModel();
            bookTimeModel.ArticleId = id;
            bookTimeModel.DateChoosen = false;
            return View(bookTimeModel);
        }

        [HttpPost]
        public async Task<ActionResult> BookTime(BookTimeModel bookTimeModel)
        {
            var bookingSystem = await new ArticleController().GetBookingSystemFromArticle(bookTimeModel.ArticleId);
                    
            bookTimeModel.DateChoosen = true;
            bookTimeModel.ListOfTimes = await CreateTimes(bookTimeModel.Time, bookTimeModel.ArticleId);

            return View(bookTimeModel);
        }

        //hämtar alla bookingtables 
        public async Task<ActionResult> AllBookingTables(int? id)
        {
            if (id != null)
            {
                await Task.Run(() => DeleteBooking(id));
            }

            var listOfBookingSystems = await GetAllBookingTables();
            UpdateModel(listOfBookingSystems);
            return View(listOfBookingSystems);
        }

        [HttpGet]
        public async Task<List<BookingTableEntity>> GetAllBookingTables()
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

        public async Task<List<Times>> CreateTimes(DateTime date, int articleId)
        {
            var listOfTimes = new List<Times>();

            var article = await new ArticleController().GetArticle(articleId);

            DateTime startTime = SetOpeningTime(date);
            DateTime endTime = startTime.AddMinutes(article.Length);

            var timesPerDay = SetTimesPerDay(article.Length);

            var bookingSystem = await new BookingSystemRepo().GetBookingSystemFromArticleAsync(articleId);

            var listOfBookingTables = await new BookingRepo().GetBookingTablesFromOneDayAndOneBookingSystem(bookingSystem.BookningSystemId, date);

            for (int i = 0; i < timesPerDay; i++)
            {
                var time = new Times();

                time.StartTime = startTime;
                time.EndTime = endTime;

                var checkIfTimeIsBookedModel = new CheckIfTimeIsBokedModel();
                checkIfTimeIsBookedModel.Times = time;
                checkIfTimeIsBookedModel.ListOfBookingTables = listOfBookingTables;

                time.TimeBooked = await CheckIfTimeIsBooked(checkIfTimeIsBookedModel);

                listOfTimes.Add(time);

                startTime = startTime.AddMinutes(article.Length);
                endTime = endTime.AddMinutes(article.Length);
            }

            return listOfTimes;
        }

        [HttpPost]
        public async Task<bool> CheckIfTimeIsBooked(CheckIfTimeIsBokedModel checkIfTimeIsBokedModel)
        {
            var url = "http://localhost:60295/api/checkiftimeisbooked/";

            var content = new StringContent(JsonConvert.SerializeObject(checkIfTimeIsBokedModel), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();

            var booked = JsonConvert.DeserializeObject<bool>(result);

            if (response.IsSuccessStatusCode)
            {
                return booked;
            }

            return booked;
        }

        //sätter när en bookningssystem öppnar
        public DateTime SetOpeningTime(DateTime date)
        {
            DateTime startTime = date;
            DateTime time = DateTime.MinValue.Date.Add(new TimeSpan(08, 00, 00));
            startTime = startTime.Date.Add(time.TimeOfDay);

            return startTime;
        }

        //räknar ut hur många gånger per dag den valda tiden är
        public double SetTimesPerDay(double length)
        {
            double openingTime = 8;
            double closingTime = 16;

            double timeOpen = closingTime - openingTime;
            double timeOpenMinutes = timeOpen * 60;
            double timesPerDay = timeOpenMinutes / length;

            timesPerDay = Math.Floor(timesPerDay);

            return timesPerDay;
        }

        //skickar in en model, med tiden man har valt.
        public async Task<ActionResult> TimeBooked(DateTime date, DateTime startTime, DateTime endTime, int articleId)
        {
            var bookingSystem = await new ArticleController().GetBookingSystemFromArticle(articleId);

            var bookingTable = new BookingTableEntity
            {
                ArticleId = articleId,
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                BookingSystemId = bookingSystem.BookningSystemId
            };

            await Task.Run(() => AddBooking(bookingTable));

            var suggestionViewModel = new SuggestionViewModel();

            suggestionViewModel.ListOfSuggestionsFromDifferentBookingSystems = await GetSuggestionsFromDifferentBookingSystems(bookingTable);
            suggestionViewModel.Article = await new ArticleRepo().GetArticleAsync(articleId);
            suggestionViewModel.BookingTable = bookingTable;
            suggestionViewModel.ListOfSuggestionsFromSameBookingSystems = await GetSuggestionsFromSameBookingSystem(bookingTable);
            suggestionViewModel.BookingSystem = bookingSystem;

            return View(suggestionViewModel);
        }

        [HttpPost]
        public async Task<List<Suggestion>> GetSuggestionsFromDifferentBookingSystems(BookingTableEntity bookingTable)
        {
            var url = "http://localhost:60295/api/getsuggestionsfromdifferentbookingsystems/";

            var content = new StringContent(JsonConvert.SerializeObject(bookingTable), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();

            var listOfSuggestions = JsonConvert.DeserializeObject<List<Suggestion>>(result);

            if (response.IsSuccessStatusCode)
            {
                return listOfSuggestions;
            }

            return listOfSuggestions;
        }

        [HttpPost]
        public async Task<List<Suggestion>> GetSuggestionsFromSameBookingSystem(BookingTableEntity bookingTable)
        {
            var url = "http://localhost:60295/api/getsuggestionsfromsamebookingsystem/";

            var content = new StringContent(JsonConvert.SerializeObject(bookingTable), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();

            var listOfSuggestions = JsonConvert.DeserializeObject<List<Suggestion>>(result);

            if (response.IsSuccessStatusCode)
            {
                return listOfSuggestions;
            }

            return listOfSuggestions;
        }

        //Sparar en vald tid i databasen
        [HttpPost]
        public async void AddBooking(BookingTableEntity bookingTable)
        {
            var url = "http://localhost:60295/api/addbooking/";

            var content = new StringContent(JsonConvert.SerializeObject(bookingTable), Encoding.UTF8, "application/json");
            var result = client.PostAsync(url, content);

        }

        //tar bort en bokningtable
        [HttpDelete]
        public async void DeleteBooking(int? id)
        {
            var url = "http://localhost:60295/api/deletebooking/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var result = client.DeleteAsync(string.Format(url, content));

        }
    }
}