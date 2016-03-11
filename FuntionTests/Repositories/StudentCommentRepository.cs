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
    public class StudentReviewRepositoryTests
    {
        private StudentReviewRepository _repository;
        private TestHelperRepository _helperRep;
        Byte _loadType;
        private int _orderId;
        //private int _invalidMemberId;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new StudentReviewRepository();
            _helperRep = new TestHelperRepository();
            //Get from DB By Id
            _loadType = (Byte)Enums.DBAccess.CommentLoadType.ByCommentId;
            _orderId = 1;
            //_invalidMemberId = 99999;
        }

        [TestMethod]
        public void Should_AddStudentReview()
        {
            Random rad = new Random();
            int Order_Id = 1;
            int Class_Id = 0;
            Byte FeedBack = (Byte)rad.Next(0, 3);
            String Comment = "";
            String phone = "";
            String deviceToken = "";

            int commentId = _repository.AddStudentReview(Order_Id, Class_Id, FeedBack, Comment, out deviceToken, out phone);
        }

        [TestMethod]
        public void ShouldNot_AddStudentReview_IfExistComment_WithSameOrderId()
        {
            Random rad = new Random();
            int Order_Id = 1;
            int Class_Id = 0;
            Byte FeedBack = (Byte)rad.Next(1, 3);
            String Comment = "";
            String phone = "";
            String deviceToken = "";

            int commentId = _repository.AddStudentReview(Order_Id, Class_Id, FeedBack, Comment, out deviceToken, out phone);
            commentId = _repository.AddStudentReview(Order_Id, Class_Id, FeedBack, Comment, out deviceToken, out phone);

            Assert.AreEqual(commentId, -1);
        }

        [TestMethod]
        public void Should_AddStudentReview_Correctly()
        {
            Random rad = new Random();
            StudentReview comment = new StudentReview();
            int Order_Id = 1;
            int Class_Id = 0;
            Byte FeedBack = (Byte)rad.Next(1, 3);
            String Comment = "This is the comment for the class I had before." + rad.Next(0, 100).ToString();
            String phone = "";
            String deviceToken = "";

            _helperRep.StudentReviewCleanUp(comment.Order_Id);
            int commentId = _repository.AddStudentReview(Order_Id, Class_Id, FeedBack, Comment, out deviceToken, out phone);
            var comments = _repository.GetStudentReviews(_loadType, commentId);
            Assert.IsNotNull(comments);
            var commentItem = comments.FirstOrDefault();
            Assert.AreEqual(commentItem.Comment, comment.Comment);
            Assert.AreEqual(commentItem.Order_Id, comment.Order_Id);
            Assert.AreEqual(commentItem.FeedBack, comment.FeedBack);
        }


        //[TestMethod]
        //public void Should_GetStudentReview_ByCommentId()
        //{
        //    var comments = _repository.GetStudentReviews(_loadType, _commentId);
        //    Assert.IsNotNull(comments);
        //}

        [TestMethod]
        public void Should_GetStudentReview_ByOrderId()
        {
            Byte loadType = (Byte)Enums.DBAccess.CommentLoadType.ByOrderId;
            var comments = _repository.GetStudentReviews(loadType, _orderId);
            Assert.IsNotNull(comments);
        }

        //[TestMethod]
        //public void Should_GetComment_ByClassId()
        //{
        //    Byte loadType = (Byte)Enums.DBAccess.CommentLoadType.ByClassId;
        //    var comments = _repository.GetComments(loadType, _orderId);
        //    Assert.IsNotNull(comments);
        //}

        

        [TestMethod]
        public void Should_GetComment_Correctly()
        {
            //CommentRepositoryRepository repository = new CommentRepository();
            //by classId, by memberid
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repository.Dispose();
            _helperRep.Dispose();
        }
    }
}
