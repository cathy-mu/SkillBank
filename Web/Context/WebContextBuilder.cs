using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.Common;

namespace SkillBank.Site.Web.Context
{
    public class WebContextBuilder:WebContextBuilderBase<WebContext>
    {
        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public Enums.MemberType MemberTypeCode
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
        ///   Get Context
        /// </summary>
        /// <returns></returns>
        public override WebContext GetContext()
        {
            var context = new WebContext(this);
            
            context.MemberId = this.MemberId;
            context.MemberType = this.MemberTypeCode;
            
            return context;
        }

        /// <summary>
        /// Reads the values from an existing context.
        /// </summary>
        /// <param name="context"></param>
        public void ReadContext(WebContext context)
        {
            base.ReadContext(context);
            this.MemberTypeCode = context.MemberType;
            this.SocialAccount = context.SocialAccount;
            this.SocialType = context.SocialType;
            this.ServerName = context.ServerName;
            this.Etag = context.Etag;
            this.ShowLL = context.ShowLL;
            this.OrderHandleDate = context.OrderHandleDate;
            this.MemberId = context.MemberId;
            this.SocialAccessInfo = context.SocialAccessInfo;
        }

        #endregion
    }
}
