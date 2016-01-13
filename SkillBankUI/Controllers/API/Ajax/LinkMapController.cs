using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.API
{
    public class LinkMapController : ApiController
    {
        public readonly IContentService _contentService;

        public class LikeMapItem
        {
            public List<LinkMap> LinkList;
            public Int16 StatusCode;
        }
        //
        // GET: /Message/

        public LinkMapController(IContentService contentService)
        {
            _contentService = contentService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }

        [HttpGet]
        public LikeMapItem Get(int id = 0)
        {
            LikeMapItem item = new LikeMapItem();
            item.LinkList = _contentService.GetLinkMapLkp(id);

            if (item.LinkList == null)
            {
                item.StatusCode = 404;
            }
            else
            {
                item.StatusCode = 200;
            }
            return item;
        }

        


    }
}
