﻿using System;
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

using EF.Frameworks.Common.ConfigurationEF;

namespace SkillBank.Site.DataSource.Data
{
    public interface IClassInfoRepository
    {
        int CreateClass(int memberId, int categoryId, Byte skillLevel, Byte teacheLevel, out Boolean isExist);
        List<ClassInfo> GetClassInfo(Byte loadBy, int paraId, int memberId = 0);
        List<ClassListItem> GetClassTabList(Byte loadBy, Byte typeId, int memberId, String searchKey = "", Decimal posX = 0, Decimal posY = 0, int cityId = 0);
        List<ClassListItem> GetClassList(Byte loadBy, Byte orderByType, int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, int memberId, out int totalNum, String searchKey = "", Decimal posX = 0, Decimal posY = 0);
        List<ClassInfo_LoadByPage_p_Result> GetClassPagingList(Byte categoryId, int cityId, Decimal posX, Decimal posY, int memberId, int minId, int maxId, Byte orderBy);
        List<ClassInfo_LoadByKey_p_Result> GetClassSearchList(String searchKey, int cityId, Decimal posX, Decimal posY, int minId, int maxId, Byte orderBy);
        List<ClassCollection_Load_p_Result> GetClassCollection(Byte loadBy, int memberId, int paraId);

        /*For new class edit page 1.1*/
        int UpdateClassEditInfo(Byte savaType, ClassInfo classInfo);
        void UpdateClassEditInfo(Byte saveType, int classId, Byte paraValue, Boolean isValue = true);
        void UpdateClassEditInfo(Byte saveType, int classId, Byte paraValue, String txtValue);
        void UpdateClassEditInfo(Byte saveType, int classId, Boolean isValue);
        void UpdateClassEditInfo(Byte saveType, int classId, String txtValue);

        [Obsolete]
        List<ClassItem> SearchClass(int cityId, Byte categoryId, Boolean isParentCate, String searchKey, int pageSize, int pageId, out int resultNum);
        List<ClassEditItem> GetClassEditInfo(Byte loadBy, int classId, int memberId = 0);
    }

    public class ClassInfoRepository : Entities, IClassInfoRepository
    {
        public ClassInfoRepository()
        // : base("name=Entities")
        {
        }

        public List<ClassCollection_Load_p_Result> GetClassCollection(Byte loadBy, int memberId, int paraId)
        {
            return ClassCollection_Load_p(loadBy, memberId, paraId);
        }

        public int CreateClass(int memberId, int categoryId, Byte skillLevel, Byte teacheLevel, out Boolean isExist)
        {
            return ClassInfo_Add_p(memberId, categoryId, skillLevel, teacheLevel, out isExist);
        }

        /*For new class edit page 1.1*/
        public void UpdateClassEditInfo(Byte saveType, int classId, Byte paraValue, Boolean isValue = true)
        {
            ClassEditInfo_Save_p(saveType, classId, paraValue, isValue, "");
        }

        public void UpdateClassEditInfo(Byte saveType, int classId, Byte paraValue, String txtValue)
        {
            ClassEditInfo_Save_p(saveType, classId, paraValue, true, txtValue);
        }

        public void UpdateClassEditInfo(Byte saveType, int classId, String txtValue)
        {
            ClassEditInfo_Save_p(saveType, classId, 0, true, txtValue);
        }

        public void UpdateClassEditInfo(Byte saveType, int classId, Boolean isValue)
        {
            ClassEditInfo_Save_p(saveType, classId, 0, isValue, "");
        }

        public int UpdateClassEditInfo(Byte savaType, ClassInfo classinfo)
        {
            var result = ClassEditInfo_Add_p(savaType, classinfo.ClassId, classinfo.Member_Id, classinfo.Category_Id, classinfo.Level, classinfo.SkillLevel, classinfo.TeacheLevel, classinfo.Title, classinfo.Summary, classinfo.WhyU, classinfo.Cover, classinfo.Location, classinfo.Period, classinfo.Remark, classinfo.Available, classinfo.Tags, classinfo.HasOnline);
            return result;
        }
        
        public List<ClassEditItem> GetClassEditInfo(Byte loadBy, int classId, int memberId = 0)
        {
            var result = ClassEditInfo_Load_p(loadBy, classId, memberId);
            return (result != null && result.Count > 0) ? result : null;
        }

        private List<ClassEditItem> ClassEditInfo_Load_p(Byte loadBy, int classId, int memberId)
        {
            var loadByParameter = new ObjectParameter("loadBy", loadBy);
            var memberIdParameter = new ObjectParameter("memberId", memberId);
            var classIdParameter = new ObjectParameter("classId", classId);

            var result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassEditInfo_Load_p_Result>("ClassEditInfo_Load_p", MergeOption.NoTracking, loadByParameter, memberIdParameter, classIdParameter);
            return ClassMapper.Map(result);
        }

