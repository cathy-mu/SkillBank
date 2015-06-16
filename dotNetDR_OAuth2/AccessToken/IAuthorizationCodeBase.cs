using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetDR_OAuth2.AccessToken
{
    /// <summary>
    /// Authorization Code授权机制的接口
    /// </summary>
    public interface IAuthorizationCodeBase : IGetCode, IGetAccessToken, IAccessToken
    {
    }
}
