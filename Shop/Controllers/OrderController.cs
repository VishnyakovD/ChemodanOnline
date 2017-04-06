﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
using WebMatrix.WebData;

namespace Shop.Controllers
{
    [InitializeSimpleMembership]
    public class OrderController : BaseController
    {
        private IOrderBuilder OrderBulder;
        private IEditOrderModelBuilder EditOrderModelBuilder;
        private long DefaultOrderState;

        public OrderController(ILogger logger,
            IAdminModelBuilder adminModelBuilder,
            IDataService dataService,
            IImagesPath imagesPath,
            ISKUModelBuilder skuModelBuilder,
            IOrderBuilder orderBulder, IEditOrderModelBuilder editOrderModelBuilder)
            : base(logger, adminModelBuilder, dataService, imagesPath, skuModelBuilder)
        {
            OrderBulder = orderBulder;
            DefaultOrderState = long.Parse(WebConfigurationManager.AppSettings["DefaultValueHasInStock"]);
            EditOrderModelBuilder = editOrderModelBuilder;
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
            return View("Order", model);
        }

        public ActionResult CreateOrder(string order)
        {
            var result = "";
            try
            {
                if (!string.IsNullOrEmpty(order))
                {
                    var ser = new JavaScriptSerializer();
                    dynamic serObject = ser.DeserializeObject(order);

                    var listProducts = (serObject["listProducts"] as dynamic[]);
                    if (listProducts != null && listProducts.Any())
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
                            DeliveryType = { Id = long.Parse(serObject["deliveryType"].ToString()) },
                            PaymentType = { Id = long.Parse(serObject["paymentType"].ToString()) },
                            OrderState = { Id = DefaultOrderState },
                            Products = tmpProductList,
                            From = DateTime.Parse(serObject["from"].ToString()),
                            To = DateTime.Parse(serObject["to"].ToString()),
                            CreateDate = DateTime.Parse(serObject["createDate"].ToString())
                        };
                        if (orderData.DeliveryType.Id==1)
                        {
                            orderData.Client.adress = null;
                            orderData.Client.editAdress = null;
                        }
                        orderData = dataService.AddOrUpdateOrder(orderData);
                        result = "Заказ создан номер: "+ orderData.OrderNumber.ToString();

                        if (orderData.PaymentType.Id==2)
                        {
                            var pay = new Pay(orderData);
                            result = pay.ToString(); 
                        }
                    }
                }
            }
            catch (Exception err)
            {
                return Content(err.ToString(), "text/html");
            }
            return Content(result, "text/html");
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public ActionResult OrdersAdmin()
        {
            var filter=new OrderFilter();
            var model = OrderBulder.BuildOrders(filter);
            return View(model);
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public ActionResult OrdersOneClickAdmin()
        {
            var filter=new OrderFilter();
            var model = OrderBulder.BuildOrdersOneClick(filter);
            return View(model);
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public ActionResult FilterOrdersOneClick(OrderFilter filter)
        {
            if (filter == null)
            {
                filter = new OrderFilter();
            }
            var model = OrderBulder.OrdersModelOneClick(filter);
            return PartialView("OrdersOneClickAdminPartial", model);
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public ActionResult FilterOrders(OrderFilter filter)
        { 
            if(filter==null)
            {
                filter = new OrderFilter();
            }
        var model = OrderBulder.OrdersModel(filter);
            return PartialView("OrdersAdminPartial", model);
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public ActionResult SearchFilterOrders(string filterType,string filterValue)
        {
            if (string.IsNullOrEmpty(filterType)|| string.IsNullOrEmpty(filterValue))
            {
                return PartialView("OrdersAdminPartial", new List<OrderModel>());
            }
        var model = OrderBulder.OrdersModel(filterType, filterValue);
            return PartialView("OrdersAdminPartial", model);
        }
        public ActionResult PayOneClick(string phone, long productId, string createDate)
        {
            var result = string.Empty;
            try
            {
                var date = DateTime.Parse(createDate);
                var userId = 0;
                var userName = string.Empty;
                if (WebSecurity.IsAuthenticated)
                {
                    userName = WebSecurity.CurrentUserName;
                    userId = WebSecurity.CurrentUserId;
                }
                var order = new OrderOneClick
                {
                    CreateDate = date,
                    Phone = phone,
                    ProductId = productId,
                    UserId = userId,
                    UserName = userName
                };
                dataService.AddOrUpdateOrderOneClick(order);
                result = "В ближайшее время вам позвонит наш сотрудник";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                throw;
            }
            return Content(result, "html");
        }

        public bool PaidOrder(int num)
        {
            bool result;
            try
            {

                result = dataService.PaidOrder(num);
            }
            catch (Exception ex)
            {
                result = false;
                throw;
            }
            return result;
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public ActionResult EditOrder(long id)
        {
            var model = EditOrderModelBuilder.Build(id);
            return View(model);
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public ActionResult ApplyOneClickOrder(long id)
        {
            var userId = 0;
            var userName = string.Empty;
            if (WebSecurity.IsAuthenticated)
            {
                userName = WebSecurity.CurrentUserName;
                userId = WebSecurity.CurrentUserId;
            }
            var model = dataService.ApplyOrderOneClick(id, userId, userName);
            return PartialView("OrderOneClickAdminPartial", model);
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public ActionResult SaveOrderAdmin(OrderModel order)
        {

            return Content("Сохранено");
        }


        //[HttpPost]
        //public object PayLockSumm(string data)
        //{
        //	Object result = null;
        //	try
        //	{
        //		var request = WebRequest.Create("https://api.wayforpay.com/api");
        //		request.Method = "POST";
        //		byte[] byteArray = Encoding.UTF8.GetBytes(data);
        //		request.ContentType = "application/x-www-form-urlencoded";
        //		request.ContentLength = byteArray.Length;
        //		var dataStream = request.GetRequestStream();
        //		dataStream.Write(byteArray, 0, byteArray.Length);
        //		dataStream.Close();
        //		var response = request.GetResponse();
        //		//((HttpWebResponse)response).StatusDescription;
        //		dataStream = response.GetResponseStream();
        //		var reader = new StreamReader(dataStream);
        //		var responseFromServer = reader.ReadToEnd();
        //		reader.Close();
        //		dataStream.Close();
        //		response.Close();
        //		result= JsonConvert.DeserializeObject(responseFromServer);
        //	}
        //	catch (Exception ex)
        //	{
        //		result = ex.Message;
        //	}
        //	return result;
        //}
    }

    public class Pay
    {
        public string merchantAccount { set; get; }
        public string merchantDomainName { set; get; }
        public string authorizationType { set; get; }
        public string merchantSignature { set; get; }
        public string merchantTransactionType { set; get; }
        public string transactionType { set; get; }
        public string orderReference { set; get; }
        public string orderDate { set; get; }
        public string amount { set; get; }
        public string currency { set; get; }
        public string productName { set; get; }
        public string productPrice { set; get; }
        public string productCount { set; get; }
        public string clientFirstName { set; get; }
        public string clientLastName { set; get; }
        public string clientEmail { set; get; }
        public string clientPhone { set; get; }
        public string apiVersion { set; get; }
        public string comment { set; get; }
        public string holdTimeout { set; get; }

        public Pay(Order order)
        {
            merchantAccount = "test_merch_n1";
            merchantDomainName = "chemodan.online";
            authorizationType = "SimpleSignature";
            orderReference = order.OrderNumber.ToString();
            orderDate = order.CreateDate.ToUniversalTime().Ticks.ToString();
            currency = "UAH";
            productName = "Оплата услуг по договору: " + order.OrderNumber;
            decimal tmpSumm = order.Products.Sum(prod => prod.PriceDay);
            productPrice =Math.Round((tmpSumm * decimal.Parse(((order.To - order.From).TotalDays).ToString())),2).ToString().Replace(",",".");
            amount = productPrice;
            productCount = "1";
            clientFirstName = order.Client.name;
            clientLastName = order.Client.lastName;
            clientEmail = order.Client.email;
            clientPhone = order.Client.mPhone;
            merchantTransactionType = "AUTH";
            holdTimeout = "1728000";
            merchantSignature = GenerateSignature();
        }

        private string HashData()
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", merchantAccount, merchantDomainName, orderReference, orderDate, amount, currency, productName, productCount, productPrice);
        }

        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(this);
            //return "\"merchantAccount\":\"" + merchantAccount
            //         + "\",\"merchantDomainName\":\"" + merchantDomainName
            //         + "\",\"authorizationType\":\"SimpleSignature\",\"merchantSignature\":\"" + merchantSignature
            //         + "\",\"orderReference\":\"" + orderReference
            //         + "\",\"orderDate\":\"" + orderDate
            //         + "\",\"amount\":" + amount
            //         + ",\"currency\":\"UAH\",\"productName\":\"" + productName
            //         + "\",\"productPrice\":" + productPrice
            //         + ",\"productCount\":" + productCount
            //         + ",\"clientFirstName\":\"" + clientFirstName
            //         + "\",\"clientLastName\":\"" + clientLastName
            //         + "\",\"clientEmail\":\"" + clientEmail
            //         + "\",\"clientPhone\":\"" + clientPhone
            //         + "\",\"holdTimeout\":1728000,\"merchantTransactionType\":\"" + merchantTransactionType
            //         + "\"";
        }

        public string GenerateSignature()
        {
            var dataBytes = Encoding.UTF8.GetBytes(HashData());
            var keyStr = "flk3409refn54t54t*FNJRET";
            //  var keyStr = "0142af3ac1904c91992e7a3c9e8b1226ae6e2732";
            var key = Encoding.UTF8.GetBytes(keyStr);
            var hmac = new HMACMD5(key);
            var hashBytes = hmac.ComputeHash(dataBytes);
            var result = System.BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return result;
        }
    }

}