        public List<ClassInfo> GetClassInfo(Byte loadBy, int paraId, int memberId = 0)
        {
            var loadByParameter = new ObjectParameter("loadBy", loadBy);
            var paraIdParameter = new ObjectParameter("paraId", paraId);
            var memberIdParameter = new ObjectParameter("memberId", memberId);
            var result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassInfo_Load_p_Result>("ClassInfo_Load_p", MergeOption.NoTracking, loadByParameter, paraIdParameter, memberIdParameter);
            return ClassMapper.Map(result);
        }

        [Obsolete]
        public List<ClassItem> SearchClass(int cityId, Byte categoryId, Boolean isParentCate, String searchKey, int pageSize, int pageId, out int resultNum)
        {
            Byte loadBy = 1;
            int memberId = 0;
            var result = ClassInfo_LoadAll_p(loadBy, cityId, categoryId, isParentCate, memberId, searchKey, pageSize, pageId, out resultNum);
            return result;
        }

        public List<ClassListItem> GetClassList(Byte loadBy, Byte orderByType, int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, int memberId, out int totalNum, String searchKey = "", Decimal posX = 0, Decimal posY = 0)
        {
            var result = ClassInfo_LoadByList_p(loadBy, orderByType, cityId, categoryId, isParentCate, pageSize, pageId, memberId, out totalNum, searchKey, posX, posY);
            return result;
        }

        public List<ClassListItem> GetClassTabList(Byte loadBy, Byte typeId, int memberId, String searchKey = "", Decimal posX = 0, Decimal posY = 0, int cityId = 0)
        {
            var result = ClassInfo_LoadByTab_p(loadBy, typeId, memberId, searchKey, posX, posY, cityId);
            return result;
        }

        public List<ClassInfo_LoadByPage_p_Result> GetClassPagingList(Byte categoryId, int cityId, Decimal posX, Decimal posY, int memberId, int pageId, int pageSize, Byte orderBy)
        {
            return ClassInfo_LoadByPage_p(categoryId, cityId, posX, posY, memberId, pageId, pageSize, orderBy);
        }

        public List<ClassInfo_LoadByKey_p_Result> GetClassSearchList(String searchKey, int cityId, Decimal posX, Decimal posY, int minId, int maxId, Byte orderBy)
        {
            var keywordParameter = new ObjectParameter("keyword", searchKey);
            var posXParameter = new ObjectParameter("posx", posX);
            var posYParameter = new ObjectParameter("posy", posY);
            var cityIdParameter = new ObjectParameter("cityId", cityId);
            var minIdParameter = new ObjectParameter("minId", minId);
            var maxIdParameter = new ObjectParameter("maxId", maxId);
            var orderByParameter = new ObjectParameter("orderBy", orderBy);

            ObjectResult<ClassInfo_LoadByKey_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassInfo_LoadByKey_p_Result>("ClassInfo_LoadByKey_p", MergeOption.NoTracking, cityIdParameter, keywordParameter, posXParameter, posYParameter, minIdParameter, maxIdParameter, orderByParameter);
            return result.ToList();
        }

        private List<ClassInfo_LoadByPage_p_Result> ClassInfo_LoadByPage_p(Byte categoryId, int cityId, Decimal posX, Decimal posY, int memberId, int minId, int maxId, Byte orderBy)
        {
            var cateIdParameter = new ObjectParameter("categoryId", categoryId);
            var memberIdParameter = new ObjectParameter("memberId", memberId);//left for member id
            var posXParameter = new ObjectParameter("posx", posX);
            var posYParameter = new ObjectParameter("posy", posY);
            var cityIdParameter = new ObjectParameter("cityId", cityId);
            var minIdParameter = new ObjectParameter("minId", minId);
            var maxIdParameter = new ObjectParameter("maxId", maxId);
            var orderByParameter = new ObjectParameter("orderBy", orderBy);

            ObjectResult<ClassInfo_LoadByPage_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassInfo_LoadByPage_p_Result>("ClassInfo_LoadByPage_p", MergeOption.NoTracking, cityIdParameter, cateIdParameter, posXParameter, posYParameter, memberIdParameter, minIdParameter, maxIdParameter, orderByParameter);
            return result.ToList();
        }

