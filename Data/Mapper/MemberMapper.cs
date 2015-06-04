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
                    IsActive = result.IsActive,
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
                    BirthDate = result.BirthDate,
                    Etag = result.Etag,
                    VerifyTag = result.VerifyTag,
                    NotifyTag = result.NotifyTag,
                    IsLike = result.IsLike,
                    MasterInfo = result.MasterInfo
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
                    IsActive = item.IsActive,
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
                    BirthDate = item.BirthDate,
                    Etag = item.Etag,
                    VerifyTag = item.VerifyTag,
                    NotifyTag = item.NotifyTag,
                    IsLike = item.IsLike,
                    MasterInfo = item.MasterInfo
                }).ToList();
                return (members.Count > 0) ? members : null;
            }
            return null;
        }
               
    }
}
