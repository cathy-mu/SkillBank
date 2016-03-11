using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.Common;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.Net.SMS;

namespace SkillBank.Site.Services.Managers
{
    public interface IMemberManager
    {
        MemberInfo GetMemberInfo(int memberId);
        MemberInfo GetMemberInfo(Byte loadType, int memberId, int relatedMemberId = 0);
        MemberInfo GetMemberInfo(Byte loadType, String socialAccount, Byte socialType, String para = "");
        List<MemberInfo> GetMemberInfos(Byte loadType, String searchKey);

        Dictionary<Enum, int> GetNumsByMember(int memberId, Byte loadBy);
        Dictionary<Enum, int> GetNumsByMemberClass(int memberId, int classId, Byte loadBy = (Byte)Enums.DBAccess.MemberNumsLoadType.ByClassId);

        int CreateMember(out int memberId, ref Byte verifyTag, ref String accessToken, ref String rcToken, String socialOpenId, Byte socialType, String memberName, String email, String avatar = "", string mobile = "", string code = "", String pass = "", String etag = "", Boolean gender = true, String device = "", String unionId = "");
        void UpdateMemberContactInfo(int memberId, String name, String phone, String email = "");
        Byte UpdateMemberInfo(Byte saveType, MemberInfo memberInfo);
        
        void SaveEmailAccount(String name, String email);
        Boolean CoinUpdate(Byte updateType, int memberId, int classId, int coinsToAdd);//admin tool
        Boolean AddMembersCoin(int memberId, int coinsToAdd, Byte addType);
        bool HasShareClassCoin(int memberId);

        Byte SendMobileVerifyCode(Byte type, int memberId, String mobile, Boolean sendSMS);
        Byte VerifyMobile(int memberId, String mobile, String verifyCode);
        Byte CheckIsMobileVerified(int memberId);
        Byte UpdateVerification(Byte saveType, int memberId, String verifyAccount);

        void UpdateMemberLikeTag(int memberId, int relatedId, bool isLike, out String deviceToken);
        List<FavoriteItem> GetFavorites(Byte loadType, int memberId, int paraId);

        Byte SaveWeChatEvent(Byte saveType, int memberId, String openId, String paraId, String paraValue);
        Byte UpdateCredit(Byte saveType, int memberId, int paraValue);
    }

    public class MemberManager : IMemberManager
    {
        private readonly IMemberRepository _repository;
        private readonly IInteractiveRepository _intRep;

        public MemberManager(IMemberRepository repository, IInteractiveRepository intRep)
        {
            _repository = repository;
            _intRep = intRep;
        }

        public void UpdateMemberLikeTag(int memberId, int relatedId, bool isLike, out String deviceToken)
        {
            _intRep.UpdateLike((Byte)Enums.DBAccess.FavoriteSaveType.SaveFavoriteTag, (Byte)Enums.FavoriteType.LikeMember, memberId, relatedId, isLike, out deviceToken);
        }

        public Byte UpdateCredit(Byte saveType, int memberId, int paraValue)
        {
            return _repository.UpdateCredit(saveType, memberId, paraValue);
        }


        /// <summary>
        /// Generate mobile verify code and send SMS
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="mobile"></param>
        /// <param name="sendSMS"></param>
        /// <returns>0 号码被验证   1 发送成功   2格式错误   3微博账号存在   4微信账号存在</returns>
        public Byte SendMobileVerifyCode(Byte saveType, int memberId, String mobile, Boolean sendSMS = true)
        {
            mobile = mobile.Trim();
            var isMobileValid = System.Text.RegularExpressions.Regex.IsMatch(mobile, Constants.ValidationExpressions.Mobile);
            if (!String.IsNullOrEmpty(mobile) && isMobileValid)
            {
                Random rd = new Random();
                String code = rd.Next(0, 999999).ToString().PadLeft(6, '0');

                var result = _repository.UpdateVerification(saveType, memberId, mobile, code);

                //号码未被占用
                if (sendSMS && result.Equals(1))
                {
                    YunPianSMS.SendMobileValidationCodeSms(mobile, code);
                }
                return result;
            }
            else
            {
                return 2;//invalid mobile
            }
        }

        public Byte CheckIsMobileVerified(int memberId)
        {
            Byte saveType = (Byte)Enums.DBAccess.MobileVerificationSaveType.CheckIsVerified;
            var result = _repository.UpdateVerification(saveType, memberId, "", "");
            return result;
        }

        public Byte VerifyMobile(int memberId, String mobile, String verifyCode)
        {
            mobile = mobile.Trim();
            var isMobileValid = System.Text.RegularExpressions.Regex.IsMatch(mobile, Constants.ValidationExpressions.Mobile);
            verifyCode = verifyCode.Trim();
            var isCodeValid = System.Text.RegularExpressions.Regex.IsMatch(verifyCode, Constants.ValidationExpressions.ValidationCode);

            if (isMobileValid && isCodeValid)
            {
                Byte saveType = (Byte)Enums.DBAccess.MobileVerificationSaveType.Verify;
                var result = _repository.UpdateVerification(saveType, memberId, mobile, verifyCode);
                return result;
            }
            else
            {
                return 2;
            }
        }

        public Byte UpdateVerification(Byte saveType, int memberId, String verifyAccount)
        {
            var result = _repository.UpdateVerification(saveType, memberId, verifyAccount, "");
            return result;
        }

