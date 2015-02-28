using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using dotNetDR_OAuth2;
using dotNetDR_OAuth2.AccessToken;
using dotNetDR_OAuth2.APIs;
using dotNetDR_OAuth2.Net;
using dotNetDR_OAuth2.JSON;
using System.Net;

using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Common;
using SkillBank.Site.Services;

namespace SkillBank.Site.Web.Social
{
    public class SocialHelper
    {
        private IAuthorizationCodeBase _authCode = Uf.C(CtorAT.Tencent);
        private TencentError _err = null;
        private IApi apit = Uf.C(CtorApi.Tencent);

        private IAuthorizationCodeBase _authCodes = Uf.C(CtorAT.Sina);
        private SinaError _errs = null;
        private IApi apis = Uf.C(CtorApi.Sina);

        public void Test(dynamic accessToken)
        {
            int q = 1;
        }
    }
}
