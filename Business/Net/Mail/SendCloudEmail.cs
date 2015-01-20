using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using CodeScales.Http;
using CodeScales.Http.Entity;
using CodeScales.Http.Entity.Mime;
using CodeScales.Http.Methods;
using SkillBank.Site.Common;

namespace SkillBank.Site.Services.Net.Mail
{
    public class SendCloudEmail
    {
        /// <summary>
        /// 使用sendcloud的WebAPI， 详见a href= "http://sendcloud.sohu.com/api-doc/web-api-ref">
        ///  </summary>
        public static void WebAPISendMailTest()
        {
            HttpClient client = new HttpClient();
            //HttpPost postMethod = new HttpPost(new Uri("http://sendcloud.sohu.com/webapi/mail.send.xml"));
            HttpPost postMethod = new HttpPost(new Uri("http://sendcloud.sohu.com/webapi/mail.send_template.xml"));
            MultipartEntity multipartEntity = new MultipartEntity();
            postMethod.Entity = multipartEntity;
            //不同于登录SendCloud站点的帐号，您需要登录后台创建发信子帐号，使用子帐号和密码才可以进行邮件的发送。
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_user", "postmaster@skillbank.sendcloud.org"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_key", "FdflUbRKok8D3vkA"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "from", "contact@skill-bank.com"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "fromname", "skillbank"));
            //multipartEntity.AddBody(new StringBody(Encoding.UTF8, "to", "cathy.mu@qq.com"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", "欢迎加入技能银行，开始你的分享之旅"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "template_invoke_name", "Welcome_Mail"));
            //multipartEntity.AddBody(new StringBody(Encoding.UTF8, "substitution_vars", "{\"to\": [\"cathy.mu@qq.com\", \"elaine11@163.com\"], \"sub\" : { \"%name%\" : [\"约翰\", \"周妮妮\"]} }"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "substitution_vars", "{\"to\": [\"cathy.mu@qq.com\", \"cathy.mu@hotmail.com\"], \"sub\" : { \"%name%\" : [\"cathy\", \"周妮妮\"]} }"));
            //multipartEntity.AddBody(new StringBody(Encoding.UTF8, "html", "欢迎加入技能银行，开始你的分享之旅"));
            //FileInfo fileInfo = new FileInfo(@"c:\附件.pdf");

            //UTF8FileBody fileBody = new UTF8FileBody("file1", "", null);// UTF8FileBody fileBody = new UTF8FileBody("file1", "", fileInfo);
            // multipartEntity.AddBody(fileBody);

            HttpResponse response = client.Execute(postMethod);

