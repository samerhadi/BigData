using DataLogic.Entities;
using DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DataLogic.Repository
{
    public class SuggestionRepo
    {
        //Returnerar distansen i KM mellan bokad tjänst och alla tjänster i samma stad
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

        //Returnerar en list med alla bookningssystem som ligger inom en 5km radius
        public List<BookingSystemEntity> ListOfAllSystemsInRadius(List<BookingSystemEntity> listOfIncBookingSystems, BookingTableEntity bookingTable)
        {
            var listOfBookingSystems = new List<BookingSystemEntity>();
            var bookingSystem = new BookingSystemRepo().GetBookingSystem(bookingTable.BookingSystemId);

            foreach (var item in listOfIncBookingSystems)
            {
                item.Distance = DistanceTo(bookingSystem.Latitude, bookingSystem.Longitude, item.Latitude, item.Longitude);

                if (item.Distance <= 5)
                {
                    listOfBookingSystems.Add(item);
                }
            }

            return listOfBookingSystems;
        }

        //Returnerar en list med alla tjänster i samma stad
        public List<BookingSystemEntity> ListOfServicesInSameCity(BookingSystemEntity bookingSystem)
        {
            var listOfSuggestedServices = new BookingSystemRepo().GetSuggestedServices(bookingSystem);

            return listOfSuggestedServices;
        }

        //Returnerar en TimeBookedModel med bokningsystem och deras lediga tider 1h innan och efter gjord bokning
        public async Task<TimeBookedModel> FindTimesForListOfBookingSystems(BookingTableEntity bookingTable, List<BookingSystemEntity> listOfBookingSystem)
        {
            var timeBookedModel = new TimeBookedModel();
            var listOfFindTimeModels = new List<FindTimeModel>();

            foreach (var item in listOfBookingSystem)
            {
                var findTimeModel = new FindTimeModel();
                findTimeModel.BookingSystem = item;

                findTimeModel.Time = bookingTable.Date;

                var time = new Times
                {
                    StartTime = bookingTable.StartTime,
                    EndTime = bookingTable.EndTime
                };

                findTimeModel.ChoosenTime = time;
                findTimeModel.ListOfTimes = await CreateListOfTimes(findTimeModel, bookingTable);


                if (findTimeModel.ListOfTimes.Count() > 0)
                {
                    listOfFindTimeModels.Add(findTimeModel);
                }

                timeBookedModel.ListOfFindTimeModels = listOfFindTimeModels;
            }

            return timeBookedModel;
        }

        //Returnerar en lista med tider för ett bokningssystem 1h innan och efter en bokad tid
        public async Task<List<Times>> CreateListOfTimes(FindTimeModel findTimeModel, BookingTableEntity bookingTable)
        {
            var listOfTimes = new List<Times>();

            DateTime startTime = findTimeModel.Time;
            DateTime time = DateTime.MinValue.Date.Add(new TimeSpan(08, 00, 00));
            startTime = startTime.Date.Add(time.TimeOfDay);
            DateTime endTime = startTime.AddMinutes(60);

            for (int i = 0; i < 3; i++)
            {
                                                        //&& startTime >= 8 && startTime < 16
                if (startTime != bookingTable.StartTime)
                {
                    var times = new Times();
                    times.StartTime = startTime;
                    times.EndTime = endTime;
                    times.TimeBooked = await new BookingRepo().CheckIfTimeIsBooked(findTimeModel, times);

                    if (!times.TimeBooked)
                    {
                        listOfTimes.Add(times);
                    }
                }
                startTime.AddMinutes(60);
                endTime.AddMinutes(60);
            }
            return listOfTimes;
        }

        public async Task<TimeBookedModel> GetSuggestions(BookingTableEntity bookingTable)
        {
            var bookingSystem = new BookingSystemRepo().GetBookingSystem(bookingTable.BookingSystemId);

            var listOfBookingSystem = ListOfServicesInSameCity(bookingSystem);

            var listOfBookingSystemInRadius = ListOfAllSystemsInRadius(listOfBookingSystem, bookingTable);

            var timeBookedModel = new TimeBookedModel();
            timeBookedModel = await FindTimesForListOfBookingSystems(bookingTable, listOfBookingSystemInRadius);
            timeBookedModel.BookingTableEntity = bookingTable;
            timeBookedModel.BookingSystemEntity = bookingSystem;

            return timeBookedModel;
        }
    }
}