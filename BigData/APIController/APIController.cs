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
using static DataLogic.Models.SuggestionViewModels;

namespace BigData.APIController
{
    public class APIController : ApiController
    {
    #pragma warning disable 1998

        [HttpPost]
        [Route("api/addbookingsystem/")]
        public async Task<IHttpActionResult> AddBookingSystem(BookingSystemEntity bookingSystem)
        {
            new BookingSystemRepo().AddBookingSystem(bookingSystem);
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
        [Route("api/getallbookingtables/")]
        public async Task<IHttpActionResult> GetAllBookingTables()
        {
            var listOfAllBookings = new BookingRepo().GetAllBookingTables();
            return Ok(listOfAllBookings);
        }

        [HttpGet]
        [Route("api/getbooking/{id}")]
        public async Task<IHttpActionResult> GetBooking(int id)
        {
            var bookingTable = new BookingRepo().GetBookingTable(id);
            return Ok(bookingTable);
        }

        [HttpPost]
        [Route("api/addbooking/")]
        public async Task<IHttpActionResult> AddBooking(BookingTableEntity bookingTable)
        {
            await Task.Run(() => new BookingRepo().AddBooking(bookingTable));
            return Ok();
        }

        [HttpDelete]
        [Route("api/deletebooking/{id}")]
        public async Task<IHttpActionResult> DeleteBooking(int id)
        {
            new BookingRepo().DeleteBookingTable(id);
            return Ok();
        }

        [HttpPost]
        [Route ("api/getsuggestionsfromdifferentbookingsystems/")]
        public async Task<IHttpActionResult> GetSuggestionsFromDifferentBookingSystems(BookingTableEntity bookingTable)
        {
            var listOfSuggestions = await new SuggestionRepo().GetSuggestionsFromDifferentBookingSystems(bookingTable);
            return Ok(listOfSuggestions);
        }

        [HttpPost]
        [Route("api/getsuggestionsfromsamebookingsystem/")]
        public async Task<IHttpActionResult> GetSuggestionsFromSameBookingSystem(BookingTableEntity bookingTable)
        {
            var listOfSuggestions = await new SuggestionRepo().GetSuggestionsFromSameBookingSystem(bookingTable);
            return Ok(listOfSuggestions);
        }

        [HttpPost]
        [Route("api/addarticle/")]
        public async Task<IHttpActionResult> AddArticle(ArticleEntity article)
        {
            await Task.Run(() => new ArticleRepo().AddArticle(article));
            return Ok();
        }

        [HttpPost]
        [Route("api/checkiftimeisbooked")]
        public async Task<IHttpActionResult> CheckIfTimeIsBooked(CheckIfTimeIsBokedModel checkIfTimeIsBokedModel)
        {
            var timeBooked = new BookingRepo().CheckIfTimeIsBooked(checkIfTimeIsBokedModel);
            return Ok(timeBooked);
        }

        [HttpGet]
        [Route("api/getbookingsystemservicetype/{id}")]
        public async Task<IHttpActionResult> GetBookingSystemServiceType(int id)
        {
            var serviceType = new BookingSystemRepo().GetBookingSystemServiceType(id);
            return Ok(serviceType);
        }

        [HttpGet]
        [Route("api/getarticlesfrombookingsystem/{id}")]
        public async Task<IHttpActionResult> GetArticlesFromBookingSystem(int id)
        {
            var listOfArticles = new ArticleRepo().GetArticlesFromBookingSystem(id);
            return Ok(listOfArticles);
        }

        [HttpGet]
        [Route("api/getallarticles/")]
        public async Task<IHttpActionResult> GetAllArticle()
        {
            var listOfAllArticles = new ArticleRepo().GetAllArticles();
            return Ok(listOfAllArticles);
        }

        [HttpGet]
        [Route("api/getarticlelength/{id}")]
        public async Task<IHttpActionResult> GetArticleLength(int id)
        {
            var articleLength = new ArticleRepo().GetArticleLength(id);
            return Ok(articleLength);
        }

        [HttpGet]
        [Route("api/getbookingsystemfromarticle/{id}")]
        public async Task<IHttpActionResult> GetBookingSystemFromArticle(int id)
        {
            var bookingSystem = new BookingSystemRepo().GetBookingSystemFromArticle(id);
            return Ok(bookingSystem);
        }

        [HttpDelete]
        [Route("api/deletearticle/{id}")]
        public async Task<IHttpActionResult> DeleteArticle(int id)
        {
            new ArticleRepo().DeleteArticle(id);
            return Ok();
        }

        [HttpGet]
        [Route("api/getarticle/{id}")]
        public async Task<IHttpActionResult> GetArticel(int id)
        {
            var article = new ArticleRepo().GetArticle(id);
            return Ok(article);
        }


    }
}