        private List<ClassListItem> ClassInfo_LoadByTab_p(Byte loadBy, Byte typeId, int memberId, String searchKey, Decimal posX, Decimal posY, int cityId)
        {
            var loadByParameter = new ObjectParameter("loadType", loadBy);
            var cateIdParameter = new ObjectParameter("categoryId", typeId);
            var memberIdParameter = new ObjectParameter("memberId", memberId);//left for member id
            var keywordParameter = new ObjectParameter("keyword", searchKey);
            var posXParameter = new ObjectParameter("posx", posX);
            var posYParameter = new ObjectParameter("posy", posY);
            var cityIdParameter = new ObjectParameter("cityId", cityId);

            ObjectResult<ClassInfo_LoadByTab_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassInfo_LoadByTab_p_Result>("ClassInfo_LoadByTab_p", MergeOption.NoTracking, loadByParameter, cateIdParameter, memberIdParameter, keywordParameter, posXParameter, posYParameter, cityIdParameter);
            return ClassMapper.Map(result);
        }

        /// <summary>
        /// TO DO:Add Class Complete Status Tag in SP (???? Not Need anymore)
        /// </summary>
        /// <param name="loadBy"></param>
        /// <param name="cityId"></param>
        /// <param name="categoryId"></param>
        /// <param name="isParentCate"></param>
        /// <param name="memberId"></param>
        /// <param name="searchKey"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageId"></param>
        /// <param name="resultNum"></param>
        /// <returns></returns>
        private List<ClassItem> ClassInfo_LoadAll_p(Byte loadBy, int cityId, Byte categoryId, Boolean isParentCate, int memberId, String searchKey, int pageSize, int pageId, out int resultNum)
        {
            resultNum = 0;
            var loadByParameter = new ObjectParameter("LoadBy", loadBy);
            var cityIdParameter = new ObjectParameter("City", cityId);
            var cateIdParameter = new ObjectParameter("CategoryId", categoryId);
            var paraIdParameter = new ObjectParameter("paraId", memberId);//left for member id
            var keywordParameter = new ObjectParameter("KeyWord", searchKey);
            var pageSizeParameter = new ObjectParameter("PageSize", pageSize);//left for member id
            var pageIdParameter = new ObjectParameter("PageId", pageId);
            var resultNumParameter = new ObjectParameter("ResultNum", resultNum);//left for member id
            var isParentCateParameter = new ObjectParameter("isParentCate", isParentCate);//left for member id

            ObjectResult<ClassInfo_LoadAll_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassInfo_LoadAll_p_Result>("ClassInfo_LoadAll_p", MergeOption.NoTracking, loadByParameter, cityIdParameter, cateIdParameter, isParentCateParameter, paraIdParameter, keywordParameter, pageSizeParameter, pageIdParameter, resultNumParameter);
            //Context.ObjectStateManager.ChangeObjectState(result,System.Data.EntityState.
            //Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, result);
            resultNum = Convert.ToInt32(resultNumParameter.Value);
            return ClassMapper.Map(result);
        }

        private List<ClassListItem> ClassInfo_LoadByList_p(Byte loadBy, Byte orderType, int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, int memberId, out int resultNum, String searchKey, Decimal posX, Decimal posY)
        {
            var loadByParameter = new ObjectParameter("loadType", loadBy);
            var orderTypeParameter = new ObjectParameter("orderType", orderType);
            var isParentCateParameter = new ObjectParameter("isParentCate", isParentCate);
            var cateIdParameter = new ObjectParameter("categoryId", categoryId);
            var cityIdParameter = new ObjectParameter("city", cityId);
            var memberIdParameter = new ObjectParameter("memberId", memberId);
            var keywordParameter = new ObjectParameter("keyword", searchKey);
            var posXParameter = new ObjectParameter("posX", posX);
            var posYParameter = new ObjectParameter("posY", posY);
            var pageSizeParameter = new ObjectParameter("pageSize", pageSize);
            var pageIdParameter = new ObjectParameter("pageId", pageId);
            var resultNumParameter = new ObjectParameter("resultNum", typeof(Int32));

            ObjectResult<ClassInfo_LoadByList_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassInfo_LoadByList_p_Result>("ClassInfo_LoadByList_p", MergeOption.NoTracking, loadByParameter, orderTypeParameter, cityIdParameter, cateIdParameter, isParentCateParameter, memberIdParameter, keywordParameter, posXParameter, posYParameter, pageSizeParameter, pageIdParameter, resultNumParameter);
            resultNum = Convert.ToInt32(resultNumParameter.Value);
            return ClassMapper.Map(result);
        }

