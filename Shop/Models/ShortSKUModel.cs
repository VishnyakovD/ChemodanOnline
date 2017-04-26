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
        public string code { get; set; }
        public string category { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal fullPrice { get; set; }
        public string smalPhotoPath { get; set; }
        public int maxCount { get; set; }
        public int orderCount { get; set; }


        public ShortSKUModel()
        {
        }

       
    }
}
