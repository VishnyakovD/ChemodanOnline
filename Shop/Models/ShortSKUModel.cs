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
        public string articul { get; set; }
        public decimal price { get; set; }
        public string smalPhotoPath { get; set; }


        public ShortSKUModel()
        {
            articul = string.Empty;
            price = 0;
            smalPhotoPath = string.Empty;
        }

       
    }
}
