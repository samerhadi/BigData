using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataLogic.Context
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var bookingsystem1 = new BookingSystemEntity()
            {
                BookningSystemId = 0,
                SystemName = "Samers Bil och Däck",
                SystemDescription = "Verkstad",
                Email = "samersbilochdack@gmail.com",
                PhoneNumber = "00000000",
                Website = "0",
                CompanyName = "Samers Bil och Däck",
                ContactEmail = "samersbilochdack@gmail.com",
                ContactPhone = "0",
                Adress = "0",
                LatitudeAndLongitude = "0",
                PostaICode = "0",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem1);

            var bookingsystem2 = new BookingSystemEntity()
            {
                BookningSystemId = 0,
                SystemName = "Frisör Niklas",
                SystemDescription = "Frisör",
                Email = "frisorniklas@gmail.com",
                PhoneNumber = "00000000",
                Website = "0",
                CompanyName = "Frisör Niklas",
                ContactEmail = "frisorniklas@gmail.com",
                ContactPhone = "0",
                Adress = "0",
                LatitudeAndLongitude = "0",
                PostaICode = "0",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem2);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}