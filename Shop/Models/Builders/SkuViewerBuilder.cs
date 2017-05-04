﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Shop.DataService;
using Shop.db.Entities;
using Shop.db.Repository;
using Shop.Modules;
using WebMatrix.WebData;

namespace Shop.Models.Builders
{

    public interface ISkuViewerBuilder
    {
        SkuViewerModel Build(/*long idCat, int sort*/FilterFoDb filters);
        SKUModel BuildSkuModel(long idSlu);
        List<ShortSKUModel> ListHiddenSku(bool isHide);
        SkuViewerModel BuildHidden(bool isHide, int sort);
        List<ShortSKUModel> BuildListProductsByFilters(FilterFoDb filters);
        List<ShortSKUModel> BuildListProductsByIds(long[] ids);
        SkuViewerModel BuildAdmin();

    }

    public class SkuViewerBuilder : MenuBuilder, ISkuViewerBuilder
    {
        private IImagesPath imagesPath;
        private ISKUModelBuilder SKUModelBuilder;
        private IAccountAdminModelBuilder AccountAdminModelBuilder;
        public long DefaultValueHasInStock { set; get; }

        public SkuViewerBuilder(IDataService dataService,

            IImagesPath imagesPath,
            ISKUModelBuilder iSKUModelBuilder,
            IAccountAdminModelBuilder iAccountAdminModelBuilder)
            : base(dataService, imagesPath)
        {
            this.imagesPath = imagesPath;
            this.SKUModelBuilder = iSKUModelBuilder;
            this.AccountAdminModelBuilder = iAccountAdminModelBuilder;
            DefaultValueHasInStock = long.Parse(WebConfigurationManager.AppSettings["DefaultValueHasInStock"]);
        }

        //private IEnumerable<ShortSKUModel> ListSkuByCategory(StaticCategory cat)
        //{

        //    return dataService.ListSkuByCategory(cat).OrderBy(sku => sku.sortPriority).Select(sku => new ShortSKUModel()
        //        {
        //            id = sku.id,
        //            articul = sku.articul,
        //            price = sku.chemodanType.priceDay,
        //            smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (sku.smalPhoto ?? new Photo() { path = "box.png" }).path)
        //        }); 
        //}

        public List<ShortSKUModel> ListHiddenSku(bool isHide)
        {

            return dataService.AllHiddenSku(isHide).OrderBy(sku => sku.sortPriority).Select(item => new ShortSKUModel()
                {
                id = item.id,
                articul = item.articul,
                price = item.chemodanType.priceDay,
                name = item.name,
                category = item.chemodanType.name,
                fullPrice = item.price,
                maxCount = item.listChemodanTracking.Count(i => i.Location.id == DefaultValueHasInStock),
                smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (item.smalPhoto ?? new Photo() { path = "box.png" }).path)
                }).ToList(); 
        }


        public SkuViewerModel Build(/*long idCat, int sort*/FilterFoDb filters)
        {
            var model = new SkuViewerModel();
            var cat = dataService.Get<StaticCategory>(filters.Categories[0].Id);
            if (cat!=null)
            {
                model.IdCat = cat.id;
                model.Name=cat.name;
                model.Keywords = cat.keywords;
                model.Description = cat.description;
                model.bodyText = cat.bodyText;
               // model.skuList = ListSkuByCategory(cat).ToList();
                model.skuList = BuildListProductsByFilters(filters);
            }

            //var tmpList = dataService.ListProductByDisplayType(DisplayType.Favorite);
            //if (filters.ChemodanLocationID > 0)
            //{
            //   // var location = session.Get<ChemodanLocation>(filters.ChemodanLocationID);
            //    tmpList =
            //        tmpList?.Where(sku => sku.listChemodanTracking.Any(track => track.Location.id == filters.ChemodanLocationID)).ToList();
            //}
            //if (tmpList != null && tmpList.Count > 0)
            //{
            //    model.ListProduct = tmpList.Select(item => new ShortSKUModel()
            //    {
            //        articul = item.articul,
            //        id = item.id,
            //        price = item.chemodanType.priceDay,
            //        smalPhotoPath =string.Format("{0}/{1}", imagesPath.GetImagesPath(), (item.smalPhoto ?? new Photo() { path = "box.png" }).path)
            //    }).ToList();
            //}

            model.Filters = dataService.Filters();
            foreach (var item in model.Filters.ChemodanTypes)
            {
                if (!filters.ChemodanTypes.Any(i => i.Id == item.Id)) continue;
                item.Selected = "active";
            }
            model.TitleProduct = Shop.Resources.Default.Chemodans;
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            return model;
        }

