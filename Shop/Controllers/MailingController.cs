using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shop.DataService;
using Shop.db.Entities;
using Shop.Filters;
using Shop.Models;
using Shop.Models.Builders;
using Shop.Modules;

namespace Shop.Controllers
{

    [InitializeSimpleMembership]
    public class MailingController : Controller
    {
        private IDataService dataService;
        private MenuBuilder menuBuilder { set; get; }

        public MailingController(IDataService dataService, ICartBuilder cartBuilder, IImagesPath imagesPath)
        {
            this.dataService = dataService;
            menuBuilder = new MenuBuilder(dataService, imagesPath);
        }

        public ActionResult AddEmailToMailing(string email)
        {
         var mailing = new Mailing();
         try
         {
             if (!string.IsNullOrEmpty(email))
             {
                 var mailingDB = dataService.GetMailingByEmail(email);
                 if (mailingDB==null)
                 {
                     mailing.email = email;
                     mailing.isActive = true;
                     dataService.AddOrUpdateMailing(mailing);
                 }
                 else
                 {
                     return Content("Вы уже подписались ранее!!!", "text/html");
                 }
             }
         }
         catch (Exception err)
         {
             //return Content(err.ToString(), "text/html");
         }
            return Content("Вы подписаны на честную рассылку акций и скидок", "text/html");
        }


          [Authorize(Roles = "Admin")]
        public ActionResult ListEmailMailingJson(bool? isActive)
        {
             var mailing = new List<Mailing>();
             try
             {
                 mailing = isActive.HasValue ? dataService.ListMailingByActive(isActive.Value) : dataService.ListMailings();
             }
             catch (Exception err)
             {
                 return Content(err.ToString(), "json/html");
             }
             var jsonSerialiser = new JavaScriptSerializer();
             var json = jsonSerialiser.Serialize(mailing.Select(it=>it.email));
       
            return Content(json, "json/html");
        }

          [Authorize(Roles = "Admin")]
          public ActionResult ListMailing(bool? isActive)
          {
              var mailing = new MailingModel();
              try
              {
                  mailing.mailings = isActive.HasValue ? dataService.ListMailingByActive(isActive.Value) : dataService.ListMailings();
                  mailing.menu = menuBuilder.BuildMenu();
                  mailing.topMenuItems = menuBuilder.BuildTopMenu();
              }
              catch (Exception err)
              {
                  return Content(err.ToString(), "json/html");
              }

              return View("ListMailing", mailing);
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
				Message.To.Add(string.Format("{0},{1}", MailTo, MailTo2));
				Message.Subject = MailSubject;
				Message.Body = MailBody;
				Smtp.SendMailAsync(Message);
			}
			catch (Exception err)
			{

			}
		}





	}
}
