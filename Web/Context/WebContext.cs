using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.Web.ContentConfiguration;
using SkillBank.Site.Common;

namespace SkillBank.Site.Web.Context
{
    public class WebContext : WebContextBase<WebContext>, IMemberTypeContext, IServerContext
    {
        #region Constructors

        internal WebContext(WebContextBuilder builder)
            : base(builder.MarketCode, builder.LanguageCode)
        {
            // Set custom properties
            ServerName = builder.ServerName;
            MemberType = builder.MemberTypeCode;
            SocialAccount = builder.SocialAccount;
            SocialType = builder.SocialType;
            Etag = builder.Etag;
            ShowLL = builder.ShowLL;
            OrderHandleDate = builder.OrderHandleDate;
            SocialAccessInfo = builder.SocialAccessInfo;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public Enums.MemberType MemberType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Byte SocialType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public String SocialAccount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string Etag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string ShowLL
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string OrderHandleDate
        {
            get;
            set;
        }

        public int MemberId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public String SocialAccessInfo
        {
            get;
            set;
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return LanguageCode + ":" + MarketCode + ":" + MemberType + ":" + SocialType + ":" + ServerName + ":" + MemberId + ":" + ShowLL;
        }

        #endregion
    }
}