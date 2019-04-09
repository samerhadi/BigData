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

        //public void AddArticle(ArticleEntity article)
        //{
        //    context.Articles.Add(article);
        //    context.SaveChanges();
        //}
    }
}