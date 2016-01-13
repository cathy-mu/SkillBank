using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Assert = NUnit.Framework.Assert;
//using NUnit.Mocks;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class UserRepositoryTest
    {
        private const int memberId = 1;
        private const String socialAccount = "cathy.mu@hotmail.com";
        private const String socialOpenId = "cathymuabcdefg";
        private const String invalidSocialAccount = "test.test@skillbank.com";
        private const int incorrectUserId = 99999999;
        MemberRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new MemberRepository();
        }

        [TestMethod]
        public void Should_GetMemberInfo_ByMemberId()
        {
            Byte loadType = (Byte)Enums.DBAccess.MemberLoadType.ByMemberId;
            var result = _repository.GetMemberInfo(loadType, memberId, 0);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Should_GetMemberInfo_Correctly_ByMemberId()
        {
            Byte loadType = (Byte)Enums.DBAccess.MemberLoadType.ByMemberId;
            var result = _repository.GetMemberInfo(loadType, memberId, 0);
            result.MemberId.AssertIsGreaterThan(0);
            Assert.AreEqual(memberId, result.MemberId);
            Assert.IsNotNull(result.CreatedDate);
        }

        [TestMethod]
        public void ShouldNot_GetMemberInfo_ByIncorrectUserId()
        {
            Byte loadType = (Byte)Enums.DBAccess.MemberLoadType.ByMemberId;
            var result = _repository.GetMemberInfo(loadType, incorrectUserId, 0);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Should_GetMemberInfo_BySocialAccount()
        {
            Byte loadType = (Byte)Enums.DBAccess.MemberLoadType.BySocialAccount;
            var result = _repository.GetMemberInfo(loadType, socialAccount, (Byte)Enums.SocialTpye.Sina);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Should_GetMemberInfo_Correctly_BySocialAccount()
        {
            Byte loadType = (Byte)Enums.DBAccess.MemberLoadType.BySocialAccount;
            var result = _repository.GetMemberInfo(loadType, socialAccount, (Byte)Enums.SocialTpye.Sina);
            result.MemberId.AssertIsGreaterThan(0);
            Assert.IsNotNull(result.SocialAccount);
            Assert.AreEqual(socialAccount, result.SocialAccount);
        }
        
        
        [TestMethod]
        public void ShouldNot_GetMemberInfo_ByIncorrectSocialAccount()
        {
            Byte loadType = (Byte)Enums.DBAccess.MemberLoadType.BySocialAccount;
            var result = _repository.GetMemberInfo(loadType, invalidSocialAccount, (Byte)Enums.SocialTpye.Sina);
            Assert.IsNull(result);
        }

        //[TestMethod]
        //public void Should_CreatNewMember_IfSocialAccountNotExists()
        //{
        //    String tempAccount = "E4064A72A5DF573EE358F5DC0FC8901C";// DateTime.Now.ToString() + "testsocialId";
        //    String name = DateTime.Now.ToString() + "testname";
        //    String email = DateTime.Now.ToString() + "test@test.com";
        //    Byte socialType = 3;// (Byte)Enums.SocialTpye.Sina;
        //    int memberId;
        //    int result = _repository.CreateMember(out memberId, tempAccount, socialType, name, email);
        //    memberId.AssertIsGreaterThan(0);
        //    result.AssertIsEqual(0);
        //}

        [TestMethod]
        public void Should_SaveEmailAccount()
        {
            _repository.LeaveEmailAddress("Cathy","cathy.mu@hotmail.com");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repository.Dispose();
        }

    }
}
