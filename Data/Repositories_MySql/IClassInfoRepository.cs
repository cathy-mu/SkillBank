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
        List<ClassInfo> GetClassInfo(Byte loadBy, int id);
        int GetClassNums(int MemberId, int ClassId, out int Result1, out int Result2);
        void UpdateClassInfo(Byte saveType, int classId, Byte paraValue);
        void UpdateClassInfo(Byte saveType, int classId, Boolean isValue);
        void UpdateClassInfo(Byte saveType, int classId, String txtValue);

        List<ClassItem> SearchClass(int cityId, int categoryId, String searchKey);
        List<ClassItem> GetMemberClass(int memberId);
    }

    public class ClassInfoRepository : DbContext, IClassInfoRepository
    {
        public ClassInfoRepository()
            : base("name=Entities")
        {
        }

        public int CreateClass(int memberId, int categoryId, Byte skillLevel, Byte teacheLevel, out Boolean isExist)
        {
            return ClassInfo_Add_p(memberId, categoryId, skillLevel, teacheLevel, out isExist);
        }

        public void UpdateClassInfo(Byte saveType, int classId, Byte paraValue)
        {
            ClassInfo_Save_p(saveType, classId, paraValue, true, "");
        }

        public void UpdateClassInfo(Byte saveType, int classId, String txtValue)
        {
            ClassInfo_Save_p(saveType, classId, 0, true, txtValue);
        }

        public void UpdateClassInfo(Byte saveType, int classId, Boolean isValue)
        {
            ClassInfo_Save_p(saveType, classId, 0, isValue, "");
        }

        private void UpdateClassInfo(Byte saveType, int classId, Byte paraValue, Boolean isValue, String txtValue)
        {
            ClassInfo_Save_p(saveType, classId, paraValue, isValue, txtValue);
        }

        public int GetClassNums(int MemberId, int ClassId, out int Result1, out int Result2)
        {
            return ClassNum_Load_p(MemberId, ClassId, out Result1, out Result2);
        }

        public List<ClassInfo> GetClassInfo(Byte loadBy, int paraId)
        {
            var loadByParameter = new ObjectParameter("LoadBy", loadBy);
            var paraIdParameter = new ObjectParameter("ParaId", paraId);

            ObjectResult<ClassInfo_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassInfo_Load_p_Result>("ClassInfo_Load_p", MergeOption.NoTracking, loadByParameter, paraIdParameter);
            return ClassMapper.Map(result);
        }


        public List<ClassItem> SearchClass(int cityId, int categoryId,String searchKey)
        {
            var loadBy = (Byte)Enums.DBAccess.ClassListLoadType.SearchClass;
            int memberId = 0;
            var result = ClassInfo_Load_p(loadBy, cityId, categoryId, memberId, searchKey);
            return result;
        }

        public List<ClassItem> GetMemberClass(int memberId)
        {
            var loadBy = (Byte)Enums.DBAccess.ClassListLoadType.ByTeacherId;
            var result = ClassInfo_Load_p(loadBy, 0, 0, memberId,"");
            return result;
        }

        // <summary>
        // TO DO:Add Class Complete Statu Tag in SP 
        // </summary>
        // <param name="loadBy"></param>
        // <param name="cityId"></param>
        // <param name="categoryId"></param>
        // <param name="memberId"></param>
        // <returns></returns>
        private List<ClassItem> ClassInfo_Load_p(Byte loadBy, int cityId, int categoryId, int memberId, String searchKey)
        {
            var loadByParameter = new ObjectParameter("LoadBy", loadBy);
            var cityIdParameter = new ObjectParameter("City", cityId);
            var cateIdParameter = new ObjectParameter("CategoryId", categoryId);
            var paraIdParameter = new ObjectParameter("paraId", memberId);//left for member id
            var keywordParameter = new ObjectParameter("KeyWord", searchKey);

            ObjectResult<ClassInfo_LoadAll_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassInfo_LoadAll_p_Result>("ClassInfo_LoadAll_p", MergeOption.NoTracking, loadByParameter, cityIdParameter, cateIdParameter, paraIdParameter, keywordParameter);
            //Context.ObjectStateManager.ChangeObjectState(result,System.Data.EntityState.
            //Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, result);

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

        private void ClassInfo_Save_p(Byte saveType, int classId, Byte paraValue, Boolean isValue, String txtValue)
        {
            ObjectParameter saveTypeParameter = new ObjectParameter("SaveType", saveType);
            ObjectParameter paraValueParameter = new ObjectParameter("ParaValue", paraValue);
            ObjectParameter isValueParameter = new ObjectParameter("IsValue", isValue);
            ObjectParameter txtValueParameter = new ObjectParameter("TxtValue", txtValue);
            ObjectParameter paraIdParameter = new ObjectParameter("ParaId", classId);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClassInfo_Save_p", saveTypeParameter, paraValueParameter, isValueParameter, txtValueParameter, paraIdParameter);
        }

        private int ClassNum_Load_p(int memberId, int classId, out int result1, out int result2)
        {
            int classNum = 0;
            result1 = 0;
            result2 = 0;
            ObjectParameter memberParameter = new ObjectParameter("Member", memberId);
            ObjectParameter classParameter = new ObjectParameter("Class", classId);
            ObjectParameter classNumParameter = new ObjectParameter("ClassNum", classNum);
            ObjectParameter result1Parameter = new ObjectParameter("Result1", result1);
            ObjectParameter result2Parameter = new ObjectParameter("Result2", result2);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClassNum_Load_p", memberParameter, classParameter, classNumParameter, result1Parameter, result2Parameter);
            result1 = Convert.ToInt32(result1Parameter.Value);
            result2 = Convert.ToInt32(result2Parameter.Value);
            return Convert.ToInt32(classNumParameter.Value);
        }

    }
}
