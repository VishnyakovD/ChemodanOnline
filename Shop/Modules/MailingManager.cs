﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Shop.Modules
{
    public interface IMailingManager
    {
        void SendMailNewOrder();
        void SendMailNewOrderOneClick();
        //string[] mails, string subject, string body
    }

    public class MailingManager : IMailingManager
    {
        public MailingManager()
        {

        }

        public void SendMailNewOrder()
        {
            try
            {
                var sMTPHost = WebConfigurationManager.AppSettings["SMTPHost"];
                var sMTPPort = 25;
                int.TryParse(WebConfigurationManager.AppSettings["SMTPPort"], out sMTPPort);
                var enableSsl = false;
                bool.TryParse(WebConfigurationManager.AppSettings["EnableSsl"], out enableSsl);
                var useDefaultCredentials = true;
                bool.TryParse(WebConfigurationManager.AppSettings["UseDefaultCredentials"], out useDefaultCredentials);
                var userName = WebConfigurationManager.AppSettings["UserName"];
                var userPassword = WebConfigurationManager.AppSettings["UserPassword"];
                var mailFrom = WebConfigurationManager.AppSettings["MailFrom"];
                var mailTo = WebConfigurationManager.AppSettings["MailTo"];
                //var MailTo2 = WebConfigurationManager.AppSettings["MailTo2"];
                var mailSubject = WebConfigurationManager.AppSettings["MailSubject"];
                var mailBody = WebConfigurationManager.AppSettings["MailBody"];
                var smtp = new SmtpClient(sMTPHost, sMTPPort);

                smtp.EnableSsl = enableSsl;
                smtp.UseDefaultCredentials = useDefaultCredentials;
                smtp.Credentials = new NetworkCredential(userName, userPassword);
                var message = new MailMessage();
                message.From = new MailAddress(mailFrom);
                // var mails = new MailAddressCollection();
                message.To.Add(string.Format("{0}", mailTo));
                message.Subject = mailSubject;
                message.Body = mailBody;
                smtp.SendMailAsync(message);
            }
            catch (Exception err)
            {

            }
        }

        public void SendMailNewOrderOneClick()
        {
            try
            {
                var sMTPHost = WebConfigurationManager.AppSettings["SMTPHost"];
                var sMTPPort = 25;
                int.TryParse(WebConfigurationManager.AppSettings["SMTPPort"], out sMTPPort);
                var enableSsl = false;
                bool.TryParse(WebConfigurationManager.AppSettings["EnableSsl"], out enableSsl);
                var useDefaultCredentials = true;
                bool.TryParse(WebConfigurationManager.AppSettings["UseDefaultCredentials"], out useDefaultCredentials);
                var userName = WebConfigurationManager.AppSettings["UserName"];
                var userPassword = WebConfigurationManager.AppSettings["UserPassword"];
                var mailFrom = WebConfigurationManager.AppSettings["MailFrom"];
                var mailTo = WebConfigurationManager.AppSettings["MailTo"];
                //var MailTo2 = WebConfigurationManager.AppSettings["MailTo2"];
                var mailSubject = WebConfigurationManager.AppSettings["MailSubjectOneClick"];
                var mailBody = WebConfigurationManager.AppSettings["MailBodyOneClick"];
                var smtp = new SmtpClient(sMTPHost, sMTPPort);

                smtp.EnableSsl = enableSsl;
                smtp.UseDefaultCredentials = useDefaultCredentials;
                smtp.Credentials = new NetworkCredential(userName, userPassword);
                var message = new MailMessage();
                message.From = new MailAddress(mailFrom);
                // var mails = new MailAddressCollection();
                message.To.Add(string.Format("{0}", mailTo));
                message.Subject = mailSubject;
                message.Body = mailBody;
                smtp.SendMailAsync(message);
            }
            catch (Exception err)
            {

            }
        }
    }

    public static class SendMailEx
    {
        public static Task SendMailExAsyncOneClick(CancellationToken token = default(CancellationToken))
        {

            var sMTPHost = WebConfigurationManager.AppSettings["SMTPHost"];
            var sMTPPort = 25;
            int.TryParse(WebConfigurationManager.AppSettings["SMTPPort"], out sMTPPort);
            var enableSsl = false;
            bool.TryParse(WebConfigurationManager.AppSettings["EnableSsl"], out enableSsl);
            var useDefaultCredentials = true;
            bool.TryParse(WebConfigurationManager.AppSettings["UseDefaultCredentials"], out useDefaultCredentials);
            var userName = WebConfigurationManager.AppSettings["UserName"];
            var userPassword = WebConfigurationManager.AppSettings["UserPassword"];

            var mailFrom = WebConfigurationManager.AppSettings["MailFrom"];
            var mailTo = WebConfigurationManager.AppSettings["MailTo"];
            //var MailTo2 = WebConfigurationManager.AppSettings["MailTo2"];
            var mailSubject = WebConfigurationManager.AppSettings["MailSubjectOneClick"];
            var mailBody = WebConfigurationManager.AppSettings["MailBodyOneClick"];
            var smtp = new SmtpClient(sMTPHost, sMTPPort);

            smtp.EnableSsl = enableSsl;
            smtp.UseDefaultCredentials = useDefaultCredentials;
            smtp.Credentials = new NetworkCredential(userName, userPassword);

            var message=new MailMessage(mailFrom, mailTo);
            message.BodyEncoding = Encoding.UTF8;
            message.Body = mailBody;
            message.Subject = mailSubject;
            message.SubjectEncoding = Encoding.UTF8; 


            return Task.Run(() => SendMailExImplAsync(smtp, message, token));
        }


        public static Task SendMailExAsyncOrder(CancellationToken token = default(CancellationToken))
        {

            var sMTPHost = WebConfigurationManager.AppSettings["SMTPHost"];
            var sMTPPort = 25;
            int.TryParse(WebConfigurationManager.AppSettings["SMTPPort"], out sMTPPort);
            var enableSsl = false;
            bool.TryParse(WebConfigurationManager.AppSettings["EnableSsl"], out enableSsl);
            var useDefaultCredentials = true;
            bool.TryParse(WebConfigurationManager.AppSettings["UseDefaultCredentials"], out useDefaultCredentials);
            var userName = WebConfigurationManager.AppSettings["UserName"];
            var userPassword = WebConfigurationManager.AppSettings["UserPassword"];

            var mailFrom = WebConfigurationManager.AppSettings["MailFrom"];
            var mailTo = WebConfigurationManager.AppSettings["MailTo"];
            //var MailTo2 = WebConfigurationManager.AppSettings["MailTo2"];
            var mailSubject = WebConfigurationManager.AppSettings["MailSubject"];
            var mailBody = WebConfigurationManager.AppSettings["MailBody"];
            var smtp = new SmtpClient(sMTPHost, sMTPPort);

            smtp.EnableSsl = enableSsl;
            smtp.UseDefaultCredentials = useDefaultCredentials;
            smtp.Credentials = new NetworkCredential(userName, userPassword);

            var message=new MailMessage(mailFrom, mailTo);
            message.BodyEncoding = Encoding.UTF8;
            message.Body = mailBody;
            message.Subject = mailSubject;
            message.SubjectEncoding = Encoding.UTF8; 


            return Task.Run(() => SendMailExImplAsync(smtp, message, token));
        }

        private static async Task SendMailExImplAsync(
            System.Net.Mail.SmtpClient client,
            System.Net.Mail.MailMessage message,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var tcs = new TaskCompletionSource<bool>();
            System.Net.Mail.SendCompletedEventHandler handler = null;
            Action unsubscribe = () => client.SendCompleted -= handler;

            handler = async (s, e) =>
            {
                unsubscribe();

                // a hack to complete the handler asynchronously
                await Task.Yield();

                if (e.UserState != tcs)
                    tcs.TrySetException(new InvalidOperationException("Unexpected UserState"));
                else if (e.Cancelled)
                    tcs.TrySetCanceled();
                else if (e.Error != null)
                    tcs.TrySetException(e.Error);
                else
                    tcs.TrySetResult(true);
            };

            client.SendCompleted += handler;
            try
            {
                client.SendAsync(message, tcs);
                using (token.Register(() => client.SendAsyncCancel(), useSynchronizationContext: false))
                {
                    await tcs.Task;
                }
            }
            finally
            {
                unsubscribe();
            }
        }
    }
}
