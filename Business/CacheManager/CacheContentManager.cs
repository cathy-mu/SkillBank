using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.Managers;
//using EF.Frameworks.Orpheus.ContentManagementEF;


namespace SkillBank.Site.Services.Managers
{
    public interface ICacheContentManager
    {
        //List<ClassListItem> GetRecommendClassList(String key);
        List<ClassListItem> GetRecommendClassList(int classType, int minId, int maxId);
        List<ClassListItem> GetClassList(Byte loadType, int pageId, int pageSize, out int classNum);
        ClassEditItem GetClassItem(int id);
    }

    public class CacheContentManager : ICacheContentManager
    {
        private readonly IRecommendClassCacheMgr _recommendClassMgr;
        private readonly IClassListCacheMgr _classListClassMgr;
        private readonly IClassItemCacheMgr _classItemClassMgr;

        public CacheContentManager(IRecommendClassCacheMgr recommendClassMgr, IClassListCacheMgr classListClassMgr, IClassItemCacheMgr classItemClassMgr)
        {
            _recommendClassMgr = recommendClassMgr;
            _classListClassMgr = classListClassMgr;
            _classItemClassMgr = classItemClassMgr;
        }

        //public List<ClassListItem> GetRecommendClassList(String key)
        //{
        //    return _recommendClassMgr.GetClassList(key);
        //}

        public List<ClassListItem> GetRecommendClassList(int classType, int minId, int maxId)
        {
            return _recommendClassMgr.GetClassList(classType, minId, maxId);
        }

        public List<ClassListItem> GetClassList(Byte loadType, int pageId, int pageSize, out int classNum)
        {
            return _classListClassMgr.GetClassList(loadType, pageId, pageSize, out classNum);
        }

        public ClassEditItem GetClassItem(int id)
        {
            var classItem = _classItemClassMgr.GetClassItem(id);
            if (classItem == null)
            {
                _classItemClassMgr.RefreshItem(id);
                classItem = _classItemClassMgr.GetClassItem(id);
            }
            return classItem;
        }
        
    }
}