        public SkuViewerModel BuildAdmin()
        {
            var model = new SkuViewerModel();
            var cat = dataService.Get<StaticCategory>(10010);
            if (cat != null)
            {
                model.IdCat = cat.id;
                model.Name = cat.name;
                model.Keywords = cat.keywords;
                model.Description = cat.description;
                model.bodyText = cat.bodyText;
                model.ListAdminProducts = BuildListAdminProducts();
            }

            model.TitleProduct = Shop.Resources.Default.Chemodans;
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            return model;
        }

        public List<AdminSkuModel> BuildListAdminProducts()
        {
            var model = new List<AdminSkuModel>();

            var tmpList = dataService.List<Sku>();
            if (tmpList != null && tmpList.Count > 0)
            {
                model = tmpList.Select(item => new AdminSkuModel()
                {
                    Articul = item.articul,
                    Id = item.id,
                    Name = item.name,
                    SmalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (item.smalPhoto ?? new Photo() { path = "box.png" }).path),
                    Codes = item.listChemodanTracking.Select(it=>new SkuCode() {Name = it.Location.name,Code = it.Code}).ToList()
                }).ToList();
            }
            return model;
        }

        public List<ShortSKUModel> BuildListProductsByFilters(FilterFoDb filters)
        {
            var model = new List<ShortSKUModel>();

            var tmpList = dataService.ListProductsByFilters(filters);
            if (tmpList != null && tmpList.Count > 0)
            {
                model = tmpList.Select(item => new ShortSKUModel()
                {
                    articul = item.articul,
                    id = item.id,
                    price = item.chemodanType.priceDay,
                    name = item.name,
                    category = item.chemodanType.name,
                    fullPrice = item.price,
                    maxCount = item.listChemodanTracking.Count(i => i.Location.id == DefaultValueHasInStock),
                    smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (item.smalPhoto ?? new Photo() { path = "box.png" }).path)
                }).ToList();
            }
            return model;
        }

        public List<ShortSKUModel> BuildListProductsByIds(long[] ids)
        {
            var model = new List<ShortSKUModel>();

            var tmpList = dataService.ListProductsByIds(ids);
            if (tmpList != null && tmpList.Count > 0)
            {
                model = tmpList.Select(item => new ShortSKUModel()
                {
                    articul = item.articul,
                    id = item.id,
                    price = item.chemodanType.priceDay,
                    name = item.name,
                    category = item.chemodanType.name,
                    fullPrice = item.price,
                    maxCount = item.listChemodanTracking.Count(i => i.Location.id == DefaultValueHasInStock),
                smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (item.smalPhoto ?? new Photo() { path = "box.png" }).path)
                }).ToList();
            }
            return model;
        }

        public SkuViewerModel BuildHidden(bool isHide, int sort)
        {
            var model = new SkuViewerModel
            {
                IdCat = 9999999,
                Name = "Скрытые товары",
                Keywords = string.Empty,
                Description = string.Empty,
                bodyText = string.Empty,
                skuList = ListHiddenSku(isHide),
                menu = BuildMenu(),
                topMenuItems = BuildTopMenu()
            };

            return model;
        }
       

        public SKUModel BuildSkuModel(long idSku)
        {
            var skuModel = new SKUModel();
            var sku = dataService.Get<Sku>(idSku);
            if (sku!=null)
            {     
                 skuModel = SKUModelBuilder.ConvertSkuBDToSkuModel(sku);
            }

            var tmpList = dataService.ListProductByDisplayType(DisplayType.Favorite);
            var DefaultValueHasInStock = long.Parse(WebConfigurationManager.AppSettings["DefaultValueHasInStock"]);
            if (DefaultValueHasInStock > 0)
            {
                // var location = session.Get<ChemodanLocation>(filters.ChemodanLocationID);
                tmpList =
                    tmpList?.Where(skuItem => skuItem.listChemodanTracking.Any(track => track.Location.id == DefaultValueHasInStock)).ToList();
            }
            if (tmpList != null && tmpList.Count > 0)
            {
                skuModel.ListProduct = tmpList.Select(item => new ShortSKUModel()
                {
                    articul = item.articul,
                    id = item.id,
                    price = item.chemodanType.priceDay,
                    category = item.chemodanType.name,
                    name = item.name,
                    fullPrice = item.price,
                    smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (item.smalPhoto ?? new Photo() { path = "box.png" }).path)
                }).ToList();
            }


            skuModel.TitleProduct = Shop.Resources.Default.FavoriteProducts;
            skuModel.menu = BuildMenu();
            skuModel.topMenuItems = BuildTopMenu();
            return skuModel;
        }
    }
}