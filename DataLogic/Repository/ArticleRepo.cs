using DataLogic.Context;
using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}