using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Shop.DataService;
using Shop.db.Entities;
using Shop.Modules;
using WebMatrix.WebData;

namespace Shop.Models.Builders
{
    public interface ISKUModelBuilder
    {
        SKUModel GetEmptySku();
        SKUModel ConvertSkuBDToSkuModel(Sku sku);
    }

    public class SKUModelBuilder :MenuBuilder, ISKUModelBuilder
    {
        private IAccountAdminModelBuilder AccountAdminModelBuilder;
        private IDataService dataService;
        private IImagesPath imagesPath;

        public SKUModelBuilder(IDataService dataService, IImagesPath imagesPath, IAccountAdminModelBuilder iAccountAdminModelBuilder)
            : base(dataService,imagesPath)
        {
            this.dataService = dataService;
            this.imagesPath = imagesPath;
            this.AccountAdminModelBuilder = iAccountAdminModelBuilder;
        }
        public SKUModel GetEmptySku()
        {
            var sku = new SKUModel();
            sku.listStaticCategory = dataService.ListStaticCategoryes();
            sku.listStaticSpecification = dataService.ListStaticSpecification();
            sku.listStaticBrand = dataService.ListBrands();
            sku.menu = BuildMenu(sku.listStaticCategory);
            sku.listChemodanProvider = dataService.List<ChemodanProvider>();
            sku.listChemodanStatus = dataService.List<ChemodanStatus>();
            sku.listChemodanType = dataService.List<ChemodanType>();


            sku.listCategory=new List<Category>();
            sku.listComment=new List<Comment>();
            sku.listPhoto=new List<PhotoBig>();
            sku.listSpecification=new List<Specification>();

            return sku;
        }

        public SKUModel ConvertSkuBDToSkuModel(Sku sku)
        {
           // var u = AccountAdminModelBuilder.BuildOneUser(WebSecurity.CurrentUserName);
            var skuModel = GetEmptySku();
            if (sku != null)
            {
                skuModel.id = sku.id;
                skuModel.name = sku.name;
                skuModel.price = sku.price;
                skuModel.articul = sku.articul;
                skuModel.isHide = sku.isHide;
                skuModel.care = sku.care;
                skuModel.sortPriority = sku.sortPriority;
                skuModel.priceAct = sku.priceAct;
                skuModel.displayType = sku.displayType;

                skuModel.description = sku.description;
                if (sku.brand!=null)
                {
                    skuModel.brandId = sku.brand.id;
                    skuModel.brandName = sku.brand.name;
                }
                
                if (sku.smalPhoto!=null)
                {
                    skuModel.smalPhotoId = sku.smalPhoto.id;
                    skuModel.smalPhotoPath =  string.Format("{0}/{1}",imagesPath.GetImagesPath(), sku.smalPhoto.path);
                }

                if (sku.chemodanProvider != null)
                {
                    skuModel.chemodanProviderId = sku.chemodanProvider.id;
                }
                if (sku.chemodanStatus!=null)
                {
                    skuModel.chemodanStatusId = sku.chemodanStatus.id;
                }
                if (sku.chemodanType!=null)
                {
                    skuModel.chemodanTypeId = sku.chemodanType.id;
                }
            

                skuModel.chemodanDaysRent = sku.chemodanDaysRent;

                skuModel.listCategory = sku.listCategory;
                skuModel.listSpecification = sku.listSpecification.OrderBy(sp=>sp.staticspec.sortPriority).ToList();
                skuModel.listPhoto = sku.listPhoto.Select(im => new PhotoBig() { id = im.id, name = im.name, path = string.Format("{0}/{1}", imagesPath.GetImagesPath(), im.path),skuId = im.skuId}).ToList();

            }
            return skuModel;
        }
    }
} 