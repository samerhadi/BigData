using DataLogic.Entities;
using Newtonsoft.Json;
using Nito.AsyncEx;
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

            addArticleModel.BookingSystemServiceType = AsyncContext.Run(() => (new BookingSystemController().GetBookingSystemServiceType(id)));
            return View(addArticleModel);
        }

        public async Task<ActionResult> ArticleAdded(AddArticleModel addArticleModel)
        {
            var article = addArticleModel.Article;

            article.Service = await SetBookingSystemServiceType(addArticleModel);

            await Task.Run(() => AddArticle(article));
            return View();
        }

        public async Task<int> SetBookingSystemServiceType(AddArticleModel addArticleModel)
        {
            int serviceType = 1;

            switch (addArticleModel.BookingSystemServiceType)
            {
                case 1:
                    return serviceType = Convert.ToInt32(addArticleModel.Hairdresser);
                case 2:
                    return serviceType = Convert.ToInt32(addArticleModel.Massage);
                case 3:
                    return serviceType = Convert.ToInt32(addArticleModel.BeautySalon);
                case 4: 
                    return serviceType = Convert.ToInt32(addArticleModel.Workshop);
            }

            return serviceType;
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