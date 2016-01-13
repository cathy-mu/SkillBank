using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.Managers;


namespace SkillBank.FunctionTests
{
    [TestClass]
    public class MemberManagerTests
    {
        private int _memberId;
        private int _invalidMemberId;
        MemberManager _mgr;

        [TestInitialize]
        public void TestInitialize()
        {
            _memberId = 1;
            _invalidMemberId = 9999;
            MemberRepository _repository = new MemberRepository();
            IInteractiveRepository _intRep = new InteractiveRepository();
            _mgr = new MemberManager(_repository, _intRep);
        }

        [TestMethod]
        public void Should_GetMemberInfo_ById()
        {
            var memberInfo = _mgr.GetMemberInfo(_memberId);
            Assert.IsNotNull(memberInfo);
        }

        [TestMethod]
        public void Should_GetMemberInfo_ById_Correctly()
        {
            var memberInfo = _mgr.GetMemberInfo(_memberId);
            Assert.AreEqual(memberInfo.MemberId, _memberId);
            Assert.AreEqual(memberInfo.IsActive, true);
        }

        [TestMethod]
        public void ShouldNot_GetMemberInfo_ByInvalidId()
        {
            var memberInfo = _mgr.GetMemberInfo(_invalidMemberId);
            Assert.IsNull(memberInfo);
        }

        //[TestMethod]
        //public void Should_CreateMember()
        //{
        //    Byte socialType = 3;
        //    String socialId = "E4064A72A5DF573EE358F5DC0FC8901C";
        //    String name = "Cathy";
        //    String email = "cathy.mu@hotmail.com";
        //    //int cityId = 1;
        //    int memberId;
        //    int result = _mgr.CreateMember(out memberId,socialId, socialType,name, email);
        //    Assert.AreEqual(result, 1);//exists member
        //    Assert.AreEqual(memberId, 1);
        //}


        //[TestMethod]
        //public void Should_CreateMember_Correctly()
        //{
        //}

        //[TestMethod]
        //public void Should_Not_CreateMember_IfExists()
        //{
        //    Byte socialType = 1;
        //    String socialId = "adcbefghijklmi";
        //    String name = "Cathy";
        //    String email = "cathy.mu@hotmail.com";
        //    int memberId1;
        //    var result1 = _mgr.CreateMember(out memberId1, socialId, socialType, name, email/*, cityId*/);
        //    Assert.AreEqual(result1, 1);
        //    int memberId2;
        //    var result2 = _mgr.CreateMember(out memberId2, socialId, socialType, name, email/*, cityId*/);
        //    Assert.AreEqual(result2, 1);
        //    Assert.AreEqual(memberId1, memberId2);
        //}

        //[TestMethod]
        //public void Should_Not_CreateMember_AndReturnMemberIdIfExists()
        //{
        //    Byte socialType = 1;
        //    String socialId = "adcbefghijklmi";
        //    String name = "Cathy";
        //    String email = "cathy.mu@hotmail.com";
        //    int memberId;
        //    var result = _mgr.CreateMember(out memberId, socialId, socialType, name, email/*, cityId*/);
        //    Assert.AreEqual(result, 1);
        //    var memberInfo = _mgr.GetMemberInfo(memberId);
        //    Assert.AreEqual(memberId, memberInfo.MemberId);
        //}

        [TestMethod]
        public void Should_UpdateMemberInfo_MemberName_Correctly()
        {
            Random rd = new Random();
            String rdValue = rd.Next(0, 100).ToString();
            Byte socialType = (Byte)Enums.DBAccess.MemberSaveType.UpdateName;
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = _memberId;
            memberInfo.Name = "Cathy's Account" + rdValue;
            var result = _mgr.UpdateMemberInfo(socialType, memberInfo);
            Assert.AreEqual(result, true);
            var exceptMemberInfo = _mgr.GetMemberInfo(memberInfo.MemberId);
            Assert.AreEqual(memberInfo.Name, exceptMemberInfo.Name);
        }

        [TestMethod]
        public void Should_UpdateMemberInfo_MemberEmail_Correctly()
        {
            Random rd = new Random();
            String rdValue = rd.Next(0, 100).ToString();
            Byte socialType = (Byte)Enums.DBAccess.MemberSaveType.UpdateEmail;
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = _memberId;
            memberInfo.Email = rdValue + "test@SkillBank.cn";
            var result = _mgr.UpdateMemberInfo(socialType, memberInfo);
            Assert.AreEqual(result, true);
            var exceptMemberInfo = _mgr.GetMemberInfo(memberInfo.MemberId);
            Assert.AreEqual(memberInfo.Email, exceptMemberInfo.Email);
        }

