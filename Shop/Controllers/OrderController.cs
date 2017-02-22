using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http.Routing.Constraints;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
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
    public class OrderController : BaseController
      {
          private IOrderBuilder OrderBulder;
        public OrderController(ILogger logger,
            IAdminModelBuilder adminModelBuilder,
            IDataService dataService,
            IImagesPath imagesPath,
            ISKUModelBuilder skuModelBuilder,
            IOrderBuilder orderBulder)
            : base(logger, adminModelBuilder, dataService, imagesPath, skuModelBuilder)
        {
            OrderBulder = orderBulder;
        }

        public ActionResult Index(string ids)
        {
            var model = new OrderClientPage();
            try
            {
                long[] idss = ids.Split(',').Select(long.Parse).ToArray();
                model = OrderBulder.Build(idss);
            }
            catch (Exception err)
            {
                return Content(err.ToString(), "text/html");
            }
            return View("Order",model);
        }


    }
}
