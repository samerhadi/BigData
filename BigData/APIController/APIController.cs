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
        [Route("api/addbookingsystem")]
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

    }
}
