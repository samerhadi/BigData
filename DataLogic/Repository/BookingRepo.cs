using DataLogic.Context;
using DataLogic.Entities;
using DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;

namespace DataLogic.Repository
{
    public class BookingRepo
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public static HttpClient client = new HttpClient();

        public List<BookingTableEntity> GetAllBookingTables()
        {
            var listOfBookingTables = context.BookingTabels.ToList();
            return listOfBookingTables;
        }

        public async Task<bool> AddBooking(BookingTableEntity bookingTable)
        {
            await Task.Run(() => context.BookingTabels.Add(bookingTable));
            context.SaveChanges();

            return true;
        }

        public BookingTableEntity GetBookingTable(int id)
        {
            var bookingTable = context.BookingTabels.Find(id);
            return bookingTable;
        }

        public void DeleteBookingTable(int id)
        {
            var bookingTable = GetBookingTable(id);
            context.BookingTabels.Remove(bookingTable);
            context.SaveChanges();
        }

        //Returnerar en bool beroende på om en tid är bokad eller inte
        public async Task<bool> CheckIfTimeIsBooked(FindTimeModel findTimeModel, Times times)
        {
            var timeBooked = false;

            var bookingTableEntity = new BookingTableEntity
            {
                StartTime = times.StartTime,
                EndTime = times.EndTime,
                Date = findTimeModel.Time,
                BookingSystemId = findTimeModel.BookingSystem.BookningSystemId
            };

            var url = "http://localhost:60295/api/getallbookings/";
            var response = await client.GetAsync(string.Format(url));
            string result = await response.Content.ReadAsStringAsync();

            var listOfBookingTables = JsonConvert.DeserializeObject<List<BookingTableEntity>>(result);

            if (response.IsSuccessStatusCode)
            {
                foreach (var item in listOfBookingTables)
                {
                    if (item.Date == bookingTableEntity.Date && item.StartTime < bookingTableEntity.EndTime && item.EndTime > bookingTableEntity.StartTime
                        && item.BookingSystemId == bookingTableEntity.BookingSystemId)
                    {
                        timeBooked = true;
                    }
                }
            }

            return timeBooked;
        }

        ////Returnerar en bool beroende på om en tid är bokad eller inte
        //public async Task<bool> CheckIfTimeIsBooked(FindTimeModel findTimeModel, Times times)
        //{
        //    var timeBooked = false;

        //    var bookingTableEntity = new BookingTableEntity
        //    {
        //        StartTime = times.StartTime,
        //        Date = findTimeModel.Time,
        //        BookingSystemId = findTimeModel.BookingSystem.BookningSystemId
        //    };

        //    var url = "http://localhost:60295/api/getallbookings/";
        //    var response = await client.GetAsync(string.Format(url));
        //    string result = await response.Content.ReadAsStringAsync();

        //    var listOfBookingTables = JsonConvert.DeserializeObject<List<BookingTableEntity>>(result);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        foreach (var item in listOfBookingTables)
        //        {
        //            if (item.Date == bookingTableEntity.Date && item.StartTime == bookingTableEntity.StartTime && item.BookingSystemId == bookingTableEntity.BookingSystemId)
        //            {
        //                timeBooked = true;
        //            }
        //        }
        //    }

        //    return timeBooked;
        //}
    }
}