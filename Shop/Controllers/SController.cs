using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebPages;
using Newtonsoft.Json;
using Shop.DataService;
using Shop.db.Entities;
using Shop.db.Repository;
using Shop.Filters;
using Shop.Logger;
using Shop.Models;
using Shop.Models.Builders;
using Shop.Modules;

namespace Shop.Controllers
{
      [InitializeSimpleMembership]
    public class SController : BaseController
    {
        private MenuBuilder menuBuilder { set; get; }
        private string Keywords { set; get; }
        private string Description { set; get; }


        public SController(ILogger logger,
            IAdminModelBuilder adminModelBuilder,
            IDataService dataService,
            IImagesPath imagesPath,
            ISKUModelBuilder SKUModelBuilder)
            : base(logger, adminModelBuilder, dataService, imagesPath, SKUModelBuilder)
        {
            menuBuilder = new MenuBuilder(dataService,imagesPath);
        }

        public ActionResult Dap()
        {
            var model = menuBuilder.BuildAllMenu();
            return View(model);
        }

        public ActionResult Faq()
        {
            var model = menuBuilder.BuildAllMenu();
            return View(model);
        }

        public ActionResult Cnt()
        {
            var model = menuBuilder.BuildAllMenu();
            return View(model);
        }

        public ActionResult Arts()
        {
            var model = menuBuilder.BuildAllMenu();
            return View(model);
        }

        public ActionResult Art(long id)
        {
            var model = menuBuilder.BuildAllMenu();
            return View(model);
        }
    }
}
