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

namespace BigData.Controllers
{
    public class ArticleController : Controller
    {
        public static HttpClient client = new HttpClient();
        // GET: Article
        public ActionResult AddArticle(int id)
        {
            var article = new ArticleEntity();
            article.BookingSystemId = id;
            return View(article);
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