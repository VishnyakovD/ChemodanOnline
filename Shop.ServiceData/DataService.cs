using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Shop.db;
using Shop.db.Entities;
using Shop.db.Repository;
using Shop.Logger;

namespace Shop.DataService
{
    public interface IDataService
    {
        List<T> List<T>() where T : class;
        T Get<T>(long id) where T : class, new();

        FilterProduct Filters();
        List<InfoBlockItem> ListInfoBlockItems(DisplayType type);
        List<Sku> ListProductsByFilters(FilterFoDb filters);
        List<Sku> ListProductsByIds(long[] ids);
        bool SetChemodanTrackingToSku(ChemodanTracking item);
        List<Order> ListOrdersByFilter(OrderFilter filter);
        List<OrderOneClick> ListOrdersOneClick(OrderFilter filter);
        List<Order> ListOrdersByPhone(string phone);
        List<Order> ListOrdersBySecondName(string name);
        List<Order> ListOrdersByOrderNumber(int num);
        //List<Order> ListOrdersByDeliveryType(long deliveryType);
        //List<Order> ListOrdersByPaymentType(long paymentType);
        //List<Order> ListOrdersByOrderState(long state);
        Order AddOrUpdateOrder(Order order);
        long AddOrUpdateOrderOneClick(OrderOneClick order);
        OrderOneClick ApplyOrderOneClick(long id, int userId, string userName);
        bool PaidOrder(int num, string paymentId, DateTime payDate);
        bool SaveOrder(Order order);
        List<ChemodanTracking> ListChemodanTrackingByProductId(long id);
        bool AddOrUpdateOrderProduct(long id, string code, string articul, long? orderId = -1);
        Order UpdateOrderNumber(long order);
        bool RemoveProductFromOrder(long orderLine);

        long AddOrUpdateChemodanLocation(ChemodanLocation chemodanLocation);
        bool AddOrUpdateMailing(Mailing mailing);
        bool AddOrUpdateBrand(Brand brand);
        bool AddOrUpdateCategory(Category category);
        bool AddOrUpdateStaticCategory(StaticCategory category);
        bool AddOrUpdateStaticSpecification(StaticSpecification spec);
        long AddOrUpdateSku(Sku sku);
        bool AddSmalPhotoToSku(long id, Photo photo);
        bool AddBigPhotoToSku(long id, PhotoBig photo);
        bool AddCategoryToSku(long id, long idCat);
        bool AddOrUpdateSpecificationToSKU(long id, long specId, string specValue);
        long AddOrUpdateMenuItem(Shop.db.Entities.MenuItem minuItem);
        long AddOrUpdateArticle(Article article);
        long AddOrUpdateInfoBlockItem(InfoBlockItem iblock);
        bool AddPhotoToArticle(long id, string path);
        bool RemoveSKUFromCategory(long id, long idCat);
        bool RemoveSpecificationFromSKU(long id, long idSpec);
        bool RemoveBigPhotoFromSKU(long id, long idPhoto);
        bool AddOrUpdateChemodanStatus(ChemodanStatus item);
        bool AddOrUpdateChemodanType(ChemodanType item);
        long AddOrUpdateChemodanProvider(ChemodanProvider item);
        long AddOrUpdateClient(Client item);
        bool RemoveMenuItem(long idMinuItem);
        List<Sku> ListProductByDisplayType(DisplayType type);
        List<Sku> AllHiddenSku(bool isHide = false);
        Mailing GetMailingByEmail(string email);
        List<Mailing> ListMailingByActive(bool isActive);
        List<StaticSpecification> ListStaticSpecification();
        List<MenuItem> ListMenuItem(int type);
        bool AddSku(Sku sku);
        void AddIPToMonitor(IPMonitor monitor);
    }

    public class DataService : IDataService
    {
        private IDb dbService;
        private ILogger logger;
        public DataService(IDb dbService, ILogger logger)
        {
            this.dbService = dbService;
            this.logger = logger;
        }

