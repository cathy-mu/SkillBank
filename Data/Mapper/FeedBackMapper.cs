using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Mapper
{
    public class FeedBackMapper
    {
        public static List<TeacherReviewItem> Map(ObjectResult<TeacherReview_Load_p_Result> objReview)
        {
            if (objReview != null)
            {
                var reviews = objReview.Select(item => new TeacherReviewItem() { Avatar = item.Avatar, Comment = item.Comment, FeedBack = item.FeedBack, CreatedDate = item.CreatedDate, Order_Id = item.Order_Id, PrivateComment = item.PrivateComment, ReviewId = item.ReviewId, MemberId = item.MemberId, Name = item.Name }).ToList();
                return (reviews.Count > 0) ? reviews : null;
            }
            return null;
        }

        public static List<StudentReviewItem> Map(ObjectResult<StudentReview_Load_p_Result> objReview)
        {
            if (objReview != null)
            {
                var reviews = objReview.Select(item => new StudentReviewItem() { Comment = item.Comment, FeedBack = item.FeedBack, CreatedDate = item.CreatedDate, Order_Id = item.Order_Id, PrivateComment = item.PrivateComment, ReviewId = item.ReviewId,  Score1 = item.Score1, Score2 = item.Score2, Score3 = item.Score3}).ToList();
                return (reviews.Count > 0) ? reviews : null;
            }
            return null;
        }

        public static List<MemberReviewItem> Map(ObjectResult<StudentReview_LoadByClass_p_Result> objReview)
        {
            if (objReview != null)
            {
                var reviews = objReview.Select(item => new MemberReviewItem() { Avatar = item.Avatar, Comment = item.Comment, FeedBack = item.FeedBack, CreatedDate = item.CreatedDate, ReviewId = item.ReviewId, MemberId = item.MemberId, Name = item.Name, Title = item.Title, ClassId = item.ClassId, TabId = item.TabId.Value }).ToList();
                return (reviews.Count > 0) ? reviews : null;
            }
            return null;
        }

        public static List<MemberReviewItem> Map(ObjectResult<StudentReview_LoadByMember_p_Result> objReview)
        {
            if (objReview != null)
            {
                var reviews = objReview.Select(item => new MemberReviewItem() { Avatar = item.Avatar, Comment = item.Comment, FeedBack = item.FeedBack, CreatedDate = item.CreatedDate, ReviewId = item.ReviewId, MemberId = item.MemberId, Name = item.Name, Title = item.Title, ClassId = item.ClassId, TabId = item.TabId.Value }).ToList();
                return (reviews.Count > 0) ? reviews : null;
            }
            return null;
        }
    }
}
