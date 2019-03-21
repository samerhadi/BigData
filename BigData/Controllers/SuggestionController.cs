using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLogic.Entities;

namespace BigData.Controllers
{
    public class SuggestionController : BaseController
    {
        public double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }

        public List<BookingSystemEntity> AllSystemsInRadius(List<BookingSystemEntity> listOfIncBookingSystems, BookingTableEntity bookingTable)
        {
            var listOfBookingSystems = new List<BookingSystemEntity>();

            foreach(var item in listOfIncBookingSystems)
            {
                var distance = DistanceTo(bookingTable.BookingSystem.Latitude, bookingTable.BookingSystem.Longitude, item.Latitude, item.Longitude);

                if (distance <= 5)
                {
                    listOfBookingSystems.Add(item);
                }
            }

            return listOfBookingSystems;
        }
    }
}