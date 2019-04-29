using DataLogic.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static DataLogic.Models.ArticleViewModels;

namespace BigData.Controllers
{
    public class ArticleController : Controller
    {
        public static HttpClient client = new HttpClient();
        // GET: Article
        public async Task<ActionResult> AddArticle(int id)
        {
            var addArticleModel = new AddArticleModel();
            addArticleModel.BookingSystemId = id;

            addArticleModel.BookingSystemServiceType = await new BookingSystemController().GetBookingSystemServiceType(id);

            return View(addArticleModel);
        }

        public ActionResult ArticleAdded()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateArticle(ArticleEntity article)
        {

            var url = "http://localhost:60295/api/addarticle";

            var content = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("ArticleAdded");
            }

            return View(article);
        }
    }
}