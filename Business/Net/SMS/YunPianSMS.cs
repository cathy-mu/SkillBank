using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            //修改为您的apikey
            string apikey = "4eaffd69cbe3cd2e7382f207a11b0229";
            //修改为您要发送的手机号
            //string mobile = "188xxxxxxxx";
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
            var result = tplSendSms(apikey, tpl_id, tpl_value, mobile);
        }

    }
}