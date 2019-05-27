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

        public async Task<List<BookingTableEntity>> GetBookingTablesFromOneDayAndOneBookingSystem(int bookingSystemId, DateTime date)
        {
            var listOfBookingTableEntity = context.BookingTabels.Where(b => b.BookingSystemId == bookingSystemId && b.Date == date).ToList();
            return listOfBookingTableEntity;
        }

        public bool CheckIfTimeIsBooked(CheckIfTimeIsBokedModel checkIfTimeIsBokedModel)
        {
            var timeBooked = false;

            foreach (var item in checkIfTimeIsBokedModel.ListOfBookingTables)
            {
                if (item.StartTime < checkIfTimeIsBokedModel.Times.EndTime && item.EndTime > checkIfTimeIsBokedModel.Times.StartTime)
                {
                    timeBooked = true;
                }
            }

            return timeBooked;
        }

        public async Task<bool> CheckIfTimeIsBookedAsync(CheckIfTimeIsBokedModel checkIfTimeIsBokedModel)
        {
            var timeBooked = false;

            foreach (var item in checkIfTimeIsBokedModel.ListOfBookingTables)
            {
                if (item.StartTime < checkIfTimeIsBokedModel.Times.EndTime && item.EndTime > checkIfTimeIsBokedModel.Times.StartTime)
                {
                    timeBooked = true;
                }
            }

            return timeBooked;
        }
    }
}