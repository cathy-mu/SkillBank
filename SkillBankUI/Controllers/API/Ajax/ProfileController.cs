using System;
using System.Collections.Generic;
using System.Web.Security; 
using System.Net;
using System.Net.Http;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Services.Utility;

namespace SkillBankWeb.API
{
    public class ProfileController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public class ProfileItem
        {
            public int Id;
            public String Name { get; set; }//Must have get and set
            public Boolean Gender { get; set; }
            public String CityName { get; set; }
            public String Intro { get; set; }
            public String Avatar { get; set; }

            public String Mobile { get; set; }
            public String Code { get; set; }
            public String Password { get; set; }

            public Decimal PosX { get; set; }
            public Decimal PosY { get; set; }
            public String Address { get; set; }

            //For socail binding
            public String SocailAccount { get; set; }
            public Byte Type { get; set; }
        }

        public class MemberProfileInfo
        {
            public int MemberId;
            public String Avatar;
            public String Name;
            public String Mobile;
            public Boolean Gender;
            public String Intro;
            public String CityName;
            public Byte SocialType;
            public Boolean IsMobileVerified;
            public String AccessToken;
            public String RCToken;
            public Int16 Result;

            //added for mobile API，not use yet
            //public int Coins { get; set; }
            //public int CoinsLocked { get; set; }
            //public int Credit { get; set; }
        }

        //public class UpdateResult
        //{
        //    public Int16 Result;
        //}


