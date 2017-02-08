using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.db.Entities;
using Shop.db.Repository;

namespace Shop.Models
{
    public class SkuSpecificatoinModel
    {
        public long skuId { set; get; }
        public long specId { set; get; }
        public string specName{ set; get; }
        public string specValue { set; get; }
        public List<StaticSpecification> listStaticSpecification { set; get; }

        public SkuSpecificatoinModel()
        {
            listStaticSpecification=new List<StaticSpecification>();
        }
    }
}