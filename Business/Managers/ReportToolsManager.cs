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
        List<RecommendationItem> GetRecommendation(int classId);
        void SaveMasterMember(int memberId, String paraStr, char split);
        void SaveRecommendationClass(int classId, String paraStr, char split);
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
    

    }
}
