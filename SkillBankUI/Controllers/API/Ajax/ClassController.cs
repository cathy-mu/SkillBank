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
using SkillBank.Site.Services.Utility;
 

namespace SkillBankWeb.API
{
    public class ClassController : ApiController
    {
        public readonly ICommonService _commonService;
        public readonly IContentService _contentService;

        public class ClassItem
        {
            public int MemberId { get; set; }
            public int ClassId { get; set; }
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
            public string Cover { get; set; }
            public Boolean HasOnline { get; set; }
            public string Tags { get; set; }

            public int CityId { get; set; }
            public string CityName { get; set; }
            public Boolean IsPublish { get; set; }//是否已发布正审核
            public Boolean IsLike { get; set; }
            public Int16 Status { get; set; }

            public Byte Category { get; set; }
        }

        public class ClassAddResult
        {
            public int ClassId { get; set; }
            public Int16 Status { get; set; }
        }

        public class ClassEditResult
        {
            //public int ClassId { get; set; }
            public Int16 Status { get; set; }
        }

        public ClassController(ICommonService commonService, IContentService contentService)
        {
            _commonService = commonService;
            _contentService = contentService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }
                

        // GET api/course/5
        /// <summary>
        /// If edit class mode ,should pass the member id as paramenter;else is using viewmode
        /// </summary>
        /// <param name="id">Class id</param>
        /// <param name="mid">Class Owner id</param>
        /// <returns></returns>
        [HttpGet]
        public ClassItem Get(int id, int mid = -1)
        {
            //Edit mode
            Byte loadType = (Byte)Enums.DBAccess.ClassLoadType.ByClassId;
            var item = _commonService.GetClassInfoItem(loadType, id, 0);//class exists and is owner of the class
            //class exists and is owner of the class
            if (item != null)//class owner
            {
                ClassItem course = new ClassItem()
                {
                    MemberId = item.Member_Id,
                    ClassId = item.ClassId,

                    Category = item.Category_Id,
                    Level = item.Level,
                    Skill = item.SkillLevel,
                    Teach = item.TeacheLevel,

                    Title = item.Title,
                    Summary = item.Summary,
                    Period = item.Period,
                    Location = item.Location,
                    Available = item.Available,
                    Remark = item.Remark,
                    WhyU = item.WhyU,
                    Cover = item.Cover,
                    HasOnline = item.HasOnline,
                    Tags = item.Tags,

                    IsPublish = item.IsProved.Equals(3),
                    IsLike = false,
                    Status = 200

                };

                //set class owners cityname
                var member = _commonService.GetMemberInfo(course.MemberId);
                if (member.CityId > 0)
                {
                    String cityName = _contentService.GetCityNameById(Constants.BizConfig.SingleLocalCode, member.CityId);
                    course.CityName = String.IsNullOrEmpty(cityName) ? "" : cityName;
                    course.CityId = member.CityId;
                }
                return course;
            }
            else
            {
                return new ClassItem { Status = 404 };
            }
        }


        // POST api/course
        //Insert new course datas
        [HttpPost]
        public ClassAddResult Post(ClassItem courseInfo)
        {
            int memberId = courseInfo.MemberId > 0 ? courseInfo.MemberId : APIHelper.GetMemberId(true);

            //save member city if changed
            if (!String.IsNullOrEmpty(courseInfo.CityName) && courseInfo.CityId.Equals(0))
            {
                var cityDic = _contentService.GetCities("cn");
                courseInfo.CityId = LookupHelper.GetCityIdByName(cityDic, courseInfo.CityName);
            }
            if (courseInfo.CityId > 0)
            {
                _commonService.UpdateMemberInfo(memberId, (Byte)Enums.DBAccess.MemberSaveType.UpdateCity, courseInfo.CityId.ToString());
            }

            if (courseInfo.Level > 0 && courseInfo.Level < 4 && courseInfo.Skill > 0 && courseInfo.Skill < 101 && courseInfo.Teach > 0 && courseInfo.Teach < 101)
            {
                Byte saveType = (Byte)Enums.DBAccess.ClassSaveType.CreateNew;

                ClassInfo classInfo = new ClassInfo()
                {
                    ClassId = 0,//new class
                    Member_Id = memberId,
                    Level = courseInfo.Level,
                    SkillLevel = courseInfo.Skill,
                    TeacheLevel = courseInfo.Teach,
                    Category_Id = courseInfo.Category,
                    Title = String.IsNullOrEmpty(courseInfo.Title) ? "" : courseInfo.Title,
                    Summary = "",
                    WhyU = "",
                    Available = "",
                    Remark = "",
                    Location = "",
                    Period = "",
                    Cover = "",
                    Tags = "",
                    HasOnline = false
                };

                var result = _commonService.UpdateClassEditInfo(saveType, classInfo);
                return new ClassAddResult() { ClassId = result, Status = 200 };
            }

            return new ClassAddResult() { ClassId = 0, Status = 400 };
        }

        // PUT api/course/5
        [HttpPut]
        public ClassEditResult Put(int id, ClassItem courseInfo)
        {
            int classId = id;
            int memberId = memberId = courseInfo.MemberId > 0 ? courseInfo.MemberId : APIHelper.GetMemberId(false);
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
            else if (courseInfo.Level > 0 && courseInfo.Level < 4 && courseInfo.Skill > 0 && courseInfo.Skill < 101 && courseInfo.Teach > 0 && courseInfo.Teach < 101)
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
            }

            if (saveType > 0)
            {
                ClassInfo classInfo = new ClassInfo()
                {
                    //TO DO:Check if memberid is useless
                    Member_Id = memberId,

                    ClassId = classId,
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
                    Cover = String.IsNullOrEmpty(courseInfo.Cover) ? "" : courseInfo.Cover,
                    Tags = String.IsNullOrEmpty(courseInfo.Tags) ? "" : courseInfo.Tags,
                    HasOnline = courseInfo.HasOnline
                };

                result = _commonService.UpdateClassEditInfo(saveType, classInfo);
                if (result.Equals(0))
                {
                    return new ClassEditResult() { Status = 404 };
                }
                return new ClassEditResult() { Status = 200 };
            }

            return new ClassEditResult() { Status = 400 };
        }

        

    }
}