            Console.WriteLine("Response Code: " + response.ResponseCode);
            var result = EntityUtils.ToString(response.Entity);
        }

        public static void SendWelcomeMail(string emailAddress, String memberName)
        {
            if (!String.IsNullOrEmpty(emailAddress))
            {
                SendTemplateMail(ConfigConstants.SendCloudMailSetting.Welcome_EmailSubject, ConfigConstants.SendCloudMailSetting.Welcome_TemplateName, "{\"to\": [\"" + emailAddress + "\"], \"sub\" : { \"%name%\" : [\"" + memberName + "\"]} }");
            }
        }

        public static void SendClassProvedMail(string emailAddress, String memberName, string className)
        {
            if (!String.IsNullOrEmpty(emailAddress))
            {
                SendTemplateMail(ConfigConstants.SendCloudMailSetting.ProveClass_EmailSubject, ConfigConstants.SendCloudMailSetting.ProveClass_TemplateName, "{\"to\": [\"" + emailAddress + "\"], \"sub\" : { \"%name%\" : [\"" + memberName + "\"], \"%classname%\" : [\"" + className + "\"]} }");
            }
        }

        public static void SendClassRejectMail(string emailAddress, String memberName, string className)
        {
            if (!String.IsNullOrEmpty(emailAddress))
            {
                SendTemplateMail(ConfigConstants.SendCloudMailSetting.RejectClass_EmailSubject, ConfigConstants.SendCloudMailSetting.RejectClass_TemplateName, "{\"to\": [\"" + emailAddress + "\"], \"sub\" : { \"%name%\" : [\"" + memberName + "\"], \"%classname%\" : [\"" + className + "\"]} }");
            }
        }

        public static void SendMessageReceiveMail(string emailAddress, String memberName, String senderName, String chatLink)
        {
            if (!String.IsNullOrEmpty(emailAddress))
            {
                SendTemplateMail(ConfigConstants.SendCloudMailSetting.MessageReceive_EmailSubject, ConfigConstants.SendCloudMailSetting.MessageReceive_TemplateName,"{\"to\": [\"" + emailAddress + "\"], \"sub\" : { \"%name%\" : [\"" + memberName + "\"], \"%id%\" : [\"" + senderName + "\"], \"%mlink%\" : [\"" + chatLink + "\"]} }");
            }
        }

        public static void SendOrderStatusUpdateMail(string emailAddress, String memberName, String className, String detailLink, string actionName)
        {
            if (!String.IsNullOrEmpty(emailAddress))
            {
                SendTemplateMail(ConfigConstants.SendCloudMailSetting.OrderStatusChanged_EmailSubject, ConfigConstants.SendCloudMailSetting.OrderStatusChanged_TemplateName, "{\"to\": [\"" + emailAddress + "\"], \"sub\" : { \"%name%\" : [\"" + memberName + "\"], \"%class%\" : [\"" + className + "\"], \"%action%\" : [\"" + actionName + "\"], \"%clink%\" : [\"" + detailLink + "\"]} }");
            }
        }

        /// <summary>
        /// User send cloud template send email 
        /// </summary>
        /// <param name="subjuect"></param>
        /// <param name="templateName"></param>
        /// <param name="parameters"></param>
        public static void SendTemplateMail(string subjuect, string templateName, String parameters)
        {
            HttpClient client = new HttpClient();
            HttpPost postMethod = new HttpPost(new Uri("http://sendcloud.sohu.com/webapi/mail.send_template.xml"));
            MultipartEntity multipartEntity = new MultipartEntity();
            postMethod.Entity = multipartEntity;

            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_user", ConfigConstants.SendCloudMailSetting.API_User));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_key", ConfigConstants.SendCloudMailSetting.API_Key));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "from", ConfigConstants.SendCloudMailSetting.Sender_Address));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "fromname", ConfigConstants.SendCloudMailSetting.Sender_Name));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", subjuect));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "template_invoke_name", templateName));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "substitution_vars", parameters));

            HttpResponse response = client.Execute(postMethod);
            //Console.WriteLine("Response Code: " + response.ResponseCode);
            var result = EntityUtils.ToString(response.Entity);
        }

        //For Send Group Email , One time only
        public static void SendTemplateMailByMailList()/*string subjuect, string templateName, String parameters*/
        {
            HttpClient client = new HttpClient();
            HttpPost postMethod = new HttpPost(new Uri("http://sendcloud.sohu.com/webapi/mail.send_template.xml"));
            MultipartEntity multipartEntity = new MultipartEntity();
            postMethod.Entity = multipartEntity;

            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_user", ConfigConstants.SendCloudMailSetting.API_ListUser));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_key", ConfigConstants.SendCloudMailSetting.API_Key));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "from", ConfigConstants.SendCloudMailSetting.Sender_Address));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "fromname", ConfigConstants.SendCloudMailSetting.Sender_Name));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "use_maillist", "true"));

            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "to", "monkey3@maillist.sendcloud.org"));//pioneer2@maillist.sendcloud.org//promofreecoin@skillbank.sendcloud.org//test@skillbank.sendcloud.org
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "template_invoke_name", "dotnet"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", "程序员，我们需要你啊"));

            //multipartEntity.AddBody(new StringBody(Encoding.UTF8, "to", "skillex22@maillist.sendcloud.org"));//pioneer2@maillist.sendcloud.org//promofreecoin@skillbank.sendcloud.org//test@skillbank.sendcloud.org
            //multipartEntity.AddBody(new StringBody(Encoding.UTF8, "template_invoke_name", "skill_exchange"));
            //multipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", "技能交换－新平台，新玩法"));
            ////multipartEntity.AddBody(new StringBody(Encoding.UTF8, "to", "pioneer5@maillist.sendcloud.org"));////promofreecoin@skillbank.sendcloud.org//test@skillbank.sendcloud.org
            //multipartEntity.AddBody(new StringBody(Encoding.UTF8, "template_invoke_name", "skill_pioneer"));
            //multipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", "不一样的校园生活从技能先锋开始"));
            HttpResponse response = client.Execute(postMethod);
            //Console.WriteLine("Response Code: " + response.ResponseCode);
            var result = EntityUtils.ToString(response.Entity);
        }


    }
}