        private int ClassInfo_Add_p(int memberId, int categoryId, Byte skillLevel, Byte teacheLevel, out Boolean isExist)
        {
            ObjectParameter paraIdParameter = new ObjectParameter("ParaId", 0);//class id
            ObjectParameter categoryIdParameter = new ObjectParameter("CategoryId", categoryId);
            ObjectParameter memberIdParameter = new ObjectParameter("MemberId", memberId);
            ObjectParameter teacheParameter = new ObjectParameter("Teache", teacheLevel);
            ObjectParameter skillParameter = new ObjectParameter("Skill", skillLevel);
            ObjectParameter isExistParameter = new ObjectParameter("IsExist", false);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClassInfo_Add_p", categoryIdParameter, memberIdParameter, teacheParameter, skillParameter, paraIdParameter, isExistParameter);
            isExist = (bool)isExistParameter.Value;
            return (int)paraIdParameter.Value;
        }

        //private void ClassInfo_Save_p(Byte saveType, int classId, Byte paraValue, Boolean isValue, String txtValue)
        //{
        //    ObjectParameter saveTypeParameter = new ObjectParameter("SaveType", saveType);
        //    ObjectParameter paraValueParameter = new ObjectParameter("ParaValue", paraValue);
        //    ObjectParameter isValueParameter = new ObjectParameter("IsValue", isValue);
        //    ObjectParameter txtValueParameter = new ObjectParameter("TxtValue", txtValue);
        //    ObjectParameter paraIdParameter = new ObjectParameter("ParaId", classId);

        //    ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClassInfo_Save_p", saveTypeParameter, paraValueParameter, isValueParameter, txtValueParameter, paraIdParameter);
        //}

        private void ClassEditInfo_Save_p(Byte saveType, int classId, Byte paraValue, Boolean isValue, String txtValue)
        {
            ObjectParameter saveTypeParameter = new ObjectParameter("SaveType", saveType);
            ObjectParameter paraValueParameter = new ObjectParameter("ParaValue", paraValue);
            ObjectParameter isValueParameter = new ObjectParameter("IsValue", isValue);
            ObjectParameter txtValueParameter = new ObjectParameter("TxtValue", txtValue);
            ObjectParameter paraIdParameter = new ObjectParameter("ParaId", classId);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClassEditInfo_Save_p", paraIdParameter, txtValueParameter, paraValueParameter, isValueParameter, saveTypeParameter);
        }

        private int ClassEditInfo_Add_p(Byte saveType, int classId, int memberId, Byte categoryId, Byte level, int skill, int teach, String title, String summary, String whyU, String cover, String location, String period, String remark, String available, String tags, Boolean hasOnline)
        {
            ObjectParameter saveTypeParameter = new ObjectParameter("saveType", saveType);
            ObjectParameter memberIdParameter = new ObjectParameter("memberId", memberId);
            ObjectParameter categoryIdParameter = new ObjectParameter("CategoryId", categoryId);
            ObjectParameter levelParameter = new ObjectParameter("level", level);
            ObjectParameter skillParameter = new ObjectParameter("skill", skill);
            ObjectParameter teachParameter = new ObjectParameter("teach", teach);

            ObjectParameter titleParameter = new ObjectParameter("title", title);
            ObjectParameter summaryParameter = new ObjectParameter("summary", summary);
            ObjectParameter whyUParameter = new ObjectParameter("whyU", whyU);
            ObjectParameter periodParameter = new ObjectParameter("period", period);
            ObjectParameter locationParameter = new ObjectParameter("location", location);
            ObjectParameter availableParameter = new ObjectParameter("available", available);
            ObjectParameter remarkParameter = new ObjectParameter("remark", remark);
            ObjectParameter coverParameter = new ObjectParameter("cover", cover);
            ObjectParameter classIdParameter = new ObjectParameter("classId", classId);
            ObjectParameter tagsParameter = new ObjectParameter("Tags", tags);
            ObjectParameter hasOnlineParameter = new ObjectParameter("HasOnline", hasOnline);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClassEditInfo_Add_p", saveTypeParameter, memberIdParameter, classIdParameter, categoryIdParameter, levelParameter, skillParameter, teachParameter, titleParameter, summaryParameter, whyUParameter, periodParameter, locationParameter, availableParameter, remarkParameter, coverParameter, tagsParameter, hasOnlineParameter);
            var result = Convert.ToInt32(classIdParameter.Value);
            return result;
        }

        private List<ClassCollection_Load_p_Result> ClassCollection_Load_p(Byte loadBy, int memberId, int paraId)
        {
            var loadByParameter = new ObjectParameter("loadBy", loadBy);
            var memberIdParameter = new ObjectParameter("memberId", memberId);
            var paraIdParameter = new ObjectParameter("paraId", paraId);

            ObjectResult<ClassCollection_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassCollection_Load_p_Result>("ClassCollection_Load_p", MergeOption.NoTracking, loadByParameter, memberIdParameter, paraIdParameter);
            return result.ToList();
        }
    }
}
