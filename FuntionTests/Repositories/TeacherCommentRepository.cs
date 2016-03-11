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
    public class TeacherReviewRepositoryTests
    {
        private TeacherReviewRepository _repository;
        private TestHelperRepository _helperRep;
        Byte _loadType;
        private int _commentId;
        private int _orderId;
        //private int _invalidMemberId;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new TeacherReviewRepository();
            _helperRep = new TestHelperRepository();
            _commentId = 1;
            //Get from DB By Id
            _loadType = (Byte)Enums.DBAccess.CommentLoadType.ByCommentId;
            _orderId = 1;
            //_invalidMemberId = 99999;
        }
                
        [TestMethod]
        public void Should_AddTeacherReview()
        {
            Random rad = new Random();
            int orderId = 1;
            Byte feedBack = (Byte)rad.Next(1, 3);
            String comment = "";
            String phone = "";
            String deviceToken = "";

            int commentId = _repository.AddTeacherReview(orderId, feedBack, comment, out deviceToken, out phone);
        }

        [TestMethod]
        public void ShouldNot_AddTeacherReview_IfExistComment_WithSameOrderId()
        {
            Random rad = new Random();
            int orderId = 1;
            Byte feedBack = (Byte)rad.Next(1, 3);
            String comment = "";
            String phone = "";
            String deviceToken = "";

            int commentId = _repository.AddTeacherReview(orderId, feedBack, comment, out deviceToken, out phone);
            commentId = _repository.AddTeacherReview(orderId, feedBack, comment, out deviceToken, out phone);

            Assert.AreEqual(commentId, -1);
        }

        [TestMethod]
        public void Should_AddTeacherReview_Correctly()
        {
            Random rad = new Random();
            int orderId = 1;
            Byte feedBack = (Byte)rad.Next(0, 3);
            String comment = "Here is teacher comment for testing." + rad.Next(0, 10).ToString();
            String deviceToken;
            String phone;

            _helperRep.TeacherReviewCleanUp(orderId);
            int commentId = _repository.AddTeacherReview(orderId, feedBack, comment, out deviceToken, out phone);
            var comments = _repository.GetTeacherReviews(_loadType, commentId,0);
            Assert.IsNotNull(comments);
            var commentItem = comments.FirstOrDefault();
            Assert.AreEqual(commentItem.Comment, comment);
            Assert.AreEqual(commentItem.Order_Id, orderId);
            Assert.AreEqual(commentItem.FeedBack, feedBack);
        }

        [TestMethod]
        public void Should_GetTeacherReviews_ByOrderId()
        {
            Byte loadType = (Byte)Enums.DBAccess.CommentLoadType.ByOrderId;
            var comments = _repository.GetTeacherReviews(loadType, _orderId);
            Assert.IsNotNull(comments);
        }

        //[TestMethod]
        //public void Should__GetTeacherReviews_ByOrderId()
        //{
        //    Byte loadType = (Byte)Enums.DBAccess.CommentLoadType.ByOrderId;
        //    var comments = _repository.GetTeacherReviews(loadType, _orderId);
        //    Assert.IsNotNull(comments);
        //}

        [TestMethod]
        public void Should_GetTeacherReview_ByCommentId()
        {
            var comments = _repository.GetTeacherReviews(_loadType, _commentId);
            Assert.IsNotNull(comments);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repository.Dispose();
            _helperRep.Dispose();
        }
    }
}


