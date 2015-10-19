using System;
using System.Data;
using System.Data.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Services.CacheProviders;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class CommonServiceTests
    {
        private int _memberId;
        private int _member2Id;
        private int _invalidMemberId;
        private int _classId;
        CommonService _svr;

        [TestInitialize]
        public void TestInitialize()
        {
            _memberId = 1;
            _member2Id = 2;
            _invalidMemberId = 9999;
            _classId = 1;
            // MemberRepository _repository = 
            //ClassRepository _repository = new ClassRepository();
            MemberManager _memberMgr = new MemberManager(new MemberRepository(), new InteractiveRepository());
            ClassManager _classMgr = new ClassManager(new ClassInfoRepository(), new ClassTagRepository(), new InteractiveRepository());
            FeedBackManager _feedbackMgr = new FeedBackManager(new StudentReviewRepository(), new TeacherReviewRepository());
            OrderManager _orderMgr = new OrderManager(new OrderRepository());
            MessageManager _messageMgr = new MessageManager(new MessageRepository());
            NotificationManager _notificationMgr = new NotificationManager(new NotificationRepository());
            ReportToolsManager _repMgr = new ReportToolsManager(new ReportToolsRepository());
            //CacheContentManager _cacheMgr = new CacheContentManager(new RecommendClassCacheMgr(new ClassInfoRepository()), new ClassListCacheMgr(new ClassInfoRepository()), new ClassItemCacheMgr(new ClassInfoRepository()));
            CacheContentManager _cacheMgr = new CacheContentManager(new RecommendClassCacheMgr(), new ClassListCacheMgr(new ClassInfoRepository()), new ClassItemCacheMgr(new ClassInfoRepository()));
            _svr = new CommonService(_memberMgr, _classMgr, _orderMgr, _feedbackMgr, _messageMgr, _notificationMgr, _repMgr, _cacheMgr);
        }

         

        #region Review Test Functions
        #endregion

        #region Class Test Functions

        [TestMethod]
        public void Should_GetClassInfo_ByClassId()
        {
            var classInfo = _svr.GetClassInfoByClassId(_classId);
            Assert.IsNotNull(classInfo);
        }

        [TestMethod]
        public void Should_GetClassInfo_ByClassId_Correctly()
        {
            var classInfo = _svr.GetClassInfoByClassId(_classId);
            Assert.IsNotNull(classInfo);
            Assert.IsNotNull(classInfo.Title);
            Assert.IsNotNull(classInfo.Summary);
        }

        [TestMethod]
        public void Should_GetClassInfo_ByUnProvedClass()
        {
            var classInfo = _svr.GetClassInfoForAdmin(true);
            Assert.IsNotNull(classInfo);
        }

        [TestMethod]
        public void Should_GetClassInfo_ByRejectedClass()
        {
            var classInfo = _svr.GetClassInfoForAdmin(false);
            Assert.IsNotNull(classInfo);
        }

        #endregion


        #region Member Test Functions

        [TestMethod]
        public void Should_GetMemberInfo_ByMemberId()
        {
            var memberInfo = _svr.GetMemberInfo(_memberId); ;
            Assert.IsNotNull(memberInfo);
        }

        [TestMethod]
        public void Should_GetMemberInfo_ByMemberId_Correctly()
        {
            var memberInfo = _svr.GetMemberInfo(_memberId); ;
            Assert.IsNotNull(memberInfo);
            Assert.AreEqual(_memberId, memberInfo.MemberId);
        }

        [TestMethod]
        public void Should_GetMemberInfo_BySocialAccount()
        {
            String socialId = "E4064A72A5DF573EE358F5DC0FC8901C";
            Byte socialType = 3;
            var memberInfo =_svr.GetMemberInfo(socialId, socialType);
            Assert.IsNotNull(memberInfo);
        }

        [TestMethod]
        public void Should_GetMemberInfo_BySocialAccount_Correctly()
        {
            String socialId = "E4064A72A5DF573EE358F5DC0FC8901C";
            Byte socialType = 3;
            var memberInfo = _svr.GetMemberInfo(socialId, socialType);
            Assert.IsNotNull(memberInfo);
            Assert.AreEqual(socialId,memberInfo.OpenId);
            Assert.AreEqual(socialType, memberInfo.SocialType);
        }

        [TestMethod]
        public void Should_UpdateMemberInfo_Correctly()
        {
            int year = 1990;
            int month = 9;
            int day = 20;
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = 4;
            memberInfo.Name = "cathy";
            memberInfo.Gender = false;
            memberInfo.BirthDate = (year == 0 || month == 0 || day == 0) ? new DateTime(1900, 01, 01) : new DateTime(year, month, day);
            memberInfo.Phone = "1234567890";
            memberInfo.Email = "cathy@test.cn1";
            memberInfo.SelfIntro = "Here is my introduction 1";

            var result = _svr.UpdateMemberProfile(memberInfo);
        }

        //[TestMethod]
        //public void Should_UpdateMemberInfo_Intro_Correctly()
        //{
        //    Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateIntro;
        //    Random rd = new Random();
        //    String rdValue = rd.Next(0, 9999).ToString();
        //    String intro = "我是凯西，喜欢音乐和运动，喜欢旅游和关注时尚咨询。很希望认识有共同兴趣爱好的朋友。" + rdValue;

        //    var result = _svr.UpdateMemberInfo(_memberId, saveType, intro);
        //    Assert.AreEqual(result, true);
        //    var exceptMemberInfo = _svr.GetMemberInfo(_memberId);
        //    Assert.AreEqual(intro, exceptMemberInfo.SelfIntro);
        //}

        [TestMethod]
        public void Should_UpdateMemberInfo_Phone_Correctly()
        {
            Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdatePhone;
            Random rd = new Random();
            String rdValue = rd.Next(0, 99999999).ToString();
            String phone = "139" + rdValue;

            var result = _svr.UpdateMemberInfo(_memberId, saveType, phone);
            Assert.AreEqual(result, true);
            var exceptMemberInfo = _svr.GetMemberInfo(_memberId);
            Assert.AreEqual(phone, exceptMemberInfo.Phone);
        }


        [TestMethod]
        public void Should_UpdateMemberInfo_Email_Correctly()
        {
            Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateEmail;
            Random rd = new Random();
            String rdValue = rd.Next(0, 99999999).ToString();
            String email = rdValue + "@test.com";

            var result = _svr.UpdateMemberInfo(_memberId, saveType, email);
            Assert.AreEqual(result, true);
            var exceptMemberInfo = _svr.GetMemberInfo(_memberId);
            Assert.AreEqual(email, exceptMemberInfo.Email);
        }

        [TestMethod]
        public void Should_UpdateMemberInfo_Name_Correctly()
        {
            Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateName;
            Random rd = new Random();
            String rdValue = rd.Next(0, 99999999).ToString();
            String name = "凯西" + rdValue ;

            var result = _svr.UpdateMemberInfo(_memberId, saveType, name);
            Assert.AreEqual(result, true);
            var exceptMemberInfo = _svr.GetMemberInfo(_memberId);
            Assert.AreEqual(name, exceptMemberInfo.Name);
        }

        [TestMethod]
        public void Should_UpdateMemberInfo_MapPosition_Correctly()
        {
            Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdatePosition;
            Random rd = new Random();
            Decimal posX = Decimal.Round(rd.Next(121410000, 121420000) / 1000000M, 5);
            Decimal posY = Decimal.Round(rd.Next(31200000, 31300000) / 1000000M, 5);
            
            String saveValue = posX.ToString() + Constants.Setting.DBDataDelimiter + posY.ToString();

            _svr.UpdateMemberInfo(_memberId, saveType, saveValue);
            var exceptMemberInfo = _svr.GetMemberInfo(_memberId);
            Assert.AreEqual(posX, exceptMemberInfo.PosX);
            Assert.AreEqual(posY, exceptMemberInfo.PosY);
        }

        [TestMethod]
        public void Should_UpdateMemberInfo_CityId_Correctly()
        {
            Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateCity;
            Random rd = new Random();
            String saveValue = rd.Next(0, 10).ToString();

            _svr.UpdateMemberInfo(_memberId, saveType, saveValue);
            var exceptMemberInfo = _svr.GetMemberInfo(_memberId);
            Assert.IsNotNull(saveValue, exceptMemberInfo.CityId.ToString());
        }

        [TestMethod]
        public void Should_UpdateMemberInfo_Gender_Correctly()
        {
            Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateGender;
            Random rd = new Random();
            String gender = rd.Next(0, 1) == 0 ? "0" : "1";
            _svr.UpdateMemberInfo(_memberId, saveType, gender);
            var exceptMemberInfo = _svr.GetMemberInfo(_memberId);
            Assert.AreEqual((gender == "1"), exceptMemberInfo.Gender);
        }
                
        [TestMethod]
        public void Should_CreateMember()
        {
            Random rd = new Random();
            String account = rd.Next(0, 10).ToString() + "social@test.com";
            Byte socialType = 3;//(Byte)rd.Next(1, 3);
            int memberId = 0;
            var result = _svr.CreateMember(out memberId, account, socialType, "name", "social@test.com","");

            var exceptMemberInfo = _svr.GetMemberInfo(memberId);
            memberId.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetMemberInfo_BySocialAccout()
        {
            var memberInfo = _svr.GetMemberInfo("ABCDEFGHIJKLMN", 3);
            Assert.IsNotNull(memberInfo);
            
            memberInfo = _svr.GetMemberInfo("tttttttttttt", 3);
            Assert.IsNull(memberInfo);
        }

        [TestMethod]
        public void Should_GetMemberInfo_ById()
        {
            var memberInfo = _svr.GetMemberInfo(_memberId);
            Assert.IsNotNull(memberInfo);
        }

        [TestMethod]
        public void Should_GetMemberInfo_ById_Correctly()
        {
            var memberInfo = _svr.GetMemberInfo(_memberId);
            Assert.AreEqual(_memberId,memberInfo.MemberId);
        }

        #endregion

        #region Message Test Functions

        [TestMethod]
        public void Should_GetMemberMessageList_ByMemberId()
        {
            var message = _svr.GetMessageList(_memberId);
            Assert.IsNotNull(message);
        }

        [TestMethod]
        public void Should_SetMessageASRead_Correctly()
        {
            int maxId = 5;
            _svr.SetMessageAsRead(maxId, _member2Id, _memberId);
            var messages = _svr.GetMessageDetail(_memberId, _member2Id);

            foreach (var item in messages)
            {
                if (item.To_Id.Equals(_memberId))
                {
                    if (item.IsRead.Equals(false))
                    {
                        item.MessageId.AssertIsGreaterThan(maxId);
                    }
                    else
                    {
                        item.MessageId.AssertIsLesserThan(maxId + 1);
                    }
                }
            }
        }

        [TestMethod]
        public void Should_UpdatedClassInfo_Title()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.UpdateTitle;
            String classTitle = "哈达瑜伽";

            _svr.UpdateClassEditInfo(updateType, _classId, classTitle);

            var classItem = _svr.GetClassInfoByClassId(_classId);
            Assert.IsNotNull(classItem);

            TimeSpan timeDiff = DateTime.Now.Subtract(classItem.LastUpdateDate).Duration();
            Assert.AreEqual(0, timeDiff.Days);
            Assert.AreEqual(0, timeDiff.Hours);
            Assert.AreEqual(0, timeDiff.Minutes);
        }

        [TestMethod]
        public void Should_UpdatedClassInfo_TitleCorrectly()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.UpdateTitle;
            String classTitle = "哈达瑜伽（基础）";

            _svr.UpdateClassEditInfo(updateType, _classId, classTitle);

            var classItem = _svr.GetClassInfoByClassId(_classId);
            Assert.IsNotNull(classItem);

            Assert.AreEqual(classItem.Title, classTitle);
        }


        [TestMethod]
        public void Should_GetClassTabList_Correctly()
        {
            Byte loadBy = (Byte)Enums.DBAccess.ClassTabListLoadType.Recommendation;
            Byte typeId = 2;
            var classList = _svr.GetClassTabList(loadBy, typeId, _memberId, "");
            Assert.IsNotNull(classList);
            classList.Count.AssertIsGreaterThan(0);
        }
        
        #endregion 

        [TestCleanup]
        public void TestCleanup()
        {
           
        }
    }
}