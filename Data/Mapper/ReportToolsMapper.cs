using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Mapper
{
    public class ReportToolsMapper
    {


        public static List<ReportNumItem> Map(ObjectResult<ReportClassMemberNum_Load_p_Result> objOrders)
        {
            if (objOrders != null)
            {
                var orders = objOrders.Select(item => new ReportNumItem() { RowNum = item.RowNum, Num1 = item.Num1.Value, Num2 = item.Num2.Value, Percentage = item.Percentage.Value }).ToList();
                return (orders.Count > 0) ? orders : null;
            }
            return null;
        }

        public static List<RecommendationItem> Map(ObjectResult<Recommendation_Load_p_Result> objItems)
        {
            if (objItems != null)
            {
                var items = objItems.Select(item => new RecommendationItem() { GroupId = item.GroupId, IsShow = item.IsShow, Rank = item.Rank }).ToList();
                return (items.Count > 0) ? items : null;
            }
            return null;
        }

        public static List<ReportOrderStatus_Load_p_Result> Map(ObjectResult<ReportOrderStatus_Load_p_Result> objItems)
        {
            if (objItems != null)
            {
                var items = objItems.Select(item => new ReportOrderStatus_Load_p_Result() { GroupId = item.GroupId,  BookedDate = item.BookedDate, CreatedDate = item.CreatedDate, Class_Id = item.Class_Id,Member_Id = item.Member_Id, Student_Id = item.Student_Id, Title=item.Title, StudentReview = item.StudentReview, TeacherReview = item.TeacherReview}).ToList();
                return (items.Count > 0) ? items : null;
            }
            return null;
        }

    }
}
