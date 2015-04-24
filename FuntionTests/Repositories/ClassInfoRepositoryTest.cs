using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class ClassInfoRepositoryTests
    {
        private ClassInfoRepository _repository;
        private TestHelperRepository _repHelper;
        private Byte _loadType;
        private Byte _categoryId;
        private int _memberId;
        private int _classId;
        private int _invalidMemberId;
        //private Boolean defaultProved = false;
        //private Boolean defaultActive = true;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new ClassInfoRepository();
            _repHelper = new TestHelperRepository();
            _memberId = 1;
            _categoryId = 3;
            _classId = 1;
            //Get Class info By ClassId
            _loadType = (Byte)Enums.DBAccess.ClassLoadType.ByClassId;
            _invalidMemberId = 99999;
        }

        //[TestMethod]
        //public void Should_AddClassInfo_Correctly()
        //{
        //    ClassInfo classInfo = new ClassInfo();
        //    classInfo.Member_Id = _memberId;
        //    classInfo.Category_Id = _categoryId;
        //    classInfo.TeacheLevel = 60;
        //    classInfo.SkillLevel = 70;
        //    classInfo.Title = String.Empty;
        //    classInfo.Summary = String.Empty;
        //    classInfo.Description = String.Empty;
        //    int classId = _repository.CreateClass(classInfo);
        //    var classes = _repository.GetClassInfo(_loadType, classId);

        //    Assert.AreEqual(classes.Count(), 1);
        //    var classItem = classes.FirstOrDefault();
        //    Assert.AreEqual(classItem.Category_Id, classInfo.Category_Id);
        //    Assert.AreEqual(classItem.TeacheLevel, classInfo.TeacheLevel);
        //    Assert.AreEqual(classItem.SkillLevel, classInfo.SkillLevel);
        //    Assert.AreEqual(classItem.Member_Id, classInfo.Member_Id);
        //}

        [TestMethod]
        public void Should_AddClass()
        {
            //_repHelper.ClassInfoCleanUp(_invalidMemberId);//update to invalid user id later
            ClassInfo classInfo = new ClassInfo();
            //classInfo.Category_Id = (SByte)_categoryId;
            classInfo.Category_Id = _categoryId;
            classInfo.Member_Id = _invalidMemberId;//update to invalid user id later
            classInfo.SkillLevel = 80;
            classInfo.TeacheLevel = 70;
            Boolean isExist;
            int classId = _repository.CreateClass(classInfo.Member_Id, classInfo.Category_Id, (Byte)classInfo.SkillLevel, (Byte)classInfo.TeacheLevel, out isExist);
            classId.AssertIsGreaterThan(0);
            //Assert.AreEqual(false, isExist);
            var classes = _repository.GetClassInfo(_loadType, classId);
        }

        [TestMethod]
        public void ShouldNot_AddClass_IfExistUncompleteClass()
        {
            Random rd = new Random();
            var Category_Id = (Byte)rd.Next(0, 5);
            var Member_Id = _memberId;
            var SkillLevel = (Byte)rd.Next(0, 100);
            var TeacheLevel = (Byte)rd.Next(0, 100);
            Boolean isExist;
            int classId = _repository.CreateClass(Member_Id, Category_Id, SkillLevel, TeacheLevel, out isExist);
            classId.AssertIsGreaterThan(0);
            Assert.AreEqual(true, isExist);
        }

        [TestMethod]
        public void Should_AddClass_Correctly()
        {
            Random rd = new Random();
            var Category_Id = (Byte)rd.Next(0, 5);
            var Member_Id = _memberId;
            var SkillLevel = (Byte)rd.Next(0, 100);
            var TeacheLevel = (Byte)rd.Next(0, 100);
            Boolean isExist;
            int classId = _repository.CreateClass(Member_Id, Category_Id, SkillLevel, TeacheLevel, out isExist);
            classId.AssertIsGreaterThan(0);
            var classes = _repository.GetClassInfo(_loadType, classId);
            Assert.AreEqual(classes[0].ClassId, classId);
            Assert.AreEqual(classes[0].Member_Id, Member_Id);
            Assert.AreEqual((Byte)classes[0].SkillLevel, SkillLevel);
            Assert.AreEqual((Byte)classes[0].TeacheLevel, TeacheLevel);
        }


        [TestMethod]
        public void Should_UpdatedClassInfo_Title()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.UpdateTitle;
            String classTitle = "哈达瑜伽";

            _repository.UpdateClassInfo(updateType, _classId, classTitle);

            var classes = _repository.GetClassInfo(_loadType, _classId);
            Assert.IsNotNull(classes);
            var classItem = classes.FirstOrDefault();

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

            _repository.UpdateClassInfo(updateType, _classId, classTitle);

            var classes = _repository.GetClassInfo(_loadType, _classId);
            Assert.IsNotNull(classes);
            var classItem = classes.FirstOrDefault();

            Assert.AreEqual(classItem.Title, classTitle);
        }

        [TestMethod]
        public void Should_UpdatedClassInfo_Level()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.Updatelevel;
            Random rd = new Random();
            var level = (Byte)rd.Next(1, 3);

            _repository.UpdateClassInfo(updateType, _classId, level);

            var classes = _repository.GetClassInfo(_loadType, _classId);
            Assert.IsNotNull(classes);
            var classItem = classes.FirstOrDefault();

            TimeSpan timeDiff = DateTime.Now.Subtract(classItem.LastUpdateDate).Duration();
            Assert.AreEqual(0, timeDiff.Days);
            Assert.AreEqual(0, timeDiff.Hours);
            Assert.AreEqual(0, timeDiff.Minutes);
        }

        [TestMethod]
        public void Should_UpdatedClassInfo_LevelCorrectly()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.Updatelevel;
            Random rd = new Random();
            var level = (Byte)rd.Next(1, 3);

            _repository.UpdateClassInfo(updateType, _classId, level);

            var classes = _repository.GetClassInfo(_loadType, _classId);
            Assert.IsNotNull(classes);
            var classItem = classes.FirstOrDefault();

            Assert.AreEqual((Byte)classItem.Level, level);
        }

        [TestMethod]
        public void Should_UpdatedClassInfo_Active()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.SetActiveTag;
            Random rd = new Random();
            Boolean isActive = (rd.Next(0, 1) == 1);

            _repository.UpdateClassInfo(updateType, _classId, isActive);

            var classes = _repository.GetClassInfo(_loadType, _classId);
            Assert.IsNotNull(classes);
            var classItem = classes.FirstOrDefault();

            TimeSpan timeDiff = DateTime.Now.Subtract(classItem.LastUpdateDate).Duration();
            Assert.AreEqual(0, timeDiff.Days);
            Assert.AreEqual(0, timeDiff.Hours);
            Assert.AreEqual(0, timeDiff.Minutes);
        }

        [TestMethod]
        public void Should_UpdatedClassInfo_Proved()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.UpdateProvedTag;
            Random rd = new Random();
            Byte proveTag = (Byte)rd.Next(0, 2);

            _repository.UpdateClassInfo(updateType, _classId, proveTag);

            var classes = _repository.GetClassInfo(_loadType, _classId);
            Assert.IsNotNull(classes);
            var classItem = classes.FirstOrDefault();

            TimeSpan timeDiff = DateTime.Now.Subtract(classItem.LastUpdateDate).Duration();
            Assert.AreEqual(0, timeDiff.Days);
            Assert.AreEqual(0, timeDiff.Hours);
            Assert.AreEqual(0, timeDiff.Minutes);
        }

        [TestMethod]
        public void Should_UpdatedClassInfo_CoverCorrectly()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.UpdatePhoto;
            Random rd = new Random();
            String cover = "mytest.jpg";

            _repository.UpdateClassInfo(updateType,3/* _classId*/, cover);

            var classes = _repository.GetClassInfo(_loadType, 3/* _classId*/);
            Assert.IsNotNull(classes);
            var classItem = classes.FirstOrDefault();
            Assert.AreEqual(cover, classItem.Cover);
        }

        [TestMethod]
        public void Should_UpdatedClassEditCover_Correctly()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.UpdatePhoto;
            Random rd = new Random();
            String cover = "mytest.gif";

            _repository.UpdateClassEditInfo(updateType, 3/* _classId*/, cover);

            var classes = _repository.GetClassEditInfo(_loadType, 3/* _classId*/);
            Assert.IsNotNull(classes);
            var classItem = classes.FirstOrDefault();
            Assert.AreEqual(cover, classItem.Cover);
        }

        [TestMethod]
        public void Should_UpdatedClassEditCoverAPublish_Correctly()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.UpdateStep3;
            Random rd = new Random();
            String cover = "mytest.png";
            Byte publishStatus = 3;

            _repository.UpdateClassEditInfo(updateType, 3/* _classId*/, cover);

            var classes = _repository.GetClassEditInfo(_loadType, 3/* _classId*/);
            Assert.IsNotNull(classes);
            var classItem = classes.FirstOrDefault();
            Assert.AreEqual(cover, classItem.Cover);
            Assert.AreEqual(publishStatus, classItem.PublishStatus);
        }

        //[TestMethod]
        //public void Should_UpdatedClassInfo_Correctly()
        //{
        //    ClassInfo classInfo = new ClassInfo();
        //    classInfo.ClassId = _classId;
        //    classInfo.Member_Id = _memberId;
        //    classInfo.Title = "Oral English";
        //    classInfo.Level = 1;
        //    classInfo.Summary = "Face to Face , Private teacher, For Beginners.";
        //    classInfo.Description = "How to Conmunicate with foreigners.";
        //    classInfo.Category_Id = _categoryId;
        //    _repository.UpdateClassInfo(classInfo);
        //    var classes = _repository.GetClassInfo(_loadType, _classId);
        //    Assert.IsNotNull(classes);
        //    var classItem = classes.FirstOrDefault();
        //    Assert.AreEqual(classInfo.ClassId, classItem.ClassId);
        //    Assert.AreEqual(classInfo.Title, classItem.Title);
        //    Assert.AreEqual(classInfo.Level, classItem.Level);
        //    Assert.AreEqual(classInfo.Summary, classItem.Summary);
        //    Assert.AreEqual(classInfo.Description, classItem.Description);
        //    Assert.AreEqual(classInfo.Category_Id, classItem.Category_Id);
        //}

        [TestMethod]
        public void Should_GetClassInfo_ByClassId()
        {
            var classes = _repository.GetClassInfo(_loadType, _classId);
            Assert.IsNotNull(classes);
        }

        [TestMethod]
        public void Should_GetClassInfo_ByClassId_Correctly()
        {
            var classes = _repository.GetClassInfo(_loadType, _classId);
            Assert.AreEqual(classes.Count(), 1);
            var classItem = classes.FirstOrDefault();
            Assert.AreEqual(classItem.ClassId, _classId);
        }

        [TestMethod]
        public void Should_GetClassInfo_ByTeacherId()
        {
            var loadType = (Byte)Enums.DBAccess.ClassLoadType.ByTeacherId;
            var classes = _repository.GetClassInfo(loadType, _memberId);

            Assert.IsNotNull(classes);
        }

        [TestMethod]
        public void ShouldNot_GetClassInfo_ByInvalidTeacherId()
        {
            var loadType = (Byte)Enums.DBAccess.ClassLoadType.ByTeacherId;
            var classes = _repository.GetClassInfo(loadType, _invalidMemberId);

            Assert.IsNull(classes);
        }

        [TestMethod]
        public void Should_GetClassInfo_ByTeacherId_Correctly()
        {
            Byte loadType = (Byte)Enums.DBAccess.ClassLoadType.ByTeacherId;
            var classItem = _repository.GetClassInfo(loadType, _memberId).FirstOrDefault();
            Assert.AreEqual(classItem.Member_Id, _memberId);
        }

        [TestMethod]
        public void Should_GetSearchClassList()
        {
            var resultNum = 0; 
            int cityId = 0;
            Byte categoryId = 0;
            var classItems = _repository.SearchClass(cityId, categoryId,false, "", 5, 1, out resultNum);
            Assert.IsNotNull(classItems);
            //resultNum.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetSearchClassList_ByCategory_Correctly()
        {
            var resultNum = 0; 
            int cityId = 0;
            Byte categoryId = 0;
            var classItems = _repository.SearchClass(cityId, categoryId, false, "", 5, 1, out resultNum);
            var classNo = classItems.Select(i => (i.Category_Id == categoryId)).Count();
            //var nonProvedClassNo = classItems.Select(i => (i. == categoryId)).Count();

            Assert.IsNotNull(classItems);
            Assert.AreEqual(classItems.Count(), classNo);
        }

        //[TestMethod]
        //public void Should_GetMemberClass()
        //{
        //    var classItems = _repository.GetMemberClass(_memberId);
        //    Assert.IsNotNull(classItems);
        //}

        //[TestMethod]
        //public void Should_GetMemberClass_Correctly()
        //{
        //    var classItems = _repository.GetMemberClass(_memberId);
        //    var classNo = classItems.Select(i => (i.Member_Id == _memberId)).Count();

        //    Assert.AreEqual(classItems.Count(), classNo);
        //}


        //public List<ClassItem> SearchClass(int cityId, Byte categoryId, Boolean isParentCate, String searchKey, int pageSize, int pageId, out int resultNum)
        //{
        //    var loadBy = (Byte)Enums.DBAccess.ClassListLoadType.SearchClass;
        //    int memberId = 0;
        //    var result = ClassInfo_LoadAll_p(loadBy, cityId, categoryId, isParentCate, memberId, searchKey, pageSize, pageId, out resultNum);
        //    return result;
        //}

        //public List<ClassListItem> GetClassList(Byte orderByType, int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, int memberId, out int totalNum, String searchKey = "", Decimal posX = 0, Decimal posY = 0)
        //{
        //    var result = ClassInfo_LoadByList_p(orderByType, cityId, categoryId, isParentCate, pageSize, pageId, memberId, out totalNum, searchKey, posX, posY);
        //    return result;
        //}

        [TestMethod]
        public void Should_GetGetClassList()
        {
            Byte loadType = 0;
            Byte orderType = 0;//by lastupdate date
            var resultNum = 0;
            int cityId = 0;
            Byte categoryId = 0;
            var classItems = _repository.GetClassList(loadType, orderType, cityId, categoryId, false, 5, 1, _memberId, out resultNum, "", 0, 0);
            //var classNo = classItems.Select(i => (i.Category_Id == categoryId)).Count();
            //var nonProvedClassNo = classItems.Select(i => (i. == categoryId)).Count();

            Assert.IsNotNull(classItems);
            resultNum.AssertIsGreaterThan(0);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repository.Dispose();
        }
    }
}

