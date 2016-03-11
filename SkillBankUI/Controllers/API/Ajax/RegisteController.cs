using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

using SkillBank.Site.Services.Utility;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;
using System.Web.Security;


namespace SkillBankWeb.Controllers.API
{
    public class RegisteController  : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;
        
        public class RegisterInfo
        {
            //check social infos for mobile register
            public String Account { get; set; }
            public String Pass { get; set; }
            public String Etag { get; set; }
            public Byte Type { get; set; }
            public String DeviceToken { get; set; }
            public String UnionId { get; set; }
        
            public String Name { get; set; }
            public String Avatar { get; set; }
            public String Mobile { get; set; }
            public String Code { get; set; }
            public Boolean Gender { get; set; }
        }

        public class RegisteResult
        {
            public int MemberId;
            public int Result;
            public Boolean IsMobileVerified;
            public String AccessToken;
            public String RCToken;
            public String Avatar;
            public String Name;
            public Boolean Gender;
            public String CityName;
            public Byte SocialType;
            public String Mobile;
        }
        
        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }
        //
        // GET: /Message/

        public RegisteController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>0 用户手机已经存在 1成功 2手机验证不通过 3手机已被验证 4数据格式错误</returns>
        [HttpPost]
        public RegisteResult Post(RegisterInfo item)
        {
            RegisteResult result = new RegisteResult();
            int memberId = 0;
            Byte verifyTag = 0;
            String accessToken = "";
            String rcToken = "";
            Boolean isValidData = false;
                                   
            //old registe process
            if (item.Type.Equals(0))
            {
                result.Result = 4;
                Boolean isMobileValid = System.Text.RegularExpressions.Regex.IsMatch(item.Mobile, Constants.ValidationExpressions.Mobile);
                Boolean isCodeValid = System.Text.RegularExpressions.Regex.IsMatch(item.Code, Constants.ValidationExpressions.ValidationCode);

                item.Etag = (WebContext.Current.Etag == null) ? "" : WebContext.Current.Etag;
                item.Account = WebContext.Current.SocialAccount;
                item.Type = WebContext.Current.SocialType;

                if (item.Type > 0 && !String.IsNullOrEmpty(item.Account) && !String.IsNullOrEmpty(item.Name) && isMobileValid && isCodeValid)
                {
                    item.Pass = String.IsNullOrEmpty(item.Pass) ? "" : FormsAuthentication.HashPasswordForStoringInConfigFile(item.Pass, "SHA1");
                    item.Etag = String.IsNullOrEmpty(item.Etag) ? "" : item.Etag;
                    item.Name = String.IsNullOrEmpty(item.Name) ? Constants.DefaultSetting.UserName : item.Name;
                    item.Avatar = String.IsNullOrEmpty(item.Avatar) ? String.Concat(Request.RequestUri.Host, Constants.DefaultSetting.AvaterImg) : item.Avatar;
                    item.UnionId = String.IsNullOrEmpty(item.UnionId) ? "" : item.UnionId;
                    item.DeviceToken = String.IsNullOrEmpty(item.DeviceToken) ? "" : item.DeviceToken;

                    result.Result = _commonService.CreateMember(out memberId, ref verifyTag, ref accessToken, ref rcToken, item.Account, item.Type, item.Name, "", item.Avatar, item.Mobile, item.Code, "", item.Etag, item.Gender, item.DeviceToken, item.UnionId);

                    if (memberId > 0)
                    {
                        result.MemberId = memberId;
                        result.AccessToken = accessToken;
                        isValidData = true;

                        WebContext.Current.MemberId = memberId;
                        //Only for web register
                        String message = ResourceHelper.GetTransText(284);
                        _commonService.AddMessage(Constants.BizConfig.AdminMemberId, memberId, message);
                    }
                }
            }
            //new registe process for APP
            else
            {
                //valid datas
                if (IsRegisteDataValid(ref item))
                {
                    result.Result = _commonService.CreateMember(out memberId, ref verifyTag, ref accessToken, ref rcToken, item.Account, item.Type, item.Name, "", item.Avatar, item.Mobile, item.Code, item.Pass, item.Etag, item.Gender, item.DeviceToken, item.UnionId);
                    //registe or login success
                    if (memberId > 0)
                    {

                        WebContext.Current.MemberId = memberId;
                        WebContext.Current.SocialType = item.Type;
                        WebContext.Current.SocialAccount = item.Account;

                        if (item.Type.Equals((Byte)Enums.SocialTpye.Mobile) && result.Result.Equals(0))
                        {
                            result.MemberId = 0;
                        }
                        else
                        {
                            result.MemberId = memberId;
                            result.Result = 1;
                        }
                        result.AccessToken = accessToken;
                        isValidData = true;
                    }
                }
                else
                {
                    result.Result = 4;
                }
                       

                if (!isValidData || result.MemberId.Equals(0))
                {
                    CleanMemberContext();
                }
            }
                        
            //Get RongCloud Token
            if (!memberId.Equals(0) && String.IsNullOrEmpty(rcToken))
            {
                rcToken = RongCloudHelper.GetToken(System.Configuration.ConfigurationManager.AppSettings["ENV"], result.MemberId, item.Name, item.Avatar);
                if (!String.IsNullOrEmpty(rcToken))
                {
                    result.RCToken = rcToken;
                    Byte type = (Byte)Enums.DBAccess.MemberSaveType.UpdateRCTokenADeviceToken;
                    MemberInfo memberInfo = new MemberInfo() { MemberId = memberId, Avatar = rcToken };
                    var updateResult = _commonService.UpdateMemberProfile(memberInfo, type);
                }
            }

            if (result.MemberId > 0)
            {
                result.IsMobileVerified = (verifyTag & 1) == 1;
                
                
                var cityDic = _contentService.GetCities("cn");
                var info = _commonService.GetMemberInfo(result.MemberId);
                if (info != null)
                {
                    result.Avatar = info.Avatar;
                    result.CityName = cityDic.ContainsKey(info.CityId) ? cityDic[info.CityId].CityName : "";
                    result.Name = info.Name;
                    result.SocialType = info.SocialType;
                    result.Gender = info.Gender;
                    result.RCToken = info.RCToken;
                    result.Mobile = info.Phone;
                }
            }
            return result;
        }

        private Boolean IsRegisteDataValid(ref RegisterInfo item)
        {
            //mobile
            if (item.Type.Equals((Byte)Enums.SocialTpye.Mobile))
            {
                return IsMobileRegisteDataValid(ref item);
            }
            //TO DO:remove valid mobile later after APP register
            else if (item.Type.Equals((Byte)Enums.SocialTpye.Sina) || item.Type.Equals((Byte)Enums.SocialTpye.WeChat) || item.Type.Equals((Byte)Enums.SocialTpye.APPWeChat))
            {
                return IsSocailRegisteDataValid(ref item);
            }
            return false;
        }

        private Boolean IsMobileRegisteDataValid(ref RegisterInfo item)
        {
            if (!String.IsNullOrEmpty(item.Mobile) && !String.IsNullOrEmpty(item.Code) && !String.IsNullOrEmpty(item.Pass))
            {
                Boolean isMobileValid = System.Text.RegularExpressions.Regex.IsMatch(item.Mobile, Constants.ValidationExpressions.Mobile);
                Boolean isCodeValid = System.Text.RegularExpressions.Regex.IsMatch(item.Code, Constants.ValidationExpressions.ValidationCode);
                Boolean isPassValid = System.Text.RegularExpressions.Regex.IsMatch(item.Pass, Constants.ValidationExpressions.Password);
                //valid data for mobile register
                if (isMobileValid && isCodeValid && isPassValid)
                {
                    item.Account = String.IsNullOrEmpty(item.Account) ? "" : item.Account;
                    item.Gender = true;
                    item.Pass = String.IsNullOrEmpty(item.Pass) ? "" : FormsAuthentication.HashPasswordForStoringInConfigFile(item.Pass, "SHA1");
                    item.Etag = String.IsNullOrEmpty(item.Etag) ? "" : item.Etag;
                    item.Name = String.IsNullOrEmpty(item.Name) ? Constants.DefaultSetting.UserName : item.Name;
                    item.Avatar = String.IsNullOrEmpty(item.Avatar) ? String.Concat(Request.RequestUri.Host, Constants.DefaultSetting.AvaterImg) : item.Avatar;
                    item.UnionId = String.IsNullOrEmpty(item.UnionId) ? "" : item.UnionId;
                    item.DeviceToken = String.IsNullOrEmpty(item.DeviceToken) ? "" : item.DeviceToken;
                    return true;
                }
            }
            return false;
        }

        private Boolean IsSocailRegisteDataValid(ref RegisterInfo item)
        {
            if (!String.IsNullOrEmpty(item.Account))
            {
                item.Mobile = String.IsNullOrEmpty(item.Mobile) ? "" : item.Mobile;
                item.Code = "";
                item.Pass = "";
                item.Etag = String.IsNullOrEmpty(item.Etag) ? "" : item.Etag;
                item.Name = String.IsNullOrEmpty(item.Name) ? Constants.DefaultSetting.UserName : item.Name;
                item.Avatar = String.IsNullOrEmpty(item.Avatar) ? String.Concat(Request.RequestUri.Host, Constants.DefaultSetting.AvaterImg) : item.Avatar;
                item.UnionId = String.IsNullOrEmpty(item.UnionId) ? "" : item.UnionId;
                item.DeviceToken = String.IsNullOrEmpty(item.DeviceToken) ? "" : item.DeviceToken;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Logout out user and clean cookies
        /// </summary>
        private void CleanMemberContext()
        {
            MemberInfo item = new MemberInfo() { MemberId = 0, SocialType = 0, SocialAccount = "", OpenId = "" };

            if (!WebContext.Current.MemberId.Equals(item.MemberId))
            {
                WebContext.Current.MemberId = item.MemberId;
            }

            if (!WebContext.Current.SocialAccount.Equals(item.SocialAccount))
            {
                WebContext.Current.SocialAccount = item.SocialAccount;
            }

            if (!WebContext.Current.SocialType.Equals(item.SocialType))
            {
                WebContext.Current.SocialType = item.SocialType;
            }

            if (!WebContext.Current.OpenId.Equals(item.OpenId))
            {
                WebContext.Current.OpenId = item.OpenId;
            }
        }


    }
}
