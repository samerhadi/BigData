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
        public List<string> ListOfTimes { get; set; }
        public string ChoosenTime { get; set; }
    }
}