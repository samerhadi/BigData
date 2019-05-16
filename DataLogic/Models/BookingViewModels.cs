﻿using System;
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
        public ArticleEntity Article { get; set; }
    }

    public class Times
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool TimeBooked { get; set; }
    }

    public class TimeBookedModel
    {
        public BookingTableEntity BookingTable { get; set; }
        public List<FindTimeModel> ListOfFindTimeModelsForDifferentBookingSystems { get; set; }
        public BookingSystemEntity BookingSystem { get; set; }
        public List<FindTimeModel> ListOfFindTimeModelsForSameBookingSystem { get; set; }
        public ArticleEntity Article { get; set; }
    }
}