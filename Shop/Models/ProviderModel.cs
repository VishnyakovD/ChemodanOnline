using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.db.Entities;

namespace Shop.Models
{
    public class ProviderModel : MenuModel
    {
        public List<ChemodanProvider> list { set; get; }

        public ProviderModel()
        {
            list = new List<ChemodanProvider>();
        }
    }

    public class ProviderDataModel : MenuModel
    {
        public ChemodanProvider provider { set; get; }

        public ProviderDataModel()
        {
            provider = new ChemodanProvider();
        }
    }
}