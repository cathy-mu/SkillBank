using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Data.Entity;

using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using SkillBank.Site.DataSource.Mapper;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.DataSource.Data
{
    public interface IReportToolsRepository
    {
        List<ReportNumItem> GetReportClassMemberNum();
        List<ReportOrderStatus_Load_p_Result> GetReportOrderStatus(Byte loadType, DateTime beginDate, DateTime endDate);
        void SaveMasterMember(int memberId, String paraStr, char split);
        void SaveRecommendationClass(int classId, String paraStr, char split);
        List<RecommendationItem> GetRecommendation(int classId);
    }

    public class ReportToolsRepository : Entities, IReportToolsRepository
    {
        public ReportToolsRepository()
        {
        }

        public List<RecommendationItem> GetRecommendation(int classId)
        {
            var result = Recommendation_Load_p(classId);
            return ReportToolsMapper.Map(result);
        }

        public List<ReportNumItem> GetReportClassMemberNum()
        {
            var result = ReportClassMemberNum_Load_p();
            return ReportToolsMapper.Map(result);
        }

        public void SaveMasterMember(int memberId, String paraStr, char split)
        {
            Master_Save_p(memberId, paraStr, split);
        }
        
        public void SaveRecommendationClass(int classId, String paraStr, char split)
        {
            Recommendation_Save_p(classId, paraStr, split);
        }

        public List<ReportOrderStatus_Load_p_Result> GetReportOrderStatus(Byte loadBy, DateTime beginDate, DateTime endDate)
        {
            var result = ReportOrderStatus_Load_p(loadBy, beginDate, endDate);
            return ReportToolsMapper.Map(result);
        }

        private ObjectResult<ReportClassMemberNum_Load_p_Result> ReportClassMemberNum_Load_p()
        {
            ObjectResult<ReportClassMemberNum_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReportClassMemberNum_Load_p_Result>("ReportClassMemberNum_Load_p", MergeOption.NoTracking);
            return result;
        }

        private ObjectResult<Recommendation_Load_p_Result> Recommendation_Load_p(int classId)
        {
            var classIdParameter = new ObjectParameter("ClassId", classId);
            ObjectResult<Recommendation_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Recommendation_Load_p_Result>("Recommendation_Load_p", MergeOption.NoTracking, classIdParameter);
            return result;
        }


        private void Recommendation_Save_p(int classId, String paraStr, char split)
        {
            var classIdParameter = new ObjectParameter("ClassId", classId);
            var paraStrParameter = new ObjectParameter("ParaStr", paraStr);
            var splitParameter = new ObjectParameter("Split", split);
            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Recommendation_Save_p", classIdParameter, paraStrParameter, splitParameter);
        }

        private void Master_Save_p(int memberId, String paraStr, char split)
        {
            var memberIdParameter = new ObjectParameter("MemberId", memberId);
            var paraStrParameter = new ObjectParameter("ParaStr", paraStr);
            var splitParameter = new ObjectParameter("Split", split);
            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Master_Save_p", memberIdParameter, paraStrParameter, splitParameter);
        }



        private ObjectResult<ReportOrderStatus_Load_p_Result> ReportOrderStatus_Load_p(Byte loadType, DateTime beginDate, DateTime endDate)
        {
            var beginDateParameter = new ObjectParameter("BeginDate", beginDate);
            var endDateParameter = new ObjectParameter("EndDate", endDate);
            var loadTypeParameter = new ObjectParameter("LoadType", loadType);
            ObjectResult<ReportOrderStatus_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReportOrderStatus_Load_p_Result>("ReportOrderStatus_Load_p", MergeOption.NoTracking, loadTypeParameter, beginDateParameter, endDateParameter);
            return result;
        }


    }
}


