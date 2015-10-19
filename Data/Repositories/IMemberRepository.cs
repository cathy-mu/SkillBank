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
        List<MemberInfo> GetMemberInfos(Byte loadBy, String searchKey);
        MemberInfo GetMemberInfo(Byte loadType, String account, Byte socialType, String para="");
        MemberInfo GetMemberInfo(Byte loadType, int memberId, int relatedMemberId = 0);
        Byte UpdateMemberInfo(int memberId, Byte saveType, String phoneNo, int cityId, String memberName, String intro, Boolean isMale, String eMail, Decimal posX, Decimal poxY, DateTime birthday, String avatar);
        int CreateMember(out int memberId, String socialOpenId, Byte socialType, String memberName, String email, String avatar = "", string mobile = "", string code = "", String pass = "", String etag = "", Boolean gender = true);
        void LeaveEmailAddress(String name, String mail);
        Boolean CoinUpdate(Byte saveType, int memberId, int classId, int amount);
        Dictionary<Enum, int> GetMemberNums(int memberId, int classId, Byte loadBy);
        Boolean AddMembersCoin(int memberId, int coinsToAdd, Byte addType);
        Boolean HasShareClassCoin(int memberId);
        Byte UpdateVerification(Byte saveType, int memberId, String verifyAccount, String code);
        Byte SaveWeChatEvent(Byte saveType, int memberId, String openId, String paraId, String paraValue);
        Byte UpdateCredit(Byte saveType, int memberId, int paraValue);
    }

    public class MemberRepository : Entities, IMemberRepository//Entities
    {
        public MemberRepository()
        //: base("name=Entities")
        {
        }

        public Byte SaveWeChatEvent(Byte saveType, int memberId, String openId, String paraId, String paraValue)
        {
            return WeChatEvent_Save_p(saveType, memberId, openId, paraId, paraValue);
        }

        public Byte UpdateVerification(Byte saveType, int memberId, String verifyAccount, String code)
        {
            return MobileVerification_Save_p(saveType, memberId, verifyAccount, code);
        }

        public Boolean AddMembersCoin(int memberId, int coinsToAdd, Byte addType)
        {
            return Coin_Update_p(memberId, coinsToAdd, addType);
        }

        public Boolean HasShareClassCoin(int memberId)
        {
            return Coin_Update_p(memberId, 0, (Byte)Enums.DBAccess.CoinUpdateType.CheckShareClassCoin);
        }

        public Byte UpdateCredit(Byte saveType, int memberId, int paraValue)
        {
            return CreditHistory_Save_p(saveType, memberId, paraValue);
        }
        
        //for admin
        public Boolean CoinUpdate(Byte saveType, int memberId, int classId, int amount)
        {
            var loadByParameter = new ObjectParameter("saveType", saveType);
            var memberIdParameter = new ObjectParameter("memberId", memberId);
            var classIdParameter = new ObjectParameter("classId", classId);
            var amountParameter = new ObjectParameter("amount", amount);
            var resultParameter = new ObjectParameter("result", 0);
            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Coin_UpdateTool_p", loadByParameter, memberIdParameter, classIdParameter, amountParameter, resultParameter);

            return ((Boolean)resultParameter.Value);
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
        public int CreateMember(out int memberId, String socialOpenId, Byte socialType, String memberName, String email, String avatar = "", string mobile = "", string code = "", String pass="", String etag = "", Boolean gender = true)
        {
            return MemberInfo_Add_p(out memberId, socialOpenId, socialType, avatar, memberName, email, mobile, code, pass, etag, gender);
        }


        /// <summary>
        /// Get member info by social account 
        /// </summary>
        public MemberInfo GetMemberInfo(Byte loadType, int memberId, int relatedMemberId = 0)
        {
            return MemberInfo_Load_p(loadType, "", 0, "", memberId, relatedMemberId);
        }

        public MemberInfo GetMemberInfo(Byte loadType, String socialAccount, Byte socialType, String para="")
        {
            return MemberInfo_Load_p(loadType, socialAccount, socialType, para, 0, 0);
        }

        public Byte UpdateMemberInfo(int memberId, Byte saveType, String phoneNo, int cityId, String memberName, String intro, Boolean isMale, String eMail, Decimal posX, Decimal poxY, DateTime birthday, String avatar)
        {
            return MemberInfo_Save_p(memberId, saveType, phoneNo, cityId, memberName, intro, isMale, eMail, posX, poxY, birthday, avatar);
        }

        private MemberInfo MemberInfo_Load_p(Byte loadType, String account, Byte socialType, String para, int memberId, int relatedMemberId)
        {
            var loadByParameter = new ObjectParameter("LoadBy", loadType);
            var accountParameter = new ObjectParameter("Account", account);
            var paraParameter = new ObjectParameter("Para", para);
            var socialTypeParameter = new ObjectParameter("Type", socialType);
            var memberIdParameter = new ObjectParameter("Id", memberId);
            var relatedIdParameter = new ObjectParameter("RelatedId", relatedMemberId);
            var Context = ((IObjectContextAdapter)this).ObjectContext;
            var result = Context.ExecuteFunction<MemberInfo_Load_p_Result>("MemberInfo_Load_p", MergeOption.NoTracking, loadByParameter, accountParameter, paraParameter, socialTypeParameter, memberIdParameter, relatedIdParameter).FirstOrDefault();
            //Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, result);

            return MemberMapper.Map(result);
        }
        
        public List<MemberInfo> GetMemberInfos(Byte loadBy, String searchKey)
        {
            var loadByParameter = new ObjectParameter("LoadBy", loadBy);
            var accountParameter = new ObjectParameter("Account", searchKey);
            var paraParameter = new ObjectParameter("Para", "");
            var socialTypeParameter = new ObjectParameter("Type", 0);
            var memberIdParameter = new ObjectParameter("Id", 0);
            var relatedIdParameter = new ObjectParameter("RelatedId", 0);

            var result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MemberInfo_Load_p_Result>("MemberInfo_Load_p", MergeOption.NoTracking, loadByParameter, accountParameter, paraParameter, socialTypeParameter, memberIdParameter, relatedIdParameter);
            return MemberMapper.Map(result);
        }

        private Byte MemberInfo_Save_p(int memberId, Byte saveType, String phoneNo, int cityId, String memberName, String intro, Boolean isMale, String eMail, Decimal posX, Decimal posY, DateTime birthday, String avatar)
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

            return ((Byte)resultParameter.Value);
        }

        private int MemberInfo_Add_p(out int memberId, String socialId, Byte socialType, String avatar, String memberName, String eMail, string mobile = "", string code = "", string pass = "", String etag = "", Boolean gender = true)
        {
            memberId = 0;
            var socialAccountParameter = new ObjectParameter("SocialId", socialId);
            var socialTypeParameter = new ObjectParameter("SType", socialType);
            var memberIdParameter = new ObjectParameter("Id", memberId);
            var mobileParameter = new ObjectParameter("Phone", mobile);
            var codeParameter = new ObjectParameter("VerifyCode", code);
            var memberNameParameter = new ObjectParameter("MemberName", memberName);
            var eMailParameter = new ObjectParameter("Mail", eMail);
            var avatarPathParameter = new ObjectParameter("AvatarPath", avatar);
            var etagParameter = new ObjectParameter("Etag", etag);
            var genderParameter = new ObjectParameter("Gender", gender);
            var passParameter = new ObjectParameter("Pass", pass);
            var resultParameter = new ObjectParameter("Result", 0);
            var result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MemberInfo_Add_p", socialAccountParameter, socialTypeParameter, memberIdParameter, mobileParameter, codeParameter, passParameter, memberNameParameter, avatarPathParameter, eMailParameter, genderParameter, etagParameter, resultParameter);
            memberId = (int)memberIdParameter.Value;
            return (int)resultParameter.Value;
        }

        public void LeaveEmailAddress(String name, String mail)
        {
            var nameParameter = new ObjectParameter("Name", name);
            var mailParameter = new ObjectParameter("Mail", mail);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EmailAccount_Save_p", nameParameter, mailParameter);
        }


        public Dictionary<Enum, int> GetMemberNums(int memberId, int classId, Byte loadBy)
        {
            return MemberNums_Load_p(loadBy, memberId, classId);
        }


        private Dictionary<Enum, int> MemberNums_Load_p(Byte loadType, int memberId, int classId)
        {
            Dictionary<Enum, int> numDic = new Dictionary<Enum, int>();
            int resultc1 = 0;
            int resultc2 = 0;
            int resultc3 = 0;
            int resulto1 = 0;
            int resulto2 = 0;
            int resulto3 = 0;
            int result1 = 0;
            int result2 = 0;
            int result3 = 0;
            int resultc = 0;

            ObjectParameter loadTypeParameter = new ObjectParameter("loadType", loadType);
            ObjectParameter memberParameter = new ObjectParameter("memberId", memberId);
            ObjectParameter classParameter = new ObjectParameter("classId", classId);
            ObjectParameter resultc1Parameter = new ObjectParameter("resultc1", resultc1);
            ObjectParameter resultc2Parameter = new ObjectParameter("resultc2", resultc2);
            ObjectParameter resultc3Parameter = new ObjectParameter("resultc3", resultc3);
            ObjectParameter resulto1Parameter = new ObjectParameter("resulto1", resulto1);
            ObjectParameter resulto2Parameter = new ObjectParameter("resulto2", resulto2);
            ObjectParameter resulto3Parameter = new ObjectParameter("resulto3", resulto3);
            ObjectParameter result1Parameter = new ObjectParameter("resultNum1", result1);
            ObjectParameter result2Parameter = new ObjectParameter("resultNum2", result2);
            ObjectParameter result3Parameter = new ObjectParameter("resultNum3", result3);
            ObjectParameter resultcParameter = new ObjectParameter("resultc", resultc);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MemberNums_Load_p", loadTypeParameter, memberParameter, classParameter, result1Parameter, result2Parameter, result3Parameter, resultc1Parameter, resultc2Parameter, resultc3Parameter, resulto1Parameter, resulto2Parameter, resulto3Parameter, resultcParameter);

            resultc1 = Convert.ToInt32(resultc1Parameter.Value);
            resultc2 = Convert.ToInt32(resultc2Parameter.Value);
            resultc3 = Convert.ToInt32(resultc3Parameter.Value);
            resulto1 = Convert.ToInt32(resulto1Parameter.Value);
            resulto2 = Convert.ToInt32(resulto2Parameter.Value);
            resulto3 = Convert.ToInt32(resulto3Parameter.Value);

            if (loadType.Equals((Byte)Enums.DBAccess.MemberNumsLoadType.ByClassId))
            {
                numDic.Add(Enums.NumberDictionaryKey.Class, Convert.ToInt32(result1Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Student, Convert.ToInt32(result2Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Rank, Convert.ToInt32(result3Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Result01, resultc1);
                numDic.Add(Enums.NumberDictionaryKey.Result02, resultc2);
                numDic.Add(Enums.NumberDictionaryKey.Result03, resultc3);
                numDic.Add(Enums.NumberDictionaryKey.Result11, resulto1 < resultc1 ? 0 : (resulto1 - resultc1));
                numDic.Add(Enums.NumberDictionaryKey.Result12, resulto2 < resultc2 ? 0 : (resulto2 - resultc2));
                numDic.Add(Enums.NumberDictionaryKey.Result13, resulto3 < resultc3 ? 0 : (resulto3 - resultc3));
                numDic.Add(Enums.NumberDictionaryKey.Comment, Convert.ToInt32(resultcParameter.Value));
            }
            else if (loadType.Equals((Byte)Enums.DBAccess.MemberNumsLoadType.ByMemberId))
            {
                numDic.Add(Enums.NumberDictionaryKey.Result01, resultc1);
                numDic.Add(Enums.NumberDictionaryKey.Result02, resultc2);
                numDic.Add(Enums.NumberDictionaryKey.Result03, resultc3);
                numDic.Add(Enums.NumberDictionaryKey.Result11, resulto1);
                numDic.Add(Enums.NumberDictionaryKey.Result12, resulto2);
                numDic.Add(Enums.NumberDictionaryKey.Result13, resulto3);
            }
            else if (loadType.Equals((Byte)Enums.DBAccess.MemberNumsLoadType.ByClassSummary))
            {
                numDic.Add(Enums.NumberDictionaryKey.Class, Convert.ToInt32(result1Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Student, Convert.ToInt32(result2Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Rank, Convert.ToInt32(result3Parameter.Value));
            }
            else if (loadType.Equals((Byte)Enums.DBAccess.MemberNumsLoadType.ByMemberSummary))
            {
                numDic.Add(Enums.NumberDictionaryKey.Class, Convert.ToInt32(result1Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Student, Convert.ToInt32(result2Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Certification, Convert.ToInt32(result3Parameter.Value) + 1);
                numDic.Add(Enums.NumberDictionaryKey.Follow, Convert.ToInt32(resulto1Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Fans, Convert.ToInt32(resulto2Parameter.Value));
            }
            else if (loadType.Equals((Byte)Enums.DBAccess.MemberNumsLoadType.ByMemberDashboard))
            {
                numDic.Add(Enums.NumberDictionaryKey.Class, Convert.ToInt32(result1Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Like, Convert.ToInt32(result2Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Certification, Convert.ToInt32(result3Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Follow, Convert.ToInt32(resulto1Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.Fans, Convert.ToInt32(resulto2Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.GotSharedCoins, Convert.ToInt32(resulto3Parameter.Value));
            }
            else if (loadType.Equals((Byte)Enums.DBAccess.MemberNumsLoadType.ByCreditGetMethods))
            {
                numDic.Add(Enums.NumberDictionaryKey.MissStudentReview, Convert.ToInt32(result1Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.MissTeacherReview, Convert.ToInt32(result2Parameter.Value));
                numDic.Add(Enums.NumberDictionaryKey.IsSignIn, Convert.ToInt32(result3Parameter.Value));
            }

            return numDic;
        }


        private Boolean Coin_Update_p(int memberId, int amount, Byte saveType)
        {
            Boolean hasShareClassCoin = false;
            ObjectParameter memberParameter = new ObjectParameter("memberId", memberId);
            ObjectParameter amountParameter = new ObjectParameter("amount", amount);
            ObjectParameter saveTypeParameter = new ObjectParameter("saveType", saveType);
            ObjectParameter resultParameter = new ObjectParameter("result", hasShareClassCoin);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Coin_Update_p", memberParameter, amountParameter, saveTypeParameter, resultParameter);
            return Convert.ToBoolean(resultParameter.Value);
        }

        private Byte MobileVerification_Save_p(Byte saveType, int memberId, String verifyAccount, String code)
        {
            var saveTypeParameter = new ObjectParameter("SaveType", saveType);
            var memberIdParameter = new ObjectParameter("MemberId", memberId);
            var mobileParameter = new ObjectParameter("VerifyAccount", verifyAccount);
            var codeParameter = new ObjectParameter("Code", code);
            var resultParameter = new ObjectParameter("Result", 0);
            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Verification_Save_p", saveTypeParameter, memberIdParameter, mobileParameter, codeParameter, resultParameter);
            return (Byte)resultParameter.Value;
        }

        private Byte WeChatEvent_Save_p(Byte saveType, int memberId, String openId, String paraId, String paraValue)
        {
            Byte result = 0;
            var saveTypePara = new ObjectParameter("SaveType", saveType);
            var memberIdPara = new ObjectParameter("MemberId", memberId);
            var openIdPara = new ObjectParameter("OpenId", openId);
            var paraIdPara = new ObjectParameter("ParaId", paraId);
            var paraValuePara = new ObjectParameter("ParaValue", paraValue);
            var resultPara = new ObjectParameter("Result", result);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("WeChatEvent_Save_p", saveTypePara, memberIdPara, openIdPara, paraIdPara, paraValuePara, resultPara);
            return (Byte)resultPara.Value;
        }

        private Byte CreditHistory_Save_p(Byte saveType, int memberId, int paraValue)
        {
            Byte result = 0;
            var saveTypePara = new ObjectParameter("SaveType", saveType);
            var memberIdPara = new ObjectParameter("MemberId", memberId);
            var paraValuePara = new ObjectParameter("ParaValue", paraValue);
            var resultPara = new ObjectParameter("Result", result);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreditHistory_Save_p", saveTypePara, memberIdPara, paraValuePara, resultPara);
            return (Byte)resultPara.Value;
        }
    }
}
