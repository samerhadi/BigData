using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLogic.Entities;

namespace DataLogic.Models
{
    public class Times
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool TimeBooked { get; set; }
    }

    public class BookTimeModel
    {
        public bool DateChoosen { get; set; }
        public int ArticleId { get; set; }
        public List<Times> ListOfTimes { get; set; }
        public DateTime Time { get; set; }
    }

    public class CheckIfTimeIsBokedModel
    {
        public List<BookingTableEntity> ListOfBookingTables { get; set; }
        public Times Times { get; set; }
    }
}