        public ProfileController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }

        /// <summary>
        /// FOR APP ONLY!!!!!!     clear user context
        /// </summary>
        /// <param name="id">page owner id</param>
        /// <param name="viewerId">current viewer member id</param>
        /// <returns></returns>

        [HttpGet]
        public MemberProfileInfo Get(int id = 0, Byte type = 0, String account = "", String para = "", String deviceToken = "")
        {
            MemberInfo memberInfo = null;
            Int16 statusCode = Constants.APIStatusCode.NotFound;
            MemberProfileInfo profileInfo;
            //Use member id get member info get own info
            if (type.Equals(0) && id > 0)
            {
                memberInfo = _commonService.GetMemberInfo(id);
            }
            //Get by mobile and password
            else if (!String.IsNullOrEmpty(account) && !String.IsNullOrEmpty(para) && type.Equals(2))
            {
                String password = String.IsNullOrEmpty(para) ? "" : FormsAuthentication.HashPasswordForStoringInConfigFile(para, "SHA1");
                memberInfo = _commonService.GetMemberInfo(account, 2, password);
                //use phone as password
                if (memberInfo != null)
                {
                    String userPass = memberInfo.Phone;
                    if (String.IsNullOrEmpty(userPass) || memberInfo.VerifyTag.Equals(0))
                    {
                        statusCode = Constants.APIStatusCode.InvalidMobileAccount;
                        memberInfo = null;
                    }
                    else if (!userPass.Equals(password))
                    {
                        statusCode = Constants.APIStatusCode.InvalidPass;
                        memberInfo = null;
                    }
                    else
                    {
                        memberInfo.Phone = account;
                    }
                }
            }
            //Social account
            else if (!String.IsNullOrEmpty(account) && type > 0)
            {
                memberInfo = _commonService.GetMemberInfo(account, type);
            }

            if (memberInfo != null)
            {
                UpdateUser3rdToken(ref memberInfo, deviceToken);

                var cityDic = _contentService.GetCities("cn");
                profileInfo = new MemberProfileInfo()
                {
                    MemberId = memberInfo.MemberId,
                    Name = memberInfo.Name,
                    Avatar = memberInfo.Avatar,
                    Gender = memberInfo.Gender,
                    Intro = memberInfo.SelfIntro,
                    Mobile = memberInfo.Phone,
                    CityName = cityDic.ContainsKey(memberInfo.CityId) ? cityDic[memberInfo.CityId].CityName : "",
                    SocialType = memberInfo.SocialType,
                    IsMobileVerified = (memberInfo.VerifyTag & 1).Equals(1),
                    Result = Constants.APIStatusCode.Success,
                    RCToken = memberInfo.RCToken,
                    //TO DO：Add token later
                    AccessToken = ""
                };

                ResetMemberContext(memberInfo);
            }
            //member not exists or disactive
            else
            {
                profileInfo = new MemberProfileInfo() { Result = statusCode };
                if (!type.Equals(0))
                {
                    ResetMemberContext(new MemberInfo() { MemberId = 0, SocialType = 0, SocialAccount = "", OpenId = "" });
                }
            }
            return profileInfo;
        }


        [HttpPut]
        public int Put(int id, ProfileItem item)
        {
            int result;

            //update position
            if (item.PosX > 0 && item.PosY > 0)
            {
                int cityId = 0;
                if (!String.IsNullOrEmpty(item.CityName))
                {
                    var cityDic = _contentService.GetCities("cn");
                    cityId = LookupHelper.GetCityIdByName(cityDic, item.CityName);
                }

                if (cityId.Equals(0))
                {
                    Byte type = (Byte)Enums.DBAccess.MemberSaveType.UpdatePosition;
                    MemberInfo memberInfo = new MemberInfo() { MemberId = id, PosX = item.PosX, PosY = item.PosY, Avatar = item.Address };
                    result = _commonService.UpdateMemberProfile(memberInfo, type);
                }
                else
                {
                    Byte type = (Byte)Enums.DBAccess.MemberSaveType.UpdatePosition;
                    MemberInfo memberInfo = new MemberInfo() { MemberId = id, PosX = item.PosX, PosY = item.PosY, Avatar = item.Address, CityId = cityId };
                    result = _commonService.UpdateMemberProfile(memberInfo, type);
                }
            }
            //update position
            else if (item.Type > 0 && !String.IsNullOrEmpty(item.SocailAccount))
            {
                Byte type = (Byte)Enums.DBAccess.MemberSaveType.RebindSicalAccount;
                MemberInfo memberInfo = new MemberInfo() { MemberId = id, Avatar = item.SocailAccount, CityId = (int)item.Type };
                result = _commonService.UpdateMemberProfile(memberInfo, type);
            }
            //update/reset password
            else if (!String.IsNullOrEmpty(item.Mobile) && !String.IsNullOrEmpty(item.Password) && !String.IsNullOrEmpty(item.Code))
            {
                Boolean isMobileValid = System.Text.RegularExpressions.Regex.IsMatch(item.Mobile, Constants.ValidationExpressions.Mobile);
                Boolean isCodeValid = System.Text.RegularExpressions.Regex.IsMatch(item.Code, Constants.ValidationExpressions.ValidationCode);
                Boolean isPassValid = System.Text.RegularExpressions.Regex.IsMatch(item.Password, Constants.ValidationExpressions.Password);
                if (isMobileValid && isCodeValid && isPassValid)
                {
                    String password = FormsAuthentication.HashPasswordForStoringInConfigFile(item.Password, "SHA1");

                    Byte type = (Byte)Enums.DBAccess.MemberSaveType.UpdatePassword;
                    MemberInfo memberInfo = new MemberInfo() { MemberId = id, Avatar = password, Email = item.Code, Phone = item.Mobile };
                    result = _commonService.UpdateMemberProfile(memberInfo, type);
                }
                else
                {
                    result = 2;
                }
            }
            else if (!String.IsNullOrEmpty(item.Intro))
            {
                result = _commonService.UpdateMemberInfo(id, (Byte)Enums.DBAccess.MemberSaveType.UpdateIntro, item.Intro);
            }
            else if (!String.IsNullOrEmpty(item.Name))
            {
                int cityId = 0;
                if (!String.IsNullOrEmpty(item.CityName))
                {
                    var cityDic = _contentService.GetCities("cn");
                    cityId = LookupHelper.GetCityIdByName(cityDic, item.CityName);
                    if (cityId.Equals(0))
                    {
                        result = 3;//invalid cityname
                    }
                }

                Byte type = (Byte)Enums.DBAccess.MemberSaveType.UpdateBasicInfo;
                item.Avatar = String.IsNullOrEmpty(item.Avatar) ? "" : item.Avatar;
                MemberInfo memberInfo = new MemberInfo() { MemberId = id, Name = item.Name, Gender = item.Gender, CityId = cityId, Avatar = item.Avatar };
                result = _commonService.UpdateMemberProfile(memberInfo, type);
            }
            else
            {
                result = 4;
            }

            return result;
        }

        /// <summary>
        /// Clean user context for switch account
        /// </summary>
        /// <param name="item"></param>
        private void ResetMemberContext(MemberInfo item)
        {
            Boolean isSwitchAccount = false;
            if (!WebContext.Current.MemberId.Equals(item.MemberId))
            {
                WebContext.Current.MemberId = item.MemberId;
            }

            if (!WebContext.Current.SocialAccount.Equals(item.SocialAccount))
            {
                WebContext.Current.SocialAccount = item.SocialAccount;
                isSwitchAccount = true;
            }

            if (!WebContext.Current.SocialType.Equals(item.SocialType))
            {
                WebContext.Current.SocialType = item.SocialType;
                isSwitchAccount = true;
            }

            if (!WebContext.Current.OpenId.Equals(item.OpenId))
            {
                WebContext.Current.OpenId = item.OpenId;
                isSwitchAccount = true;
            }

            if (isSwitchAccount)
            {
                WebContext.Current.SocialAccessInfo = "";
            }
        }

        /// <summary>
        /// Check if should update user Rong cloud token and ClientID
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="deviceToken">new client id for para</param>
        private void UpdateUser3rdToken(ref MemberInfo memberInfo, String deviceToken)
        {
            Boolean isResave = false;
            deviceToken = String.IsNullOrEmpty(deviceToken) ? "" : deviceToken;

            //Not has Rong Cloud token before, generate token
            if (String.IsNullOrEmpty(memberInfo.RCToken))
            {
                memberInfo.RCToken = RongCloudHelper.GetToken(memberInfo.MemberId, memberInfo.Name, memberInfo.Avatar);
                isResave = !String.IsNullOrEmpty(memberInfo.RCToken);
            }
            //Has rong cloud token ,should update device token
            if (!isResave && !memberInfo.DeviceToken.Equals(deviceToken) && !String.IsNullOrEmpty(deviceToken))
            {
                isResave = true;
            }

            if (isResave)
            {
                Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateRCTokenADeviceToken;
                MemberInfo memberTokenInfo = new MemberInfo() { MemberId = memberInfo.MemberId, Avatar = memberInfo.RCToken, SelfIntro = deviceToken };
                var updateResult = _commonService.UpdateMemberProfile(memberTokenInfo, saveType);
            }
        }

    }
}
