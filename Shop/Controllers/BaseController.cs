using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.DataService;
using Shop.Logger;
using Shop.Models.Builders;
using Shop.Modules;

namespace Shop.Controllers
{
    public class BaseController : Controller
    {
        public ILogger logger;
        public IAdminModelBuilder adminModelBuilder;
        public IDataService dataService;
        public ISKUModelBuilder skuModelBuilder;
        public IImagesPath imagesPath;

        public BaseController(ILogger logger, IAdminModelBuilder adminModelBuilder, IDataService dataService,
            IImagesPath imagesPath, ISKUModelBuilder SKUModelBuilder)
        {
            this.logger = logger;
            this.adminModelBuilder = adminModelBuilder;
            this.dataService = dataService;
            this.imagesPath = imagesPath;
            this.skuModelBuilder = SKUModelBuilder;

        }

        protected string ConvertToHTMLString(string safestr)
        {
            if (string.IsNullOrEmpty(safestr))
                return string.Empty;
            return safestr.Replace("Љ", "<").Replace("Њ", ">").Replace("µ", "</").Replace(@"\r\n", "");
        }

    }
}
