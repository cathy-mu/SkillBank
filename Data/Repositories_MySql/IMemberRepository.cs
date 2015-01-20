using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Data.Entity;

using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using SkillBank.Site.Common;
using SkillBank.Site.DataSource.Mapper;
using SkillBank.Site.DataSource.Data;


namespace SkillBank.Site.DataSource.Data
{
    public interface IMemberRepository
    {
        MemberInfo GetMemberInfo(Byte loadType, String socialAccount, Byte socialType, int memberId);
        Boolean UpdateMemberInfo(int memberId, Byte saveType, String phoneNo, int cityId, String memberName, String intro, Boolean isMale, String eMail, Decimal posX, Decimal poxY, DateTime birthday, String avatar);
        int CreateMember(out int memberId, String socialOpenId, Byte socialType, String memberName, String email, String avatar = "", int cityId = 0);
        void LeaveEmailAddress(String name,String mail);
    }

    public class MemberRepository : DbContext, IMemberRepository//Entities
    {
        public MemberRepository()
            : base("name=Entities")
        {
        }

        /// <summary>
        /// Create new member
        /// </summary>
        /// <param name="socialOpenId"></param>
        /// <param name="socialType"></param>
        /// <param name="memberName"></param>
        /// <param name="email"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public int CreateMember(out int memberId, String socialOpenId, Byte socialType, String memberName, String email, String avatar="", int cityId = 0)
        {
            //TO DO:2.0 Decide to use social accout or OpenId later
            return MemberInfo_Add_p(out memberId, socialOpenId, socialType, avatar, memberName, email, cityId);
        }

        /// <summary>
        /// Get member info by social account 
        /// </summary>
        /// <param name="loadType"></param>
        /// <param name="socialAccount"></param>
        /// <param name="socialType"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public MemberInfo GetMemberInfo(Byte loadType, String socialAccount, Byte socialType, int memberId)
        {
            return MemberInfo_Load_p(loadType, socialAccount, socialType, memberId);
        }

        public Boolean UpdateMemberInfo(int memberId, Byte saveType, String phoneNo, int cityId, String memberName, String intro, Boolean isMale, String eMail, Decimal posX, Decimal poxY, DateTime birthday,String avatar)
        {
            return MemberInfo_Save_p(memberId, saveType, phoneNo, cityId, memberName, intro, isMale, eMail,posX,poxY, birthday, avatar);
            //int result = 0;
            //return base.MemberInfo_Save_p(saveType, memberId, /*isMale*/0, new ObjectParameter("Mail", eMail), phoneNo, cityId, memberName, intro, new ObjectParameter("result", result), posX, poxY, birthday) == 0;
        }

        private MemberInfo MemberInfo_Load_p(Byte loadType, String socialAccount, Byte socialType, int memberId)
        {
            var loadByParameter = new ObjectParameter("LoadBy", loadType);
            var socialAccountParameter = new ObjectParameter("Account", socialAccount);
            var socialTypeParameter = new ObjectParameter("Type", socialType);
            var memberIdParameter = new ObjectParameter("Id", memberId);
            var Context = ((IObjectContextAdapter)this).ObjectContext;
            var result = Context.ExecuteFunction<MemberInfo_Load_p_Result>("MemberInfo_Load_p", MergeOption.NoTracking, loadByParameter, socialAccountParameter, socialTypeParameter, memberIdParameter).FirstOrDefault();
            //Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, result);
            
            if (result != null)
            {
                MemberInfo memberInfo = new MemberInfo() { MemberId = result.MemberId, CityId = result.CityId, Coins = result.Coins, Avatar = result.Avatar, CoinsLocked = result.CoinsLocked, CreatedDate = result.CreatedDate, Email = result.Email, Gender = result.Gender, IsActive = result.IsActive, LastUpdateDate = result.LastUpdateDate, LearnTag = result.LearnTag, Name = result.Name, OpenId = result.OpenId, Phone = result.Phone, PosX = result.PosX, PosY = result.PosY, SelfIntro = result.SelfIntro, Social1 = result.Social1, Social2 = result.Social2, Social3 = result.Social3, TeachTag = result.TeachTag, SocialType = result.SocialType };
                return memberInfo;
            }
            else
            {
                return null;
            }
        }

        private Boolean MemberInfo_Save_p(int memberId, Byte saveType, String phoneNo, int cityId, String memberName, String intro, Boolean isMale, String eMail, Decimal posX, Decimal posY, DateTime birthday, String avatar)
        {
            var loadByParameter = new ObjectParameter("SaveType", saveType);
            var memberIdParameter = new ObjectParameter("Id", memberId);
            var phoneNoParameter = new ObjectParameter("PhoneNo", phoneNo);
            var cityIdParameter = new ObjectParameter("City", cityId);
            var memberNameParameter = new ObjectParameter("MemberName", memberName);
            var introParameter = new ObjectParameter("Intro", intro);
            var genderParameter = new ObjectParameter("IsMale", isMale);
            var eMaileParameter = new ObjectParameter("Mail", eMail);
            var resultParameter = new ObjectParameter("Result", 0);
            var posXParameter = new ObjectParameter("X", posX);
            var posYParameter = new ObjectParameter("Y", posY);
            var birthdayParameter = new ObjectParameter("Birthday", birthday);
            var avatarParameter = new ObjectParameter("Img", avatar);
            var result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MemberInfo_Save_p", loadByParameter, memberIdParameter, phoneNoParameter, cityIdParameter, memberNameParameter, introParameter, genderParameter, eMaileParameter, resultParameter, posXParameter, posYParameter, birthdayParameter, avatarParameter);

            return ((Byte)resultParameter.Value == 0);
        }

        private int MemberInfo_Add_p(out int memberId, String socialId, Byte socialType, String avatar, String memberName, String eMail, int cityId=0)
        {
            memberId = 0;
            //var loadByParameter = new ObjectParameter("SaveType", saveType);
            var socialAccountParameter = new ObjectParameter("SocialId", socialId);
            var socialTypeParameter = new ObjectParameter("SType", socialType);
            var memberIdParameter = new ObjectParameter("Id", memberId);
            //var cityIdParameter = new ObjectParameter("City", cityId);
            var cityIdParameter = new ObjectParameter("City", 0);
            var memberNameParameter = new ObjectParameter("MemberName", memberName);
            var eMaileParameter = new ObjectParameter("Mail", eMail);
            var avatarPathParameter = new ObjectParameter("AvatarPath", avatar);
            var resultParameter = new ObjectParameter("Result", 0);
            var result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MemberInfo_Add_p", socialAccountParameter, socialTypeParameter, memberIdParameter, /*phoneNoParameter,*/ cityIdParameter, memberNameParameter, avatarPathParameter, eMaileParameter, resultParameter);
            memberId = (int)memberIdParameter.Value;
            return (int)resultParameter.Value;
        }

        public void LeaveEmailAddress(String name,String mail)
        {
            var nameParameter = new ObjectParameter("Name", name);
            var mailParameter = new ObjectParameter("Mail", mail);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EmailAccount_Save_p", nameParameter, mailParameter);
        }

    }
}
