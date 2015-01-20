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
        //public static MemberInfo Map(ObjectResult<MemberInfo> objectUser)
        //{
        //    if (objectUser != null && objectUser.Count() != 0)
        //    {
        //        List<MemberInfo> userInfos = objectUser.ToList<MemberInfo>();
        //        return userInfos[0];
        //    }
        //    return null;
        //}

        public static MemberInfo Map(MemberInfo_Load_p_Result result)
        {
            if (result != null)
            {
                MemberInfo memberInfo = new MemberInfo() { MemberId = result.MemberId, CityId = result.CityId, Coins = result.Coins, Avatar = result.Avatar, CoinsLocked = result.CoinsLocked, CreatedDate = result.CreatedDate, Email = result.Email, Gender = result.Gender, IsActive = result.IsActive, LastUpdateDate = result.LastUpdateDate, Name = result.Name, OpenId = result.OpenId, Phone = result.Phone, PosX = result.PosX, PosY = result.PosY, SelfIntro = result.SelfIntro, SocialAccount = result.SocialAccount, ContactName = result.ContactName, SocialType = result.SocialType, Address = result.Address, BirthDate = result.BirthDate, IsMobileVerified = result.IsMobileVerified, IsEmailVerified = result.IsEmailVerified, Etag = result.Etag, IsLike = result.IsLike, MasterInfo = result.MasterInfo };//, IsLike = result.IsLike, MasterInfo = result.MasterInfo
                return memberInfo;
            }
            return null;
        }

        public static List<MemberInfo> Map(ObjectResult<MemberInfo_Load_p_Result> result)
        {
            if (result != null)
            {
                var members = result.Select(item => new MemberInfo() { MemberId = item.MemberId, CityId = item.CityId, Coins = item.Coins, Avatar = item.Avatar, CoinsLocked = item.CoinsLocked, CreatedDate = item.CreatedDate, Email = item.Email, Gender = item.Gender, IsActive = item.IsActive, LastUpdateDate = item.LastUpdateDate, Name = item.Name, OpenId = item.OpenId, Phone = item.Phone, PosX = item.PosX, PosY = item.PosY, SelfIntro = item.SelfIntro, SocialAccount = item.SocialAccount, ContactName = item.ContactName, SocialType = item.SocialType, Address = item.Address, BirthDate = item.BirthDate, IsMobileVerified = item.IsMobileVerified, IsEmailVerified = item.IsEmailVerified, Etag = item.Etag, IsLike = item.IsLike, MasterInfo = item.MasterInfo }).ToList();//
                return (members.Count > 0) ? members : null;
            }
            return null;
        }

        //public static MemberInfoItem MapItem(MemberInfo_Load_p_Result result)
        //{
        //    if (result != null)
        //    {
        //        MemberInfoItem memberInfo = new MemberInfoItem() { MemberId = result.MemberId, CityId = result.CityId, Coins = result.Coins, Avatar = result.Avatar, CoinsLocked = result.CoinsLocked, CreatedDate = result.CreatedDate, Email = result.Email, Gender = result.Gender, IsActive = result.IsActive, LastUpdateDate = result.LastUpdateDate, Name = result.Name, OpenId = result.OpenId, Phone = result.Phone, PosX = result.PosX, PosY = result.PosY, SelfIntro = result.SelfIntro, SocialAccount = result.SocialAccount, ContactName = result.ContactName, SocialType = result.SocialType, Address = result.Address, BirthDate = result.BirthDate, IsMobileVerified = result.IsMobileVerified, IsEmailVerified = result.IsEmailVerified, Etag = result.Etag, IsLike = result.IsLike, MasterInfo = result.MasterInfo };//, IsLike = result.IsLike, MasterInfo = result.MasterInfo
        //        return memberInfo;
        //    }
        //    return null;
        //}

        //public static List<MemberInfoItem> MapItem(ObjectResult<MemberInfo_Load_p_Result> result)
        //{
        //    if (result != null)
        //    {
        //        var members = result.Select(item => new MemberInfoItem() { MemberId = item.MemberId, CityId = item.CityId, Coins = item.Coins, Avatar = item.Avatar, CoinsLocked = item.CoinsLocked, CreatedDate = item.CreatedDate, Email = item.Email, Gender = item.Gender, IsActive = item.IsActive, LastUpdateDate = item.LastUpdateDate, Name = item.Name, OpenId = item.OpenId, Phone = item.Phone, PosX = item.PosX, PosY = item.PosY, SelfIntro = item.SelfIntro, SocialAccount = item.SocialAccount, ContactName = item.ContactName, SocialType = item.SocialType, Address = item.Address, BirthDate = item.BirthDate, IsMobileVerified = item.IsMobileVerified, IsEmailVerified = item.IsEmailVerified, Etag = item.Etag, IsLike = result.IsLike, MasterInfo = result.MasterInfo }).ToList();//
        //        return (members.Count > 0) ? members : null;
        //    }
        //    return null;
        //}

               
    }
}
