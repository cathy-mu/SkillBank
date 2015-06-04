using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.Common;

namespace SkillBank.Site.Services.Net.SMS
{
    public static class YunPianSMS
    {
        /**
		* 服务http地址
		*/
        private static string BASE_URI = "http://yunpian.com";
        /**
        * 服务版本号
        */
        private static string VERSION = "v1";
        /**
        * 查账户信息的http地址
        */
        private static string URI_GET_USER_INFO = BASE_URI + "/" + VERSION + "/user/get.json";
        /**
        * 通用接口发短信的http地址
        */
        private static string URI_SEND_SMS = BASE_URI + "/" + VERSION + "/sms/send.json";
        /**
        * 模板接口短信接口的http地址
        */
        private static string URI_TPL_SEND_SMS = BASE_URI + "/" + VERSION + "/sms/tpl_send.json";

        private static string YunPianApiKey = "4eaffd69cbe3cd2e7382f207a11b0229";
        
        /**
        * 取账户信息
        * @return json格式字符串
        */
        public static string getUserInfo(string apikey)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI_GET_USER_INFO + "?apikey=" + apikey);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }
        /**
        * 通用接口发短信
        * @param text　短信内容　
        * @param mobile　接受的手机号
        * @return json格式字符串
        */
        public static string sendSms(string apikey, string text, string mobile)
        {
            //注意：参数必须进行Uri.EscapeDataString编码。以免&#%=等特殊符号无法正常提交
            string parameter = "apikey=" + apikey + "&text=" + Uri.EscapeDataString(text) + "&mobile=" + mobile;
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI_SEND_SMS);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(parameter);//这里编码设置为utf8
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        /**
        * 模板接口发短信
        * @param tpl_id 模板id
        * @param tpl_value 模板变量值
        * @param mobile　接受的手机号
        * @return json格式字符串
        */
        public static string tplSendSms(string apikey, long tpl_id, string tpl_value, string mobile)
        {
            string encodedTplValue = Uri.EscapeDataString(tpl_value);
            string parameter = "apikey=" + apikey + "&tpl_id=" + tpl_id + "&tpl_value=" + encodedTplValue + "&mobile=" + mobile;
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI_TPL_SEND_SMS);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(parameter);//这里编码设置为utf8
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }


        public static void SendMobileValidationCodeSms(string mobile, string code)
        {
            //设置您要发送的内容
            //string text = "您的验证码是1234【云片网】";
            //string text = "欢迎使用#app#，您的手机验证码是#code#。本条信息无需回复【#company#】";


            ////查账户信息
            //System.Console.WriteLine(getUserInfo(apikey));

            ////调用通用接口发短信
            //System.Console.WriteLine(sendSms(apikey, text, mobile));

            //调用模板接口发短信

            long tpl_id = 6; //使用模板1，对应的模板内容为：您的验证码是#code#【#company#】
            //注意：参数必须进行Uri.EscapeDataString编码。以免&#%=等特殊符号无法正常提交
            string tpl_value = "#code#=" + Uri.EscapeDataString(code) + "&#company#=" + Uri.EscapeDataString("技能银行") + "&#app#=" + Uri.EscapeDataString("技能银行");
            var result = tplSendSms(YunPianApiKey, tpl_id, tpl_value, mobile);
        }

        /// <summary>
        /// 发送订单状态改变短消息
        /// </summary>
        /// <param name="statusType"></param>
        /// <param name="mobile"></param>
        /// <param name="className"></param>
        /// <param name="link"></param>
        public static void SendOrderUpdateSms(Byte statusType, string mobile, String className)//, String link
        {
            long tpl_id = 0;
            String pageUrl = default(String);
            switch (statusType)
            {
                //add order
                case 1: tpl_id = 752781;//hello，老师，有人在技能银行跟你预定了课程《#class#》哦，快来回复吧#link#
                    pageUrl = Constants.PageURL.MobileTeachPage;
                    break;
                //reject order
                case 2: tpl_id = 781417;//您预订的《#class#》未被接受。请务必先和老师沟通过后再来订课，来完善你的自我介绍也会对订课有帮助哦。#link#
                    pageUrl = Constants.PageURL.MobileLearnPage;
                    break;
                //cancle order
                case 3: tpl_id = 0;//cancle
                    pageUrl = Constants.PageURL.MobileTeachPage;
                    break;
                //accept order
                case 4: tpl_id = 781423;//恭喜，老师已经接受了您预订的《#class#》,快来看一下吧#link#
                    pageUrl = Constants.PageURL.MobileLearnPage;
                    break;
                //refund order
                case 6: tpl_id = 781429;//您好，在您教授的《#class#》中出现了退币申请
                    pageUrl = Constants.PageURL.MobileTeachPage;
                    break;
                //refund prove
                case 7: tpl_id = 781437;//您好，您的《#class#》退币请求已被接受，退回的课币已经返回您的账户
                    pageUrl = Constants.PageURL.MobileLearnPage;
                    break;
                //refund reject
                case 8: tpl_id = 781443;//您好，您的《#class#》退币请求未被接受
                    pageUrl = Constants.PageURL.MobileLearnPage;
                    break;
                //order confirm
                case 9: tpl_id = 781451;//恭喜，学生已经对你的《#class#》支付了课币并做出了评价，快来看一下吧#link#
                    pageUrl = Constants.PageURL.MobileTeachPage;
                    break;

                default:
                    break;
            }
            if (!tpl_id.Equals(0))
            {
                string tpl_value = "#class#=" + Uri.EscapeDataString(className) + "&#link#=" + Uri.EscapeDataString(pageUrl);
                var result = tplSendSms(YunPianApiKey, tpl_id, tpl_value, mobile);
            }
        }

        /// <summary>
        /// 发送课程批准，拒绝短消息
        /// </summary>
        /// <param name="isProve"></param>
        /// <param name="mobile"></param>
        /// <param name="className"></param>
        /// <param name="link"></param>
        public static void SendClassProveSms(Boolean isProve, string mobile, String className, String link)
        {
            long tpl_id;
            if (isProve)
            {
                tpl_id = 781493;
            }
            else
            {
                tpl_id = 781509;
            }

            string tpl_value = "#class#=" + Uri.EscapeDataString(className) + "&#link#=" + Uri.EscapeDataString(link);
            var result = tplSendSms(YunPianApiKey, tpl_id, tpl_value, mobile);
        }

        /// <summary>
        /// 收到新私信的短消息
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="name"></param>
        /// <param name="link"></param>
        public static void SendNewMessageSMS(String mobile, String name, String link)
        {
            long tpl_id = 792823;
            string tpl_value = "#name#=" + name + "&#link#=" + Uri.EscapeDataString(link);
            var result = tplSendSms(YunPianApiKey, tpl_id, tpl_value, mobile);
        }


    }
}