using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.DataService;
using Shop.db.Entities;
using Shop.Modules;
using WebMatrix.WebData;

namespace Shop.Models.Builders
{

    public interface IArticleBuilder
    {
        ArticlesModel Build();
      

    }

    public class ArticleBuilder : MenuBuilder, IArticleBuilder
    {
        private IImagesPath imagesPath;
        private IAccountAdminModelBuilder AccountAdminModelBuilder;

        public ArticleBuilder(IDataService dataService, IAccountAdminModelBuilder iAccountAdminModelBuilder, IImagesPath imagesPath)
            : base(dataService, imagesPath)
        {
            this.AccountAdminModelBuilder = iAccountAdminModelBuilder;
            this.imagesPath = imagesPath;
        }

        public ArticlesModel Build()
        {
            var model = new ArticlesModel();
            var path = imagesPath.GetImagesPath();
            model.articles = dataService.ListArticles();

            foreach (var article in model.articles)
            {
                article.imgPath = path + "/" + article.imgPath;
            }
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            return model;
        }

    }
}