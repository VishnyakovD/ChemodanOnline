using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class SkuViewerModel:MenuModel
    {
        public long IdCat { set; get; }
        public string Name { set; get; }
        public string Keywords { set; get; }
        public string Description { set; get; }
        public string bodyText { set; get; }
        public List<ShortSKUModel> skuList { set; get; }
        public List<ShortSKUModel> ListProduct { set; get; }
        public string TitleProduct { set; get; }

        public SkuViewerModel()
        {
            skuList=new List<ShortSKUModel>();
            ListProduct = new List<ShortSKUModel>();
        }
    }
}