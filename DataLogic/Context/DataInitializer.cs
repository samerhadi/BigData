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
                ServiceType = ServiceType.Workshop,
                BookningSystemId = 1,
                SystemName = "Bil och Däck",
                SystemDescription = "Verkstad",
                Email = "bilochdack@gmail.com",
                PhoneNumber = "070-000 00 00",
                Website = "www.bilochdack.se",
                CompanyName = "Bil och Däck",
                ContactEmail = "bilochdack@gmail.com",
                ContactPhone = "070-000 00 00",
                Adress = "Glanshammarsvägen 16",
                Latitude = 59.294810,
                Longitude = 15.231510,
                PostaICode = "703 64",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem1);

            var bookingsystem2 = new BookingSystemEntity()
            {
                ServiceType = ServiceType.Hairdresser,
                BookningSystemId = 2,
                SystemName = "Salong Klipp",
                SystemDescription = "Frisör",
                Email = "salong.klipp@gmail.com",
                PhoneNumber = "070-000 00 00",
                Website = "www.salongklipp.se",
                CompanyName = "Salong Klipp",
                ContactEmail = "salong.klipp@gmail.com",
                ContactPhone = "070-000 00 00",
                Adress = "Skolgatan 42",
                Latitude = 59.280750,
                Longitude = 15.223650,
                PostaICode = "703 65",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem2);

            var bookingsystem3 = new BookingSystemEntity()
            {
                ServiceType = ServiceType.Hairdresser,
                BookningSystemId = 3,
                SystemName = "Klipp och Trimm",
                SystemDescription = "Frisör",
                Email = "klippochtrimm@gmail.com",
                PhoneNumber = "070-000 00 00",
                Website = "www.klippochtrimm.se",
                CompanyName = "Klipp och Trimm",
                ContactEmail = "klippochtrimm@gmail.com",
                ContactPhone = "070-000 00 00",
                Adress = "Sandkullsvägen 24",
                Latitude = 59.3825808,
                Longitude = 17.917903,
                PostaICode = "703 65",
                City = "Stockholm",
            };
            context.BookingSystems.Add(bookingsystem3);

            var bookingsystem4 = new BookingSystemEntity()
            {
                ServiceType = ServiceType.Massage,
                BookningSystemId = 4,
                SystemName = "Saras Massage",
                SystemDescription = "Massage",
                Email = "saras.massage@gmail.com",
                PhoneNumber = "070-000 00 00",
                Website = "www.sarasmassage.se",
                CompanyName = "Saras Massage",
                ContactEmail = "saras.massage@gmail.com",
                ContactPhone = "070-000 00 00",
                Adress = "Sandkullsvägen 24",
                Latitude = 59.382950,
                Longitude = 17.917660,
                PostaICode = "703 65",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem4);

            var bookingsystem5 = new BookingSystemEntity()
            {
                ServiceType = ServiceType.BeautySalon,
                BookningSystemId = 5,
                SystemName = "Stefans Skön och Fin",
                SystemDescription = "Skönhetssalong",
                Email = "stefansskonochfin@gmail.com",
                PhoneNumber = "070-000 00 00",
                Website = "www.stefansskonochfin.se",
                CompanyName = "Stefans Skön och Fin",
                ContactEmail = "stefansskonochfin@gmail.com",
                ContactPhone = "070-000 00 00",
                Adress = "Skolgatan 42",
                Latitude = 59.281750,
                Longitude = 15.223650,
                PostaICode = "703 65",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem5);

            var bookingsystem6 = new BookingSystemEntity()
            {
                ServiceType = ServiceType.BeautySalon,
                BookningSystemId = 6,
                SystemName = "Salong Finast",
                SystemDescription = "Skönhetssalong",
                Email = "salong.finast@gmail.com",
                PhoneNumber = "070-000 00 00",
                Website = "www.salongfinast.se",
                CompanyName = "Salong Finast",
                ContactEmail = "salong.finast@gmail.com",
                ContactPhone = "070-000 00 00",
                Adress = "Skolgatan 42",
                Latitude = 59.280750,
                Longitude = 15.233650,
                PostaICode = "703 65",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem6);

            var bookedTime1 = new BookingTableEntity()
            {
                BookingTableId = 1,
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                ArticleId = 1
            };
            context.BookingTabels.Add(bookedTime1);

            var bookedTime2 = new BookingTableEntity()
            {
                BookingTableId = 1,
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                ArticleId = 1
            };
            context.BookingTabels.Add(bookedTime2);

            var article1 = new ArticleEntity()
            {
                ArticleId = 1,
                Name = "Herrklippning",
                Length = 20,
                Price = 300,
                BookingSystemId = 2,
                Service = 1
            };
            context.Articles.Add(article1);

            var article2 = new ArticleEntity()
            {
                ArticleId = 2,
                Name = "Damklippning",
                Length = 30,
                Price = 400,
                BookingSystemId = 2,
                Service = 1
            };
            context.Articles.Add(article2);

            var article3 = new ArticleEntity()
            {
                ArticleId = 3,
                Name = "Permanenta",
                Length = 60,
                Price = 700,
                BookingSystemId = 2,
                Service = 2
            };
            context.Articles.Add(article3);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}