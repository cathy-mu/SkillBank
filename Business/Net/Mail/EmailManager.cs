#region
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Collections.Generic;

using SkillBank.Site.Common;
using SkillBank.Site.Services.CacheProviders;
//using EF.Frameworks.Common.FactoryEF;
//using EF.Frameworks.Orpheus.ContentManagementEF;
//using EF.Frameworks.Orpheus.ResourcesEF;
//using EFSchools.Englishtown.ContentConfigurationET;
//using EFSchools.Englishtown.ContentManagementET;
//using EFSchools.Englishtown.NetET.MailET;


#endregion

namespace SkillBank.Site.Services
{
    /// <summary>
    /// </summary>
    public static class EmailManager
    {
        #region Private fields



        #endregion

        #region Constructors

        //static EmailManager()
        //{
        //    _txtRsxMgr = Factory<TextResourceManager>.Create();
        //}

        #endregion

        #region Public methods

        //<summary>
        //Send an arbitrary email base don a template
        //</summary>
        public static void SendMessage(string recipientEmail, string recipientName, string subject, string templateName, Dictionary<string, string> templateParams = null)
        {
            SendMessage(recipientEmail, recipientName,default(String), default(String), "cs", default(String), subject, templateName, templateParams);
        }


        //<summary>
        //Send an arbitrary email base don a template
        //</summary>
        public static void SendMessage(string recipientEmail, string recipientName, string senderEmail, string senderName, string language, string cacheSvr, string subject, string templateName, Dictionary<string, string> templateParams = null)
        {
            //Byte siteVersion = 1;
            //Use website default setting if sender leave as blank
            var from = new MailAddress(string.IsNullOrWhiteSpace(senderEmail) ? ConfigConstants.MailSetting.FromAddress : senderEmail, string.IsNullOrWhiteSpace(senderName) ? ConfigConstants.MailSetting.SenderName : senderName);
            var to = new MailAddress(recipientEmail, recipientName);

            Dictionary<string, string> userValues = new Dictionary<string, string>();

            string body = "";// MailTemplatesProvider.GetTemplate(templateName, siteVersion);

            //if (templateParams != null)
            //{
            //    body = templateParams.Aggregate(body, (current, param) => current.Replace(string.Format("{{{0}}}", param.Key), param.Value));
            //}
            foreach (var paraItem in templateParams)
            {
                body = body.Replace("{" + paraItem.Key + "}", paraItem.Value);
            }
            subject = subject.Replace('\r', ' ').Replace('\n', ' ');

            MailMessage message = new MailMessage(from, to)
                                      {
                                          IsBodyHtml = true,
                                          Subject = subject,
                                          Body = body,
                                          BodyEncoding = System.Text.Encoding.UTF8,
                                          SubjectEncoding = System.Text.Encoding.UTF8
                                          //,Priority = MailPriority.High
                                      };

            SendMessage(message);
        }
                       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageParams"></param>
        public static void SendMessage(MailMessage message)
        {
            message.IsBodyHtml = true;
            //message.BodyEncoding = System.Text.UTF8Encoding.Unicode;

            var Client = new SmtpClient
             {
                 Host = ConfigConstants.MailSetting.ServerHost,
                 Port = ConfigConstants.MailSetting.ServerPort,
                 //EnableSsl = true,
                 UseDefaultCredentials = false,
                 DeliveryMethod = SmtpDeliveryMethod.Network,
                 Credentials = new NetworkCredential(ConfigConstants.MailSetting.FromAddress, ConfigConstants.MailSetting.Password)
             };

            try
            {

                Client.Send(message);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                var exMsg = ex.Message;

                //for (int i = 0; i < ex.InnerExceptions.Length; i++)
                //{
                //    SmtpStatusCode status = ex.StatusCode;
                //    if (status == SmtpStatusCode.MailboxBusy ||
                //     status == SmtpStatusCode.MailboxUnavailable)
                //    {

                //    }
                //}
            }
            finally
            {
                message.Dispose();
            }


        }

        #endregion

    }
}