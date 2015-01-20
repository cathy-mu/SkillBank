using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.Services.Managers
{
    public interface IClassManager
    {
        int CreateClass(int memberId, int categoryId, Byte skillLevel, Byte teacheLevel, out Boolean isExist);
        Boolean UpdateClassInfo(Byte updateType, int classId, Byte paraValue, Byte completeStatus = 1);
        Boolean UpdateClassInfo(Byte updateType, int classId, Boolean paraValue, Byte completeStatus = 1);
        Boolean UpdateClassInfo(Byte updateType, int classId, String paraValue, Byte completeStatus = 1);

        int GetClassNumsByMember(int memberId, out int classId);
        int GetClassNumsByClass(int memberId, int classId, out int result1, out int result2);

        List<ClassItem> SearchClass(int cityId, int categoryId, Byte OrderByType, Boolean isAsc, Decimal posX = 0, Decimal posY = 0, String searchKey = "");
        List<ClassItem> GetMemberClass(int memberId);
        ClassInfo GetClassInfoByClassId(int paraId);
        List<ClassInfo> GetClassInfoByTeacherId(int memberId);
        List<ClassInfo> GetClassInfoByStudentId(int memberId);
        List<ClassInfo> GetClassInfoByUnProvedClass();
        List<int> GetClassCounters(int classId, int memberId);
    }

    public class ClassManager : IClassManager
    {
        private readonly IClassInfoRepository _classRep;
        private readonly IClassTagRepository _tagRep;

        public ClassManager(IClassInfoRepository classRep, IClassTagRepository tagRep)
        {
            _classRep = classRep;
            _tagRep = tagRep;
        }

        public int CreateClass(int memberId, int categoryId, Byte skillLevel, Byte teacheLevel, out Boolean isExist)
        {
            var result = _classRep.CreateClass(memberId, categoryId, skillLevel, teacheLevel, out isExist);
            return result;
        }

        public Boolean UpdateClassInfo(Byte updateType, int classId, Byte paraValue, Byte completeStatus = 1)
        {
            _classRep.UpdateClassInfo(updateType, classId, paraValue);
            return true;
        }

        public Boolean UpdateClassInfo(Byte updateType, int classId, Boolean paraValue, Byte completeStatus = 1)
        {
            _classRep.UpdateClassInfo(updateType, classId, paraValue);
            return true;
        }

        public Boolean UpdateClassInfo(Byte updateType, int classId, String paraValue, Byte completeStatus = 1)
        {
            _classRep.UpdateClassInfo(updateType, classId, paraValue);
            return true;
        }

        public List<ClassItem> SearchClass(int cityId, int categoryId, Byte OrderByType, Boolean isAsc, Decimal posX = 0, Decimal posY = 0, String searchKey="")
        {
            var resultNum = 0;
            var classes = _classRep.SearchClass(cityId, categoryId, searchKey, 1000, 1, out resultNum);//, int pageSize, int pageId, out int resultNum
            if (classes != null && classes.Count() > 0)
            {
                List<ClassItem> result;
                switch ((ClientSetting.ClassListOrderType)OrderByType)
                {
                    case ClientSetting.ClassListOrderType.ByRank:
                        if (isAsc)
                        {
                            result = classes.OrderBy(c => c.Rank).ThenByDescending(c => c.LastUpdateDate).ToList();
                        }
                        else
                        {
                            result = classes.OrderByDescending(c => c.Rank).ThenByDescending(c => c.LastUpdateDate).ToList();
                        }
                        break;
                    case ClientSetting.ClassListOrderType.ByLevel:
                        if (isAsc)
                        {
                            result = classes.OrderBy(c => c.Level).ThenByDescending(c => c.LastUpdateDate).ToList();
                        }
                        else
                        {
                            result = classes.OrderByDescending(c => c.Level).ThenByDescending(c => c.LastUpdateDate).ToList();
                        }
                        break;
                    case ClientSetting.ClassListOrderType.ByLastUpdate:
                        if (isAsc)
                        {
                            result = classes.OrderBy(c => c.LastUpdateDate).ToList();
                        }
                        else
                        {
                            result = classes.OrderByDescending(c => c.LastUpdateDate).ToList();
                        }
                        break;
                    default://by Distince
                        result = classes;
                        if (isAsc)
                        {
                            result = classes.OrderBy(c => (posX - c.PosX) * (posX - c.PosX) + (posY - c.PosY) * (posY - c.PosY)).ToList();
                        }
                        else
                        {
                            result = classes.OrderByDescending(c => (posX - c.PosX) * (posX - c.PosX) + (posY - c.PosY) * (posY - c.PosY)).ToList();
                        }
                        break;
                } return result;
            }
            return null;
        }

        public List<ClassItem> GetMemberClass(int memberId)
        {
            return _classRep.GetMemberClass(memberId);
        }

        public ClassInfo GetClassInfoByClassId(int classId)
        {
            var classes = _classRep.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByClassId, classId);
            return ((classes!=null && classes.Count> 0)? classes[0] : null);
        }

        public List<ClassInfo> GetClassInfoByTeacherId(int memberId)
        {
            return _classRep.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByTeacherId, memberId);
        }

        public List<ClassInfo> GetClassInfoByStudentId(int memberId)
        {
            return _classRep.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByStudentId, memberId);
        }
        
        public List<ClassInfo> GetClassInfoByUnProvedClass()
        {
            return _classRep.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByStudentId, 0);
        }

        //TO DO:Check it's class id or member id
        //Get Class number
        public List<int> GetClassCounters(int classId,int memberId)
        {
            return new List<int>();
        }

        public List<Boolean> CheckClassSteps(Int16 stepNo)
        {
            List<short> ClassStepList = new List<short>() { 1, 2, 4, 8 };
            List<Boolean> isFinishList = new List<Boolean>();

            for (int i = 0; i < ClassStepList.Count(); i++)
            {
                isFinishList.Add(!Equals((ClassStepList[i] & stepNo), 0));
            }

            return isFinishList;
        }


        public int GetClassNumsByMember(int memberId, out int result1)
        {
            int result2 = 0;
            return _classRep.GetClassNums(memberId, 0/*ClassId*/, out result1, out result2);
        }

        public int GetClassNumsByClass(int memberId, int classId, out int result1, out int result2)
        {
            return _classRep.GetClassNums(memberId, classId, out result1, out result2);
        }

    }

}
