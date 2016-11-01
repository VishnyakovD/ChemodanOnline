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



       public void ipmonitor(string num = "-1")
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
