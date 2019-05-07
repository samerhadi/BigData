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

        public List<ArticleEntity> CreateArticle(int id)
        {
            var listOfArticles = context.Articles.Where(x => x.BookingSystemId == id).ToList();
            return listOfArticles;
        }

        public List<ArticleEntity> GetAllArticles()
        {
            var listOfAllArticles = context.Articles.ToList();
            return listOfAllArticles;
        }

        //Hämtar ett bokningssystem med hjälp av id
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

    }
}