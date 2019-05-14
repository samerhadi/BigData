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
                ServiceType = 4,
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
                ServiceType = 1,
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
                ServiceType = 1,
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
                Latitude = 59.382580,
                Longitude = 17.917903,
                PostaICode = "703 65",
                City = "Stockholm",
            };
            context.BookingSystems.Add(bookingsystem3);

            var bookingsystem4 = new BookingSystemEntity()
            {
                ServiceType = 2,
                BookningSystemId = 4,
                SystemName = "Saras Massage",
                SystemDescription = "Massage",
                Email = "saras.massage@gmail.com",
                PhoneNumber = "070-000 00 00",
                Website = "www.sarasmassage.se",
                CompanyName = "Saras Massage",
                ContactEmail = "saras.massage@gmail.com",
                ContactPhone = "070-000 00 00",
                Adress = "Glanshammarsvägen 16",
                Latitude = 59.382950,
                Longitude = 17.917660,
                PostaICode = "703 65",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem4);

            var bookingsystem5 = new BookingSystemEntity()
            {
                ServiceType = 3,
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
                ServiceType = 3,
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

            var bookingsystem7 = new BookingSystemEntity()
            {
                ServiceType = 5,
                BookningSystemId = 7,
                SystemName = "Aspholmens Biltvätt",
                SystemDescription = "Biltvätt",
                Email = "aspholmens.biltvatt@gmail.com",
                PhoneNumber = "070-000 00 00",
                Website = "www.aspholmensbiltvatt.se",
                CompanyName = "Aspholmens Biltvätt",
                ContactEmail = "aspholmens.biltvatt@gmail.com",
                ContactPhone = "070-000 00 00",
                Adress = "Skolgatan 42",
                Latitude = 59.280750,
                Longitude = 15.233650,
                PostaICode = "703 65",
                City = "Örebro",
            };
            context.BookingSystems.Add(bookingsystem7);

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
                Name = "Däck Byte",
                Length = 20,
                Price = 150,
                BookingSystemId = 1,
                Service = 1
            };
            context.Articles.Add(article1);

            var article2 = new ArticleEntity()
            {
                ArticleId = 2,
                Name = "Service",
                Length = 60,
                Price = 1000,
                BookingSystemId = 1,
                Service = 1
            };
            context.Articles.Add(article2);

            var article3 = new ArticleEntity()
            {
                ArticleId = 3,
                Name = "Besiktning",
                Length = 30,
                Price = 600,
                BookingSystemId = 1,
                Service = 2
            };
            context.Articles.Add(article3);

            var article4 = new ArticleEntity()
            {
                ArticleId = 4,
                Name = "Herrklippning",
                Length = 20,
                Price = 300,
                BookingSystemId = 2,
                Service = 1
            };
            context.Articles.Add(article4);

            var article5 = new ArticleEntity()
            {
                ArticleId = 5,
                Name = "Damklippning",
                Length = 30,
                Price = 400,
                BookingSystemId = 2,
                Service = 1
            };
            context.Articles.Add(article5);

            var article6 = new ArticleEntity()
            {
                ArticleId = 6,
                Name = "Permanenta",
                Length = 60,
                Price = 700,
                BookingSystemId = 2,
                Service = 2
            };
            context.Articles.Add(article6);

            var article7 = new ArticleEntity()
            {
                ArticleId = 7,
                Name = "Herrklippning",
                Length = 20,
                Price = 300,
                BookingSystemId = 3,
                Service = 1
            };
            context.Articles.Add(article7);

            var article8 = new ArticleEntity()
            {
                ArticleId = 8,
                Name = "Damklippning",
                Length = 30,
                Price = 400,
                BookingSystemId = 3,
                Service = 1
            };
            context.Articles.Add(article8);

            var article9 = new ArticleEntity()
            {
                ArticleId = 9,
                Name = "Permanenta",
                Length = 60,
                Price = 700,
                BookingSystemId = 3,
                Service = 2
            };
            context.Articles.Add(article9);

            var article10 = new ArticleEntity()
            {
                ArticleId = 10,
                Name = "Svensk Massage",
                Length = 30,
                Price = 500,
                BookingSystemId = 4,
                Service = 1
            };
            context.Articles.Add(article10);

            var article11 = new ArticleEntity()
            {
                ArticleId = 11,
                Name = "Thailändsk Massage",
                Length = 30,
                Price = 500,
                BookingSystemId = 4,
                Service = 1
            };
            context.Articles.Add(article11);

            var article12 = new ArticleEntity()
            {
                ArticleId = 12,
                Name = "Nack Massage",
                Length = 15,
                Price = 250,
                BookingSystemId = 4,
                Service = 2
            };
            context.Articles.Add(article12);

            var article13 = new ArticleEntity()
            {
                ArticleId = 13,
                Name = "Ansiktsbehandling",
                Length = 30,
                Price = 400,
                BookingSystemId = 5,
                Service = 1
            };
            context.Articles.Add(article13);

            var article14 = new ArticleEntity()
            {
                ArticleId = 14,
                Name = "Ansiktsmask",
                Length = 30,
                Price = 350,
                BookingSystemId = 5,
                Service = 1
            };
            context.Articles.Add(article14);

            var article15 = new ArticleEntity()
            {
                ArticleId = 15,
                Name = "Pedekyr",
                Length = 30,
                Price = 300,
                BookingSystemId = 5,
                Service = 2
            };
            context.Articles.Add(article15);

            var article16 = new ArticleEntity()
            {
                ArticleId = 16,
                Name = "Ansiktsbehandling",
                Length = 30,
                Price = 400,
                BookingSystemId = 6,
                Service = 1
            };
            context.Articles.Add(article16);

            var article17 = new ArticleEntity()
            {
                ArticleId = 17,
                Name = "Ansiktsmask",
                Length = 30,
                Price = 350,
                BookingSystemId = 6,
                Service = 1
            };
            context.Articles.Add(article17);

            var article18 = new ArticleEntity()
            {
                ArticleId = 18,
                Name = "Pedekyr",
                Length = 30,
                Price = 300,
                BookingSystemId = 6,
                Service = 2
            };
            context.Articles.Add(article18);

            var article19 = new ArticleEntity()
            {
                ArticleId = 19,
                Name = "Biltvätt",
                Length = 20,
                Price = 200,
                BookingSystemId = 7,
                Service = 1
            };
            context.Articles.Add(article19);

            var article20 = new ArticleEntity()
            {
                ArticleId = 20,
                Name = "Städning",
                Length = 15,
                Price = 150,
                BookingSystemId = 7,
                Service = 1
            };
            context.Articles.Add(article20);

            var article21 = new ArticleEntity()
            {
                ArticleId = 21,
                Name = "Biltvätt och Städning",
                Length = 45,
                Price = 300,
                BookingSystemId = 7,
                Service = 1
            };
            context.Articles.Add(article21);

            var article22 = new ArticleEntity()
            {
                ArticleId = 22,
                Name = "Waxning",
                Length = 30,
                Price = 300,
                BookingSystemId = 7,
                Service = 2
            };
            context.Articles.Add(article22);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}