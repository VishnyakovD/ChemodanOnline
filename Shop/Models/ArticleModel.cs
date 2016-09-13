using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.db.Entities;

namespace Shop.Models
{
    public class ArticleModel:MenuModel
    {
        public Article article { set; get; }

        public ArticleModel()
        {
            article=new Article();
        }
    }

    public class ArticlesModel : MenuModel
    {
        public List<Article> articles { set; get; }

        public ArticlesModel()
        {
            articles=new List<Article>();
        }
    }

}