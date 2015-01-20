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
        int AddComment(int memberId, int classId, String commentText);
        void RemoveComment(int memberId, int commentId);
        void UpdateClassLikeTag(int memberId, int classId, Boolean isLike);

        int CreateClass(int memberId, int categoryId, Byte skillLevel, Byte teacheLevel, out Boolean isExist);
        Boolean UpdateClassInfo(Byte updateType, int classId, Byte paraValue, Byte completeStatus = 1);
        Boolean UpdateClassInfo(Byte updateType, int classId, Boolean paraValue, Byte completeStatus = 1);
        Boolean UpdateClassInfo(Byte updateType, int classId, String paraValue, Byte completeStatus = 1);

        Boolean UpdateClassEditInfo(Byte updateType, int classId, Byte paraValue);
        Boolean UpdateClassEditInfo(Byte updateType, int classId, Boolean paraValue);
        Boolean UpdateClassEditInfo(Byte updateType, int classId, String paraValue);
        List<ClassEditItem> GetClassEditInfoForAdmin(Boolean isRejected);
        List<ClassEditItem> GetClassEditInfoByMemberId(int memberId, Byte loadType);

        List<ClassListItem> GetClassTabList(Byte loadBy, Byte typeId, int memberId, String searchKey = "", Decimal posX = 0, Decimal posY = 0, int cityId = 0);
        List<ClassListItem> GetClassList(Byte loadBy, Byte OrderByType, int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, int memberId, out int totalNum, String searchKey = "", Decimal posX = 0, Decimal posY = 0);
        List<ClassItem> SearchClass(int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, out int totalNum, out int pageNum, Byte OrderByType, Boolean isAsc, Decimal posX = 0, Decimal posY = 0, String searchKey = "");
        ClassInfo GetClassInfoByClassId(int paraId);
        ClassInfo GetClassInfoByClassMemberId(int paraId, int memberId = 0);
        List<ClassInfo> GetClassInfoByTeacherId(int memberId, Byte loadType);
        List<ClassInfo> GetClassInfoByStudentId(int memberId);
        List<ClassInfo> GetClassInfoForAdmin(Boolean isRejected);
        //void AddShareClassCoin(int memberId);
        //Boolean HasShareClassCoin(int memberId);
        ClassEditItem GetClassEditInfo(int classId, int memberId = 0, Boolean isEdit = true);
        ClassEditItem GetClassEditInfo(int classId, Byte loadType);
    }

    public class ClassManager : IClassManager
    {
        private readonly IClassInfoRepository _classRep;
        private readonly IClassTagRepository _tagRep;
        private readonly IInteractiveRepository _intRep;

        public ClassManager(IClassInfoRepository classRep, IClassTagRepository tagRep, IInteractiveRepository intRep)
        {
            _classRep = classRep;
            _tagRep = tagRep;
            _intRep = intRep;
        }
        
        public int AddComment(int memberId, int classId, String commentText)
        {
            Byte saveType = (Byte)Enums.DBAccess.CommentSaveType.AddComment;
            return _intRep.UpdateComment(saveType, memberId, classId, commentText);
        }

        public void RemoveComment(int memberId, int commentId)
        {
            Byte saveType = (Byte)Enums.DBAccess.CommentSaveType.RemoveComment;
            _intRep.UpdateComment(saveType, 0, commentId, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public ClassEditItem GetClassEditInfo(int classId, int memberId = 0, Boolean isEdit = true)
        {
            Byte loadType = isEdit ? (Byte)Enums.DBAccess.ClassLoadType.ByClassId : (Byte)Enums.DBAccess.ClassLoadType.ByClassDetail;
            var result = _classRep.GetClassEditInfo(loadType, classId, memberId);
            return (result != null && result.Count > 0) ? result[0] : null;
        }

        public ClassEditItem GetClassEditInfo(int classId, Byte loadType)
        {
            var result = _classRep.GetClassEditInfo(loadType, classId, 0);
            return (result != null && result.Count > 0) ? result[0] : null;
        }

        public void UpdateClassLikeTag(int memberId, int classId, Boolean isLike)
        {
            _intRep.UpdateLike((Byte)Enums.DBAccess.FavoriteSaveType.SaveFavoriteTag, (Byte)Enums.FavoriteType.LikeClass, memberId, classId, isLike);
        }

        public int CreateClass(int memberId, int categoryId, Byte skillLevel, Byte teacheLevel, out Boolean isExist)
        {
            var result = _classRep.CreateClass(memberId, categoryId, skillLevel, teacheLevel, out isExist);
            return result;
        }

        public List<ClassEditItem> GetClassEditInfoForAdmin(Boolean isRejected)
        {
            if (isRejected)
            {
                return _classRep.GetClassEditInfo((Byte)Enums.DBAccess.ClassLoadType.ByRejected, 0, 0);
            }
            else
            {
                return _classRep.GetClassEditInfo((Byte)Enums.DBAccess.ClassLoadType.ByUnProved, 0, 0);
            }
        }

        /*For new class edit page 1.1*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public Boolean UpdateClassEditInfo(Byte updateType, int classId, Byte paraValue)
        {
            _classRep.UpdateClassEditInfo(updateType, classId, paraValue);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public Boolean UpdateClassEditInfo(Byte updateType, int classId, Boolean paraValue)
        {
            _classRep.UpdateClassEditInfo(updateType, classId, paraValue);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public Boolean UpdateClassEditInfo(Byte updateType, int classId, String paraValue)
        {
            _classRep.UpdateClassEditInfo(updateType, classId, paraValue);
            return true;
        }

        public List<ClassListItem> GetClassTabList(Byte loadBy, Byte typeId, int memberId, String searchKey = "", Decimal posX = 0, Decimal posY = 0, int cityId = 0)
        {
            var classes = _classRep.GetClassTabList(loadBy, typeId, memberId, searchKey, posX, posY, cityId);
            if (classes != null && classes.Count() > 0)
            {
                return classes;
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderByType"></param>
        /// <param name="cityId"></param>
        /// <param name="categoryId"></param>
        /// <param name="isParentCate"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageId"></param>
        /// <param name="memberId"></param>
        /// <param name="totalNum"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public List<ClassListItem> GetClassList(Byte loadBy, Byte OrderByType, int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, int memberId, out int totalNum, String searchKey = "", Decimal posX = 0, Decimal posY = 0)
        {
            var classes = _classRep.GetClassList(loadBy, OrderByType, cityId, categoryId, isParentCate, pageSize, pageId, memberId, out totalNum, searchKey, posX, posY);
            if (classes != null && classes.Count() > 0)
            {
                return classes;
            }
            return null;
        }

        public List<ClassItem> SearchClass(int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, out int totalNum, out int pageNum, Byte OrderByType, Boolean isAsc, Decimal posX = 0, Decimal posY = 0, String searchKey = "")
        {
            totalNum = 0;
            pageNum = 0;
            var classes = _classRep.SearchClass(cityId, categoryId, isParentCate, searchKey, pageSize, pageId, out totalNum);
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
                }

                totalNum = result.Count();
                pageNum = (totalNum % pageSize == 0) ? (totalNum / pageSize) : (totalNum / pageSize) + 1;

                // return null when selected page id bigger than page number
                if (pageId > pageNum)
                {
                    return null;
                }

                result = result.Skip(pageSize * (pageId - 1))
                                   .Take(pageSize)
                                   .ToList();
                return result;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public ClassInfo GetClassInfoByClassId(int classId)
        {
            var classes = _classRep.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByClassId, classId);

            return ((classes != null && classes.Count > 0) ? classes[0] : null);
        }

        public ClassInfo GetClassInfoByClassMemberId(int classId, int memberId = 0)
        {
            var classes = _classRep.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByClassAndCurrMemberId, classId, memberId);
            return ((classes != null && classes.Count > 0) ? classes[0] : null);
        }

        public List<ClassEditItem> GetClassEditInfoByMemberId(int memberId, Byte loadType)
        {
            return _classRep.GetClassEditInfo(loadType, 0, memberId);
        }

        /// <summary>
        /// Get member class info without member info
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="onlyPublished"></param>
        /// <returns></returns>
        public List<ClassInfo> GetClassInfoByTeacherId(int memberId, Byte loadType)
        {
            return _classRep.GetClassInfo(loadType, memberId);
        }

        public List<ClassInfo> GetClassInfoByStudentId(int memberId)
        {
            return _classRep.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByStudentId, memberId);
        }

        #region For class edit page 1.0, old class edit process

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

        public List<ClassInfo> GetClassInfoForAdmin(Boolean isRejected)
        {
            if (isRejected)
            {
                return _classRep.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByRejected, 0);
            }
            else
            {
                return _classRep.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByUnProved, 0);
            }
        }

        #endregion

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

    }

}