        public List<T> List<T>() where T : class
        {
            var result = new List<T>();
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<T>().All();
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public T Get<T>(long id) where T : class, new()
        {
            var result = new T();
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<T>().TryOne(id);
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public long AddOrUpdateChemodanProvider(ChemodanProvider item)
        {
            long result = -1;
            try
            {
                dbService.Run(db =>
                {
                    var itemDB = db.GetRepository<ChemodanProvider>().TryOne(item.id);
                    if (itemDB == null)
                    {
                        result = db.GetRepository<ChemodanProvider>().Add(item).id;
                    }
                    else
                    {
                        itemDB.name = item.name;
                        itemDB.lastName = item.lastName;
                        itemDB.patronymic = item.patronymic;
                        itemDB.email = item.email;
                        itemDB.mPhone = item.mPhone;
                        itemDB.passSeria = item.passSeria;
                        itemDB.passNumber = item.passNumber;
                        itemDB.passDate = item.passDate;
                        itemDB.passIssuedBy = item.passIssuedBy;
                        itemDB.fullName = item.fullName;
                        itemDB.typeOwnership = item.typeOwnership;
                        //itemDB.adress = item.adress;
                        if (itemDB.editAdress == null)
                        {
                            itemDB.editAdress = new EditAdress();
                        }
                        itemDB.editAdress.office = item.editAdress.office;
                        itemDB.editAdress.postCode = item.editAdress.postCode;
                        itemDB.editAdress.area = item.editAdress.area;
                        itemDB.editAdress.bulk = item.editAdress.bulk;
                        itemDB.editAdress.city = item.editAdress.city;
                        itemDB.editAdress.level = item.editAdress.level;
                        itemDB.editAdress.numFlat = item.editAdress.numFlat;
                        itemDB.editAdress.numHome = item.editAdress.numHome;
                        itemDB.editAdress.street = item.editAdress.street;
                        itemDB.editAdress.typeCity = item.editAdress.typeCity;
                        itemDB.editAdress.typeStreet = item.editAdress.typeStreet;
                        itemDB.isUse = item.isUse;
                        db.GetRepository<ChemodanProvider>().Update(itemDB);
                        result = item.id;
                    }
                });
            }
            catch (Exception err)
            {
                result = -1;
                logger.Error(err.Message);
            }
            return result;
        }

        public long AddOrUpdateClient(Client item)
        {
            long result = -1;
            try
            {
                dbService.Run(db =>
                {
                    var itemDB = db.GetRepository<Client>().TryOne(item.id);
                    if (itemDB == null)
                    {
                        result = db.GetRepository<Client>().Add(item).id;
                    }
                    else
                    {
                        itemDB.name = item.name;
                        itemDB.lastName = item.lastName;
                        itemDB.patronymic = item.patronymic;
                        itemDB.email = item.email;
                        itemDB.mPhone = item.mPhone;
                        itemDB.passSeria = item.passSeria;
                        itemDB.passNumber = item.passNumber;
                        itemDB.passDate = item.passDate;
                        itemDB.passIssuedBy = item.passIssuedBy;
                        itemDB.birthDate = item.birthDate;

                        if (itemDB.sex == null)
                        {
                            itemDB.sex = new Sex();
                        }
                        itemDB.sex = item.sex;

                        if (itemDB.editAdress == null)
                        {
                            itemDB.editAdress = new EditAdress();
                        }
                        itemDB.editAdress.office = item.editAdress.office;
                        itemDB.editAdress.postCode = item.editAdress.postCode;
                        itemDB.editAdress.area = item.editAdress.area;
                        itemDB.editAdress.bulk = item.editAdress.bulk;
                        itemDB.editAdress.city = item.editAdress.city;
                        itemDB.editAdress.level = item.editAdress.level;
                        itemDB.editAdress.numFlat = item.editAdress.numFlat;
                        itemDB.editAdress.numHome = item.editAdress.numHome;
                        itemDB.editAdress.street = item.editAdress.street;
                        itemDB.editAdress.typeCity = item.editAdress.typeCity;
                        itemDB.editAdress.typeStreet = item.editAdress.typeStreet;

                        itemDB.isUse = item.isUse;
                        db.GetRepository<Client>().Update(itemDB);
                        result = item.id;
                    }
                });
            }
            catch (Exception err)
            {
                result = -1;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddOrUpdateChemodanType(ChemodanType item)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var itemDB = db.GetRepository<ChemodanType>().TryOne(item.id);
                    if (itemDB == null)
                    {
                        db.GetRepository<ChemodanType>().Add(item);
                    }
                    else
                    {
                        itemDB.name = item.name;
                        itemDB.isUse = item.isUse;
                        itemDB.sortPriority = item.sortPriority;
                        itemDB.priceDay = item.priceDay;
                        db.GetRepository<ChemodanType>().Update(itemDB);
                    }
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddOrUpdateChemodanStatus(ChemodanStatus item)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var itemDB = db.GetRepository<ChemodanStatus>().TryOne(item.id);
                    if (itemDB == null)
                    {
                        db.GetRepository<ChemodanStatus>().Add(item);
                    }
                    else
                    {
                        itemDB.name = item.name;
                        itemDB.isUse = item.isUse;
                        itemDB.sortPriority = item.sortPriority;
                        db.GetRepository<ChemodanStatus>().Update(itemDB);
                    }
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddOrUpdateMailing(Mailing mailing)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var mailingDB = db.GetRepository<Mailing>().TryOne(mailing.id);
                    if (mailingDB == null)
                    {
                        db.GetRepository<Mailing>().Add(mailing);
                    }
                    else
                    {
                        mailingDB.name = mailing.name;
                        mailingDB.isActive = mailing.isActive;
                        mailingDB.email = mailing.email;
                        db.GetRepository<Mailing>().Update(mailingDB);
                    }
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddOrUpdateBrand(Brand brand)
        {
            bool result = false;
            try
            {
                dbService.Run(db =>
                {
                    var brandDB = db.GetRepository<Brand>().TryOne(brand.id);
                    if (brandDB == null)
                    {
                        db.GetRepository<Brand>().Add(brand);
                    }
                    else
                    {
                        brandDB.name = brand.name;
                        db.GetRepository<Brand>().Update(brandDB);
                    }
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public long AddOrUpdateArticle(Article article)
        {
            long result = 0;
            try
            {
                result = article.id;
                dbService.Run(db =>
                {
                    var articleDB = db.GetRepository<Article>().TryOne(article.id);
                    if (articleDB == null)
                    {
                        result = db.GetRepository<Article>().Add(article).id;
                    }
                    else
                    {
                        articleDB.body = article.body;
                        articleDB.title = article.title;
                        articleDB.imgPath = article.imgPath;
                        db.GetRepository<Article>().Update(articleDB);
                    }
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public long AddOrUpdateInfoBlockItem(InfoBlockItem iblock)
        {
            long result = 0;
            try
            {
                result = iblock.id;
                dbService.Run(db =>
                {
                    var iblockDB = db.GetRepository<InfoBlockItem>().TryOne(iblock.id);
                    if (iblockDB == null)
                    {
                        result = db.GetRepository<InfoBlockItem>().Add(iblock).id;
                    }
                    else
                    {
                        iblockDB.DisplayType = iblock.DisplayType;
                        iblockDB.Image = iblock.Image;
                        iblockDB.Link = iblock.Link;
                        iblockDB.Title = iblock.Title;
                        iblockDB.name = iblock.name;
                        iblockDB.Description = iblock.Description;
                        db.GetRepository<InfoBlockItem>().Update(iblockDB);
                    }
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddOrUpdateStaticSpecification(StaticSpecification spec)
        {
            bool result;
            try
            {
                dbService.Run(db =>
                {
                    var specDB = db.GetRepository<StaticSpecification>().TryOne(spec.id);
                    if (specDB == null)
                    {
                        db.GetRepository<StaticSpecification>().Add(spec);
                    }
                    else
                    {
                        specDB.name = spec.name;
                        specDB.sortPriority = spec.sortPriority;
                        db.GetRepository<StaticSpecification>().Update(specDB);
                    }
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddOrUpdateStaticCategory(StaticCategory category)
        {
            bool result;
            try
            {
                dbService.Run(db =>
                {
                    var categoryDB = db.GetRepository<StaticCategory>().TryOne(category.id);
                    if (categoryDB == null)
                    {
                        db.GetRepository<StaticCategory>().Add(category);
                    }
                    else
                    {
                        categoryDB.name = category.name;
                        categoryDB.description = category.description;
                        categoryDB.bodyText = category.bodyText;
                        categoryDB.keywords = category.keywords;
                        db.GetRepository<StaticCategory>().Update(categoryDB);
                    }
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddOrUpdateCategory(Category category)
        {
            bool result;
            try
            {
                dbService.Run(db =>
                {
                    var categoryDB = db.GetRepository<Category>().TryOne(category.id);
                    if (categoryDB == null)
                    {
                        db.GetRepository<Category>().Add(category);
                    }
                    else
                    {
                        categoryDB.staticcat = category.staticcat;
                        categoryDB.description = category.description;
                        categoryDB.photoPath = category.photoPath;
                        db.GetRepository<Category>().Update(categoryDB);
                    }
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public long AddOrUpdateSku(Sku sku)
        {
            long result = 0;
            try
            {
                dbService.Run(db =>
                {
                    var SkuDB = db.GetRepository<Sku>().TryOne(sku.id);
                    if (SkuDB == null)
                    {
                        result = db.GetRepository<Sku>().Add(sku).id;
                    }
                    else
                    {
                        SkuDB.name = sku.name;
                        SkuDB.care = sku.care;
                        SkuDB.sortPriority = sku.sortPriority;
                        SkuDB.articul = sku.articul;
                        SkuDB.brand = sku.brand;
                        SkuDB.description = sku.description;
                        SkuDB.price = sku.price;
                        SkuDB.priceAct = sku.priceAct;
                        SkuDB.chemodanDaysRent = sku.chemodanDaysRent;
                        SkuDB.chemodanStatus = sku.chemodanStatus;
                        SkuDB.chemodanProvider = sku.chemodanProvider;
                        SkuDB.chemodanType = sku.chemodanType;
                        //SkuDB.smalPhoto = sku.smalPhoto;
                        //SkuDB.listCategory = sku.listCategory;
                        //SkuDB.listComment = sku.listComment;
                        //SkuDB.listPhoto = sku.listPhoto;
                        //SkuDB.listSpecification = sku.listSpecification;
                        SkuDB.isHide = sku.isHide;
                        SkuDB.displayType = sku.displayType;
                        db.GetRepository<Sku>().Update(SkuDB);
                        result = sku.id;
                    }
                });
            }
            catch (Exception err)
            {
                result = 0;
                logger.Error(err.Message);
            }
            return result;
        }

        public long AddOrUpdateMenuItem(Shop.db.Entities.MenuItem minuItem)
        {
            long result = 0;
            try
            {
                dbService.Run(db =>
                {
                    var minuItemDB = db.GetRepository<MenuItem>().TryOne(minuItem.id);
                    if (minuItemDB == null)
                    {
                        result = db.GetRepository<MenuItem>().Add(minuItem).id;
                    }
                    else
                    {
                        minuItemDB.name = minuItem.name;
                        minuItemDB.path = minuItem.path;
                        minuItemDB.type = minuItem.type;
                        minuItemDB.sortPriority = minuItem.sortPriority;

                        db.GetRepository<MenuItem>().Update(minuItemDB);
                        result = minuItemDB.id;
                    }
                });
            }
            catch (Exception err)
            {
                result = 0;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool RemoveMenuItem(long idMinuItem)
        {
            bool result = false;
            try
            {
                dbService.Run(db =>
                {
                    var menu = db.GetRepository<MenuItem>().TryOne(idMinuItem);
                    db.GetRepository<MenuItem>().Remove(menu);
                });
                result = true;
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddSmalPhotoToSku(long id, Photo photo)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var SkuDB = db.GetRepository<Sku>().TryOne(id);
                    if (SkuDB != null)
                    {
                        SkuDB.smalPhoto = photo;
                        db.GetRepository<Sku>().Update(SkuDB);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddPhotoToArticle(long id, string path)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var articleDB = db.GetRepository<Article>().TryOne(id);
                    if (articleDB != null)
                    {
                        articleDB.imgPath = path;
                        db.GetRepository<Article>().Update(articleDB);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddBigPhotoToSku(long id, PhotoBig photo)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var sku = db.GetRepository<Sku>().TryOne(id);
                    if (sku != null)
                    {
                        if (sku.listPhoto == null)
                        {
                            sku.listPhoto = new List<PhotoBig>();
                        }
                        sku.listPhoto.Add(new PhotoBig() { name = photo.name, path = photo.path, skuId = sku.id });
                        db.GetRepository<Sku>().Update(sku);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool RemoveBigPhotoFromSKU(long id, long idPhoto)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var pho = db.GetRepository<PhotoBig>().TryOne(idPhoto);
                    if (pho != null)
                    {
                        db.GetRepository<PhotoBig>().Remove(pho);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Mailing> ListMailingByActive(bool isActive)
        {
            var result = new List<Mailing>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((MailingRepository)db.GetRepository<Mailing>()).AllMailingByActive(isActive).ToList();
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<InfoBlockItem> ListInfoBlockItems(DisplayType type)
        {
            var result = new List<InfoBlockItem>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((InfoBlockItemRepository)db.GetRepository<InfoBlockItem>()).ListByType(type).ToList();
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public FilterProduct Filters()
        {
            var result = new FilterProduct();
            try
            {
                var v = new List<FilterSpecification>();
                dbService.Run(db =>
                {
                    result = ((StaticSpecificationRepository)db.GetRepository<StaticSpecification>()).ListFilters();
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<StaticSpecification> ListStaticSpecification()
        {
            var result = new List<StaticSpecification>();
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<StaticSpecification>().All().OrderBy(sp => sp.sortPriority).ToList();
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<MenuItem> ListMenuItem(int type)
        {
            var result = new List<MenuItem>();
            try
            {
                dbService.Run(db =>
                {
                    if (type > 0)
                    {
                        result = ((MenuItemRepository)db.GetRepository<MenuItem>()).AllByType(type);
                    }
                    else
                    {
                        result = db.GetRepository<MenuItem>().All();
                    }
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddSku(Sku sku)
        {
            bool result;
            try
            {
                dbService.Run(db =>
                {
                    db.GetRepository<Sku>().Add(sku);
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public void AddIPToMonitor(IPMonitor monitor)
        {
            try
            {
                dbService.Run(db => db.GetRepository<IPMonitor>().Add(monitor));
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
        }

        public bool AddOrUpdateSpecificationToSKU(long id, long specId, string specValue)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var sku = db.GetRepository<Sku>().TryOne(id);
                    var spec = db.GetRepository<StaticSpecification>().TryOne(specId);
                    if (sku != null && spec != null)
                    {
                        if (sku.listSpecification == null)
                        {
                            sku.listSpecification = new List<Specification>();
                        }

                        var oldspec = sku.listSpecification.FirstOrDefault(s => s.staticspec.id == spec.id);
                        if (oldspec != null)
                        {
                            oldspec.value = specValue;
                        }
                        else
                        {
                            sku.listSpecification.Add(new Specification() { staticspec = spec, skuId = sku.id, value = specValue });
                        }
                        db.GetRepository<Sku>().Update(sku);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
                result = false;
            }
            return result;
        }

        public List<ChemodanTracking> ListChemodanTrackingByProductId(long id)
        {
            var result = new List<ChemodanTracking>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((ChemodanTrackingRepository)db.GetRepository<ChemodanTracking>()).AllByProductId(id);
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result.ToList();
        }

        public bool SetChemodanTrackingToSku(ChemodanTracking item)
        {
            var result = false;
            if (item.skuId < 1 && item.Id < 1)
            {
                return false;
            }

            try
            {
                dbService.Run(db =>
                {
                    if (item.Id < 1)
                    {
                        var location = db.GetRepository<ChemodanLocation>().TryOne(item.Location.id);
                        item.Location = location;

                        db.GetRepository<ChemodanTracking>().Add(item);
                        result = true;
                    }
                    else
                    {
                        var trackerDB = db.GetRepository<ChemodanTracking>().TryOne(item.Id);
                        var location = db.GetRepository<ChemodanLocation>().TryOne(item.Location.id);
                        trackerDB.Location = location;
                        //trackerDB.Code = item.Code;
                        //trackerDB.skuId = item.skuId;
                        db.GetRepository<ChemodanTracking>().Update(trackerDB);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
                result = false;
            }
            return result;
        }

        public List<Order> ListOrdersByFilter(OrderFilter filter)
        {
            var result = new List<Order>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((OrderRepository)db.GetRepository<Order>()).AllByFilters(filter);
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<OrderOneClick> ListOrdersOneClick(OrderFilter filter)
        {
            var result = new List<OrderOneClick>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((OrderOneClickRepository)db.GetRepository<OrderOneClick>()).AllOrdersOneClick(filter);
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Order> ListOrdersByPhone(string phone)
        {
            var result = new List<Order>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((OrderRepository)db.GetRepository<Order>()).AllByClientPhone(phone);
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Order> ListOrdersBySecondName(string name)
        {
            var result = new List<Order>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((OrderRepository)db.GetRepository<Order>()).AllByClientSecondName(name);
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Order> ListOrdersByOrderNumber(int num)
        {
            var result = new List<Order>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((OrderRepository)db.GetRepository<Order>()).AllByOrderNumber(num);
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        //public List<Order> ListOrdersByDeliveryType(long deliveryType)
        //{
        //    var result = new List<Order>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = ((OrderRepository)db.GetRepository<Order>()).AllByDeliveryType(deliveryType);
        //        });
        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public List<Order> ListOrdersByPaymentType(long paymentType)
        //{
        //    var result = new List<Order>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = ((OrderRepository)db.GetRepository<Order>()).AllByPaymentType(paymentType);
        //        });
        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public List<Order> ListOrdersByOrderState(long state)
        //{
        //    var result = new List<Order>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = ((OrderRepository)db.GetRepository<Order>()).AllByOrderState(state);
        //        });
        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        public bool RemoveSpecificationFromSKU(long id, long idSpec)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var spec = db.GetRepository<Specification>().TryOne(idSpec);
                    if (spec != null)
                    {
                        db.GetRepository<Specification>().Remove(spec);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddCategoryToSku(long id, long idCat)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var sku = db.GetRepository<Sku>().TryOne(id);
                    var category = db.GetRepository<StaticCategory>().TryOne(idCat);
                    if (sku != null && category != null)
                    {
                        if (sku.listCategory == null)
                        {
                            sku.listCategory = new List<Category>();
                        }

                        if (!sku.listCategory.Any(s => s.staticcat.id == category.id))
                        {
                            sku.listCategory.Add(new Category() { staticcat = category, skuId = sku.id });
                            db.GetRepository<Sku>().Update(sku);
                        }
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public bool RemoveSKUFromCategory(long id, long idCat)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var cat = db.GetRepository<Category>().TryOne(idCat);
                    if (cat != null)
                    {
                        db.GetRepository<Category>().Remove(cat);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Sku> AllHiddenSku(bool isHide = false)
        {
            var result = new List<Sku>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((SkuRepository)db.GetRepository<Sku>()).AllHiddenSku(true).ToList();
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Sku> ListProductsByFilters(FilterFoDb filters)
        {
            var result = new List<Sku>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((SkuRepository)db.GetRepository<Sku>()).ListProductsByFilters(filters).ToList();
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Sku> ListProductsByIds(long[] ids)
        {
            var result = new List<Sku>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((SkuRepository)db.GetRepository<Sku>()).Many(ids);
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }
        public List<Sku> ListProductByDisplayType(DisplayType type)
        {
            var result = new List<Sku>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((SkuRepository)db.GetRepository<Sku>()).AllByDisplayType(type).ToList();
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public Mailing GetMailingByEmail(string email)
        {
            var result = new Mailing();
            try
            {
                dbService.Run(db =>
                {
                    result = ((MailingRepository)db.GetRepository<Mailing>()).OneByEmail(email);
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public long AddOrUpdateChemodanLocation(ChemodanLocation chemodanLocation)
        {
            long result = 0;
            try
            {
                dbService.Run(db =>
                {
                    var itemDB = db.GetRepository<ChemodanLocation>().TryOne(chemodanLocation.id);
                    if (itemDB == null)
                    {
                        result = db.GetRepository<ChemodanLocation>().Add(chemodanLocation).id;
                    }
                    else
                    {
                        itemDB.name = chemodanLocation.name;
                        itemDB.sortPriority = chemodanLocation.sortPriority;
                        itemDB.isUse = chemodanLocation.isUse;
                        db.GetRepository<ChemodanLocation>().Update(itemDB);
                        result = itemDB.id;
                    }
                });
            }
            catch (Exception err)
            {
                result = 0;
                logger.Error(err.Message);
            }
            return result;
        }

        public Order AddOrUpdateOrder(Order order)
        {
            Order result = null;
            try
            {
                dbService.Run(db =>
                {
                    var orderDb = db.GetRepository<Order>().TryOne(order.Id);
                    if (orderDb == null)
                    {
                        var skuList = db.GetRepository<Sku>().Many(order.Products.Select(item => item.ProductId).Distinct().ToArray());
                        var tmpOrderProducts = new List<OrderProduct>();
                        tmpOrderProducts.AddRange(order.Products);
                        order.Products = null;

                        result = db.GetRepository<Order>().Add(order);

                        foreach (var item in tmpOrderProducts)
                        {
                            var tmpItem = skuList.First(elem => elem.id == item.ProductId);
                            item.Article = tmpItem.articul;
                            item.FullPrice = tmpItem.price;
                            item.NaturalPrice = tmpItem.priceAct;
                            item.PriceDay = tmpItem.chemodanType.priceDay;
                            item.ProductName = tmpItem.name;
                            item.OrderId = result.Id;
                        }
                        result.Products = tmpOrderProducts;
                        db.GetRepository<Order>().Update(result);
                    }
                    else
                    {
                        long tmpId = 0;
                        orderDb.From = order.From;
                        orderDb.To = order.To;
                        orderDb.DeliveryType = order.DeliveryType;
                        orderDb.OrderState = order.OrderState;
                        orderDb.PaymentType = order.PaymentType;
                        orderDb.Pdf = order.Pdf;
                        orderDb.IsPaid = order.IsPaid;

                        if (orderDb.Client == null)
                        {
                            orderDb.Client = new Client();
                        }
                        tmpId = orderDb.Client.id;
                        orderDb.Client = order.Client;
                        orderDb.Client.id = tmpId;

                        if (orderDb.Client.editAdress == null)
                        {
                            orderDb.Client.editAdress = new EditAdress();
                        }
                        order.Client.editAdress.id = orderDb.Client.editAdress.id;
                        orderDb.Client.editAdress = order.Client.editAdress;
                        orderDb.OrderPrefix = order.OrderPrefix;
                        orderDb.OrderComment = order.OrderComment;

                        db.GetRepository<Order>().Update(orderDb);
                        result = orderDb;
                    }
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public Order UpdateOrderNumber(long order)
        {
            Order result = null;
            try
            {
                dbService.Run(db =>
                {
                    var orderDb = db.GetRepository<Order>().TryOne(order);

                    orderDb.OrderPrefix++;
                    // orderDb.FullOrderNumber = orderDb.OrderNumber + "-" + orderDb.OrderPrefix;
                    db.GetRepository<Order>().Update(orderDb);

                    result = orderDb;
                });
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }
        public long AddOrUpdateOrderOneClick(OrderOneClick order)
        {
            long result = 0;
            try
            {
                dbService.Run(db =>
                {
                    var orderDb = db.GetRepository<OrderOneClick>().TryOne(order.Id);
                    if (orderDb == null)
                    {
                        result = db.GetRepository<OrderOneClick>().Add(order).Id;
                    }
                    else
                    {
                        orderDb.CreateDate = order.CreateDate;
                        orderDb.IsComplite = order.IsComplite;
                        orderDb.Phone = order.Phone;
                        orderDb.ProductId = order.ProductId;
                        orderDb.UserId = order.UserId;
                        orderDb.UserName = order.UserName;
                        db.GetRepository<OrderOneClick>().Update(orderDb);
                        result = orderDb.Id;
                    }
                });
            }
            catch (Exception err)
            {
                result = 0;
                logger.Error(err.Message);
            }
            return result;
        }

        public OrderOneClick ApplyOrderOneClick(long id, int userId, string userName)
        {
            try
            {
                OrderOneClick orderDb = null;
                dbService.Run(db =>
                {
                    orderDb = db.GetRepository<OrderOneClick>().TryOne(id);
                    if (orderDb != null)
                    {
                        orderDb.IsComplite = true;
                        orderDb.UserId = userId;
                        orderDb.UserName = userName;
                        db.GetRepository<OrderOneClick>().Update(orderDb);
                    }
                });
                return orderDb;
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return null;
        }

        public bool PaidOrder(int num, string paymentId, DateTime payDate)
        {
            bool result = false;
            try
            {
                dbService.Run(db =>
                {
                    var orderDb = ((OrderRepository)db.GetRepository<Order>()).OneByOrderNumber(num);
                    if (orderDb != null)
                    {
                        orderDb.IsPaid = true;
                        orderDb.PaymentId = paymentId;
                        orderDb.PayDate = payDate;
                        db.GetRepository<Order>().Update(orderDb);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool SaveOrder(Order order)
        {
            bool result = false;
            try
            {
                dbService.Run(db =>
                {
                    var orderDb = db.GetRepository<Order>().One(order.Id);
                    if (orderDb != null)
                    {
                        //orderDb.IsPaid = true;

                        //orderDb.Id = order.Id;
                        //orderDb.OrderNumber = order.OrderNumber;

                        //Поставить иф который будет запрещать редактировать что-то если договор оплачен или находится под конкретным статусом

                        orderDb.Client.name = order.Client.name;
                        orderDb.Client.lastName = order.Client.lastName;
                        orderDb.Client.mPhone = order.Client.mPhone;
                        orderDb.Client.email = order.Client.email;

                        orderDb.OrderState = order.OrderState;
                        orderDb.DeliveryType = order.DeliveryType;
                        orderDb.PaymentType = order.PaymentType;
                        orderDb.OrderComment = order.OrderComment;

                        orderDb.IsHaveContract = order.IsHaveContract;
                        orderDb.PayDate = order.PayDate;
                        orderDb.UserId = order.UserId;
                        orderDb.UserName = order.UserName;
                        //orderDb.PaymentId = order.PaymentId;

                        orderDb.IsPaid = order.IsPaid;

                        if (order.DeliveryType.Id != 1)
                        {
                            orderDb.Client.editAdress.city = order.Client.editAdress.city;
                            orderDb.Client.editAdress.typeStreet = order.Client.editAdress.typeStreet;
                            orderDb.Client.editAdress.street = order.Client.editAdress.street;
                            orderDb.Client.editAdress.numHome = order.Client.editAdress.numHome;
                            orderDb.Client.editAdress.level = order.Client.editAdress.level;
                            orderDb.Client.editAdress.numFlat = order.Client.editAdress.numFlat;
                        }


                        orderDb.From = order.From;
                        orderDb.To = order.To;

                        db.GetRepository<Order>().Update(orderDb);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddOrUpdateOrderProduct(long id, string code, string articul, long? orderId = -1)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var orderProd = db.GetRepository<OrderProduct>().TryOne(id);
                    if (orderProd == null)
                    {
                        if (orderId.HasValue&& orderId.Value!=-1)
                        {
                            var sku = ((SkuRepository)db.GetRepository<Sku>()).OneByArticul(articul);
                            orderProd = new OrderProduct(sku, orderId.Value);
                            db.GetRepository<OrderProduct>().Add(orderProd);
                            result = true;
                        }
                    }
                    else
                    {
                        orderProd.Code = code;
                        db.GetRepository<OrderProduct>().Update(orderProd);
                        result = true;
                    }
                });
                
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool RemoveProductFromOrder(long orderLine)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var item = db.GetRepository<OrderProduct>().TryOne(orderLine);
                    db.GetRepository<OrderProduct>().Remove(item);
                    result = true;
                });
                
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }
    }
}
//1570 lines