using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.DataSource.Data;
//using EF.Frameworks.Common.CachingEF;
//using EF.Frameworks.Orpheus.ContentManagementEF;


namespace SkillBank.Site.Services.Managers
{
    public interface IReportToolsManager
    {
        List<ReportNumItem> GetReportClassMemberNum();
        List<ReportOrderRemind_Load_p_Result> GetReportOrderRemindList(Byte loadBy, int dayBuffer, out DateTime handleDate);
        List<ReportOrderStatus_Load_p_Result> GetReportClassMemberNum(Byte loadBy, DateTime beginDate, DateTime endDate);
        List<RecommendationItem> GetRecommendation(int classId);
        void SaveMasterMember(int memberId, String paraStr, char split);
        void SaveRecommendationClass(int classId, String paraStr, char split);
        List<String> GetWeChatUser();
    }

    public class ReportToolsManager : IReportToolsManager
    {
        private readonly IReportToolsRepository _repository;

        public ReportToolsManager(IReportToolsRepository repository)
        {
            _repository = repository;
        }

        public List<ReportNumItem> GetReportClassMemberNum()
        {
            return _repository.GetReportClassMemberNum();
        }

        public List<ReportOrderStatus_Load_p_Result> GetReportClassMemberNum(Byte loadBy, DateTime beginDate, DateTime endDate)
        {
            return _repository.GetReportOrderStatus(loadBy, beginDate, endDate);
        }

        public List<ReportOrderRemind_Load_p_Result> GetReportOrderRemindList(Byte loadBy, int dayBuffer, out DateTime handleDate)
        {
            return _repository.GetReportOrderRemindList(loadBy, dayBuffer, out handleDate);
        }

        public List<RecommendationItem> GetRecommendation(int classId)
        {
            return _repository.GetRecommendation(classId);
        }

        public void SaveMasterMember(int memberId, String paraStr, char split)
        {
            _repository.SaveMasterMember(memberId, paraStr, split);
        }

        public void SaveRecommendationClass(int classId, String paraStr, char split)
        {
            _repository.SaveRecommendationClass(classId, paraStr, split);
        }

        public List<String> GetWeChatUser()
        {
            return _repository.GetWeChatUser();
        }

    }
}
