using DataLogic.Context;
using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DataLogic.Repository
{
    public class ArticleRepo
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public void AddArticle(ArticleEntity article)
        {
            context.Articles.Add(article);
            context.SaveChanges();
        }

        public List<ArticleEntity> GetAllArticles()
        {
            var listOfAllArticles = context.Articles.ToList();
            return listOfAllArticles;
        }

        public ArticleEntity GetArticle(int id)
        {
            var article = context.Articles.Find(id);
            return article;
        }

        public void DeleteArticle(int id)
        {
            var article = GetArticle(id);
            context.Articles.Remove(article);
            context.SaveChanges();
        }

        public double GetArticleLength(int id)
        {
            var article = GetArticle(id);
            var length = article.Length;
            return length;
        }

        public double GetArticleLengthAsync(int id)
        {
            var article = GetArticle(id);
            var length = article.Length;
            return length;
        }

        public async Task<ArticleEntity> GetArticleAsync(int id)
        {
            var article = context.Articles.Find(id);
            return article;
        }

        public async Task<List<ArticleEntity>> GetArticlesFromBookingSystemAsync(int id)
        {
            var listOfArticles = context.Articles.Where(a => a.BookingSystemId == id).ToList();
            return listOfArticles;
        }

        public List<ArticleEntity> GetArticlesFromBookingSystem(int id)
        {
            var listOfArticles = context.Articles.Where(a => a.BookingSystemId == id).ToList();
            return listOfArticles;
        }

        public async Task<List<ArticleEntity>> GetDifferentArticlesFromBookingSystem(int id, int service)
        {
            var listOfArticles = context.Articles.Where(a => a.BookingSystemId == id && a.Service != service).ToList();
            return listOfArticles;
        }
    }
}