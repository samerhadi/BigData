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
        public bool DateChoosen { get; set; }
        public List<Times> ListOfTimes { get; set; }
        public Times ChoosenTime { get; set; }
        public Times CheckTime { get; set; }
        public double TimeLength { get; set; }
        public int ArticleId { get; set; }
    }

    public class Times
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool TimeBooked { get; set; }
    }

    public class TimeBookedModel
    {
        public BookingTableEntity BookingTableEntity { get; set; }
        public List<FindTimeModel> ListOfFindTimeModels { get; set; }
        public BookingSystemEntity BookingSystemEntity { get; set; }
    }
}