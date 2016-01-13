using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Mapper
{
    public class MemberMapper
    {
        public static MemberInfo Map(MemberInfo_Load_p_Result result)
        {
            if (result != null)
            {
                MemberInfo memberInfo = new MemberInfo()
                {
                    MemberId = result.MemberId,
                    CityId = result.CityId,
                    Coins = result.Coins,
                    Avatar = result.Avatar,
                    CoinsLocked = result.CoinsLocked,
                    CreatedDate = result.CreatedDate,
                    Email = result.Email,
                    Gender = result.Gender,
                    BirthDate = result.BirthDate,
                    IsActive = true,
                    LastUpdateDate = result.LastUpdateDate,
                    Name = result.Name,
                    OpenId = result.OpenId,
                    Phone = result.Phone,
                    PosX = result.PosX,
                    PosY = result.PosY,
                    SelfIntro = result.SelfIntro,
                    SocialAccount = result.SocialAccount,
                    ContactName = result.ContactName,
                    SocialType = result.SocialType,
                    Address = result.Address,
                    Etag = result.Etag,
                    VerifyTag = result.VerifyTag,
                    NotifyTag = result.NotifyTag,
                    Credit = result.Credit,
                    IsLike = result.IsLike,
                    ExtraInfo = result.ExtraInfo,
                    RCToken = result.RCToken,
                    DeviceToken = result.DeviceToken
                };
                return memberInfo;
            }
            return null;
        }

        public static List<MemberInfo> Map(ObjectResult<MemberInfo_Load_p_Result> result)
        {
            if (result != null)
            {
                var members = result.Select(item => new MemberInfo()
                {
                    MemberId = item.MemberId,
                    CityId = item.CityId,
                    Coins = item.Coins,
                    Avatar = item.Avatar,
                    CoinsLocked = item.CoinsLocked,
                    CreatedDate = item.CreatedDate,
                    Email = item.Email,
                    Gender = item.Gender,
                    BirthDate = item.BirthDate,
                    IsActive = true,
                    LastUpdateDate = item.LastUpdateDate,
                    Name = item.Name,
                    OpenId = item.OpenId,
                    Phone = item.Phone,
                    PosX = item.PosX,
                    PosY = item.PosY,
                    SelfIntro = item.SelfIntro,
                    SocialAccount = item.SocialAccount,
                    ContactName = item.ContactName,
                    SocialType = item.SocialType,
                    Address = item.Address,
                    Etag = item.Etag,
                    VerifyTag = item.VerifyTag,
                    NotifyTag = item.NotifyTag,
                    Credit = item.Credit,
                    IsLike = item.IsLike,
                    ExtraInfo = item.ExtraInfo
                    //,RCToken = item.RCToken
                }).ToList();
                return (members.Count > 0) ? members : null;
            }
            return null;
        }
               
    }
}
