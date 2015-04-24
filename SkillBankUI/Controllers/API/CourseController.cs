using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using SkillBank.Site.Web;
using SkillBank.Site.Common;
using SkillBank.Site.Services;
using SkillBank.Site.DataSource.Data;

namespace SkillBankWeb.API
{
    public class CourseController : ApiController
    {
        public readonly ICommonService _commonService;
        public readonly IContentService _contentService;

        public CourseController(ICommonService commonService, IContentService contentService)
        {
            _commonService = commonService;
            _contentService = contentService;
        }

        public class CourseInfo
        {
            //public int ClassId { get; set; }
            public Byte Category { get; set; }
            public byte Level { get; set; }
            public byte Skill { get; set; }
            public byte Teach { get; set; }
            
            public String Title { get; set; }
            public String Summary { get; set; }
            public string Period { get; set; }
            public string Location { get; set; }
            public string Available { get; set; }
            public string Remark { get; set; }
            public string WhyU { get; set; }
            public int CityId { get; set; }
            public string Cover { get; set; }
            public Boolean IsPublish { get; set; }
        }

        // GET api/course/5
        //public CourseInfo Get(int id)
        //{
        //     return new CourseInfo();
        //}

        // POST api/course
        public int Post(CourseInfo courseInfo)
        {
            int memberId = APIHelper.GetMemberId(true);

            //save member city if changed
            if (courseInfo.CityId>0)
            {
                _commonService.UpdateMemberInfo(memberId, (Byte)Enums.DBAccess.MemberSaveType.UpdateCity, courseInfo.CityId.ToString());
            }
            
            Byte saveType = (Byte)Enums.DBAccess.ClassSaveType.CreateNew;//update
                        
            ClassInfo classInfo = new ClassInfo()
            {
                ClassId = 0,//new class
                Member_Id = memberId,
                Level = courseInfo.Level,
                SkillLevel = courseInfo.Skill,
                TeacheLevel = courseInfo.Teach,
                Category_Id = courseInfo.Category,
                Title = String.IsNullOrEmpty(courseInfo.Title) ? "" : courseInfo.Title,
                Summary = String.IsNullOrEmpty(courseInfo.Summary) ? "" : courseInfo.Summary,
                WhyU = String.IsNullOrEmpty(courseInfo.WhyU) ? "" : courseInfo.WhyU,
                Available = String.IsNullOrEmpty(courseInfo.Available) ? "" : courseInfo.Available,
                Remark = String.IsNullOrEmpty(courseInfo.Remark) ? "" : courseInfo.Remark,
                Location = String.IsNullOrEmpty(courseInfo.Location) ? "" : courseInfo.Location,
                Period = String.IsNullOrEmpty(courseInfo.Period) ? "" : courseInfo.Period
            };

            var result = _commonService.UpdateClassEditInfo(saveType, classInfo);
            return 0;
        }

        // PUT api/course/5
        public int Put(int id, CourseInfo courseInfo)
        {
            int classId = id;
            int memberId = APIHelper.GetMemberId(true);
            //save member city if changed
            if (courseInfo.CityId > 0)
            {
                _commonService.UpdateMemberInfo(memberId, (Byte)Enums.DBAccess.MemberSaveType.UpdateCity, courseInfo.CityId.ToString());
            }

            var result = 0;
            Byte saveType = 0;
            //publish class
            if (!String.IsNullOrEmpty(courseInfo.Summary))//update class
            {
                saveType = (Byte)Enums.DBAccess.ClassSaveType.UpdateStep2;//class info
            }
            else if (courseInfo.Category > 0)//update class
            {
                saveType = (Byte)Enums.DBAccess.ClassSaveType.UpdateStep1;//class basic info
            }
            else if (courseInfo.IsPublish)//update class
            {
                saveType = (Byte)Enums.DBAccess.ClassSaveType.UpdateStep3;//class basic info
            }
            else if (!String.IsNullOrEmpty(courseInfo.Cover))
            {
                _commonService.UpdateClassEditInfo((Byte)Enums.DBAccess.ClassSaveType.UpdatePhoto, classId, courseInfo.Cover);
                return 1;
            }
            
            if (saveType > 0)
            {
                ClassInfo classInfo = new ClassInfo()
                {
                    ClassId = classId,
                    Member_Id = memberId,
                    Level = courseInfo.Level,
                    SkillLevel = courseInfo.Skill,
                    TeacheLevel = courseInfo.Teach,
                    Category_Id = courseInfo.Category,
                    Title = String.IsNullOrEmpty(courseInfo.Title) ? "" : courseInfo.Title,
                    Summary = String.IsNullOrEmpty(courseInfo.Summary) ? "" : courseInfo.Summary,
                    WhyU = String.IsNullOrEmpty(courseInfo.WhyU) ? "" : courseInfo.WhyU,
                    Available = String.IsNullOrEmpty(courseInfo.Available) ? "" : courseInfo.Available,
                    Remark = String.IsNullOrEmpty(courseInfo.Remark) ? "" : courseInfo.Remark,
                    Location = String.IsNullOrEmpty(courseInfo.Location) ? "" : courseInfo.Location,
                    Period = String.IsNullOrEmpty(courseInfo.Period) ? "" : courseInfo.Period,
                    Cover = String.IsNullOrEmpty(courseInfo.Cover) ? "" : courseInfo.Cover
                };

                result = _commonService.UpdateClassEditInfo(saveType, classInfo);
                return result;
            }
            return 0;
        }

        // DELETE api/course/5
        public Boolean Delete(int id)
        {
            return true;
        }

    }
}
