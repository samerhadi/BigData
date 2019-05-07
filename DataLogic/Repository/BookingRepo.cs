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

        public async Task<List<BookingTableEntity>> GetAllBookingTablesAsync()
        {
            var listOfBookingTables = context.BookingTabels.ToList();
            return listOfBookingTables;
        }

        public void AddBooking(BookingTableEntity bookingTable)
        {
            context.BookingTabels.Add(bookingTable);
            context.SaveChanges();
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
        public bool CheckIfTimeIsBooked(FindTimeModel findTimeModel)
        {
            var timeBooked = false;

            var bookingTableEntity = new BookingTableEntity
            {
                StartTime = findTimeModel.CheckTime.StartTime,
                EndTime = findTimeModel.CheckTime.EndTime,
                Date = findTimeModel.Time,
                ArticleId = findTimeModel.BookingSystem.BookningSystemId
            };

            var listOfBookingTables = GetAllBookingTables();

            foreach (var item in listOfBookingTables)
            {
                if (item.Date == bookingTableEntity.Date && item.StartTime < bookingTableEntity.EndTime && item.EndTime > bookingTableEntity.StartTime
                    && item.ArticleId == bookingTableEntity.ArticleId)
                {
                    timeBooked = true;
                }
            }

            return timeBooked;
        }

        public async Task<bool> CheckIfTimeIsBookedAsync(FindTimeModel findTimeModel, List<BookingTableEntity> listOfBookingTables)
        {
            var timeBooked = false;

            var bookingTableEntity = new BookingTableEntity
            {
                StartTime = findTimeModel.CheckTime.StartTime,
                EndTime = findTimeModel.CheckTime.EndTime,
                Date = findTimeModel.Time,
                ArticleId = findTimeModel.ArticleId
                //Inget BokningssystemId
            };

            foreach (var item in listOfBookingTables)
            {
              if (item.Date == bookingTableEntity.Date && item.StartTime < bookingTableEntity.EndTime && item.EndTime > bookingTableEntity.StartTime
                    /*&& item.BookingSystemId == bookingTableEntity.BookingSystemId*/)
                {
                     timeBooked = true;
                }
            }

            return timeBooked;
        }
    }
}