        public MemberInfo GetMemberInfo(int memberId)
        {
            var result = _repository.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberId, memberId, 0);
            return result;
        }

        public MemberInfo GetMemberInfo(Byte loadType, int memberId, int relatedMemberId = 0)
        {
            var result = _repository.GetMemberInfo(loadType, memberId, relatedMemberId);
            return result;
        }

        public MemberInfo GetMemberInfo(Byte loadType, String socialAccount, Byte socialType, String para = "")
        {
            var result = _repository.GetMemberInfo(loadType, socialAccount, socialType, para);
            return result;
        }

        public List<MemberInfo> GetMemberInfos(Byte loadType, String searchKey)
        {
            var result = _repository.GetMemberInfos(loadType, searchKey);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="socialOpenId"></param>
        /// <param name="socialType"></param>
        /// <param name="memberName"></param>
        /// <param name="email"></param>
        /// <returns>1 new member, 0 exists member</returns>
        public int CreateMember(out int memberId, ref Byte verifyTag, ref String accessToken, ref String rcToken, String socialOpenId, Byte socialType, String memberName, String email, String avatar = "", string mobile = "", string code = "", String pass = "", String etag = "", Boolean gender = true, String device = "", String unionId = "")
        {
            int result = _repository.CreateMember(out memberId, ref verifyTag, ref accessToken, ref rcToken, socialOpenId, socialType, memberName, email, avatar, mobile, code, pass, etag, gender, device, unionId);
            return result;
        }

        //update member info when book class or accept order
        public void UpdateMemberContactInfo(int memberId, String name, String phone, String email = "")
        {
            if (memberId > 0)
            {
                MemberInfo memberInfo = new MemberInfo();
                memberInfo.MemberId = memberId;
                if (!String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(name))
                {
                    memberInfo.Name = name;
                    memberInfo.Phone = phone;
                    memberInfo.Email = email;
                    UpdateMemberInfo((Byte)Enums.DBAccess.MemberSaveType.UpdateContactInfo, memberInfo);
                }
                else if (!String.IsNullOrEmpty(name))
                {
                    memberInfo.Name = name;
                    UpdateMemberInfo((Byte)Enums.DBAccess.MemberSaveType.UpdateName, memberInfo);
                }
            }
        }

        public Byte UpdateMemberInfo(Byte saveType, MemberInfo memberInfo)
        {
            memberInfo.Phone = (String.IsNullOrEmpty(memberInfo.Phone) ? "" : memberInfo.Phone);
            memberInfo.SelfIntro = (String.IsNullOrEmpty(memberInfo.SelfIntro) ? "" : memberInfo.SelfIntro);
            memberInfo.Email = (String.IsNullOrEmpty(memberInfo.Email) ? "" : memberInfo.Email);
            memberInfo.CityId = (memberInfo.CityId.Equals(null) ? 0 : memberInfo.CityId);
            memberInfo.Gender = (memberInfo.Gender.Equals(null) ? true : memberInfo.Gender);
            memberInfo.Name = (String.IsNullOrEmpty(memberInfo.Name) ? "" : memberInfo.Name);
            memberInfo.PosX = (memberInfo.PosX.Equals(null) ? 0 : memberInfo.PosX);
            memberInfo.PosY = (memberInfo.PosY.Equals(null) ? 0 : memberInfo.PosY);
            memberInfo.BirthDate = ((memberInfo.BirthDate.Equals(null) || memberInfo.BirthDate.Year < 1900) ? new DateTime(1900, 01, 01) : memberInfo.BirthDate);
            memberInfo.Avatar = (String.IsNullOrEmpty(memberInfo.Avatar) ? "" : memberInfo.Avatar);
            var result = _repository.UpdateMemberInfo(memberInfo.MemberId, saveType, memberInfo.Phone, memberInfo.CityId, memberInfo.Name, memberInfo.SelfIntro, memberInfo.Gender, memberInfo.Email, memberInfo.PosX, memberInfo.PosY, memberInfo.BirthDate, memberInfo.Avatar);
            return (result);
        }

        public void SaveEmailAccount(String name, String email)
        {
            _repository.LeaveEmailAddress(name, email);
        }

        public Boolean CoinUpdate(Byte updateType, int memberId, int classId, int coinsToAdd)
        {
            return _repository.CoinUpdate(updateType, memberId, classId, coinsToAdd);
        }

        public Dictionary<Enum, int> GetNumsByMember(int memberId, Byte loadBy)
        {
            return _repository.GetMemberNums(memberId, 0, loadBy);
        }

        public Dictionary<Enum, int> GetNumsByMemberClass(int memberId, int classId, Byte loadBy = (Byte)Enums.DBAccess.MemberNumsLoadType.ByClassId)
        {
            return _repository.GetMemberNums(memberId, classId, loadBy);
        }

        public Boolean AddMembersCoin(int memberId, int coinsToAdd, Byte addType)
        {
            return _repository.AddMembersCoin(memberId, coinsToAdd, addType);
        }

        public bool HasShareClassCoin(int memberId)
        {
            return _repository.HasShareClassCoin(memberId);
        }

        public List<FavoriteItem> GetFavorites(Byte loadType, int memberId, int paraId)
        {
            return _intRep.GetFavorites(loadType, memberId, paraId);
        }

        public Byte SaveWeChatEvent(Byte saveType, int memberId, String openId, String paraId, String paraValue)
        {
            return _repository.SaveWeChatEvent(saveType, memberId, openId, paraId, paraValue);
        }


    }

}
