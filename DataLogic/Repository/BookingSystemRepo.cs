using DataLogic.Context;
using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DataLogic.Repository
{
    public class BookingSystemRepo
    {
        ApplicationDbContext context = new ApplicationDbContext();
        
        //Spara ett nytt bokningssystem
        public void AddBookingSystem(BookingSystemEntity bookingSystem)
        {
            context.BookingSystems.Add(bookingSystem);
            context.SaveChanges();
        }

        //Hämtar ett bokningssystem med hjälp av id
        public BookingSystemEntity GetBookingSystem(int id)
        {
            var bookingSystem = context.BookingSystems.Find(id);
            return bookingSystem;
        }

        //Hämtar ett bokningssystem med hjälp av id
        public async Task<BookingSystemEntity> GetBookingSystemAsync(int id)
        {
            var bookingSystem = context.BookingSystems.Find(id);
            return bookingSystem;
        }

        //Hämtar ut alla bokningssystem
        public List<BookingSystemEntity> GetAllBookingSystems()
        {
            var listOfAllBookingSystems = context.BookingSystems.ToList();
            return listOfAllBookingSystems;
        }

        public List<BookingSystemEntity> GetBookingSystemsFromCity(string city)
        {
            var listOfBookingSystemFromACity = context.BookingSystems.Where(b => b.City == city).ToList();
            return listOfBookingSystemFromACity;
        }

        public void DeleteBookingSystem(int id)
        {
            var bookingSystem = GetBookingSystem(id);
            context.BookingSystems.Remove(bookingSystem);
            context.SaveChanges();
        }

        public List<BookingSystemEntity> GetSuggestedServices(BookingSystemEntity bookingSystem)
        {
            var listOfServices = context.BookingSystems.Where(b => b.City == bookingSystem.City && b.BookningSystemId != bookingSystem.BookningSystemId
            && bookingSystem.ServiceType != b.ServiceType).ToList();

            return listOfServices;
        }

        public async Task<List<BookingSystemEntity>> GetSuggestedServicesAsync(BookingSystemEntity bookingSystem)
        {
            var listOfServices = context.BookingSystems.Where(b => b.City == bookingSystem.City && b.BookningSystemId != bookingSystem.BookningSystemId).ToList();

            return listOfServices;
        }

        public int GetBookingSystemServiceType(int id)
        {
            var bookingSystem = GetBookingSystem(id);
            var serviceType = GetServiceType(bookingSystem);

            return serviceType;
        }

        public int GetServiceType(BookingSystemEntity bookingSystem)
        {
            var serviceType = Convert.ToInt32(bookingSystem.ServiceType);
            return serviceType;
        }


        public BookingSystemEntity GetBookingSystemFromArticle(int id)
        {
            var article = new ArticleRepo().GetArticle(id);
            var bookingSystem = new BookingSystemRepo().GetBookingSystem(article.BookingSystemId);
            return bookingSystem;

        }

        public async Task<BookingSystemEntity> GetBookingSystemFromArticleAsync(int id)
        {
            var article = await new ArticleRepo().GetArticleAsync(id);
            var bookingSystem = await new BookingSystemRepo().GetBookingSystemAsync(article.BookingSystemId);
            return bookingSystem;
        }

    }
}