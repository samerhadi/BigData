using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLogic.Entities;

namespace DataLogic.Models
{
    public class Test
    {
        public DateTime Tid { get; set; }
    }

    public class FindTimeModel
    {
        public DateTime Tid { get; set; }
        public BookingSystemEntity BookingSystem { get; set; }
        public Boolean DateChoosen { get; set; }
    }
}