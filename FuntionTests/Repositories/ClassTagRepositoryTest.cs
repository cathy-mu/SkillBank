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
    public class ClassTagRepositoryTests
    {
        private ClassTagRepository _repository;
        //private SByte _loadType;
        private int _classId;
        //private int _memberId;
        //private int _invalidMemberId;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new ClassTagRepository();
            _classId = 1;
            //Get Class info By ClassId
            //_invalidMemberId = 99999;
        }

        [TestMethod]
        public void Should_AddClassTag()
        {
            Char delimiter = Constants.Setting.DBDataDelimiter;
            String[] tags = new String[] { "tag1", "tag2", "tag3", "tag4" };
            String tagsText = String.Format("{1}{0}{2}{0}{3}{0}{4}",delimiter, tags[0], tags[1], tags[2], tags[3]);
            int classId = _repository.AddClassTags(_classId, tagsText);
            var classTags = _repository.GetClassTags(classId);
            Assert.IsNotNull(classTags);
        }

        [TestMethod]
        public void Should_AddClassTag_Correctly()
        {
            String[] tags = new String[] { "tag1", "tag2", "tag3", "tag4" };
            String tagsText = String.Format("{0},{1},{2},{3}", tags[0], tags[1], tags[2], tags[3]);
            int classId = _repository.AddClassTags(_classId, tagsText);
            var classTags = _repository.GetClassTags(classId);
            Assert.IsNotNull(classTags);

            foreach (var item in tags)
            {
                Assert.IsTrue(classTags.Contains(item));
            }

        }

        [TestMethod]
        public void Should_GetClassTags_ByClassId()
        {
            var classes = _repository.GetClassTags(_classId);
            Assert.IsNotNull(classes);
        }

//        [TestMethod]
//        public void Should_GetClassTag_ByClassId_Correctly()
//        {
//            var classes = _repository.GetClassTags(_loadType, _classId);
//            Assert.AreEqual(classes.Count(), 1);
//            var classItem = classes.FirstOrDefault();
//            Assert.AreEqual(classItem.ClassId,_classId);
//        }

//        [TestMethod]
//        public void Should_GetClassTag_ByTeacherId()
//        {
//            Byte loadType = (Byte)Enums.DBAccessClassLoadType.ByTeacherId;
//            var classes = _repository.GetClassTags(loadType, _memberId);
            
//            Assert.IsNotNull(classes);
//        }

//        [TestMethod]
//        public void ShouldNot_GetClassTag_ByInvalidTeacherId()
//        {
//            Byte loadType = (Byte)Enums.DBAccessClassLoadType.ByTeacherId;
//            var classes = _repository.GetClassTags(loadType, _invalidMemberId);

//            Assert.IsNull(classes);
//        }

//        [TestMethod]
//        public void Should_GetClassTag_ByTeacherId_Correctly()
//        {
//            Byte loadType = (Byte)Enums.DBAccessClassLoadType.ByTeacherId;
//            var classItem = _repository.GetClassTags(loadType, _memberId).FirstOrDefault();
//            Assert.AreEqual(classItem.Member_Id, _memberId);
//        }
        
        
//        //[TestMethod]
//        //public void Should_GetClassTag_ByStudentId()
//        //{
//        //}


        [TestCleanup]
        public void TestCleanup()
        {
            _repository.Dispose();
        }
    }
}

