using Common.Enums;
using Interfaces.Authorizes;
using Interfaces.HTTP;
using Interfaces.PublicAccountIDs;
using Interfaces.WX.AccessToken;
using Interfaces.WX.AppIDSecret;
using Models.Authorizes;
using Models.Enum.Errors;
using Models.Error;
using Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBank.Site.Services.Tencent
{
    public class AuthorizeHandler : IAuthorizeHandler
    {
        private IHttper _httper;
        private IRepository _repository;
        private IPublicAccountIDHandler _publicAccountIDHandler;
        private IWXAppIDSecret _wxAppIDSecret;
        private IWXAccessToken _wxAccessToken;
        public AuthorizeHandler(
            IHttper httper,
            IRepository repository,
            IPublicAccountIDHandler publicAccountIDHandler,
            IWXAppIDSecret wxAppIDSecret,
            IWXAccessToken wxAccessToken
            )
        {
            this._httper = httper;
            this._repository = repository;
            this._publicAccountIDHandler = publicAccountIDHandler;
            this._wxAppIDSecret = wxAppIDSecret;
            this._wxAccessToken = wxAccessToken;
        }

        #region 根据AppID,Secret和Code获取Token

        public Models.Authorizes.AuthorizesForAccessTokenModel getAccessTokenByCode(string appID, string secret, string code, string grant_type = null)
        {
            ///获取Token的URL
            var getTokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appID + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code";
            ///请求URl返回的来JSon字符串
            var ReturnJson = _httper.Get(getTokenUrl);
            //将Json字条串转换成Model
            var model = JSONHelper.ConvertToObject<AuthorizesForAccessTokenModel>(ReturnJson);
            //验证Token是否是有效的Token
            var checkTokenUrl = "https://api.weixin.qq.com/sns/auth?access_token=" + model.Access_Token + "&openid=" + model.OpenID;
            var resultJson = _httper.Get(checkTokenUrl);
            var result = JSONHelper.ConvertToObject<ErrorModel>(resultJson);
            if (result.ErrCode == 40001 || result.ErrCode == 41001)
            {
                var referenshUrl = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + appID + "&grant_type=refresh_token&refresh_token=" + model.Refresh_Token;
                var referenshJson = _httper.Get(referenshUrl);
                model = JSONHelper.ConvertToObject<AuthorizesForAccessTokenModel>(referenshJson);
            }
            return model;
        }

        #endregion

        #region 根据code和state获取用户信息

        public AuthorizesForUserInfoModel GetUserInfoByOpenIDAndToken(string code, string state, out int publiAccountIDOutPut, int publicAccountID = 0)
        {
            var userInfoModel = new AuthorizesForUserInfoModel();
            try
            {
                var setpublicAccountID = 0;
                ///得到主公众账号
                if (publicAccountID == 0)
                {
                    setpublicAccountID = this._publicAccountIDHandler.GetPrimaryPublicAccountID();
                }
                else
                {
                    setpublicAccountID = publicAccountID; ;
                }
                publiAccountIDOutPut = setpublicAccountID;

                //获取主公众账号的APPID和Secret
                Dictionary<string, string> appIDAndScret = _wxAppIDSecret.GetAppIDSecret(setpublicAccountID);
                //通过APPID 和Secret 获取TokenModel
                var model = getAccessTokenByCode(appIDAndScret["AppID"], appIDAndScret["Secret"], code);
                ///如果返回的错误码是40029为Code没有通过验证
                if (model.ErrCode == 40029)
                {
                    userInfoModel.ErrCode = 40029;
                    userInfoModel.ErrMsg = EnumConvertor.GetEnumDescription(ErrorCode.InvalidCode);
                    //HttpStatusCode.BadRequest, new { ErrorCode = ErrorCode.UnknownException, ErrorMessage = EnumConvertor.GetEnumDescription(ErrorCode.UnknownException) }
                    return userInfoModel;
                }
                var getUserInfoUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=" + model.Access_Token + "&openid=" + model.OpenID + "&lang=zh_CN";
                var userInfoJson = _httper.Get(getUserInfoUrl);
                userInfoModel = JSONHelper.ConvertToObject<AuthorizesForUserInfoModel>(userInfoJson);
                userInfoModel.Access_Token = model.Access_Token;
                userInfoModel.Refresh_Token = model.Refresh_Token;
                userInfoModel.Expires_In = model.Expires_In;

            }
            catch (Exception)
            {

                throw;
            }
            return userInfoModel;
        }

        #endregion

        #region 获取完整用户信息（含UnionID）
        public AuthorizesForUserInfoModel GetUserInfoForUnionID(string code, string state, out int publiAccountIDOutPut, int publicAccountID = 0)
        {
            var userInfoModel = new AuthorizesForUserInfoModel();
            try
            {
                var setpublicAccountID = 0;
                ///得到主公众账号
                if (publicAccountID == 0)
                {
                    setpublicAccountID = this._publicAccountIDHandler.GetPrimaryPublicAccountID();
                }
                else
                {
                    setpublicAccountID = publicAccountID; ;
                }
                publiAccountIDOutPut = setpublicAccountID;

                //获取主公众账号的APPID和Secret
                Dictionary<string, string> appIDAndScret = _wxAppIDSecret.GetAppIDSecret(setpublicAccountID);
                //通过APPID 和Secret 获取TokenModel
                var model = getAccessTokenByCode(appIDAndScret["AppID"], appIDAndScret["Secret"], code);
                ///如果返回的错误码是40029为Code没有通过验证
                if (model.ErrCode == 40029)
                {
                    userInfoModel.ErrCode = 40029;
                    userInfoModel.ErrMsg = EnumConvertor.GetEnumDescription(ErrorCode.InvalidCode);
                    //HttpStatusCode.BadRequest, new { ErrorCode = ErrorCode.UnknownException, ErrorMessage = EnumConvertor.GetEnumDescription(ErrorCode.UnknownException) }
                    return userInfoModel;
                }
                //这个Token调用接口凭证的Token
                var token = _wxAccessToken.GetWXAccessToken(setpublicAccountID);
                var tokenModel = JSONHelper.ConvertToObject<AuthorizesForAccessTokenModel>(token);
                var url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + tokenModel.Access_Token + "&openid=" + model.OpenID + "&lang=zh_CN";
                var unionIDJsonStr = _httper.Get(url);
                userInfoModel = JSONHelper.ConvertToObject<AuthorizesForUserInfoModel>(unionIDJsonStr);
                if (userInfoModel.ErrCode == 40001)
                {
                    //HttpStatusCode.BadRequest, new { ErrorCode = ErrorCode.UnknownException, ErrorMessage = EnumConvertor.GetEnumDescription(ErrorCode.UnknownException) }
                    return userInfoModel;
                }
                if (userInfoModel.ErrCode == 41001)
                {
                    //HttpStatusCode.BadRequest, new { ErrorCode = ErrorCode.UnknownException, ErrorMessage = EnumConvertor.GetEnumDescription(ErrorCode.UnknownException) }
                    return userInfoModel;
                }
                userInfoModel.Access_Token = model.Access_Token;
                userInfoModel.Refresh_Token = model.Refresh_Token;
                userInfoModel.Expires_In = model.Expires_In;
            }
            catch
            {
                throw;
            }
            return userInfoModel;
        } 
        #endregion
  
    }
}
