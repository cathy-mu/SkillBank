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
    public class CreditController : ApiController
    {
        public readonly ICommonService _commonService;
        
        public class CreditUpdateItem
        {
            public Byte Type { get; set; }
            public int ParaValue { get; set; }
            public int? MemberId { get; set; }
        }

        public class CreditItem
        {
            public int Credit { get; set; }
            public int Coins { get; set; }
            public int CoinsForExchange { get; set; }
            public Boolean ShouldTeacherReview { get; set; }
            public Boolean ShouldStudentReview { get; set; }
            public Boolean IsSignIn { get; set; }
        }

        //
        // GET: /Message/

        public CreditController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Member Id default 0 for webpage</param>
        /// <returns></returns>
        [HttpGet]
        public CreditItem Get(int id = 0)
        {
            var memberInfo = id > 0 ? _commonService.GetMemberInfo(id) : null;

            if (memberInfo != null)
            {
                var numDic = _commonService.GetNumsByMember(id, (Byte)Enums.DBAccess.MemberNumsLoadType.ByCreditGetMethods);
                Boolean shouldStudentReview = (numDic[Enums.NumberDictionaryKey.MissStudentReview] > 0);
                Boolean shouldTeacherReview = (numDic[Enums.NumberDictionaryKey.MissTeacherReview] > 0);
                Boolean isSignIn = (numDic[Enums.NumberDictionaryKey.IsSignIn].Equals(0));
                int coins4Exchange = memberInfo.Credit / 30;
                CreditItem item = new CreditItem() { Credit = memberInfo.Credit, Coins = memberInfo.Coins, ShouldStudentReview = shouldStudentReview, ShouldTeacherReview = shouldTeacherReview, IsSignIn = isSignIn, CoinsForExchange = coins4Exchange };
                return item;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>0 not action     1 success    2 credit not enough/sign up already </returns>
        [HttpPost]
        public Byte Post(CreditUpdateItem item)
        {
            int memberId;
            if (item.MemberId.HasValue && !item.MemberId.Value.Equals(0))
            {
                memberId = item.MemberId.Value;
            }
            else
            {
                memberId = GetMemberId(true);
            }
            if (memberId > 0)
            {
                var result = _commonService.UpdateCredit(item.Type, memberId, item.ParaValue);
                return result;
            }
            return 0;
        }


        //for website ajax without memberid
        private int GetMemberId(Boolean shouldAuthorize)
        {

            int memberId = WebContext.Current.MemberId;
            if (shouldAuthorize && memberId == 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.StatusCode = 401;
                HttpContext.Current.Response.End();
            }
            return memberId;
        }

    }
}
