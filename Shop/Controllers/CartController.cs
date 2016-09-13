using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Shop.DataService;
using Shop.db.Entities;
using Shop.Models;
using Shop.Models.Builders;

namespace Shop.Controllers
{
    public class CartController : Controller
    {
        public IDataService dataService;
        private ICartBuilder CartBuilder;
        public CartController(IDataService dataService, ICartBuilder cartBuilder)
        {
            this.dataService = dataService;
            this.CartBuilder = cartBuilder;
        }

        public ActionResult AddToCart(long idSku, string num="-1")
        {
         var cartModel = new Cart();
         try
         {
             if (Session["shoppingCart"] != null)
             {
                 cartModel = (Cart)Session["shoppingCart"];
                 if (cartModel.listSku == null)
                 {
                     cartModel.listSku=new List<CartItem>();
                 }

                 if (cartModel.listSku.Any(sku => sku.idSku == idSku))
                 {
                     foreach (CartItem t in cartModel.listSku.Where(t => t.idSku==idSku))
                     {
                         t.count += 1;
                     }
                 }
                 else
                 {
                     var sku = dataService.GetSkuById(idSku);
                     cartModel.listSku.Add(new CartItem(){count = 1,idSku = sku.id,price = sku.price,priceAct = sku.priceAct});
                 }

             }
             else
             {
                 var sku = dataService.GetSkuById(idSku);
                 cartModel.listSku.Add(new CartItem() { count = 1, idSku = sku.id, price = sku.price, priceAct = sku.priceAct });
             }
             cartModel.totalCount = cartModel.listSku.Sum(sku => sku.count);
             cartModel.state = null;
            //cartModel= dataService.AddorUpdateSkuToCart(cartModel);

             Session["shoppingCart"] = cartModel;
         }
         catch (Exception err)
         {
             return Content(err.ToString(), "text/html");
         }
            LoadContent(num);
            return Content(cartModel.totalCount.ToString(), "text/html");
        }

        public ActionResult AddToBuyCart(long idSku)//Добавить 1шт на странице завершения заказа
        {
            var cartModel = new Cart();
            var cart = new CartModel();
            try
            {
                if (Session["shoppingCart"] != null)
                {
                    cartModel = (Cart)Session["shoppingCart"];
                    if (cartModel.listSku == null)
                    {
                        cartModel.listSku = new List<CartItem>();
                    }

                    if (cartModel.listSku.Any(sku => sku.idSku == idSku))
                    {
                        foreach (CartItem t in cartModel.listSku.Where(t => t.idSku == idSku))
                        {
                            t.count += 1;
                        }
                    }
                }
                cartModel.totalCount = cartModel.listSku.Sum(sku => sku.count);
                cartModel.state = null;
                //cartModel = dataService.AddorUpdateSkuToCart(cartModel);
                cart = getCartModel(cartModel);
                Session["shoppingCart"] = cartModel;
            }
            catch (Exception err)
            {
                return Content(err.ToString(), "text/html");
            }
            return PartialView("ListSkuInCart", cart);
        }

        public ActionResult RemoveSkuFromCart(long idSku)
        {
            var onDelete = false;
            var cartModel = new Cart();
            var cart = new CartModel();
            try
            {
                if (Session["shoppingCart"] != null)
                {
                    cartModel = (Cart)Session["shoppingCart"];
                    if (cartModel.listSku == null)
                    {
                        cartModel.listSku = new List<CartItem>();
                    }

                    foreach (CartItem t in cartModel.listSku.Where(t => t.idSku == idSku))
                    {
                        t.count -= 1;
                        if (t.count < 1)
                        {
                            onDelete = true;
                        }
                    }
                    if (onDelete)
                    {
                        cartModel.listSku.Remove(cartModel.listSku.First(sku => sku.idSku == idSku));
                    }

                }
                cartModel.totalCount = cartModel.listSku.Sum(sku => sku.count);
                cartModel.state = null;
                //cartModel = dataService.AddorUpdateSkuToCart(cartModel);
                cart = getCartModel(cartModel);
                Session["shoppingCart"] = cartModel;
            }
            catch (Exception err)
            {
                return Content(err.ToString(), "text/html");
            }
            return PartialView("ListSkuInCart", cart);
        }

       private CartModel getCartModel(Cart model)
        {
            return CartBuilder.Build(model);
        }

       public ActionResult BuyCart()
        {
         var cartAll = new CartModel();
         var cartModel =new Cart();
         try
         {
             if (Session["shoppingCart"] != null)
             {
                 cartModel = (Cart) Session["shoppingCart"];
             }
             cartAll= getCartModel(cartModel);
         }
         catch (Exception err)
         {
            
         }
         return View("BuyCart", cartAll);
        }

