using System;
using System.Collections.Generic;
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
    public class OrderHandlerController : ApiController//Validation
    {
        public readonly ICommonService _commonService;

        public class OrderHandleResult
        {
            public Int16 Status { get; set; }
        }

        //
        // GET: /Message/

        public OrderHandlerController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }

        /// <summary>
        /// To hadler user's order by member id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public OrderHandleResult Get(int id)
        {
            OrderHandleResult result = new OrderHandleResult();

            try
            {
                _commonService.HandleMemberOrder(id);
                result.Status = 200;
            }
            catch
            {
                result.Status = 500;
            }
            return result;
        }
    }
}
