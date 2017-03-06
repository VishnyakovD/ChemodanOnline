using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Http;
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
          private long DefaultOrderState;
        public OrderController(ILogger logger,
            IAdminModelBuilder adminModelBuilder,
            IDataService dataService,
            IImagesPath imagesPath,
            ISKUModelBuilder skuModelBuilder,
            IOrderBuilder orderBulder)
            : base(logger, adminModelBuilder, dataService, imagesPath, skuModelBuilder)
        {
            OrderBulder = orderBulder;
            DefaultOrderState = long.Parse(WebConfigurationManager.AppSettings["DefaultValueHasInStock"]);
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

        public ActionResult CreateOrder(string order)
        {
            try
            {
                if (!string.IsNullOrEmpty(order))
                {
                    var ser = new JavaScriptSerializer();
                    dynamic serObject =ser.DeserializeObject(order);
                   
                    var listProducts = (serObject["listProducts"] as dynamic[]);
                    if (listProducts != null&& listProducts.Any())
                    {
                        var tmpProductList = new List<OrderProduct>();
                        foreach (var item in listProducts)
                        {
                            var count = int.Parse(item["count"].ToString());
                            for (int i = 0; i < count; i++)
                            {
                                tmpProductList.Add(new OrderProduct() { ProductId = long.Parse(item["productId"].ToString()) });
                            }
                        }
                          
                        var orderData = new Order
                        {
                            Client =
                            {
                                lastName = serObject["clientLastName"],
                                name = serObject["clientFirstName"],
                                email = serObject["clientEmail"],
                                mPhone = serObject["clientPhone"],
                                editAdress =
                                {
                                    city = serObject["city"],
                                    numHome = serObject["home"],
                                    typeStreet = serObject["typeStreet"],
                                    level = serObject["level"],
                                    street = serObject["street"],
                                    numFlat = serObject["flat"]
                                }
                            },
                            DeliveryType = {Id = long.Parse(serObject["deliveryType"].ToString())},
                            PaymentType = {Id = long.Parse(serObject["paymentType"].ToString()) },
                            OrderState= { Id = DefaultOrderState},
                            Products = tmpProductList,
                            From = DateTime.Parse(serObject["from"].ToString()),
                            To = DateTime.Parse(serObject["to"].ToString())

                        };
                        dataService.AddOrUpdateOrder(orderData);
                    }
                }
            }
            catch (Exception err)
            {
                return Content(err.ToString(), "text/html");
            }
            return Content("Ла ла ла, заказ создан", "text/html");
        }

        public ActionResult OrdersAdmin()
        {
            var model=OrderBulder.BuildOrders(DateTime.Now.AddYears(-2), DateTime.Now.AddYears(2));


            return View(model);
        }
    }
}
