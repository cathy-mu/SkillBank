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
using SkillBank.Site.Services.Utility;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.API
{
    public class PortalController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public class PortalModel
        {
            public List<PortalBanner> PortalBanner;
            public Int16 StatusCode;
        }

        public PortalController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PortalModel Get()
        {
            
            PortalModel model = new PortalModel();
            model.PortalBanner = _contentService.GetPortalBannerLkp();
            if (model.PortalBanner == null)
            {
                model.StatusCode = 404;
            }
            else
            {
                model.StatusCode = 200;
            }
            return model;
        }

    }
}
