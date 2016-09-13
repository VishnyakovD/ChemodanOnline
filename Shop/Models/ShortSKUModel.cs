using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Shop.db.Entities;

namespace Shop.Models
{
    public class ShortSKUModel
    {


        public long id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal priceAct { get; set; }
        public long brandId { get; set; }
        public string brandName { get; set; }
        public long categotyId { get; set; }
        public string categotyName { get; set; }
        public string description { get; set; }
        public string smalPhotoPath { get; set; }


        public ShortSKUModel()
        {
            name = string.Empty;
            price = 0;
            priceAct = 0;
            brandId = 0;
            brandName = string.Empty;
            description = string.Empty;
            smalPhotoPath = string.Empty;
            categotyId = 0;
            categotyName = string.Empty;
        }

       
    }
}