        [TestMethod]
        public void Should_UpdateMemberInfo_MemberIntro_Correctly()
        {
            Random rd = new Random();
            String rdValue = rd.Next(0, 100).ToString() + DateTime.Now.ToString();
            Byte socialType = (Byte)Enums.DBAccess.MemberSaveType.UpdateIntro;
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = _memberId;
            memberInfo.SelfIntro = rdValue;
            var result = _mgr.UpdateMemberInfo(socialType, memberInfo);
            Assert.AreEqual(result, true);
            var exceptMemberInfo = _mgr.GetMemberInfo(memberInfo.MemberId);
            Assert.AreEqual(memberInfo.SelfIntro, exceptMemberInfo.SelfIntro);
        }

        [TestMethod]
        public void Should_UpdateMemberInfo_MemberPhone_Correctly()
        {
            Random rd = new Random();
            String rdValue = rd.Next(0, 99999999).ToString();
            Byte socialType = (Byte)Enums.DBAccess.MemberSaveType.UpdatePhone;
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = _memberId;
            memberInfo.Phone = "139" + rdValue;
            var result = _mgr.UpdateMemberInfo(socialType, memberInfo);
            Assert.AreEqual(result, true);
            var exceptMemberInfo = _mgr.GetMemberInfo(memberInfo.MemberId);
            Assert.AreEqual(memberInfo.Phone, exceptMemberInfo.Phone);
        }


        [TestMethod]
        public void Should_UpdateMemberInfo_MemberCity_Correctly()
        {
            Random rd = new Random();
            int rdValue = rd.Next(0, 10);
            Byte socialType = (Byte)Enums.DBAccess.MemberSaveType.UpdateCity;
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = _memberId;
            memberInfo.CityId = rdValue;
            var result = _mgr.UpdateMemberInfo(socialType, memberInfo);
            Assert.AreEqual(result, true);
            var exceptMemberInfo = _mgr.GetMemberInfo(memberInfo.MemberId);
            Assert.AreEqual(memberInfo.CityId, exceptMemberInfo.CityId);
        }

        //[TestMethod]
        //public void Should_UpdateMemberInfo_MemberCityWithNoClassCity_Correctly()
        //{
        //    Random rd = new Random();
        //    int rdValue = rd.Next(5, 10);
        //    Byte socialType = (Byte)Enums.DBAccess.MemberSaveType.UpdateCity;
        //    MemberInfo memberInfo = new MemberInfo();
        //    memberInfo.MemberId = _memberId;
        //    memberInfo.CityId = rdValue;
        //    memberInfo.Gender = true;
        //    var result = _mgr.UpdateMemberInfo(socialType, memberInfo);
        //    Assert.AreEqual(result, true);
        //    var exceptMemberInfo = _mgr.GetMemberInfo(memberInfo.MemberId);
        //    Assert.AreEqual(memberInfo.CityId, exceptMemberInfo.CityId);
        //}

        [TestMethod]
        public void Should_UpdateMemberInfo_MemberGender_Correctly()
        {
            Random rd = new Random();
            Boolean rdValue = (rd.Next(0, 1) == 1);
            Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateGender;
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = _memberId;
            memberInfo.Gender = rdValue;
            var result = _mgr.UpdateMemberInfo(saveType, memberInfo);
            Assert.AreEqual(result, true);
            var exceptMemberInfo = _mgr.GetMemberInfo(memberInfo.MemberId);
            Assert.AreEqual(memberInfo.Gender, exceptMemberInfo.Gender);
        }

        [TestMethod]
        public void Should_UpdateMemberInfo_MapPosition_Correctly()
        {
            Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdatePosition;

            Random rd = new Random();
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = _memberId;
            memberInfo.PosX = Decimal.Round(rd.Next(121410000, 121420000) / 1000000M, 5);
            memberInfo.PosY = Decimal.Round(rd.Next(31200000, 31300000) / 1000000M, 5);
            _mgr.UpdateMemberInfo(saveType, memberInfo);
            var exceptMemberInfo = _mgr.GetMemberInfo(_memberId);
            Assert.AreEqual(exceptMemberInfo.PosX, memberInfo.PosX);
            Assert.AreEqual(exceptMemberInfo.PosY, memberInfo.PosY);
        }

