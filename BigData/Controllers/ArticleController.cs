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
        
        public ActionResult CreateArticle(int id)
        {
            var addArticleModel = new AddArticleModel();
            addArticleModel.BookingSystemId = id;

            addArticleModel.BookingSystemServiceType = new BookingSystemController().GetBookingSystemServiceType(id);

            return View(addArticleModel);
        }

        public ActionResult ArticleAdded(AddArticleModel addArticleModel)
        {
            var article = addArticleModel.Article;

            if (addArticleModel.BookingSystemServiceType == 1)
            {
                article.Service = Convert.ToInt32(addArticleModel.Hairdresser);
            }

            if (addArticleModel.BookingSystemServiceType == 2)
            {
                article.Service = Convert.ToInt32(addArticleModel.Massage);
            }

            if (addArticleModel.BookingSystemServiceType == 3)
            {
                article.Service = Convert.ToInt32(addArticleModel.BeautySalon);
            }

            if (addArticleModel.BookingSystemServiceType == 4)
            {
                article.Service = Convert.ToInt32(addArticleModel.Workshop);
            }

            AddArticle(article);
            return View();
        }

        [HttpPost]
        public async void AddArticle(ArticleEntity article)
        {
            var url = "http://localhost:60295/api/addarticle";

            var content = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);
        }
    }
}