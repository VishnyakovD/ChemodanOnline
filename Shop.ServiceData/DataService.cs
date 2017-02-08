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
        List<Sku> ListProductsByFilters(FilterFoDB filters);


        long AddOrUpdateChemodanLocation(ChemodanLocation chemodanLocation);
        bool AddOrUpdateMailing(Mailing mailing);
        bool AddOrUpdateBrand(Brand brand);
        bool AddOrUpdateCategory(Category category);
        bool AddOrUpdateStaticCategory(StaticCategory category);
        bool AddOrUpdateStaticSpecification(StaticSpecification spec);
        long AddOrUpdateSKU(Sku sku);
        bool AddSmalPhotoToSKU(long id, Photo photo);
        bool AddBigPhotoToSKU(long id, PhotoBig photo);
        bool AddCategoryToSKU(long id, long idCat);
        bool AddOrUpdateSpecificationToSKU(long id, long specId, string specValue);
        long AddOrUpdateMenuItem(Shop.db.Entities.MenuItem minuItem);
        long AddOrUpdateArticle(Article article);
        long AddOrUpdateInfoBlockItem(InfoBlockItem iblock);
        bool AddPhotoToArticle(long id, string path);
        //Article GetArticleById(long id);
        bool RemoveSKUFromCategory(long id, long idCat);
        bool RemoveSpecificationFromSKU(long id, long idSpec);
        bool RemoveBigPhotoFromSKU(long id, long idPhoto);
        //Sku GetSkuById(long id);

        bool AddOrUpdateChemodanStatus(ChemodanStatus item);
        //ChemodanStatus GetChemodanStatus(long id);
       // List<ChemodanStatus> ListChemodanStatus();

        bool AddOrUpdateChemodanType(ChemodanType item);
        //ChemodanType GetChemodanType(long id);
        //List<ChemodanType> ListChemodanType();

        long AddOrUpdateChemodanProvider(ChemodanProvider item);
        //ChemodanProvider GetChemodanProvider(long id);
        //List<ChemodanProvider> ListChemodanProvider();

        long AddOrUpdateClient(Client item);
       // Client GetClient(long id);
        //List<Client> ListClient();

     

        //MenuItem GetMenuItem(long idMinuItem);
        bool RemoveMenuItem(long idMinuItem);
        
        //StaticCategory GetStaticCategoryById(long idCat);

        List<Sku> ListSkuByCategory(StaticCategory cat);
        List<Sku> ListProductByDisplayType(DisplayType type);
        //List<Sku> ListSku();


        List<Sku> AllHiddenSku(bool isHide = false);

       // List<Article> ListArticles();

        Mailing GetMailingByEmail(string email);

        //List<Brand> ListBrands();
        //Brand GetBrand(long id);
        //List<Mailing> ListMailings();
        List<Mailing> ListMailingByActive(bool isActive);
        //List<Category> ListCategoryes();

        //List<StaticCategory> ListStaticCategoryes();
        List<StaticSpecification> ListStaticSpecification();
       // StaticSpecification GetStaticSpecification(long idSpec);
        List<MenuItem> ListMenuItem(int type);
        bool AddSku(Sku sku);
        void AddIPToMonitor(IPMonitor monitor);
    }

    public class DataService:IDataService
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
            var result =new T();
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
                        result=  db.GetRepository<ChemodanProvider>().Add(item).id;
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
                            itemDB.editAdress=new EditAdress();
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
                result =-1;
                logger.Error(err.Message);
            }
            return result;

        }

        //public ChemodanProvider GetChemodanProvider(long id)
        //{
        //    var result = new ChemodanProvider();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<ChemodanProvider>().TryOne(id);
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public List<ChemodanProvider> ListChemodanProvider()
        //{
        //    var result = new List<ChemodanProvider>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<ChemodanProvider>().All();
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

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

        //public Client GetClient(long id)
        //{
        //    var result = new Client();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<Client>().TryOne(id);
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public List<Client> ListClient()
        //{
        //    var result = new List<Client>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<Client>().All();
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

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

        //public ChemodanType GetChemodanType(long id)
        //{
        //    var result = new ChemodanType();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<ChemodanType>().TryOne(id);
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public List<ChemodanType> ListChemodanType()
        //{
        //    var result = new List<ChemodanType>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<ChemodanType>().All();
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

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

        //public ChemodanStatus GetChemodanStatus(long id)
        //{
        //    var result = new ChemodanStatus();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<ChemodanStatus>().TryOne(id);
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public List<ChemodanStatus> ListChemodanStatus()
        //{
        //    var result = new List<ChemodanStatus>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<ChemodanStatus>().All();
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}


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
                    var brandDB= db.GetRepository<Brand>().TryOne(brand.id);
                    if (brandDB==null)
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
                      result=  db.GetRepository<Article>().Add(article).id;
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
                      result=  db.GetRepository<InfoBlockItem>().Add(iblock).id;
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
            bool result = false;
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
            bool result = false;
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
            bool result = false;
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

        public long AddOrUpdateSKU(Sku sku)
        {
            long result = 0;
            try
            {
                dbService.Run(db =>
                {
                    var SkuDB = db.GetRepository<Sku>().TryOne(sku.id);
                    if (SkuDB == null)
                    {
                      result=  db.GetRepository<Sku>().Add(sku).id;
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

        //public MenuItem GetMenuItem(long idMinuItem)
        //{
        //    var result = new MenuItem();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<MenuItem>().TryOne(idMinuItem);
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

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
        //public StaticSpecification GetStaticSpecification(long idSpec)
        //{
        //    var result = new StaticSpecification();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<StaticSpecification>().TryOne(idSpec);
        //        });
        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        public bool AddSmalPhotoToSKU(long id, Photo photo)
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

        public bool AddBigPhotoToSKU(long id, PhotoBig photo)
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

                        sku.listPhoto.Add(new PhotoBig() { name = photo.name,path = photo.path,skuId = sku.id });
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

        //public List<Brand> ListBrands()
        //{
        //    var result = new List<Brand>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //           result= db.GetRepository<Brand>().All();
        //        });
          
        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public List<Mailing> ListMailings()
        //{
        //    var result = new List<Mailing>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<Mailing>().All();
        //        });
          
        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}
        public List<Mailing> ListMailingByActive(bool isActive)
        {
            var result = new List<Mailing>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((MailingRepository) db.GetRepository<Mailing>()).AllMailingByActive(isActive).ToList();
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
                    result = ((InfoBlockItemRepository) db.GetRepository<InfoBlockItem>()).ListByType(type).ToList();
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



        //public List<Article> ListArticles()
        //{
        //    var result = new List<Article>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<Article>().All();
        //        });
          
        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}


        //public List<Category> ListCategoryes()
        //{
        //    var result = new List<Category>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<Category>().All();
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public List<StaticCategory> ListStaticCategoryes()
        //{
        //    var result = new List<StaticCategory>();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<StaticCategory>().All();
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        public List<StaticSpecification> ListStaticSpecification()
        {
            var result = new List<StaticSpecification>();
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<StaticSpecification>().All().OrderBy(sp=>sp.sortPriority).ToList();
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
                        result = ((MenuItemRepository)db.GetRepository<MenuItem>()).AllByType(type).ToList();
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

        //public Sku GetSkuById(long id)
        //{
        //    var result = new Sku();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<Sku>().TryOne(id);
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public Article GetArticleById(long id)
        //{
        //    var result = new Article();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<Article>().TryOne(id);
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public StaticCategory GetStaticCategoryById(long idCat)
        //{
        //    var result = new StaticCategory();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<StaticCategory>().TryOne(idCat);
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        //public Brand GetBrand(long id)
        //{
        //    var result = new Brand();
        //    try
        //    {
        //        dbService.Run(db =>
        //        {
        //            result = db.GetRepository<Brand>().TryOne(id);
        //        });
        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err.Message);
        //    }
        //    return result;
        //}

        public bool AddSku(Sku sku)
        {
            bool result = false;
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
                        if (oldspec!=null)
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

        public bool AddCategoryToSKU(long id, long idCat)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var sku = db.GetRepository<Sku>().TryOne(id);
                    var category = db.GetRepository<StaticCategory>().TryOne(idCat);
                    if (sku!=null&&category!=null)
                    {
                        if (sku.listCategory == null)
                        {
                            sku.listCategory=new List<Category>();
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

        public bool RemoveSKUFromCategory(long id,long idCat)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var cat = db.GetRepository<Category>().TryOne(idCat);
                    if (cat!=null)
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
        
        public List<Sku> ListSkuByCategory(StaticCategory cat)
        {
            var result = new List<Sku>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((SkuRepository)db.GetRepository<Sku>()).ListSkuByCategory(cat).ToList();
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Sku> ListProductsByFilters(FilterFoDB filters)
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

		//public List<Sku> ListSku()
		//{
		//    var result = new List<Sku>();
		//    try
		//    {
		//        dbService.Run(db =>
		//        {
		//            result = db.GetRepository<Sku>().All();
		//        });

		//    }
		//    catch (Exception err)
		//    {
		//        logger.Error(err.Message);
		//    }
		//    return result;
		//}


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

    }
}
//1570 lines