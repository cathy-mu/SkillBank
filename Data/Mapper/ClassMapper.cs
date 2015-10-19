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

        public static List<ClassInfo> Map(ObjectResult<ClassInfo_Load_p_Result> objClasses)
        {
            if (objClasses != null)
            {
                var classes = objClasses.Select(item => new ClassInfo()
                {
                    Category_Id = item.Category_Id,
                    ClassId = item.ClassId,
                    IsActive = item.IsActive,
                    TeacheLevel = item.TeacheLevel,
                    Cover = item.Cover,
                    CreatedDate = item.CreatedDate,
                    IsProved = item.IsProved,
                    LastUpdateDate = item.LastUpdateDate,
                    Level = item.Level,
                    Member_Id = item.Member_Id,
                    Rank = item.Rank,
                    SkillLevel = item.SkillLevel,
                    Summary = item.Summary,
                    Title = item.Title,
                    Detail = item.Detail,
                    HasOffline = item.HasOffline,
                    Tags = item.Tags
                }).ToList<ClassInfo>();
                return (classes.Count > 0) ? classes : null;
            }
            return null;
        }

        public static List<ClassEditItem> Map(ObjectResult<ClassEditInfo_Load_p_Result> objClasses)
        {
            if (objClasses != null)
            {
                var classes = objClasses.Select(item => new ClassEditItem()
                {
                    ClassId = item.ClassId,
                    Category_Id = item.Category_Id,
                    TeacheLevel = item.TeacheLevel,
                    PublishStatus = item.PublishStatus,
                    Cover = item.Cover,
                    IsProved = item.IsProved,
                    Level = item.Level,
                    Member_Id = item.Member_Id,
                    SkillLevel = item.SkillLevel,
                    Summary = item.Summary,
                    Title = item.Title,
                    Period = item.Period,
                    Available = item.Available,
                    Location = item.Location,
                    Remark = item.Remark,
                    WhyU = item.WhyU,
                    IsLike = item.IsLike,
                    LikeNum = item.LikeNum,
                    HasOffline = item.HasOffline,
                    Tags = item.Tags
                }).ToList<ClassEditItem>();
                return (classes.Count > 0) ? classes : null;
            }
            return null;
        }
        
        public static List<ClassListItem> Map(ObjectResult<ClassInfo_LoadByList_p_Result> objClasses)
        {
            if (objClasses != null)
            {
                var classes = objClasses.Select(item => new ClassListItem() { ClassId = item.ClassId, Cover = item.Cover, Level = item.Level, Member_Id = item.Member_Id, Title = item.Title, Avatar = item.Avatar, Name = item.Name, CityId = item.CityId, PosX = item.PosX, PosY = item.PosY, ReviewNum = item.ReviewNum, LikeNum = item.LikeNum, IsLike = item.IsLike, ClassNum = item.ClassNum }).ToList<ClassListItem>();
                return (classes.Count > 0) ? classes : null;
            }
            return null;
        }

        public static List<ClassListItem> Map(ObjectResult<ClassInfo_LoadByTab_p_Result> objClasses)
        {
            if (objClasses != null)
            {
                var classes = objClasses.Select(item => new ClassListItem() { ClassId = item.ClassId, Cover = item.Cover, Level = item.Level, Member_Id = item.Member_Id, Title = item.Title, Avatar = item.Avatar, Name = item.Name, CityId = item.CityId, PosX = item.PosX, PosY = item.PosY, ReviewNum = item.ReviewNum, LikeNum = item.LikeNum, IsLike = item.IsLike, ClassNum = 0 }).ToList<ClassListItem>();
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


        public static List<ClassItem> Map(ObjectResult<ClassInfo_LoadAll_p_Result> objItems)
        {
            var items = objItems.Select(item => new ClassItem()
            {
                ClassId = item.ClassId,
                Category_Id = item.Category_Id,
                Cover = item.Cover,
                Member_Id = item.Member_Id,
                Name = item.Name,
                CityId = item.CityId,
                Avatar = item.Avatar,
                PosX = item.PosX,
                PosY = item.PosY,
                Rank = item.Rank,
                Title = item.Title,
                Level = item.Level,
                LastUpdateDate = item.LastUpdateDate,
                CompleteStatus = item.CompleteStatus.HasValue ? item.CompleteStatus.Value : (Byte)0
            }).ToList<ClassItem>();
            return (items.Count > 0) ? items : null;
        }
         
        ////public static List<ClassCollectionItem> Map(ObjectResult<ClassCollection_Load_p_Result> objItems)
        ////{
        ////    var items = objItems.Select(item => new ClassCollectionItem()
        ////    {
        ////        ClassId = item.ClassId,
        ////        Title = item.Title,
        ////        Cover = item.Cover,
        ////        MemberId = item.MemberId,
        ////        Name = item.Name,
        ////        CityId = item.CityId
        ////    }).ToList<ClassCollectionItem>();
        ////    return (items.Count > 0) ? items : null;
        ////}
        

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
           

    }
}
