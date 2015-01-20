using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Services.CacheProviders;


namespace SkillBank.FunctionTests
{
    [TestClass]
    public class MessageManagerTests
    {
        private int _memberId;
        private int _invalidMemberId;
        BlurbsProvider _blurbProvider;
        MessageManager _mgr;

        [TestInitialize]
        public void TestInitialize()
        {
            _memberId = 1;
            _invalidMemberId = 9999;
            MessageRepository rep = new MessageRepository();
            _mgr = new MessageManager(rep);
            _blurbProvider = new BlurbsProvider(new BlurbsRepository());
        }

        [TestMethod]
        public void Should_GetUnReadMessageNum()
        {
            var result = _mgr.GetMessagesUnReadNum(_memberId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Should_SendWelcomeMessage()
        {
            String message = _blurbProvider.GetBlurb(284, "cs", 1);
            int toMemberId = 2;
            //_mgr.AddMessage(Constants.BizConfig.AdminMemberId, memberId, message);
            _mgr.AddMessage(_memberId, toMemberId, message);
        }
    }
}