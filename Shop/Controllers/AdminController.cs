using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Shop.DataService;
using Shop.db.Entities;
using Shop.db.Repository;
using Shop.Logger;
using Shop.Models;
using Shop.Models.Builders;
using Shop.Modules;
using Shop.Filters;
using MenuItem = Shop.db.Entities.MenuItem;

namespace Shop.Controllers
{
    [Authorize(Roles = "Admin")]
    [InitializeSimpleMembership]
    public class AdminController : BaseController
    {
        private MenuBuilder menuBuilder { set; get; }
        private IArticleBuilder articleBuilder { set; get; }
        private ISkuViewerBuilder skuViewerBuilder { set; get; }
        private IDataService dataService { set; get; }
        private IClientModelBuilder clientModelBuilder { set; get; }
        private IMainPageBuilder mainPageBuilder { set; get; }

        public AdminController(
            ILogger logger,
            IAdminModelBuilder adminModelBuilder,
            IDataService dataService,
            IImagesPath imagesPath,
            ISKUModelBuilder SKUModelBuilder,
            IArticleBuilder ArticleBuilder,
            ISkuViewerBuilder SkuViewerBuilder,
            IClientModelBuilder clientModelBuilder,
            IMainPageBuilder mainPageBuilder
            )
            : base(logger, adminModelBuilder, dataService, imagesPath, SKUModelBuilder)
        {
            menuBuilder = new MenuBuilder(dataService,imagesPath);
            articleBuilder = ArticleBuilder;
            skuViewerBuilder = SkuViewerBuilder;
            this.dataService = dataService;
            this.clientModelBuilder = clientModelBuilder;
            this.mainPageBuilder = mainPageBuilder;
        }

        public ActionResult Administrator()
        {
            var model = adminModelBuilder.Build();
            return View("Administrator",model);
        }

  
        public ActionResult AddOrUpdateBrand(Brand brand)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (dataService.AddOrUpdateBrand(brand))
                    {
                        return Content("Бренд сохранен", "text/html");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Бренд НЕ сохранен " + message, "text/html");  
        }

        public ActionResult ShowBrand(long idBrand = -1)
        {
            var model = new BrandModel();
            try
            {
              var modelDB = dataService.Get<Brand>(idBrand);
              if (modelDB!=null)
                {
                    model.id = modelDB.id;
                    model.name = modelDB.name;
                }
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }

            return PartialView("BrandDataPartial", model);
        }

        [HttpPost]
        public ActionResult AddOrUpdateCategory(Category category)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (dataService.AddOrUpdateCategory(category))
                    {
                        return Content("Категория сохранена", "text/html");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Категория НЕ сохранена " + message, "text/html");
        }

