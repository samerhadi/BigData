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

        public double GetArticleLength(int id)
        {
            var article = GetArticle(id);
            var articleLength = article.Length;

            return articleLength;
        }

        public ArticleEntity GetArticle(int id)
        {
            var article = context.Articles.Find(id);
            return article;
        }

        public BookingSystemEntity GetBookingSystemFromArticle(int id)
        {
            var article = GetArticle(id);
            var bookingSystem = new BookingSystemRepo().GetBookingSystem(article.BookingSystemId);
            return bookingSystem;
        }
        //Hämtar ett bokningssystem med hjälp av id
   


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

        public async Task<ArticleEntity> GetArticleAsync(int id)
        {
            var article = context.Articles.Find(id);
            return article;
        }

        public BookingSystemEntity GetBookingSystemFromArticle(int id)
        {
            var article = GetArticle(id);
            var bookingSystem = new BookingSystemRepo().GetBookingSystem(article.BookingSystemId);
            return bookingSystem;
            
        }

        public async Task<BookingSystemEntity> GetBookingSystemFromArticleAsync(int id)
        {
            var article = await GetArticleAsync(id);
            var bookingSystem = await new BookingSystemRepo().GetBookingSystemAsync(article.BookingSystemId);
            return bookingSystem;
        }

        public async Task<List<ArticleEntity>> GetArticlesFromBookingSystem(int id)
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