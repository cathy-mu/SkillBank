﻿using System;
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
        MemberInfo GetMemberInfo(int memberId, int relatedMemberId = 0);
        MemberInfo GetMemberInfo(String openId);
        MemberInfo GetMemberInfo(String socialAccount, Byte socialType);
        List<MemberInfo> GetMemberInfos(Byte loadType, String searchKey);

        Dictionary<String, int> GetNumsByMember(int memberId);
        Dictionary<String, int> GetNumsByMemberClass(int memberId, int classId);

        int CreateMember(out int memberId, String socialOpenId, Byte socialType, String memberName, String email, String avatar = "", string mobile = "", string code = "", String etag = "");
        Boolean UpdateMemberInfo(Byte saveType, MemberInfo memberInfo);
        void SaveEmailAccount(String name, String email);
        Boolean CoinUpdate(Byte updateType, int memberId, int classId, int coinsToAdd);//admin tool
        void AddMembersCoin(int memberId, int coinsToAdd, Byte addType);
        //void AddShareClassCoin(int memberId);
        bool HasShareClassCoin(int memberId);

        Byte SendMobileVerifyCode(int memberId, String mobile, Boolean sendSMS);
        Byte VerifyMobile(int memberId, String mobile, String verifyCode);
        Byte CheckIsMobileVerified(int memberId);

        void UpdateMemberLikeTag(int memberId, int relatedId, bool isLike);
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
        public void UpdateMemberLikeTag(int memberId, int relatedId, bool isLike)
        {
            _intRep.UpdateLike((Byte)Enums.DBAccess.FavoriteSaveType.SaveFavoriteTag, (Byte)Enums.FavoriteType.LikeMember, memberId, relatedId, isLike);
        }

        public Byte SendMobileVerifyCode(int memberId, String mobile, Boolean sendSMS = true)
        {
            Random rd = new Random();
            String code = rd.Next(0, 99999).ToString().PadLeft(6,'0');

            Byte saveType = memberId.Equals(0) ? (Byte)Enums.DBAccess.MobileVerificationSaveType.NewMember : (Byte)Enums.DBAccess.MobileVerificationSaveType.OldMember;
            var result = _repository.UpdateVerification(saveType, memberId, mobile, code);
            if (sendSMS)// && result != 0
            {
                YunPianSMS.SendMobileValidationCodeSms(mobile, code);
            }
            return result;
        }

        public Byte CheckIsMobileVerified(int memberId)
        {
            Byte saveType = (Byte)Enums.DBAccess.MobileVerificationSaveType.CheckIsVerified;
            var result = _repository.UpdateVerification(saveType, memberId, "", "");
            return result;
        }
        
        public Byte VerifyMobile(int memberId, String mobile, String verifyCode)
        {
            Byte saveType = (Byte)Enums.DBAccess.MobileVerificationSaveType.Verify;
            var result = _repository.UpdateVerification(saveType, memberId, mobile, verifyCode);
            return result;
        }

        public MemberInfo GetMemberInfo(int memberId, int relatedMemberId = 0)
        {
            Byte loadType = relatedMemberId.Equals(0) ? (Byte)Enums.DBAccess.MemberLoadType.ByMemberId : (Byte)Enums.DBAccess.MemberLoadType.ByMemberIdAndRelatedMemberId;
            var result = _repository.GetMemberInfo(loadType, "", 0, memberId, relatedMemberId);
            return result;
        }

        public MemberInfo GetMemberInfo(String openId)
        {
            Byte loadType = (Byte)Enums.DBAccess.MemberLoadType.ByOpenId;
            var result = _repository.GetMemberInfo(loadType, openId, 0, 0);
            return result;
        }

        public MemberInfo GetMemberInfo(String socialAccount, Byte socialType)
        {
            Byte loadType = (Byte)Enums.DBAccess.MemberLoadType.BySocialAccount;
            var result = _repository.GetMemberInfo(loadType, socialAccount, socialType, 0);
            return result;
        }

        public List<MemberInfo> GetMemberInfos(Byte loadType,String  searchKey)
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
        /// <returns>1 exists member, 0 new member</returns>
        public int CreateMember(out int memberId, String socialId, Byte socialType, String memberName, String email, String avatar = "", string mobile = "", string code = "", String etag = "")
        {
            int result = _repository.CreateMember(out memberId, socialId, socialType, memberName, email, avatar, mobile, code, etag);
            return result;
        }

        public Boolean UpdateMemberInfo(Byte saveType, MemberInfo memberInfo)
        {
            memberInfo.Phone = (String.IsNullOrEmpty(memberInfo.Phone) ? "" : memberInfo.Phone);
            memberInfo.SelfIntro = (String.IsNullOrEmpty(memberInfo.SelfIntro) ? "" : memberInfo.SelfIntro);
            memberInfo.Email = (String.IsNullOrEmpty(memberInfo.Email) ? "" : memberInfo.Email);
            memberInfo.CityId = (memberInfo.CityId.Equals(null) ? 0 : memberInfo.CityId);
            memberInfo.Gender = (memberInfo.Gender.Equals(null) ? true : memberInfo.Gender);
            memberInfo.Name = (String.IsNullOrEmpty(memberInfo.Name) ? "" : memberInfo.Name);
            memberInfo.PosX = (memberInfo.PosX.Equals(null) ? 0 : memberInfo.PosX);
            memberInfo.PosY = (memberInfo.PosY.Equals(null) ? 0 : memberInfo.PosY);
            memberInfo.BirthDate = ((memberInfo.BirthDate.Equals(null) || memberInfo.BirthDate.Year<1900) ? new DateTime(1900, 01, 01) : memberInfo.BirthDate);
            memberInfo.Avatar = (String.IsNullOrEmpty(memberInfo.Avatar) ? "" : memberInfo.Avatar); 

            var result = _repository.UpdateMemberInfo(memberInfo.MemberId, saveType, memberInfo.Phone, memberInfo.CityId, memberInfo.Name, memberInfo.SelfIntro, memberInfo.Gender, memberInfo.Email, memberInfo.PosX, memberInfo.PosY, memberInfo.BirthDate,memberInfo.Avatar);
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

        public Dictionary<String, int> GetNumsByMember(int memberId)
        {
            return _repository.GetMemberNums(memberId);
        }

        public Dictionary<String, int> GetNumsByMemberClass(int memberId, int classId)
        {
            return _repository.GetMemberNums(memberId, classId);
        }

        public void AddMembersCoin(int memberId, int coinsToAdd, Byte addType)
        {
            _repository.AddMembersCoin(memberId, coinsToAdd, addType);
        }

        //public void AddShareClassCoin(int memberId)
        //{
        //    _repository.AddMembersCoin(memberId, 3, (Byte)Enums.DBAccess.CoinUpdateType.AddClassShareCoin);
        //}
                
        public bool HasShareClassCoin(int memberId)
        {
            return _repository.HasShareClassCoin(memberId);
        }



    }

}