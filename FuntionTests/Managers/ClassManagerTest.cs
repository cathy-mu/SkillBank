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


namespace SkillBank.FunctionTests
{
    [TestClass]
    public class ClassManagerTests
    {
        private int _classId;
        private int _memberId;
        private int _invalidClassId;
        ClassManager _mgr;
        //private Byte _loadType;

        [TestInitialize]
        public void TestInitialize()
        {
            _classId = 1;
            _invalidClassId = 9999;
            ClassInfoRepository classRep = new ClassInfoRepository();
            ClassTagRepository tagRep = new ClassTagRepository();
            InteractiveRepository intRep = new InteractiveRepository();
            _mgr = new ClassManager(classRep, tagRep, intRep);
            _memberId = 1;
            //_loadType = (Byte)Enums.DBAccess.ClassLoadType.ByClassId;
        }

        [TestMethod]
        public void Should_CheckClassSteps_Correctly()
        {
            Int16 stepNo = 1;
            var result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
            Assert.AreEqual(false, result[2]);
            Assert.AreEqual(false, result[3]);

            stepNo = 2;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(false, result[0]);
            Assert.AreEqual(true, result[1]);
            Assert.AreEqual(false, result[2]);
            Assert.AreEqual(false, result[3]);

            stepNo = 3;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(true, result[1]);
            Assert.AreEqual(false, result[2]);
            Assert.AreEqual(false, result[3]);

            stepNo = 4;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(false, result[0]);
            Assert.AreEqual(false, result[1]);
            Assert.AreEqual(true, result[2]);
            Assert.AreEqual(false, result[3]);

            stepNo = 5;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
            Assert.AreEqual(true, result[2]);
            Assert.AreEqual(false, result[3]);

            stepNo = 6;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(false, result[0]);
            Assert.AreEqual(true, result[1]);
            Assert.AreEqual(true, result[2]);
            Assert.AreEqual(false, result[3]);

            stepNo = 7;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(true, result[1]);
            Assert.AreEqual(true, result[2]);
            Assert.AreEqual(false, result[3]);

            stepNo = 8;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(false, result[0]);
            Assert.AreEqual(false, result[1]);
            Assert.AreEqual(false, result[2]);
            Assert.AreEqual(true, result[3]);

            stepNo = 9;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(2, result.Count(i => (i == true)));

            stepNo = 10;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(2, result.Count(i => (i == true)));

            stepNo = 11;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(3, result.Count(i => (i == true)));

            stepNo = 12;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(2, result.Count(i => (i == true)));

            stepNo = 13;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(3, result.Count(i => (i == true)));

            stepNo = 14;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(3, result.Count(i => (i == true)));

            stepNo = 15;
            result = _mgr.CheckClassSteps(stepNo);
            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(true, result[1]);
            Assert.AreEqual(true, result[2]);
            Assert.AreEqual(true, result[3]);
        }

        [TestMethod]
        public void Should_CreateClass()
        {
            bool isExist = false;
            var categoryId = 2;
            byte teachLevel = 80;
            byte skillLevel = 90;
            var result = _mgr.CreateClass(_memberId, categoryId, skillLevel, teachLevel, out isExist);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Should_UpdatedClassInfo_Title()
        {
            var updateType = (Byte)Enums.DBAccess.ClassSaveType.UpdateTitle;
            String classTitle = "哈达瑜伽";

            _mgr.UpdateClassEditInfo(updateType, _classId, classTitle);

            var classItem = _mgr.GetClassInfoByClassId(_classId);
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

            _mgr.UpdateClassEditInfo(updateType, _classId, classTitle);

            var classItem = _mgr.GetClassInfoByClassId(_classId);
            Assert.IsNotNull(classItem);

            Assert.AreEqual(classItem.Title, classTitle);
        }


        [TestMethod]
        public void Should_GetClassInfo_ByClassId()
        {
            var classItem = _mgr.GetClassInfoByClassId(_classId);
            Assert.IsNotNull(classItem);
        }

        [TestMethod]
        public void Should_GetClassInfo_ByClassIdCorrectly()
        {
            var classItem = _mgr.GetClassInfoByClassId(_classId);
            Assert.IsNotNull(classItem);

            Assert.AreEqual(classItem.ClassId, _classId);
        }



    }
}
