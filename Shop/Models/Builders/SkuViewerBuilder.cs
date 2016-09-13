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

    public interface ISkuViewerBuilder
    {
        SkuViewerModel Build(long idCat, int sort);
        SKUModel BuildSkuModel(long idSlu);
        List<ShortSKUModel> ListHiddenSku(bool isHide);
        SkuViewerModel BuildHidden(bool isHide, int sort);
    }

    public class SkuViewerBuilder : MenuBuilder, ISkuViewerBuilder
    {
        private IImagesPath imagesPath;
        private ISKUModelBuilder SKUModelBuilder;
        private IAccountAdminModelBuilder AccountAdminModelBuilder;
        public SkuViewerBuilder(IDataService dataService, IImagesPath imagesPath, ISKUModelBuilder iSKUModelBuilder, IAccountAdminModelBuilder iAccountAdminModelBuilder)
            : base(dataService, imagesPath)
        {
            this.imagesPath = imagesPath;
            this.SKUModelBuilder = iSKUModelBuilder;
            this.AccountAdminModelBuilder = iAccountAdminModelBuilder;
        }

        private IEnumerable<ShortSKUModel> ListSkuByCategory(StaticCategory cat)
        {
            var u = AccountAdminModelBuilder.BuildOneUser(WebSecurity.CurrentUserName);
            if (u!=null&&u.Discount>0)
            {
                return dataService.ListSkuByCategory(cat).OrderBy(sku=>sku.sortPriority).Select(sku => new ShortSKUModel()
                {
                    id = sku.id,
                    brandId = sku.brand.id,
                    brandName = sku.brand.name,
                    categotyId = cat.id,
                    categotyName = cat.name,
                    description = sku.description,
                    name = sku.name,
                    price = sku.price,
                    priceAct = sku.priceAct-((sku.priceAct / 100) * u.Discount),
                    smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (sku.smalPhoto ?? new Photo() { path = "box.png" }).path)
                });
            }

            return dataService.ListSkuByCategory(cat).OrderBy(sku => sku.sortPriority).Select(sku => new ShortSKUModel()
                {
                    id = sku.id,
                    brandId = sku.brand.id,
                    brandName = sku.brand.name,
                    categotyId = cat.id,
                    categotyName = cat.name,
                    description = sku.description,
                    name = sku.name,
                    price = sku.price,
                    priceAct = sku.priceAct,
                    smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (sku.smalPhoto ?? new Photo() { path = "box.png" }).path)
                }); 
        }

        public List<ShortSKUModel> ListHiddenSku(bool isHide)
        {
            var u = AccountAdminModelBuilder.BuildOneUser(WebSecurity.CurrentUserName);
            if (u!=null&&u.Discount>0)
            {
                return dataService.AllHiddenSku(isHide).OrderBy(sku=>sku.sortPriority).Select(sku => new ShortSKUModel()
                {
                    id = sku.id,
                    brandId = sku.brand.id,
                    brandName = sku.brand.name,
                    categotyId = -1,
                    categotyName = string.Empty,
                    description = sku.description,
                    name = sku.name,
                    price = sku.price,
                    priceAct = sku.priceAct-((sku.priceAct / 100) * u.Discount),
                    smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (sku.smalPhoto ?? new Photo() { path = "box.png" }).path)
                }).ToList();
            }

            return dataService.AllHiddenSku(isHide).OrderBy(sku => sku.sortPriority).Select(sku => new ShortSKUModel()
                {
                    id = sku.id,
                    brandId = sku.brand.id,
                    brandName = sku.brand.name,
                    categotyId = -1,
                    categotyName =string.Empty,
                    description = sku.description,
                    name = sku.name,
                    price = sku.price,
                    priceAct = sku.priceAct,
                    smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (sku.smalPhoto ?? new Photo() { path = "box.png" }).path)
                }).ToList(); 
        }


        public SkuViewerModel Build(long idCat, int sort)
        {
            var model = new SkuViewerModel();
            var cat = dataService.GetStaticCategoryById(idCat);
            if (cat!=null)
            {
                model.IdCat = cat.id;
                model.Name=cat.name;
                model.Keywords = cat.keywords;
                model.Description = cat.description;
                model.bodyText = cat.bodyText;
                model.skuList = SortListSku(ListSkuByCategory(cat).ToList(), sort);
            }
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            model.caruselItems = BuildCarusel();
            return model;
        }

        public SkuViewerModel BuildHidden(bool isHide, int sort)
        {
            var model = new SkuViewerModel();
            
            model.IdCat = 9999999;
            model.Name = "Скрытые товары";
            model.Keywords = string.Empty;
            model.Description = string.Empty;
            model.bodyText = string.Empty;
            model.skuList = SortListSku(ListHiddenSku(isHide),sort);
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            model.caruselItems = BuildCarusel();
            return model;
        }

        private List<ShortSKUModel> SortListSku(List<ShortSKUModel> list, int sort)
        {
            switch (sort)
            {
                case 1://сорт от А до Я
                    return list.OrderBy(it => it.name).ToList();
                case 2://сорт от Я до А
                    return list.OrderByDescending(it => it.name).ToList();
                case 3://сорт по цене Возростание
                    return list.OrderBy(it => it.priceAct).ToList();
                case 4://сорт по цене Убывание
                    return list.OrderByDescending(it => it.priceAct).ToList();
                default:
                    return list;
            }
        }
       

        public SKUModel BuildSkuModel(long idSku)
        {
            var skuModel = new SKUModel();
            var sku = dataService.GetSkuById(idSku);
            if (sku!=null)
            {     
                 var u = AccountAdminModelBuilder.BuildOneUser(WebSecurity.CurrentUserName);
                 skuModel = SKUModelBuilder.ConvertSkuBDToSkuModel(sku);
                if (u != null && u.Discount > 0)
                {
                    skuModel.priceAct = skuModel.priceAct - ((skuModel.priceAct/100)*u.Discount);
                }
    
            }
            skuModel.menu = BuildMenu();
            skuModel.topMenuItems = BuildTopMenu();
            return skuModel;
        }
    }
}