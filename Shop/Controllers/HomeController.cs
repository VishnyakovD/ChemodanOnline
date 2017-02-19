using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebPages;
using Newtonsoft.Json;
using Shop.DataService;
using Shop.db.Entities;
using Shop.db.Repository;
using Shop.Filters;
using Shop.Logger;
using Shop.Models;
using Shop.Models.Builders;
using Shop.Modules;

namespace Shop.Controllers
{
      [InitializeSimpleMembership]
    public class HomeController : BaseController
    {
        private ISkuViewerBuilder skuViewerBuilder { set; get; }
        private IArticleBuilder articleBuilder { set; get; }
        private MenuBuilder menuBuilder { set; get; }
        private IMainPageBuilder mainPageBuilder { set; get; }


        public HomeController(ILogger logger,
            IAdminModelBuilder adminModelBuilder,
            IDataService dataService,
            IImagesPath imagesPath,
            ISkuViewerBuilder SkuViewerBuilder,
            ISKUModelBuilder SKUModelBuilder,
            IArticleBuilder ArticleBuilder,
            IMainPageBuilder mainPageBuilder)
            : base(logger, adminModelBuilder, dataService, imagesPath, SKUModelBuilder)
        {
            menuBuilder = new MenuBuilder(dataService,imagesPath);
            skuViewerBuilder = SkuViewerBuilder;
            articleBuilder = ArticleBuilder;
            this.mainPageBuilder = mainPageBuilder;
        }

        public ActionResult ListSkuOnCategory(long idCat=-1)
        {
            var filters = new FilterFoDb();
            filters.ChemodanLocationID = DefaultValueHasInStock;
            filters.Categories = idCat < 1 ?
                new FilterItemValue[] {new FilterItemValue() {Id = DefaultSkuCategory}} :
                new FilterItemValue[] { new FilterItemValue() { Id = idCat } };

            var model = skuViewerBuilder.Build(filters);
            return View("ListSkuOnCategory", model);
        }

        public ActionResult ListProducts(string filtersSp, string filtersTp, string filtersCt)
        {

            var specs= JsonConvert.DeserializeObject<FilterItemValue[]>(filtersSp);
            var types= JsonConvert.DeserializeObject<FilterItemValue[]>(filtersTp);
            var cats= JsonConvert.DeserializeObject<FilterItemValue[]>(filtersCt);

            var model = skuViewerBuilder.BuildListProductsByFilters(
                new FilterFoDb{
                    Specifications = specs,
                    ChemodanTypes = types,
                    Categories = cats,
                    ChemodanLocationID = DefaultValueHasInStock
                }
                );
            return View("ShortProductListPartial", model);
        }

        public ActionResult SkuInfo(long idSku)
        {
            ViewBag.isHideLeftMenu = true;
            var result = skuViewerBuilder.BuildSkuModel(idSku);
            result.description = result.description;
            result.care = result.care;
            return View("SkuInfo", result);
        }

        public ActionResult Index()
        {
            var model = mainPageBuilder.Build();
            return View("MainPage", model);
    
        }

        public ActionResult ArticleInfo(long? idArticle)
        {
            var result = new ArticleModel();
            try
            {
                result.menu = menuBuilder.BuildMenu();
                result.topMenuItems = menuBuilder.BuildTopMenu();
                if (idArticle.HasValue && idArticle.Value > 0)
                {
                    var articleDB = dataService.Get<Article>(idArticle.Value);
                    articleDB.imgPath = imagesPath.GetImagesPath() + "/" + articleDB.imgPath;
                    result.article = articleDB;
                    result.article.body = ConvertToHTMLString(articleDB.body);
                }
            }
            catch (Exception err)
            {
                return View("MessagesPartial", "Ошибка " + err.Message);
            }

            return View("ArticleInfo", result);
        }

        public void LoadContent(string num="-1")
          {
              try
              {
                  Session["clid"] = num;
                  var request = Request;
                  var ip = string.Empty;
                  var page = string.Empty;
                  var date = DateTime.Now;
                  if (request != null)
                  {
                      ip = request.UserHostAddress;
                      page = (request.UrlReferrer != null) ? request.UrlReferrer.PathAndQuery : "url is empty";
                      dataService.AddIPToMonitor(new IPMonitor(){date = date,ip = ip,page = page,clientId = num});
                     
                  }
              }
              catch (Exception)
              {

              }
          }

    }
}
