using DataLogic.Context;
using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DataLogic.Repository
{
    public class BookingRepo
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public List<BookingTableEntity> GetAllBookingTables()
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
    }
}