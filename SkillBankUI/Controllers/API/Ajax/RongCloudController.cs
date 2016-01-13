using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;

namespace SkillBankWeb.API
{
    public class RongCloudController : ApiController
    {
        public readonly ICommonService _commonService;

        //public class RongCloud
        //{
        //    public int MemberId { get; set; }
        //    public Byte SocailType { get; set; }
        //    public String SocailAccount { get; set; }
        //    public String OpenId { get; set; }
        //    public String Mobile { get; set; }
        //    public String Pass { get; set; }
        //}

        public RongCloudController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">page owner id</param>
        /// <param name="viewerId">current viewer member id</param>
        /// <returns></returns>
       
        public String Get()
        {
           return null;
        }

     


    }
}
