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
using static DataLogic.Models.SuggestionViewModels;

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
            var bookingSystem = await new BookingSystemRepo().GetBookingSystemFromArticleAsync(bookingTable.ArticleId);

            foreach (var item in listOfIncBookingSystems)
            {
                var distance = await DistanceTo(bookingSystem.Latitude, bookingSystem.Longitude, item.Latitude, item.Longitude);

                if (distance <= 5)
                {
                    listOfBookingSystems.Add(item);
                }
            }

            return listOfBookingSystems;
        }

        //Returnerar en TimeBookedModel med bokningsystem och deras lediga tider 1h innan och efter gjord bokning
        public async Task<List<Suggestion>> CreateTimesForSuggestions(BookingTableEntity bookingTable, List<ArticleEntity> listOfArticles)
        {
            var listOfSuggestions = new List<Suggestion>();

            foreach (var item in listOfArticles)
            {
                var time = new Times
                {
                    StartTime = bookingTable.StartTime,
                    EndTime = bookingTable.EndTime
                };

                var suggestion = new Suggestion();

                suggestion.ListOfTimes = await CreateTimes(item, bookingTable);
                suggestion.Article = item;
                suggestion.Date = bookingTable.Date;
                suggestion.BookingSystem = await new BookingSystemRepo().GetBookingSystemFromArticleAsync(item.ArticleId);


                if (suggestion.ListOfTimes.Count() > 0)
                {
                    listOfSuggestions.Add(suggestion);
                }
            }

            return listOfSuggestions;
        }

        public async Task<List<Times>> CreateTimes(ArticleEntity article, BookingTableEntity bookingTable)
        {
            var listOfTimes = new List<Times>();

            DateTime startTime = bookingTable.StartTime.AddMinutes(-article.Length);
            DateTime endTime = startTime.AddMinutes(article.Length);

            DateTime openingTime = SetOpeningTime(bookingTable.Date);
            DateTime closingTime = SetClosingTime(bookingTable.Date);

            var bookingSystem = await new BookingSystemRepo().GetBookingSystemAsync(article.BookingSystemId);

            var listOfBookingTables = await new BookingRepo().GetBookingTablesFromOneDayAndOneBookingSystem(bookingSystem.BookningSystemId, bookingTable.Date);

            for (int i = 0; i < 3; i++)
            {
                if (startTime != bookingTable.StartTime && startTime >= openingTime && endTime <= closingTime)
                {
                    var time = new Times();

                    time.StartTime = startTime;
                    time.EndTime = endTime;

                    var checkIfTimeIsBookedModel = new CheckIfTimeIsBokedModel();
                    checkIfTimeIsBookedModel.Times = time;
                    checkIfTimeIsBookedModel.ListOfBookingTables = listOfBookingTables;

                    time.TimeBooked = await new BookingRepo().CheckIfTimeIsBookedAsync(checkIfTimeIsBookedModel);

                    if (!time.TimeBooked)
                    {
                        listOfTimes.Add(time);
                    }
                    
                }
                    
                if (i == 1)
                {
                    var bookedArticle = await new ArticleRepo().GetArticleAsync(bookingTable.ArticleId);
                    startTime = startTime.AddMinutes(bookedArticle.Length);
                    endTime = endTime.AddMinutes(bookedArticle.Length);
                }

                else
                {
                    startTime = startTime.AddMinutes(article.Length);
                    endTime = endTime.AddMinutes(article.Length);
                }
            }

            return listOfTimes;
        }

        public DateTime SetOpeningTime(DateTime date)
        {
            DateTime startTime = date;
            DateTime time = DateTime.MinValue.Date.Add(new TimeSpan(08, 00, 00));
            startTime = startTime.Date.Add(time.TimeOfDay);

            return startTime;
        }

        public DateTime SetClosingTime(DateTime date)
        {
            DateTime endTime = DateTime.MinValue.Date.Add(new TimeSpan(16, 00, 00));
            DateTime closingTime = date;
            closingTime = closingTime.Date.Add(endTime.TimeOfDay);

            return closingTime;
        }

        public async Task<List<Suggestion>> GetSuggestionsFromDifferentBookingSystems(BookingTableEntity bookingTable)
        {
            var bookingSystem = await new BookingSystemRepo().GetBookingSystemFromArticleAsync(bookingTable.ArticleId);

            var listOfBookingSystemFromSameCity = await new BookingSystemRepo().GetSuggestedServicesAsync(bookingSystem);

            var listOfBookingSystemInRadius = await ListOfBookingSystemsInRadius(listOfBookingSystemFromSameCity, bookingTable);

            var listOfArticles = await GetArticlesFromBookingSystemsBasedOnServiceType(bookingTable, listOfBookingSystemInRadius);

            var listOfRandomizedArticles = await RandomizeArticles(listOfArticles);

            var listOfSuggestions = await CreateTimesForSuggestions(bookingTable, listOfRandomizedArticles);

            return listOfSuggestions;
        }

        public async Task<List<Suggestion>> GetSuggestionsFromSameBookingSystem(BookingTableEntity bookingTable)
        {
            var bookingSystem = await new BookingSystemRepo().GetBookingSystemFromArticleAsync(bookingTable.ArticleId);

            var listOfArticles = await GetDifferentArticlesFromBookingSystem(bookingTable);

            var listOfRandomizedArticles = await RandomizeArticles(listOfArticles);

            var listOfSuggestions = await CreateTimesForSuggestions(bookingTable, listOfRandomizedArticles);

            return listOfSuggestions;
        }

        public async Task<List<ArticleEntity>> GetArticlesFromBookingSystemsBasedOnServiceType(BookingTableEntity bookingTable, List<BookingSystemEntity> listOfBookingSystems)
        {
            var bookingSystem = await new BookingSystemRepo().GetBookingSystemFromArticleAsync(bookingTable.ArticleId);

            var listOfArticles = new List<ArticleEntity>();

            if (bookingSystem.ServiceType == 5 || bookingSystem.ServiceType == 4)
            {
                listOfArticles = await ServiceTypeCar(listOfBookingSystems, bookingTable, bookingSystem);
            }

            if (bookingSystem.ServiceType == 1 || bookingSystem.ServiceType == 2 || bookingSystem.ServiceType == 3)
            {
                listOfArticles = await ServiceTypeBeuty(listOfBookingSystems, bookingTable, bookingSystem);
            }

            if(listOfArticles.Count() < 3)
            {
                listOfArticles = await FillListWithRandomArticles(listOfArticles, listOfBookingSystems, bookingSystem.ServiceType);
            }

            return listOfArticles;
        }

        public async Task<List<ArticleEntity>> FillListWithRandomArticles(List<ArticleEntity> listOfArticles, List<BookingSystemEntity> listOfBookingSystems, int serviceType)
        {
            var article = new ArticleEntity();

            for (var i = listOfArticles.Count(); i < 3; i++)
            {
                article = (await AddRandomArticle(listOfBookingSystems, serviceType));

                var bookingSystem = await new BookingSystemRepo().GetBookingSystemAsync(article.BookingSystemId);

                var bookingSystemToRemove = listOfBookingSystems.FirstOrDefault(b => b.BookningSystemId == bookingSystem.BookningSystemId);

                listOfBookingSystems.Remove(bookingSystemToRemove);
                listOfArticles.Add(article);
            }

            return listOfArticles;
        }

        public async Task<ArticleEntity> AddRandomArticle(List<BookingSystemEntity> listOfBookingSystems, int serviceType)
        {
            var article = new ArticleEntity();

            if (serviceType == 1 || serviceType == 2 || serviceType == 3)
            {
                article = await AddRandomArticleForCategoryBeuty(listOfBookingSystems, serviceType);
            }

            if (serviceType == 4 || serviceType == 5)
            {
                article = await AddRandomArticleForCategoryCar(listOfBookingSystems, serviceType);
            }

            return article;
        }

        public async Task<ArticleEntity> AddRandomArticleForCategoryBeuty(List<BookingSystemEntity> listOfBookingSystems, int serviceType)
        {
            var listOfBookingSystemsFromDifferentCategory = listOfBookingSystems.Where(b => b.ServiceType != 1 && b.ServiceType != 2 && b.ServiceType != 3).ToList();
            var bookingSystem = await RandomizeABookingSystem(listOfBookingSystemsFromDifferentCategory);
            var listOfArticles = await new ArticleRepo().GetArticlesFromBookingSystemAsync(bookingSystem.BookningSystemId);

            var article = await RandomizeOneArticle(listOfArticles);

            return article;
        }

        public async Task<ArticleEntity> AddRandomArticleForCategoryCar(List<BookingSystemEntity> listOfBookingSystems, int serviceType)
        {
            var newListOfBookingSystems = listOfBookingSystems.Where(b => b.ServiceType != 4 && b.ServiceType != 5).ToList();
            var bookingSystem = await RandomizeABookingSystem(newListOfBookingSystems);
            var listOfArticles = await new ArticleRepo().GetArticlesFromBookingSystemAsync(bookingSystem.BookningSystemId);

            var article = await RandomizeOneArticle(listOfArticles);

            return article;
        }

        public async Task<List<ArticleEntity>> ServiceTypeCar(List<BookingSystemEntity> listOfBookingSystem, BookingTableEntity bookingTable, BookingSystemEntity bookingSystem)
        {
            var listOfArticles = new List<ArticleEntity>();

            var workshopListOfBookingSystem = new List<BookingSystemEntity>();
            var carWashListOfBookingSystem = new List<BookingSystemEntity>();

            foreach (var item in listOfBookingSystem)
            {
                if (item.ServiceType == 4)
                {
                    workshopListOfBookingSystem.Add(item);
                }

                if (item.ServiceType == 5)
                {
                    carWashListOfBookingSystem.Add(item);
                }
            }

            var workshopListOfArticles = await GetArticlesFromBookingSystems(workshopListOfBookingSystem, bookingTable, bookingSystem);
            var carWashListOfArticles = await GetArticlesFromBookingSystems(carWashListOfBookingSystem, bookingTable, bookingSystem);

            if(workshopListOfArticles.Count() > 0)
            {
                listOfArticles.Add(await RandomizeOneArticle(workshopListOfArticles));
            }
            
            if(carWashListOfArticles.Count() > 0)
            {
                listOfArticles.Add(await RandomizeOneArticle(carWashListOfArticles));
            }
            
            return listOfArticles;
        }

        public async Task<List<ArticleEntity>> ServiceTypeBeuty(List<BookingSystemEntity> listOfBookingSystem, BookingTableEntity bookingTable, BookingSystemEntity bookingSystem)
        {
            var listOfArticles = new List<ArticleEntity>();

            var hairdresserListOfBookingSystem = new List<BookingSystemEntity>();
            var massageListOfBookingSystem = new List<BookingSystemEntity>();
            var beutySalonListOfBookingSystem = new List<BookingSystemEntity>();

            foreach (var item in listOfBookingSystem)
            {
                if (item.ServiceType == 1)
                {
                    hairdresserListOfBookingSystem.Add(item);
                }

                if(item.ServiceType == 2)
                {
                    massageListOfBookingSystem.Add(item);
                }
                
                if(item.ServiceType == 3)
                {
                    beutySalonListOfBookingSystem.Add(item);
                }
            }

            var hairdresserListOfArticles = await GetArticlesFromBookingSystems(hairdresserListOfBookingSystem, bookingTable, bookingSystem);
            var massageListOfArticles = await GetArticlesFromBookingSystems(massageListOfBookingSystem, bookingTable, bookingSystem);
            var beutySalondListOfArticles = await GetArticlesFromBookingSystems(beutySalonListOfBookingSystem, bookingTable, bookingSystem);

            if(hairdresserListOfArticles.Count() > 0)
            {
                listOfArticles.Add(await RandomizeOneArticle(hairdresserListOfArticles));
            }
            
            if(massageListOfArticles.Count() > 0)
            {
                listOfArticles.Add(await RandomizeOneArticle(massageListOfArticles));
            }
            
            if(beutySalondListOfArticles.Count() > 0)
            {
                listOfArticles.Add(await RandomizeOneArticle(beutySalondListOfArticles));
            }
            
            return listOfArticles;
        }

        public async Task<ArticleEntity> RandomizeOneArticle(List<ArticleEntity> listOfArticles)
        {
            Random rnd = new Random();

            var index = rnd.Next(listOfArticles.Count);

            var article = listOfArticles[index];

            return article;
        }

        public async Task<BookingSystemEntity> RandomizeABookingSystem(List<BookingSystemEntity> listOfBookingSystems)
        {
            Random rnd = new Random();

            var index = rnd.Next(listOfBookingSystems.Count);

            var bookingSystem = listOfBookingSystems[index];

            return bookingSystem;
        }

        public async Task<List<ArticleEntity>> GetArticlesFromBookingSystems(List<BookingSystemEntity> listOfBookingSystems, BookingTableEntity bookingTable, BookingSystemEntity bookingSystem)
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

        public async Task<List<ArticleEntity>> GetDifferentArticlesFromBookingSystem(BookingTableEntity bookingTable)
        {
            var article = await new ArticleRepo().GetArticleAsync(bookingTable.ArticleId);

            var bookingSystem =  await new BookingSystemRepo().GetBookingSystemFromArticleAsync(bookingTable.ArticleId);

            var listOfArticles = await new ArticleRepo().GetDifferentArticlesFromBookingSystem(bookingSystem.BookningSystemId, article.Service);

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