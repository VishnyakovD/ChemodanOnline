using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.db.Entities;
using Shop.db.Repository;

namespace Shop.Models
{
    public class ChemodanTrackingModel
    {
        public long skuId { set; get; }
        public long Id { set; get; }
        public string Code{ set; get; }
        public ChemodanLocation Locatoin { set; get; }
        public List<ChemodanLocation> ListChemodanLocation { set; get; }

        public ChemodanTrackingModel()
        {
            Locatoin=new ChemodanLocation();
            ListChemodanLocation = new List<ChemodanLocation>();
        }
    }
}