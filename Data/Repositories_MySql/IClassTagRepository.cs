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
    public interface IClassTagRepository
    {
        int AddClassTags(int classId, string classTags);
        int UpdateClassTags(int classId, string classTags);
        List<String> GetClassTags(int classId);
    }

    public class ClassTagRepository : DbContext, IClassTagRepository
    {
        public ClassTagRepository()
            : base("name=Entities")
        {
        }

        public int AddClassTags(int classId, string classTags)
        {
            Char delimiter = Constants.Setting.DBDataDelimiter;
            SByte saveType = (SByte)Enums.DBAccess.ClassTagSaveType.AddNew;
            return ClassTag_Save_p(saveType, classId, classTags, delimiter);
        }

        public int UpdateClassTags(int classId, string classTags)
        {
            Char delimiter = Constants.Setting.DBDataDelimiter;
            SByte saveType = (SByte)Enums.DBAccess.ClassTagSaveType.UpdateTags;
            return ClassTag_Save_p(saveType, classId, classTags, delimiter);
        }

        public List<String> GetClassTags(int classId)
        {
            SByte loadType = (SByte)Enums.DBAccess.ClassTagLoadType.ByClassId;
            var result = ClassTag_Load_p(loadType, classId);
            return ClassMapper.Map(result);
        }
                      
        private ObjectResult<String> ClassTag_Load_p(SByte loadType, int paraId)
        {
            var loadByParameter = new ObjectParameter("LoadBy", loadType);
            var paraIdParameter = new ObjectParameter("ParaId", paraId);

            ObjectResult<String> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<String>("ClassTag_Load_p", loadByParameter, paraIdParameter);
            return result;
        }

        private int ClassTag_Save_p(SByte saveType, int classId, string tagText,char delimiter)
        {
            ObjectParameter saveTypeParameter = new ObjectParameter("SaveType", saveType);
            ObjectParameter classIdParameter = new ObjectParameter("ClassId", classId);
            ObjectParameter tagsTextParameter = new ObjectParameter("TagsText", tagText);
            ObjectParameter tagsDelimiterParameter = new ObjectParameter("TagsDelimiter", delimiter);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClassTag_Save_p", saveTypeParameter,classIdParameter, tagsTextParameter, tagsDelimiterParameter);
            return (int)classIdParameter.Value;
        }
    }
}

