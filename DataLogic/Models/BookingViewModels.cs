using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLogic.Entities;

namespace DataLogic.Models
{
   

    public class FindTimeModel
    {
        public DateTime Time { get; set; }
        public BookingSystemEntity BookingSystem { get; set; }
        public Boolean DateChoosen { get; set; }
        public List<Times> ListOfTimes { get; set; }
        public Times ChoosenTime { get; set; }
    }

    public class Times
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public bool TimeBooked { get; set; }
    }

    public class TimeBookedModel
    {
        public BookingTableEntity BookingTableEntity { get; set; }
        public List<BookingSystemEntity> ListOfBookingSystem { get; set; }
    }
}