﻿using DataLogic.Entities;
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

            article.BookingSystemId = addArticleModel.BookingSystemId;

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

        public async Task<ActionResult> GetAllArticles(int? id)
        {
            if (id != null)
            {
                await Task.Run(() => DeleteArticle(id));

            }


            var getAllArticles = await GetArticles();
            UpdateModel(getAllArticles);

            return View(getAllArticles);

        }
        [HttpGet]
        public async Task<List<ArticleEntity>> GetArticles()
        {
            if (ModelState.IsValid)
            {
                var listOfAllArticles = new List<ArticleEntity>();

                var url = "http://localhost:60295/api/getallarticles/";
                var response = await client.GetAsync(string.Format(url));
                string result = await response.Content.ReadAsStringAsync();

                listOfAllArticles = JsonConvert.DeserializeObject<List<ArticleEntity>>(result);

                if (response.IsSuccessStatusCode)
                {
                    return listOfAllArticles;
                }


            }
            var fail = new List<ArticleEntity>();
            return fail;
        }

        [HttpGet]
        public async Task<List<ArticleEntity>> GetArticlesFromBookingSystem(int id)
        {

            var url = "http://localhost:60295/api/getarticlesfrombookingsystem/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var response = await client.GetAsync(string.Format(url, content));
            string result = await response.Content.ReadAsStringAsync();

            var listOfArticles = JsonConvert.DeserializeObject<List<ArticleEntity>>(result);

            if (response.IsSuccessStatusCode)
            {
                return listOfArticles;
            }

            return listOfArticles;
        }

        [HttpGet]
        public async Task<BookingSystemEntity> GetBookingSystemFromArticle(int id)
        {
            var url = "http://localhost:60295/api/getbookingsystemfromarticle/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var response = await client.GetAsync(string.Format(url, content));
            string result = await response.Content.ReadAsStringAsync();

            var bookingSystem = JsonConvert.DeserializeObject<BookingSystemEntity>(result);

            if (response.IsSuccessStatusCode)
            {
                return bookingSystem;
            }

            return bookingSystem;
        }

        //tar bort ett system
        [HttpDelete]
        public async void DeleteArticle(int? id)
        {
            var url = "http://localhost:60295/api/deletearticle/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var result = await client.DeleteAsync(string.Format(url, content));
        }

        [HttpGet]
        public async Task<double> GetArticleLength(int id)
        {
            var url = "http://localhost:60295/api/getarticlelength/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var response = await client.GetAsync(string.Format(url, content));
            string result = await response.Content.ReadAsStringAsync();

            var length = JsonConvert.DeserializeObject<double>(result);

            if (response.IsSuccessStatusCode)
            {
                return length;
            }

            return length;
        }

        [HttpGet]
        public async Task<ArticleEntity> GetArticle(int id)
        {

            var url = "http://localhost:60295/api/getarticle/" + id;

            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var response = await client.GetAsync(string.Format(url, content));
            string result = await response.Content.ReadAsStringAsync();

            var article = JsonConvert.DeserializeObject<ArticleEntity>(result);

            if (response.IsSuccessStatusCode)
            {
                return article;
            }

            return article;
        }
    }
}