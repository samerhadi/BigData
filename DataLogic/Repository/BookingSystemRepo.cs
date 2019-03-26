using DataLogic.Context;
using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataLogic.Repository
{
    public class BookingSystemRepo
    {
        ApplicationDbContext context = new ApplicationDbContext();
        
        //Spara ett nytt bokningssystem
        public void AddBookingSystem(BookingSystemEntity system)
        {
            context.BookingSystems.Add(system);
            context.SaveChanges();
        }

        //Hämtar ett bokningssystem med hjälp av id
        public BookingSystemEntity GetBookingSystem(int id)
        {
            var bookingSystem = context.BookingSystems.Find(id);
            return bookingSystem;
        }

        //Hämtar ut alla bokningssystem
        public List<BookingSystemEntity> GetAllBookingSystem()
        {
            var listOfAllServices = context.BookingSystems.ToList();
            return listOfAllServices;
        }
    }
}