       public ActionResult ComfirmCart(string nameClient, string phone, string comment)
        {
         var cartAll = new CartModel();
         var cartModel =new Cart();
         try
         {
             if (string.IsNullOrEmpty(nameClient) || string.IsNullOrEmpty(phone))
             {
                 return View("BuyCart", getCartModel((Cart)Session["shoppingCart"]));
             }

             if (Session["shoppingCart"] != null)
             {
                 cartModel = (Cart)Session["shoppingCart"];
                 cartModel.nameClient = nameClient;
                 cartModel.phone = phone;
                 cartModel.state = dataService.ListCartState().Find(i => i.value == 1);
                 cartModel.createDate = DateTime.Now;
                 cartModel.comment = comment;
                 cartModel.totalCount = cartModel.listSku.Sum(sku => sku.count);
                 cartAll.cart.id= dataService.AddorUpdateSkuToCart(cartModel).id;
                 //cartAll = CartBuilder.Build(cartModel);
             }
             else
             {
                 return Content("Сначала нужно выбрать товары!", "text/html");
             }
         }
         catch (Exception err)
         {
             return Content("Ошибка "+err.Message, "text/html");
         }
           try
           {
               var oThread = new Thread(new ThreadStart(SendMail));
               oThread.Start();
               oThread.Join();
           }
           catch (Exception)
           {
               
           }
           Session["shoppingCart"] = null;
           return Content("<h4>Заказ передан ответственным сотрудникам! Ожидайте звонка оператора для подтверждения заказа! Номер заказа : <b style='font-size:120%'>" + cartAll.cart.id.ToString() + "</b></h4>", "text/html");
        }

       private void SendMail()
       {
           try
           {
               var SMTPHost = WebConfigurationManager.AppSettings["SMTPHost"];
               var SMTPPort = 25;
               int.TryParse(WebConfigurationManager.AppSettings["SMTPPort"], out SMTPPort);
               var EnableSsl = false;
               bool.TryParse(WebConfigurationManager.AppSettings["EnableSsl"], out EnableSsl);
               var UseDefaultCredentials = true;
               bool.TryParse(WebConfigurationManager.AppSettings["UseDefaultCredentials"], out UseDefaultCredentials);
               var UserName = WebConfigurationManager.AppSettings["UserName"];
               var UserPassword = WebConfigurationManager.AppSettings["UserPassword"];
               var MailFrom = WebConfigurationManager.AppSettings["MailFrom"];
               var MailTo = WebConfigurationManager.AppSettings["MailTo"];
               var MailTo2 = WebConfigurationManager.AppSettings["MailTo2"];
               var MailSubject = WebConfigurationManager.AppSettings["MailSubject"];
               var MailBody = WebConfigurationManager.AppSettings["MailBody"];
               var Smtp = new SmtpClient(SMTPHost, SMTPPort);

               Smtp.EnableSsl = EnableSsl;
               Smtp.UseDefaultCredentials = UseDefaultCredentials;
               Smtp.Credentials = new NetworkCredential(UserName, UserPassword);
               var Message = new MailMessage();
               Message.From = new MailAddress(MailFrom);
               var mails = new MailAddressCollection();
               Message.To.Add(string.Format("{0},{1}",MailTo,MailTo2));
               Message.Subject = MailSubject;
               Message.Body = MailBody;
               Smtp.SendMailAsync(Message);
           }
           catch (Exception err)
           {
               
           }
       }


          [Authorize(Roles = "Admin")]
        public ActionResult ListCarts(DateTime? start, DateTime? end, int? stateValue)
       {
           ViewBag.isHideLeftMenu = true;
           var model = new ListCartsModel();
           try
           {
               if (!start.HasValue || !end.HasValue || !stateValue.HasValue)
               {
                   model = CartBuilder.BuildListCarts(dataService.GetCartsByDateAndStatus(DateTime.Now.Date, DateTime.Now.Date, 1), DateTime.Now.Date, DateTime.Now.Date, 1);
               }
               else
               {
                   model = CartBuilder.BuildListCarts(dataService.GetCartsByDateAndStatus(start.Value, end.Value, stateValue.Value), start.Value, end.Value, stateValue.Value);
               }
           }
           catch (Exception err)
           {

           }
           return View("ListCarts", model);
       }