        public ActionResult ListBrands()
        {
            List<BrandModel> brands = null;
            try
            {
                brands = dataService.List<Brand>().Select(br=>new BrandModel(){id=br.id,name = br.name}).ToList();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListBrandsPartial", brands);
        }
   
        public ActionResult AddOrUpdateStaticCategory(StaticCategory category)
        {
             var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    category.bodyText = ConvertToHTMLString(category.bodyText);
                    if (dataService.AddOrUpdateStaticCategory(category))
                    {
                        return Content("Категория сохранена", "text/html");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Категория НЕ сохранена " + message, "text/html");
        }

        public ActionResult ShowStaticCategory(long idCat=-1)
        {
            var model = new StaticCategory();
            try
            {
                model = dataService.Get<StaticCategory>(idCat);
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }

            return PartialView("StaticCategoryDataPartial", model);
        }

        public ActionResult ListStaticCategoryes()
        {
            List<StaticCategory> categoryes = null;
            try
            {
                categoryes = dataService.List<StaticCategory>();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListStaticCategoryesPartial", categoryes);
        }

        public ActionResult ListStaticSpecification()
        {
            List<StaticSpecification> specifications = null;
            try
            {
                specifications = dataService.ListStaticSpecification();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListStaticSpecificationPartial", specifications);
        }

        public ActionResult ListMenuItem()
        {
            List<Shop.db.Entities.MenuItem> menuItems = null;
            try
            {
                menuItems = dataService.ListMenuItem(-1);
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListMenuItemPartial", menuItems);
        }

        public ActionResult ListProductLocation()
        {
            List<ChemodanLocation> model = null;
            try
            {
                model = dataService.List<ChemodanLocation>();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListProductLocationPartial", model);
        }

        [HttpPost]
        public ActionResult AddOrUpdateMenuItem(Shop.db.Entities.MenuItem menuItem)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (dataService.AddOrUpdateMenuItem(menuItem)>0)
                    {
                        return Content("Меню сохранено", "text/html");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Меню НЕ сохранено " + message, "text/html");
        }

        public ActionResult ShowMenuItem(long idMenuItem=-1)
        {
            var model = new MenuItem();
            try
            {
                  model=dataService.Get<MenuItem>(idMenuItem);
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }

            return PartialView("MenuItemDataPartial", model);
        }

        public ActionResult RemoveMenuItem(long idMenuItem = -1)
        {
            try
            {
                if (dataService.RemoveMenuItem(idMenuItem))
                {
                    return Content("Пункт меню удален ", "text/html");
                }
            }
            catch (Exception err)
            {
                return Content("Ошибка : " + err.Message, "text/html");
            }

            return Content("Пункт НЕ меню удален ", "text/html");
        }

        [HttpPost]
        public ActionResult AddOrUpdateStaticSpecification(StaticSpecification spec)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (dataService.AddOrUpdateStaticSpecification(spec))
                    {
                        return Content("Спецификация сохранена", "text/html");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Спецификация НЕ сохранена " + message, "text/html");
        }

        public ActionResult ShowSpec(long idSpec = -1)
        {
            var model = new StaticSpecification();
            try
            {
                model = dataService.Get<StaticSpecification>(idSpec);
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }

            return PartialView("StaticSpecificationDataPartial", model);
        }

        public ActionResult SkuData(long? id)
        {
            var result = new SKUModel();
       
            try
            {
                if (id.HasValue)
                {
                    var skuDB = dataService.Get<Sku>(id.Value);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    result = skuModelBuilder.GetEmptySku();
                }
                result.menu = menuBuilder.BuildMenu();
                result.topMenuItems = menuBuilder.BuildTopMenu();
             
 
            }
            catch (Exception err)
            {
                return View("MessagesPartial", "Ошибка " + err.Message);
            }

            return View("SkuData", result);
        }

        [HttpPost]
        public ActionResult AddOrUpdateSku(SKUModel sku)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                   // sku.care = ConvertToHTMLString(sku.care);
                   //sku.description = ConvertToHTMLString(sku.description);
                    var id = dataService.AddOrUpdateSku(sku.GetSKUDB());
                   
                    if (id >0)
                    {
                        return RedirectToAction("SkuData",  new {id=id});
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Товар НЕ сохранен " + message, "text/html");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadSmalPhoto(long id, HttpPostedFileBase smalPhotoFile)
        {
            try
            {
                if (smalPhotoFile != null)
                {
                        if (smalPhotoFile.ContentLength > 0)
                        {
                            var path = UploadPhoto(id, smalPhotoFile);
                            if (!string.IsNullOrEmpty(path))
                            {
                                var pho=new Photo(){name = string.Empty, path = path, skuId = id};
                                if (dataService.AddSmalPhotoToSku(id, pho))
                                {
                                    return RedirectToAction("SkuData", "Admin", new {id=id}); 
                                }
                            }
                        }
                }
                else
                {
                    return Content("Фото НЕ сохранено ", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content("Фото НЕ сохранено " + err, "text/html");
            }

            return RedirectToAction("SkuData", "Admin", new { id = id }); 
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadBigPhoto(long id, HttpPostedFileBase photoFile)
        {
            try
            {
                if (photoFile != null)
                {
                    if (photoFile.ContentLength > 0)
                    {
                        var path = UploadPhoto(id, photoFile);
                        if (!string.IsNullOrEmpty(path))
                        {
                            var pho=new PhotoBig(){name = string.Empty, path = path, skuId = id};
                            if (dataService.AddBigPhotoToSku(id, pho))
                            {
                                return RedirectToAction("SkuData", "Admin", new {id=id}); 
                            }
                        }
                    }
                }
                else
                {
                    return Content("Фото НЕ сохранено ", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content("Фото НЕ сохранено " + err, "text/html");
            }

            return RedirectToAction("SkuData", "Admin", new { id = id }); 
        }

        public ActionResult RemoveBigPhotoFromSKU(long idSku, long idPhoto)
        {
            SKUModel result = null;
            try
            {
                if (dataService.RemoveBigPhotoFromSKU(idSku, idPhoto))
                {
                    var skuDB = dataService.Get<Sku>(idSku);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("Фото НЕ удалено", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Фото НЕ удалено " + err, "text/html");
            }

            return PartialView("SkuListPhotosPartial", result.listPhoto);
        }

        private string UploadPhoto(long id, HttpPostedFileBase file, string type="")
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FileName.Trim(' ').Replace(' ', '_'));//имя файла
            var fileExt = Path.GetExtension(file.FileName);//расширение файла
            var path = string.Empty;
            if (string.IsNullOrEmpty(type))
            {
                path = string.Format(@"{0}\{1}", Server.MapPath("~"+imagesPath.GetImagesPath()), id);
            }
            else
            {
                path = string.Format(@"{0}\{1}", Server.MapPath("~" + imagesPath.GetImagesPath()+@"\"+ type), id);
            }
            var ticks = DateTime.Now.Ticks;
            if (Directory.Exists(path))
            {
                path = Path.Combine(path, string.Format("{0}-{1}{2}", fileName, ticks, fileExt));
            }
            else
            {
                /*DirectoryInfo di =*/ Directory.CreateDirectory(path);
                if (Directory.Exists(path))
                {
                    path = Path.Combine(path, string.Format("{0}-{1}{2}", fileName, ticks, fileExt));
                }
                else
                {
                    return string.Empty;
                }
            }
            file.SaveAs(path);
            if (string.IsNullOrEmpty(type))
            {
                return string.Format(@"{0}/{1}", id, string.Format("{0}-{1}{2}", fileName, ticks, fileExt));
            }
            return string.Format(@"{2}/{0}/{1}", id, string.Format("{0}-{1}{2}", fileName, ticks, fileExt), type);
        }

        public ActionResult AddSKUFromCategory(long idSku, long catId)
        {
            SKUModel result = null;
            try
            {
                if (dataService.AddCategoryToSku(idSku, catId))
                {
                    var skuDB = dataService.Get<Sku>(idSku);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("Категория НЕ добавлена", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content("Категория НЕ добавлена " + err, "text/html");
            }

            return PartialView("SkuListCategoriesPartial", result.listCategory); 
        }

        public ActionResult RemoveSKUFromCategory(long idSku, long idCat)
        {
            SKUModel result = null;
            try
            {
                if (dataService.RemoveSKUFromCategory(idSku, idCat))
                {
                    var skuDB = dataService.Get<Sku>(idSku);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("Категория НЕ удалена", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content("Категория НЕ удалена " + err, "text/html");
            }

            return PartialView("SkuListCategoriesPartial", result.listCategory); 
        }

        public ActionResult ShowSkuSpecificatoin(long skuId, long specId=-1)
        {
            var model = new SkuSpecificatoinModel();
            try
            {
                var sku = dataService.Get<Sku>(skuId);
                if (sku==null)
                {
                    return Content("Ошибка загрузки: нет такого товара", "text/html"); 
                }
                model.skuId = skuId;

                Specification spec = null;
                if (specId>0 && sku.listSpecification != null && sku.listSpecification.Any())
                {
                   spec = sku.listSpecification.First(sp => sp.staticspec.id == specId); 
                }
                
                if (spec!=null)
                {
                    model.specId = spec.staticspec.id;
                    model.specValue = spec.value;
                    model.listStaticSpecification = new List<StaticSpecification>() { new StaticSpecification() { id = spec.staticspec.id, name = spec.staticspec.name} };
                }
                else
                {
                    model.listStaticSpecification = dataService.ListStaticSpecification(); 
                }
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }
            return PartialView("SkuSpecificatoinDataPartial", model);
        }

        public ActionResult AddOrUpdateSpecificationToSku(long skuId, long specId, string specValue)
        {
            SKUModel result = null;
            try
            {
                if (dataService.AddOrUpdateSpecificationToSKU(skuId, specId, specValue))
                {
                    var skuDB = dataService.Get<Sku>(skuId);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("Спецификация НЕ добавлена", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Спецификация НЕ добавлена " + err, "text/html");
            }

            return PartialView("SkuListSpecificationPartial", result.listSpecification);
        }

        public ActionResult RemoveSpecificationFromSKU(long idSku, long idSpec)
        {
            SKUModel result = null;
            try
            {
                if (dataService.RemoveSpecificationFromSKU(idSku, idSpec))
                {
                    var skuDB = dataService.Get<Sku>(idSku);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("Спецификация НЕ удалена", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content("Спецификация НЕ удалена " + err, "text/html");
            }

            return PartialView("SkuListSpecificationPartial", result.listSpecification);
        }

        public ActionResult ListArticles()
        {
            var model = new ArticlesModel();
            try
            {
                model = articleBuilder.Build();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListArticles", model);
        }

        [HttpPost]
        public ActionResult AddOrUpdateArticle(Article article, HttpPostedFileBase photoFile)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(article.imgPath))
                    {
                        article.imgPath = string.Empty;
                    }
                    article.imgPath = article.imgPath.Replace(imagesPath.GetImagesPath() + "/", "");
                    article.body = ConvertToHTMLString(article.body);
                    var id = dataService.AddOrUpdateArticle(article);
                    if (id > 0)
                    {
                        if (photoFile != null&&photoFile.ContentLength > 0)
                        {
                            var path = UploadPhoto(id, photoFile, "article");
                                if (!dataService.AddPhotoToArticle(id, path))
                                {
                                    message = "Фото НЕ добавлено";
                                }
                        }
                        return RedirectToAction("ListArticles");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Статья НЕ сохранена " + message, "text/html");
        }

        public ActionResult ArticleData(long? idArticle)
        {
            var result = new ArticleModel();
            try
            {
                result.menu = menuBuilder.BuildMenu();
                result.topMenuItems = menuBuilder.BuildTopMenu();
                if (idArticle.HasValue && idArticle.Value > 0)
                {
                    var articleDB = dataService.Get<Article>(idArticle.Value);
                    articleDB.imgPath = imagesPath.GetImagesPath() +"/" +articleDB.imgPath;
                    result.article = articleDB;
                }
                else
                {
                    result.article = new Article();
                }

            }
            catch (Exception err)
            {
                return View("MessagesPartial", "Ошибка " + err.Message);
            }
      
            return View("ArticleData", result);
        }

        public ActionResult HidenSku(int? sort)
        {

            if (!sort.HasValue)
            {
                sort = -1;
            }
            Session["sort"] = sort;
            var model = skuViewerBuilder.BuildHidden(true,sort.Value);
            return View("~/Views/Home/ListSkuOnCategory.cshtml", model);
        }

        public ActionResult ShowChemodanStatus(long id = -1)
        {
            var model = new ChemodanStatus();
            try
            {
                model = dataService.Get<ChemodanStatus>(id);
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }

            return PartialView("ChemodanStatusDataPartial", model);
        }

        public ActionResult AddOrUpdateChemodanStatus(ChemodanStatus сhemodanStatus)
        {
            try
            {
                if (сhemodanStatus!=null)
                {
                        dataService.AddOrUpdateChemodanStatus(сhemodanStatus);
                    return Content("Статус сохранен", "text/html");
                }
            }
            catch (Exception err)
            {
                return Content("Статус сохранен->"+err.Message, "text/html");
            }
            return Content("Статус НЕ сохранен", "text/html");
        }

        public ActionResult ListChemodanStatus()
        {
            List<ChemodanStatus> list = null;
            try
            {
				//4444
                //list = dataService.ListChemodanStatus();
				list = dataService.List<ChemodanStatus>();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListChemodanStatusPartial", list);
        }


        public ActionResult ShowChemodanType(long id = -1)
        {
            var model = new ChemodanType();
            try
            {
                model = dataService.Get<ChemodanType>(id);
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }

            return PartialView("ChemodanTypeDataPartial", model);
        }

        public ActionResult AddOrUpdateChemodanType(ChemodanType chemodanType)
        {
            try
            {
                if (chemodanType != null)
                {
                        dataService.AddOrUpdateChemodanType(chemodanType);
                    return Content("Тип сохранен", "text/html");
                }
            }
            catch (Exception err)
            {
                return Content("Тип сохранен->"+err.Message, "text/html");
            }
            return Content("Тип НЕ сохранен", "text/html");
        }

        public ActionResult ListChemodanType()
        {
            List<ChemodanType> list = null;
            try
            {
				//4444
               // list = dataService.ListChemodanType();
				list = dataService.List<ChemodanType>();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListChemodanTypePartial", list);
        }


        public ActionResult ListProvider(bool? isActive)
        {
            var list = new ProviderModel();
            try
            {
                list.list = dataService.List<ChemodanProvider>();
                list.menu = menuBuilder.BuildMenu();
                list.topMenuItems = menuBuilder.BuildTopMenu();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return View("ListProvider", list);
        }

        public ActionResult ShowProvider(long id = -1)
        {
            var model = new ProviderDataModel();
            try
            {
                var modelDB = dataService.Get<ChemodanProvider>(id);
                model.provider = modelDB != null ? modelDB : new ChemodanProvider();
                if (model.provider.editAdress==null)
                {
                    model.provider.editAdress=new EditAdress();
                }
                model.menu = menuBuilder.BuildMenu();
                model.topMenuItems = menuBuilder.BuildTopMenu();
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }

            return View("ProviderData", model);
        }

        public ActionResult AddOrUpdateProvider(ChemodanProvider provider, EditAdress adress)
        {
            try
            {
                if (provider != null)
                {
                    provider.editAdress = adress;
                    provider.editAdress.id= 0;
                    var id= dataService.AddOrUpdateChemodanProvider(provider);
                    var result = new {id = id, message = "Сохранено"};
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "НЕ сохранено" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListClients(bool? isActive)
        {
            var list = new ClientModel();
            try
            {
                list.list = dataService.List<Client>();
                list.menu = menuBuilder.BuildMenu();
                list.topMenuItems = menuBuilder.BuildTopMenu();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return View("ListClients", list);
        }

        public ActionResult ShowClient(long id = -1)
        {

            ClientDataModel model = null;
            try
            {
                model = clientModelBuilder.Build(id);
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }

            return View("ClientData", model);
        }

        public ActionResult AddOrUpdateClient(Client client, EditAdress adress, long sexId)
        {
            try
            {
                if (client != null)
                {
                    client.editAdress = adress;
                    client.editAdress.id = 0;
                    client.sex=new Sex();
                    client.sex.id = sexId;
                    var id = dataService.AddOrUpdateClient(client);
                    var result = new { id = id, message = "Сохранено" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "НЕ сохранено" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MainPageConfigurator()
        {
            var model = mainPageBuilder.Build();
            return View("MainPageConfigurator", model);
        }


        public ActionResult ShowInfoBlockItem(long id, DisplayType type)
        {
            
            InfoBlockItem model = null;
            try
            {
                model = dataService.Get<InfoBlockItem>(id);
                if (model==null)
                {
                    model = new InfoBlockItem();
                    model.id =-1;
                    model.DisplayType = type;
                }
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }

            return PartialView("InfoBlockDataPartial", model);
        }

        [HttpPost]
        public ActionResult AddOrUpdateInfoBlock(InfoBlockItem iblock, HttpPostedFileBase ImageBody)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(iblock.Image))
                    {
                        iblock.Image = string.Empty;
                    }
                    iblock.Image = iblock.Image.Replace(imagesPath.GetImagesPath() + "/", "");

                    var id = dataService.AddOrUpdateInfoBlockItem(iblock);
                    if (id > 0)
                    {
                        if (ImageBody != null && ImageBody.ContentLength > 0)
                        {
                            var path = UploadPhoto(id, ImageBody, iblock.DisplayType.ToString());
                            iblock.Image = path;
                            if (dataService.AddOrUpdateInfoBlockItem(iblock)>0)
                            {
                                message = "фото НЕ добавлено";
                            }
                        }
                        return RedirectToAction("MainPageConfigurator");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Ошибка: " + message, "text/html");
        }

        public ActionResult ShowChemodanLocation(long id = -1)
        {
            var model = new ChemodanLocation();
            try
            {
                model = dataService.Get<ChemodanLocation>(id);
            }
            catch (Exception err)
            {
                return Content("Ошибка загрузки: " + err.Message, "text/html");
            }

            return PartialView("ChemodanLocationDataPartial", model);
        }

        [HttpPost]
        public ActionResult AddOrUpdateChemodanLocation(ChemodanLocation chemodanLocation)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (dataService.AddOrUpdateChemodanLocation(chemodanLocation) > 0)
                    {
                        return Content(Resources.Default.Saved, "text/html");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content($"{Resources.Default.NotSaved} {message}", "text/html");
        }

        public ActionResult ShowChemodanTracking(long Id=-1, long skuId=-1)
        {

            if (Id<1&&skuId<1)
            {
                return Content("Ошибка : нет такого товара", "text/html");
            }

            var model = new ChemodanTrackingModel();
            model.skuId = skuId;
            try
            {
                if (Id < 1 && skuId > 0)
                {
                    
                }
                else
                {
                    var item = dataService.Get<ChemodanTracking>(Id);
                    model.Id = item.Id;
                    model.Code = item.Code;
                    model.Locatoin = item.Location;
                    model.skuId = item.skuId;
                }
                model.ListChemodanLocation = dataService.List<ChemodanLocation>();
                
            }
            catch (Exception err)
            {
                return Content("Ошибка : " + err.Message, "text/html");
            }
            return PartialView("SkuChemodanTrackingDataPartial", model);
        }


        public ActionResult SetChemodanTrackingToSku(ChemodanTrackingModel model)
        {
            SKUModel result = null;
            try
            {
                var item = new ChemodanTracking();
                item.Code = model.Code;
                item.Id = model.Id;
                item.Location = model.Locatoin;
                item.skuId = model.skuId;

                if (dataService.SetChemodanTrackingToSku(item))
                {
                    var skuDB = dataService.Get<Sku>(model.skuId);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("ошибка", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Не добавлено " + err, "text/html");
            }

            return PartialView("SkuListChemodanTrackingPartial", result.ListChemodanTracking);
        }

        public ActionResult ListSku()
        {


            var model = skuViewerBuilder.BuildAdmin();
            return View("ListSkuStatus", model);
        }
    }
}
