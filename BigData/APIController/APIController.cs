using BigData.Controllers;
using DataLogic.Entities;
using DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DataLogic.Repository;

namespace BigData.APIController
{
    public class APIController : ApiController
    {
        [HttpPost]
        [Route("api/addbookingsystem/")]
        public async Task<IHttpActionResult> AddBookingSystem(BookingSystemEntity bookingSystemEntity)
        {
            new BookingSystemRepo().AddBookingSystem(bookingSystemEntity);
            return Ok();
        }

        [HttpGet]
        [Route("api/getbookingsystem/{id}")]
        public async Task<IHttpActionResult> GetBookingSystem(int id)
        {
            var bookingSystem = new BookingSystemRepo().GetBookingSystem(id);
            return Ok(bookingSystem);
        }

        [HttpGet]
        [Route("api/getallbookingsystems/")]
        public async Task<IHttpActionResult> GetAllBookingSystems()
        {
            var listOfBookingSystems = new BookingSystemRepo().GetAllBookingSystems();
            return Ok(listOfBookingSystems);
        }

        [HttpGet]
        [Route("api/getbookingsystemsfromcity/{city}")]
        public async Task<IHttpActionResult> GetBookingSystemsFromCity(string city)
        {
            var listOfBookingSystem = new BookingSystemRepo().GetBookingSystemsFromCity(city);
            return Ok(listOfBookingSystem);
        }

        [HttpDelete]
        [Route("api/deletebookingsystem/{id}")]
        public async Task<IHttpActionResult> DeleteBookingSystem(int id)
        {
            new BookingSystemRepo().DeleteBookingSystem(id);
            return Ok();
        }

        [HttpGet]
        [Route("api/getallbookings/")]
        public async Task<IHttpActionResult> GetAllBookings()
        {
            var listOfAllBookings = new BookingRepo().GetAllBookingTables();
            return Ok(listOfAllBookings);
        }


        [HttpPost]
        [Route("api/addbooking/")]
        public async Task<IHttpActionResult> AddBooking(BookingTableEntity bookingTable)
        {
            new BookingRepo().AddBooking(bookingTable);
            return Ok();
        }

    }
}
