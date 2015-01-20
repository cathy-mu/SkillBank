using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Mapper
{
    public class ClassMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objClasses"></param>
        /// <returns></returns>
        public static List<ClassInfo> Map(ObjectResult<ClassInfo> objClasses)
        {
            if (objClasses != null)
            {
                var classes = objClasses.ToList<ClassInfo>();
                return (classes.Count > 0) ? classes : null;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objItems"></param>
        /// <returns></returns>
        public static List<ClassItem> Map(ObjectResult<ClassItem> objItems)
        {
            if (objItems != null)
            {
                var items = objItems.Select(item => new ClassItem()
                {
                    ClassId = item.ClassId,
                    Category_Id = item.Category_Id,
                    Cover = item.Cover,
                    Member_Id = item.Member_Id,
                    Name = item.Name,
                    CityId = item.CityId,
                    PosX = item.PosX,
                    PosY = item.PosY,
                    Rank = item.Rank,
                    Title = item.Title,
                    Level = item.Level,
                    LastUpdateDate = item.LastUpdateDate,
                    CompleteStatus = item.CompleteStatus
                 }).ToList<ClassItem>();
                return (items.Count > 0) ? items : null;
            }
            
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objTags"></param>
        /// <returns></returns>
        public static List<String> Map(ObjectResult<String> objTags)
        {
            if (objTags != null)
            {
                var tags = objTags.ToList<String>();
                return (tags.Count > 0) ? tags : null;
            }
            return null;
        }

        //public static List<ClassTag> Map(ObjectResult<ClassTag> objTags)
        //{
        //    if (objTags != null)
        //    {
        //        var tags = objTags.ToList<ClassTag>();
        //        return (tags.Count > 0) ? tags : null;
        //    }
        //    return null;
        //}


        //SQL
        //public static List<ClassItem> Map(ObjectResult<ClassInfo_LoadAll_p_Result> objItems)
        //{
        //    if (objItems != null)
        //    {
        //        var items = objItems.Select(item => new ClassItem()
        //        {
        //            ClassId = item.ClassId,
        //            Category_Id = item.Category_Id,
        //            Cover = item.Cover,
        //            Member_Id = item.Member_Id,
        //            Name = item.Name,
        //            CityId = item.CityId,
        //            PosX = item.PosX,
        //            PosY = item.PosY,
        //            Rank = item.Rank,
        //            Title = item.Title,
        //            Level = item.Level,
        //            LastUpdateDate = item.LastUpdateDate,
        //            CompleteStatus = item.CompleteStatus.HasValue?item.CompleteStatus.Value:(byte)0
        //        }).ToList<ClassItem>();
        //        return (items.Count > 0) ? items : null;
        //    }
            
        //    return null;
        //}

        //public static List<ClassInfo> Map(ObjectResult<ClassInfo_Load_p_Result> objClasses)
        //{
        //    if (objClasses != null)
        //    {
        //        var classes = objClasses.Select(item => new ClassInfo() { Category_Id = item.Category_Id, ClassId = item.Category_Id, IsActive = item.IsActive, TeacheLevel = item.TeacheLevel, CompleteStatus = item.CompleteStatus, Cover = item.Cover, CreatedDate = item.CreatedDate, Description = item.Description, Detail = item.Detail, IsProved = item.IsProved, LastUpdateDate = item.LastUpdateDate, Level = item.Level, Member_Id = item.Member_Id, Rank = item.Rank, SkillLevel = item.SkillLevel, Summary= item.Summary, Title = item.Title}).ToList<ClassInfo>();
        //        return (classes.Count > 0) ? classes : null;
        //    }
        //    return null;
        //}

    }
}
