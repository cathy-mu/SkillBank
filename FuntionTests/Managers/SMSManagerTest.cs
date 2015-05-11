using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Services.Net.SMS;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class SMSManagerTest
    {
        

        [TestMethod]
        public void ShouldSendVerificationSMS()
        {
            //YunPianSMS.SendMobileValidationCodeSms("13917782601", "123456");//移动的周同学
            //YunPianSMS.SendMobileValidationCodeSms("18501654623", "123789");//联通的林同学
            //YunPianSMS.SendMobileValidationCodeSms("18917566169", "987654");//联通的陆同学
        }

        [TestMethod]
        public void ShouldSendOrderSMS()
        {
            YunPianSMS.SendOrderUpdateSms(1,"13917782601", "如何寻找最近的公交车站", "http://m.skillbank.cn/m");
            //YunPianSMS.SendOrderUpdateSms("13564813923", "市场营销", "http://m.skillbank.cn/m");
        }

    }
}