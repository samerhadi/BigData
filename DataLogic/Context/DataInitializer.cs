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
                ServiceType = ServiceType.Frisör,
                BookningSystemId = 0,
                SystemName = "Samers Bil och Däck",
                SystemDescription = "Verkstad",
                Email = "samersbilochdack@gmail.com",
                PhoneNumber = "0",
                Website = "0",
                CompanyName = "Samers Bil och Däck",
                ContactEmail = "samersbilochdack@gmail.com",
                ContactPhone = "0",
                Adress = "Glanshammarsvägen 16",
                Latitude = 59.294810,
                Longitude = 15.231510,
                PostaICode = "0",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem1);

            var bookingsystem2 = new BookingSystemEntity()
            {
                ServiceType = ServiceType.Ansiktebehandling,
                BookningSystemId = 0,
                SystemName = "Frisör Niklas",
                SystemDescription = "Frisör",
                Email = "frisorniklas@gmail.com",
                PhoneNumber = "00000000",
                Website = "0",
                CompanyName = "Frisör Niklas",
                ContactEmail = "frisorniklas@gmail.com",
                ContactPhone = "0",
                Adress = "Skolgatan 42",
                Latitude = 59.280750,
                Longitude = 15.223650,
                PostaICode = "0",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem2);

            var bookingsystem3 = new BookingSystemEntity()
            {
                ServiceType = ServiceType.Frisör,
                BookningSystemId = 0,
                SystemName = "Borrmaskiner AB",
                SystemDescription = "Borrmaskin",
                Email = "0",
                PhoneNumber = "00000000",
                Website = "0",
                CompanyName = "0",
                ContactEmail = "0",
                ContactPhone = "0",
                Adress = "Sandkullsvägen 24",
                Latitude = 0,
                Longitude = 0,
                PostaICode = "0",
                City = "Stockholm",
            };
            context.BookingSystems.Add(bookingsystem3);

            var bookingsystem4 = new BookingSystemEntity()
            {
                ServiceType = ServiceType.Massage,
                BookningSystemId = 0,
                SystemName = "Bam",
                SystemDescription = "Hej",
                Email = "frisorniklas@gmail.com",
                PhoneNumber = "00000000",
                Website = "0",
                CompanyName = "Bam",
                ContactEmail = "frisorniklas@gmail.com",
                ContactPhone = "0",
                Adress = "Sandkullsvägen 24",
                Latitude = 59.382950,
                Longitude = 17.917660,
                PostaICode = "0",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem4);

            var bookingsystem5 = new BookingSystemEntity()
            {
                ServiceType = ServiceType.Ansiktebehandling,
                BookningSystemId = 0,
                SystemName = "Petters Ansiktesbehandling",
                SystemDescription = "Frisör",
                Email = "frisorniklas@gmail.com",
                PhoneNumber = "00000000",
                Website = "0",
                CompanyName = "Petters Ansiktesbehandling",
                ContactEmail = "frisorniklas@gmail.com",
                ContactPhone = "0",
                Adress = "Skolgatan 42",
                Latitude = 59.281750,
                Longitude = 15.223650,
                PostaICode = "0",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem5);

            var bookingsystem6 = new BookingSystemEntity()
            {
                ServiceType = ServiceType.Ansiktebehandling,
                BookningSystemId = 0,
                SystemName = "Salong Nageltrång",
                SystemDescription = "Frisör",
                Email = "frisorniklas@gmail.com",
                PhoneNumber = "00000000",
                Website = "0",
                CompanyName = "Salong Nageltrång",
                ContactEmail = "frisorniklas@gmail.com",
                ContactPhone = "0",
                Adress = "Skolgatan 42",
                Latitude = 59.280750,
                Longitude = 15.233650,
                PostaICode = "0",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem6);

            var bookedTime1 = new BookingTableEntity()
            {
                BookingTableId = 1,
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                BookingSystemId = 1
            };
            context.BookingTabels.Add(bookedTime1);

            var bookedTime2 = new BookingTableEntity()
            {
                BookingTableId = 1,
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                BookingSystemId = 1
            };
            context.BookingTabels.Add(bookedTime2);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}