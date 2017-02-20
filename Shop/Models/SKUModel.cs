using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Shop.db.Entities;
using Shop.Resources;

namespace Shop.Models
{
    public class SKUModel:MenuModel
    {
        [DisplayName("ид")]
        public long id { get; set; }

        [Required(ErrorMessageResourceType = typeof (Default), ErrorMessageResourceName = "RequiredInput")]
        [DisplayName("Название")]
        public string name { get; set; }

        [Required(ErrorMessageResourceType = typeof (Default), ErrorMessageResourceName = "RequiredInput")]
        [DisplayName("Цена")]
        public decimal price { get; set; }

        [DisplayName("Спец. цена")]
        public decimal priceAct { get; set; }

        public long brandId { get; set; }
        [DisplayName("Бренд")]
        public string brandName { get; set; }

        public long categotyId { get; set; }
        [DisplayName("Категория")]
        public string categotyName { get; set; }

        [DisplayName("Описание")]
        public string description { get; set; }

        
        public long smalPhotoId { get; set; }
        [DisplayName("Фотография")]
        public string smalPhotoPath { get; set; }

        [DisplayName("Артикул")]
        public string articul { get; set; }
        
        [DisplayName("Скрыть")]
        public bool isHide { get; set; }  
    
        [DisplayName("Уход")]
        public string care { get; set; }

        [DisplayName("Поставщик")]
        public long chemodanProviderId { get; set; }

        [DisplayName("Статус")]
        public long chemodanStatusId { get; set; }

        public List<ChemodanStatus> listChemodanStatus { get; set; }
        public List<ChemodanType> listChemodanType { get; set; }

        public int chemodanDaysRent { get; set; }

        [DisplayName("Тип чемодана")]
        public long chemodanTypeId { get; set; }

        [DisplayName("Цена в день")]
        public decimal chemodanPriceDay { get; set; }

        public IList<PhotoBig> listPhoto { get; set; }
        public IList<Category> listCategory { get; set; }
        public IList<Specification> listSpecification { get; set; }
        public List<ChemodanTracking> ListChemodanTracking { set; get; }

        public List<StaticCategory> listStaticCategory { get; set; }
        public List<StaticSpecification> listStaticSpecification { get; set; }
        public List<Brand> listStaticBrand { get; set; }
        public List<ChemodanProvider> listChemodanProvider { get; set; }
        public int sortPriority { get; set; }
        public DisplayType displayType { get; set; }

        public int maxCount { get; set; }



        public SKUModel()
        {
            care = string.Empty;
            isHide = false;
            price = 0;
            priceAct = 0;
            brandId = 0;
            sortPriority = 0;
            brandName = string.Empty;
            description = string.Empty;
            smalPhotoId = 0;
            smalPhotoPath = string.Empty;
            articul = string.Empty;
            displayType = DisplayType.Default;
            categotyId = 0;
            categotyName = string.Empty;
            listPhoto=new List<PhotoBig>();
            listCategory=new List<Category>();
            listSpecification=new List<Specification>();
            listStaticCategory=new List<StaticCategory>();
            listStaticSpecification = new List<StaticSpecification>();
            listStaticBrand = new List<Brand>();
            listChemodanProvider = new List<ChemodanProvider>();
            listChemodanStatus=new List<ChemodanStatus>();
            listChemodanType=new List<ChemodanType>();
            ListChemodanTracking=new List<ChemodanTracking>();
        }
        public Sku GetSKUDB()
        {
            return new Sku() {
                id = this.id,
                name = this.name,
                sortPriority = this.sortPriority,
                care = this.care,
                isHide = this.isHide,
                brand = new Brand() { id = this.brandId, name = this.brandName },
                articul = this.articul,
                description = this.description,
                price = this.price,
                priceAct = this.priceAct,
                displayType= this.displayType,
                smalPhoto =  /*new Photo() { id = this.smalPhotoId, path = this.smalPhotoPath, skuId = this.id}*/null,
                chemodanDaysRent = chemodanDaysRent,
                chemodanProvider = new ChemodanProvider() { id = this.chemodanProviderId },
                chemodanStatus = new ChemodanStatus() { id = this.chemodanStatusId },
                chemodanType = new ChemodanType() { id = this.chemodanTypeId }
            };
        }

       
    }
}
