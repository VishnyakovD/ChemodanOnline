using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.DataService;
using Shop.db.Entities;
using Shop.db.Repository;
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
        List<ShortSKUModel> BuildListProductsByFilters(FilterFoDB filters);

    }

    public class SkuViewerBuilder : MenuBuilder, ISkuViewerBuilder
    {
        private IImagesPath imagesPath;
        private ISKUModelBuilder SKUModelBuilder;
        private IAccountAdminModelBuilder AccountAdminModelBuilder;
        public SkuViewerBuilder(IDataService dataService,
            IImagesPath imagesPath,
            ISKUModelBuilder iSKUModelBuilder,
            IAccountAdminModelBuilder iAccountAdminModelBuilder)
            : base(dataService, imagesPath)
        {
            this.imagesPath = imagesPath;
            this.SKUModelBuilder = iSKUModelBuilder;
            this.AccountAdminModelBuilder = iAccountAdminModelBuilder;
        }

        private IEnumerable<ShortSKUModel> ListSkuByCategory(StaticCategory cat)
        {

            return dataService.ListSkuByCategory(cat).OrderBy(sku => sku.sortPriority).Select(sku => new ShortSKUModel()
                {
                    id = sku.id,
                    articul = sku.articul,
                    price = sku.chemodanType.priceDay,
                    smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (sku.smalPhoto ?? new Photo() { path = "box.png" }).path)
                }); 
        }

        public List<ShortSKUModel> ListHiddenSku(bool isHide)
        {

            return dataService.AllHiddenSku(isHide).OrderBy(sku => sku.sortPriority).Select(sku => new ShortSKUModel()
                {
                    id = sku.id,
                    articul = sku.articul,
                    price = sku.chemodanType.priceDay,
                    smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (sku.smalPhoto ?? new Photo() { path = "box.png" }).path)
                }).ToList(); 
        }


        public SkuViewerModel Build(long idCat, int sort)
        {
            var model = new SkuViewerModel();
            var cat = dataService.Get<StaticCategory>(idCat);
            if (cat!=null)
            {
                model.IdCat = cat.id;
                model.Name=cat.name;
                model.Keywords = cat.keywords;
                model.Description = cat.description;
                model.bodyText = cat.bodyText;
                model.skuList = SortListSku(ListSkuByCategory(cat).ToList(), sort);
            }

            var tmpList = dataService.ListProductByDisplayType(DisplayType.Favorite);
            if (tmpList != null && tmpList.Count > 0)
            {
                model.ListProduct = tmpList.Select(item => new ShortSKUModel()
                {
                    articul = item.articul,
                    id = item.id,
                    price = item.chemodanType.priceDay,
                    smalPhotoPath = imagesPath.GetImagesPath() + item.smalPhoto.path
                }).ToList();
            }

            model.Filters = dataService.Filters();
            model.TitleProduct = Shop.Resources.Default.FavoriteProducts;
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            return model;
        }

    public List<ShortSKUModel> BuildListProductsByFilters(FilterFoDB filters)
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
                    smalPhotoPath = imagesPath.GetImagesPath() + item.smalPhoto.path
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
                skuList = SortListSku(ListHiddenSku(isHide), sort),
                menu = BuildMenu(),
                topMenuItems = BuildTopMenu()
            };

            return model;
        }

        private List<ShortSKUModel> SortListSku(List<ShortSKUModel> list, int sort)
        {
            switch (sort)
            {
                case 1://сорт от А до Я
                  //  return list.OrderBy(it => it.name).ToList();
                case 2://сорт от Я до А
                   // return list.OrderByDescending(it => it.name).ToList();
                case 3://сорт по цене Возростание
                   // return list.OrderBy(it => it.priceAct).ToList();
                case 4://сорт по цене Убывание
                   // return list.OrderByDescending(it => it.priceAct).ToList();
                default:
                    return list;
            }
        }
       

        public SKUModel BuildSkuModel(long idSku)
        {
            var skuModel = new SKUModel();
            var sku = dataService.Get<Sku>(idSku);
            if (sku!=null)
            {     
                // var u = AccountAdminModelBuilder.BuildOneUser(WebSecurity.CurrentUserName);
                 skuModel = SKUModelBuilder.ConvertSkuBDToSkuModel(sku);
            }
            skuModel.menu = BuildMenu();
            skuModel.topMenuItems = BuildTopMenu();
            return skuModel;
        }
    }
}