        //[TestMethod]
        //public void Should_UpdateMemberInfo_MemberPosition_Correctly()
        //{
        //    Random rd = new Random();
        //    Boolean rdValue = (rd.Next(0, 1) == 1);
        //    Byte socialType = (Byte)Enums.DBAccess.MemberSaveType.UpdateGender;
        //    MemberInfo memberInfo = new MemberInfo();
        //    memberInfo.MemberId = _memberId;
        //    memberInfo.Gender = rdValue;
        //    var result = _mgr.UpdateMemberInfo(socialType, memberInfo);
        //    Assert.AreEqual(result, true);
        //    var exceptMemberInfo = _mgr.GetMemberInfo(memberInfo.MemberId);
        //    Assert.AreEqual(memberInfo.Gender, exceptMemberInfo.Gender);
        //}

        //[TestMethod]
        //public void Should_UpdateMemberPhoto_Correctly()
        //{
        //}

        //[TestMethod]
        //public void Should_UpdateMemberPosition_Correctly()
        //{
        //}

        [TestMethod]
        public void Should_UpdateMemberProfile_Correctly()
        {
            Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateProfile;
            Random rd = new Random();
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = _memberId;
            memberInfo.Gender = true;
            memberInfo.Phone = "1350000000"+(rd.Next(0, 9)).ToString();
            memberInfo.Email = rd.Next(0, 9999).ToString()+"Cathy@test.com";
            memberInfo.CityId = rd.Next(1, 2);
            memberInfo.Name = "Cathy"+rd.Next(0, 10).ToString();
            memberInfo.SelfIntro = "喜欢体育运动,喜爱音乐,擅长瑜伽,攀岩.喜欢结交有共同兴趣爱好的朋友.Test" + rd.Next(0, 100).ToString();
            var result = _mgr.UpdateMemberInfo(saveType, memberInfo);
            var exceptMemberInfo = _mgr.GetMemberInfo(memberInfo.MemberId);
            Assert.AreEqual(memberInfo.Gender, exceptMemberInfo.Gender);
            Assert.AreEqual(memberInfo.Name, exceptMemberInfo.Name);
            Assert.AreEqual(memberInfo.Phone, exceptMemberInfo.Phone);
            Assert.AreEqual(memberInfo.SelfIntro, exceptMemberInfo.SelfIntro);
        }

        [TestMethod]
        public void Should_UpdateMemberContactInfo_Correctly()
        {
            String studentName = "凯西in技能银行";
            String studentPhone = "1234567890";
            if (!String.IsNullOrEmpty(studentName) || !String.IsNullOrEmpty(studentPhone))
            {
                MemberInfo studentInfo = new MemberInfo();
                studentInfo.MemberId = 2;
                studentInfo.Name = String.IsNullOrEmpty(studentName) ? "" : studentName;
                studentInfo.Phone = String.IsNullOrEmpty(studentPhone) ? "" : studentPhone;
                _mgr.UpdateMemberInfo((Byte)Enums.DBAccess.MemberSaveType.UpdateContactInfo, studentInfo);
            }
        }

        #region VerificationCode

        [TestMethod]
        public void Should_SendAndSaveVerificationCode()
        {
            String mobile = "22222222222";
            var result = _mgr.SendMobileVerifyCode((Byte)Enums.DBAccess.MobileVerificationSaveType.GetVerifyCode,_memberId, mobile, false);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Should_PassVerificationCode()
        {
            String mobile = "11111111111";
            String code = "123456";
            var result = _mgr.VerifyMobile(_memberId, mobile, code);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void ShouldNot_PassVerificationCode()
        {
            String mobile = "11111111111";
            String code = "111111";
            var result = _mgr.VerifyMobile(_memberId, mobile, code);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Should_ValidMemberInfo()
        {
            String mobile1 = "14512345678";
            String code1 = "123456";
            String mobile2 = "12000000000";
            String code2 = "eerrty";
            var result = System.Text.RegularExpressions.Regex.IsMatch(mobile1, @"^[1]+[3,4,5,8]+\d{9}");
            Assert.AreEqual(true, result);
            result = System.Text.RegularExpressions.Regex.IsMatch(mobile2, @"^[1]+[3,4,5,8]+\d{9}");
            Assert.AreEqual(false, result);
            result = System.Text.RegularExpressions.Regex.IsMatch(code1, @"^\d{6}$");
            Assert.AreEqual(true, result);
            result = System.Text.RegularExpressions.Regex.IsMatch(code2, @"^\d{6}$");
            Assert.AreEqual(false, result);
           
        }


        #endregion
    }
}