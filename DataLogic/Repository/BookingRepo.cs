﻿using DataLogic.Context;
using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}