           [Authorize(Roles = "Admin")]
       public ActionResult GetCart(long idCart)
       {
           var cartAll = new CartModel();
           var cartModel = new Cart();
           try
           {
             cartModel= dataService.GetCartById(idCart);
               if (cartModel!=null)
               {
                   cartAll = getCartModel(cartModel);  
               }
           }
           catch (Exception err)
           {

           }
           return PartialView("finalCartPartial", cartAll);
       }

          [Authorize(Roles = "Admin")]
       public ActionResult EditCart(long idCart, string nameClient, string phone, string comment, string city, string street, string numHome, string numFlat, string email,int stateValue)
       {
           var cartAll = new CartModel();
           var cartModel = new Cart();
           try
           {
               if (string.IsNullOrEmpty(nameClient) || string.IsNullOrEmpty(phone))
               {
                   return Content("Ошибка ''Имя клиента'' и ''Телефон'' - обязательные поля", "text/html");
               }
                   cartModel.id = idCart;
                   cartModel.nameClient = nameClient;
                   cartModel.phone = phone;
                   cartModel.state = dataService.ListCartState().Find(i => i.value == stateValue);
                   cartModel.comment = comment;
                   cartModel.city = city;
                   cartModel.street = street;
                   cartModel.numHome = numHome;
                   cartModel.numFlat = numFlat;
                   cartModel.email = email;
                 //cartModel.totalCount = cartModel.listSku.Sum(sku => sku.count);
                   dataService.AddorUpdateSkuToCart(cartModel);
           }
           catch (Exception err)
           {
               return Content("Ошибка " + err.Message, "text/html");
           }
           return Content("Заказ сохранен", "text/html");
       }

       public ActionResult AddToCartByIdCart(long idCart, long idSku)
       {
           var cartAll = new CartModel();
           var cartModel = new Cart();
           try
           {
               cartModel = dataService.GetCartById(idCart);
               if (cartModel != null)
               {
                   if (cartModel.listSku.Any(sku => sku.idSku == idSku))
                   {
                       foreach (CartItem t in cartModel.listSku.Where(t => t.idSku == idSku))
                       {
                           t.count += 1;
                       }
                   }
               }
               cartModel.totalCount = cartModel.listSku.Sum(sku => sku.count);
               cartModel = dataService.AddorUpdateSkuToCart(cartModel);
               cartAll = getCartModel(cartModel);
           }
           catch (Exception err)
           {

           }
           return PartialView("ListSkuInCartFinalPartial", cartAll);
       }

       public ActionResult RemoveSkuFromCartByIdCart(long idCart, long idSku)
       {
           var cartAll = new CartModel();
           var cartModel = new Cart();
           var onDelete = false;
           try
           {
               cartModel = dataService.GetCartById(idCart);
               if (cartModel != null)
               {
                   if (cartModel.listSku.Any(sku => sku.idSku == idSku))
                   {
                       foreach (CartItem t in cartModel.listSku.Where(t => t.idSku == idSku))
                       {
                           t.count -= 1;
                           if (t.count<1)
                           {
                               onDelete = true;
                           }
                       }
                       if (onDelete)
                       {
                           cartModel.listSku.Remove(cartModel.listSku.First(sku => sku.idSku == idSku));
                           dataService.RemoveSkuCartId(idCart, idSku);
                       }
                   }
               }
               cartModel.totalCount = cartModel.listSku.Sum(sku => sku.count);
               cartModel = dataService.AddorUpdateSkuToCart(cartModel);
               cartAll = getCartModel(cartModel);
           }
           catch (Exception err)
           {

           }
           return PartialView("ListSkuInCartFinalPartial", cartAll);
       }


       public ActionResult CountSkuOnCart()
       {
           var cartModel = new Cart();
           int count = 0;
           try
           {
               if (Session["shoppingCart"] != null)
               {
                   cartModel = (Cart)Session["shoppingCart"];
                 count=  cartModel.listSku.Sum(sku => sku.count);
               }
           }
           catch (Exception err)
           {
               return Content(err.ToString(), "text/html");
           }
           return Content(count.ToString(), "text/html");
       }

       public void LoadContent(string num = "-1")
       {
           try
           {
               if (Session["clid"]!=null)
               {
                   num = Session["clid"].ToString();
               }
             
               var request = Request;
               var ip = string.Empty;
               var page = string.Empty;
               var date = DateTime.Now;
               if (request != null)
               {
                   ip = request.UserHostAddress;
                   if (request.Url != null) page =  request.Url.PathAndQuery;
                   dataService.AddIPToMonitor(new IPMonitor() { date = date, ip = ip, page = page, clientId = num });

               }
           }
           catch (Exception)
           {

           }
       }


    }
}
