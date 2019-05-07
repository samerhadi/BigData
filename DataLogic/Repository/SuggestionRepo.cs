using DataLogic.Entities;
using DataLogic.Models;
using Newtonsoft.Json;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DataLogic.Repository
{
    public class SuggestionRepo
    {
        HttpClient client = new HttpClient();

        //Returnerar distansen i KM mellan bokad tjänst och alla tjänster i samma stad
        public async Task<double> DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }

        //Returnerar en list med alla bookningssystem som ligger inom en 5km radius
        public async Task<List<BookingSystemEntity>> ListOfBookingSystemsInRadius(List<BookingSystemEntity> listOfIncBookingSystems, BookingTableEntity bookingTable)
        {
            var listOfBookingSystems = new List<BookingSystemEntity>();
            var bookingSystem = await new ArticleRepo().GetBookingSystemFromArticleAsync(bookingTable.ArticleId);

            foreach (var item in listOfIncBookingSystems)
            {
                item.Distance = await DistanceTo(bookingSystem.Latitude, bookingSystem.Longitude, item.Latitude, item.Longitude);

                if (item.Distance <= 5)
                {
                    listOfBookingSystems.Add(item);
                }
            }

            return listOfBookingSystems;
        }

        //Returnerar en TimeBookedModel med bokningsystem och deras lediga tider 1h innan och efter gjord bokning
        public async Task<TimeBookedModel> FindTimesForListOfBookingSystems(BookingTableEntity bookingTable, List<BookingSystemEntity> listOfBookingSystem)
        {
            var timeBookedModel = new TimeBookedModel();
            var listOfFindTimeModels = new List<FindTimeModel>();

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
                findTimeModel.ListOfTimes = await CreateListOfTimes(findTimeModel, bookingTable);


                if (findTimeModel.ListOfTimes.Count() > 0)
                {
                    listOfFindTimeModels.Add(findTimeModel);
                }

                timeBookedModel.ListOfFindTimeModels = listOfFindTimeModels;
            }

            return timeBookedModel;
        }

        //Returnerar en lista med tider för ett bokningssystem 1h innan och efter en bokad tid
        public async Task<List<Times>> CreateListOfTimes(FindTimeModel findTimeModel, BookingTableEntity bookingTable)
        {
            var listOfTimes = new List<Times>();

            DateTime startTime = bookingTable.StartTime.AddMinutes(-60);
            DateTime endTime = startTime.AddMinutes(60);

            DateTime openingTime = SetOpeningTime(findTimeModel);
            DateTime closingTime = SetClosingTime(findTimeModel);

            var listOfBookingTables = await new BookingRepo().GetAllBookingTablesAsync();

            for (int i = 0; i < 3; i++)
            {
                if (startTime != bookingTable.StartTime && startTime >= openingTime && startTime < closingTime)
                {
                    var times = new Times();
                    times.StartTime = startTime;
                    times.EndTime = endTime;
                    findTimeModel.CheckTime = times;
                    times.TimeBooked = await CheckIfTimeIsBooked(findTimeModel, listOfBookingTables);

                    if (!times.TimeBooked)
                    {
                        listOfTimes.Add(times);
                    }
                }
                startTime = startTime.AddMinutes(60);
                endTime = endTime.AddMinutes(60);
            }

            return listOfTimes;
        }

        public async Task<bool> CheckIfTimeIsBooked(FindTimeModel findTimeModel, List<BookingTableEntity> listOfBookingTables)
        {
            var booked = await new BookingRepo().CheckIfTimeIsBookedAsync(findTimeModel, listOfBookingTables);
            return booked;
        }

        public DateTime SetOpeningTime(FindTimeModel findTimeModel)
        {
            DateTime timeEarly = DateTime.MinValue.Date.Add(new TimeSpan(08, 00, 00));
            DateTime openingTime = findTimeModel.Time;
            openingTime = openingTime.Date.Add(timeEarly.TimeOfDay);

            return openingTime;
        }

        public DateTime SetClosingTime(FindTimeModel findTimeModel)
        {
            DateTime timeLate = DateTime.MinValue.Date.Add(new TimeSpan(16, 00, 00));
            DateTime closingTime = findTimeModel.Time;
            closingTime = closingTime.Date.Add(timeLate.TimeOfDay);

            return closingTime;
        }

        public async Task<TimeBookedModel> GetSuggestions(BookingTableEntity bookingTable)
        {
            var bookingSystem = await new ArticleRepo().GetBookingSystemFromArticleAsync(bookingTable.ArticleId);

            var listOfBookingSystem = await new BookingSystemRepo().GetSuggestedServicesAsync(bookingSystem);

            var listOfBookingSystemInRadius = await ListOfBookingSystemsInRadius(listOfBookingSystem, bookingTable);

            var listOfArticles = await GetArticlesFromListOfBookingSystem(listOfBookingSystemInRadius, bookingTable, bookingSystem);

            var listOfRandomizedArticles = await RandomizeArticles(listOfArticles);

            var timeBookedModel = new TimeBookedModel();
            timeBookedModel = await FindTimesForListOfBookingSystems(bookingTable, listOfBookingSystemInRadius);
            timeBookedModel.BookingTableEntity = bookingTable;
            timeBookedModel.BookingSystemEntity = bookingSystem;

            return timeBookedModel;
        }

        public async Task<List<ArticleEntity>> GetArticlesFromListOfBookingSystem(List<BookingSystemEntity> listOfBookingSystems, BookingTableEntity bookingTable, BookingSystemEntity bookingSystem)
        {
            var listOfArticles = new List<ArticleEntity>();
            var tempListOfArticles = new List<ArticleEntity>();

            var article = await new ArticleRepo().GetArticleAsync(bookingTable.ArticleId);

            foreach(var item in listOfBookingSystems)
            {

                if (item.ServiceType == bookingSystem.ServiceType)
                {
                    tempListOfArticles = await new ArticleRepo().GetDifferentArticlesFromBookingSystem(item.BookningSystemId, article.Service);
                }

                else
                {
                    tempListOfArticles = await new ArticleRepo().GetArticlesFromBookingSystemAsync(item.BookningSystemId);
                }

                foreach(var i in tempListOfArticles)
                {
                    listOfArticles.Add(i);
                }
            }

            return listOfArticles;
        }

        public async Task<List<ArticleEntity>> GetArticlesFromListOfBookingSystem(BookingTableEntity bookingTable)
        {
            var article = await new ArticleRepo().GetArticleAsync(bookingTable.ArticleId);

            var bookingSystem =  await new ArticleRepo().GetBookingSystemFromArticleAsync(bookingTable.ArticleId);

            var listOfArticles = await new ArticleRepo().GetDifferentArticlesFromBookingSystem(bookingTable.ArticleId, article.Service);

            return listOfArticles; 
        }

        public async Task<List<ArticleEntity>> RandomizeArticles(List<ArticleEntity> listOfArticles)
        {
            Random rnd = new Random();

            var randomArticles = listOfArticles.OrderBy(x => rnd.Next()).Take(3);

            var listOfRandomArticles = randomArticles.ToList();

            return listOfRandomArticles;
        }
    }
}