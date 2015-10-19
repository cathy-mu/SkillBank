using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.CacheProviders;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Services
{
    public interface ICommonService
    {
        //Member
        List<MemberInfo> GetMemberInfos(Byte loadBy, String searchKey);
        MemberInfo GetMemberInfo(int memberId);
        MemberInfo GetMemberInfo(Byte loadType, int memberId, int relatedMemberId = 0);
        MemberInfo GetMemberInfo(String socialAccount, Byte socialType, String para = "");
        int CreateMember(out int memberId, String account, Byte socialType, String memberName, String email, String avatar, string mobile = "", string code = "", String pass = "", string etag = "", Boolean isMale = true);
        Byte UpdateMemberInfo(int memberId, Byte saveType, String saveValue, String saveValue2 = "");
        Byte UpdateMemberProfile(MemberInfo memberInfo, Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateProfile);
        void SaveEmailAccount(String name, String email);
        Boolean CoinUpdate(Byte updateType, int memberId, int classId, int coinsToAdd);//tool
        Dictionary<Enum, int> GetNumsByMember(int memberId, Byte loadBy = (Byte)Enums.DBAccess.MemberNumsLoadType.ByMemberId);
        Dictionary<Enum, int> GetNumsByMemberClass(int memberId, int classId, Byte loadBy = (Byte)Enums.DBAccess.MemberNumsLoadType.ByClassId);
        Boolean AddMembersCoin(int memberId, int coinsToAdd, Byte addType);
        Boolean HasShareClassCoin(int memberId);
        Byte SendMobileVerifyCode(Byte type, int memberId, String mobile, Boolean sendSMS = true);
        Byte VerifyMobile(int memberId, String mobile, String verifyCode);
        //[Obsolete]
        //Byte CheckIsMobileVerified(int memberId);
        Byte UpdateVerification(Byte saveType, int memberId, String verifyAccount);
        void UpdateMemberLikeTag(int memberId, int relatedId, Boolean isLike);
        List<FavoriteItem> GetFavorites(Byte loadType, int memberId, int paraId);
        Byte SaveWeChatEvent(Byte saveType, int memberId, String openId, String paraId, String paraValue);
        Byte UpdateCredit(Byte saveType, int memberId, int paraValue);

        //Class
        int CreateClass(int memberId, int categoryId, Byte teacheLevel, Byte skillLevel, out Boolean isExist);
        int UpdateClassEditInfo(Byte savaType, ClassInfo classInfo);
        Boolean UpdateClassEditInfo(Byte updateType, int classId, Byte paraValue, Boolean isValue = true);
        Boolean UpdateClassEditInfo(Byte updateType, int classId, Boolean paraValue);
        Boolean UpdateClassEditInfo(Byte updateType, int classId, String paraValue);
        List<ClassEditItem> GetClassEditInfoByMemberId(int memberId, Byte loadType);

        //List<ClassItem> SearchClass(int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, out int resultNum, out int pageNum, Byte OrderByType = (Byte)ClientSetting.ClassListOrderType.ByDisctince, Boolean isAsc = false, Decimal posX = 0, Decimal posY = 0, String searchKey = "");
        List<ClassListItem> GetClassTabList(Byte loadBy, Byte typeId, int memberId, String searchKey = "", Decimal posX = 0, Decimal posY = 0, int cityId = 0);
        List<ClassListItem> GetClassList(Byte loadBy, Byte OrderByType, int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, int memberId, out int totalNum, String searchKey = "", Decimal posX = 0, Decimal posY = 0);
        //un-cached class list
        List<ClassInfo_LoadByPage_p_Result> GetClassPagingList(Byte categoryId, int cityId, Decimal posX, Decimal posY, int memberId, int minId, int maxId, Byte orderBy);
        List<ClassInfo_LoadByKey_p_Result> GetClassSearchList(String searchKey, int cityId, Decimal posX, Decimal posY, int minId, int maxId, Byte orderBy);
        
        List<ClassListItem> GetRecommendationClassList(Byte typeId, int pageSize, int pageId, int memberId, out int totalNum);
        List<ClassListItem> GetRecommendationClassPopList(int memberId);
        //cached class list
        List<ClassListItem> GetCachedRecommendationClassList(Byte typeId, int pageSize, int pageId, int memberId, out int totalNum);
        List<ClassListItem> GetClassList(Byte loadType, int pageId, int pageSize, out int classNum);
        ClassEditItem GetClassItem(int id);

        ClassInfo GetClassInfoByClassId(int paraId);
        ClassEditItem GetClassInfoItem(Byte loadType, int classId, int memberId = 0);
        List<ClassEditItem> GetClassEditInfoForAdmin(Boolean isRejected);
        List<ClassInfo> GetClassInfo(Byte loadType, int paraId, int memberId = 0);
        List<ClassInfo> GetClassInfoForAdmin(Boolean isRejected);

        int AddComment(int memberId, int classId, String commentText);
        void RemoveComment(int memberId, int commentId);
        void UpdateClassLikeTag(int memberId, int classId, Boolean isLike);
        List<ClassCollection_Load_p_Result> GetClassCollection(Byte loadBy, int memberId, int paraId = 0);
        
        //Review
        List<TeacherReviewItem> GetTeacherReviewsByMemberId(int memberId, int maxReviewId = 0);
        //List<StudentReviewItem> GetStudentReviewsByMemberId(int memberId, int minReviewId = 0);
        Boolean AddStudentReview(int orderId, int classId, Byte feedBack, String comment, String privateComment);
        Boolean AddTeacherReview(int orderId, Byte feedBack, String comment, String privateComment);
        List<MemberReviewItem> GetClassReviews(Byte loadBy, int memberId, int classId, Byte feedback = 0, int maxReviewId = 0);
        List<MemberReviewItem> GetMemberReviews(Byte loadBy, int memberId, Byte feedback = 0, int maxReviewId = 0);

        //Order
        //Byte UpdateOrderStatus(int orderId, Byte orderStatus);
        Byte UpdateOrderStatus(int orderId, Byte orderStatus, int studentId = 0, int teacherId = 0);
        Byte UpdateOrderStatusWithCoins(int orderId, Byte orderStatus, int studentId, int teacherId = 0);
        Boolean AddOrder(int studentId, int classId, DateTime bookDate, String remark, String studentName = "", String studentPhone = "", String studentEmail = "");
        Byte UpdateOrderDate(int orderId, DateTime bookDate);
        Byte AcceptOrder(int orderId, int studentId, int teacherId, String name = "", String phone = "", String email = "");
        List<OrderItem> GetOrderListByStudent(int studentId, Boolean shouldCheck);
        List<OrderItem> GetOrderListByTeacher(int teacherId, Boolean shouldCheck);
        void HandleMemberOrder(int memberId);

        //Message
        List<ComplaintItem> GetComplaintList(Byte loadType);
        Byte UpdateComplaint(Byte saveType, int memberId, int relatedId, Byte type);
        int AddMessage(int fromId, int toId, String messageText);
        Boolean SetMessageAsRead(int maxId, int memberId, int contactorId);
        Boolean SetMessageAsDeleted(int maxId, int memberId, int contactorId);

        List<MessageListItem> GetMessageList(int memberId, int pageSize = 0, Byte loadBy = 1);
        List<Message> GetMessageDetail(int memberId, int contactId, Byte loadBy = 1);
        Dictionary<int, int> GetMessageUnReadNum(int memberId);

        //Notification
        List<NotificationItem> GetNotification(int memberId, Byte loadType);
        List<NotificationAlertItem> GetPopNotification(int memberId, Byte loadType);
        void UpdateNotification(Byte saveType, int memberId, int paraId = 0);
        void SendOrderUpdateSMS(Byte statusType, String mobile, String className, Boolean sendSMS = true);
        void SendClassProveSMS(Boolean isProve, String mobile, String className, String link, Boolean sendSMS = true);
        void SendNewMessageSMS(String mobile, String name, String link, Boolean sendSMS = true);

        // report and tools
        List<ReportNumItem> GetReportClassMemberNum();
        List<ReportOrderStatus_Load_p_Result> GetReportClassMemberNum(Byte loadBy, DateTime beginDate, DateTime endDate);
        List<RecommendationItem> GetRecommendation(int classId);
        void SaveMasterMember(int memberId, String paraStr, char split);
        void SaveRecommendationClass(int classId, String paraStr, char split);
    }

    public class CommonService : ICommonService
    {
        private readonly IMemberManager _memberMgr;
        private readonly IClassManager _classMgr;
        private readonly IOrderManager _orderMgr;
        private readonly IFeedBackManager _feedbackMgr;
        private readonly IMessageManager _messageMgr;
        private readonly INotificationManager _notificationMgr;
        private readonly IReportToolsManager _repMgr;
        private readonly ICacheContentManager _cacheMgr;

        #region Constructors

        public CommonService(IMemberManager memberMgr, IClassManager classManager, IOrderManager orderMgr, IFeedBackManager feedbackMgr, IMessageManager messageMgr, INotificationManager notificationMgr, IReportToolsManager repMgr, ICacheContentManager cacheMgr)
        {
            this._memberMgr = memberMgr;
            this._classMgr = classManager;
            this._feedbackMgr = feedbackMgr;
            this._orderMgr = orderMgr;
            this._messageMgr = messageMgr;
            this._notificationMgr = notificationMgr;
            this._repMgr = repMgr;
            this._cacheMgr = cacheMgr;
        }

        #endregion


        #region Notification Management


        public List<NotificationItem> GetNotification(int memberId, Byte loadType)
        {
            return _notificationMgr.GetNotification(loadType, memberId);
        }

        public List<NotificationAlertItem> GetPopNotification(int memberId, Byte loadType)
        {
            return _notificationMgr.GetPopNotification(memberId, loadType);
        }

        public void UpdateNotification(Byte saveType, int memberId, int paraId = 0)
        {
            _notificationMgr.UpdateNotification(saveType, memberId, paraId);
        }

        public void SendOrderUpdateSMS(Byte statusType, String mobile, String className, Boolean sendSMS = true)
        {
            _notificationMgr.SendOrderUpdateSMS(statusType, mobile, className, sendSMS);
        }

        public void SendClassProveSMS(Boolean isProve, String mobile, String className, String link, Boolean sendSMS = true)
        {
            _notificationMgr.SendClassProveSMS(isProve, mobile, className, link, sendSMS);
        }

        public void SendNewMessageSMS(String mobile, String name, String link, Boolean sendSMS = true)
        {
            _notificationMgr.SendNewMessageSMS(mobile, name, link, sendSMS);
        }

        #endregion

        #region Member Management

        public void UpdateMemberLikeTag(int memberId, int relatedId, Boolean isLike)
        {
            _memberMgr.UpdateMemberLikeTag(memberId, relatedId, isLike);
        }

        public Byte CheckIsMobileVerified(int memberId)
        {
            return _memberMgr.CheckIsMobileVerified(memberId);
        }

        public Byte UpdateVerification(Byte saveType, int memberId, String verifyAccount)
        {
            return _memberMgr.UpdateVerification(saveType, memberId, verifyAccount);
        }

        public Byte VerifyMobile(int memberId, String mobile, String verifyCode)
        {
            return _memberMgr.VerifyMobile(memberId, mobile, verifyCode);
        }

        public Byte SendMobileVerifyCode(Byte type, int memberId, String mobile, Boolean sendSMS = true)
        {
            return _memberMgr.SendMobileVerifyCode(type, memberId, mobile, sendSMS);
        }

        public Boolean CoinUpdate(Byte updateType, int memberId, int classId, int coinsToAdd)//tool
        {
            return _memberMgr.CoinUpdate(updateType, memberId, classId, coinsToAdd);
        }

        public void SaveEmailAccount(String name, String email)
        {
            _memberMgr.SaveEmailAccount(name, email);
        }


        public List<MemberInfo> GetMemberInfos(Byte loadBy, String searchKey)
        {
            return this._memberMgr.GetMemberInfos(loadBy, searchKey);
        }

        public MemberInfo GetMemberInfo(String socialAccount, Byte socialType, String para = "")
        {
            return this._memberMgr.GetMemberInfo(socialAccount, socialType, para);
        }
        
        public MemberInfo GetMemberInfo(Byte loadType, int memberId, int relatedMemberId = 0)
        {
            return this._memberMgr.GetMemberInfo(loadType, memberId, relatedMemberId);
        }

        public MemberInfo GetMemberInfo(int memberId)
        {
            return this._memberMgr.GetMemberInfo(memberId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="socialOpenId"></param>
        /// <param name="socialType"></param>
        /// <param name="memberName"></param>
        /// <param name="email"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public int CreateMember(out int memberId, String account, Byte socialType, String memberName, String email, String avatar, string mobile = "", string code = "", String pass = "", String etag = "", Boolean gender = true)
        {
            return this._memberMgr.CreateMember(out memberId, account, socialType, memberName, email, avatar, mobile, code, pass, etag, gender);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public Byte UpdateMemberInfo(int memberId, Byte saveType, String saveValue, String saveValue2 = "")
        {
            Byte result = 0;
            Boolean isValid = false;
            int tempValue;

            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = memberId;
            switch ((Enums.DBAccess.MemberSaveType)saveType)
            {
                case Enums.DBAccess.MemberSaveType.UpdateCity:
                    if (int.TryParse(saveValue, out tempValue))
                    {
                        isValid = true;
                        memberInfo.CityId = tempValue;
                        memberInfo.Gender = !String.IsNullOrEmpty(saveValue2);
                    }
                    break;
                case Enums.DBAccess.MemberSaveType.UpdateEmail:
                    isValid = true;
                    memberInfo.Email = saveValue;
                    break;
                case Enums.DBAccess.MemberSaveType.UpdateGender:
                    isValid = true;
                    memberInfo.Gender = saveValue.Equals("1");
                    break;
                case Enums.DBAccess.MemberSaveType.UpdateName:
                    isValid = true;
                    memberInfo.Name = saveValue;
                    break;
                case Enums.DBAccess.MemberSaveType.UpdatePhone:
                    isValid = true;
                    memberInfo.Phone = saveValue;
                    break;
                case Enums.DBAccess.MemberSaveType.UpdatePosition:
                    var position = saveValue.Split(Constants.Setting.DBDataDelimiter);
                    if (position.Length == 2)
                    {
                        Decimal posX, posY;
                        if (Decimal.TryParse(position[0], out posX) && Decimal.TryParse(position[1], out posY))
                        {
                            isValid = true;
                            memberInfo.PosX = posX;
                            memberInfo.PosY = posY;
                            memberInfo.Avatar = saveValue2;
                        }
                    }
                    break;
                case Enums.DBAccess.MemberSaveType.UpdateIntro:
                    isValid = true;
                    memberInfo.SelfIntro = saveValue;
                    break;

                case Enums.DBAccess.MemberSaveType.UpdatePhoto:
                    isValid = true;
                    memberInfo.Avatar = saveValue;
                    break;

                default:
                    break;
            }
            if (isValid)
            {
                result = this._memberMgr.UpdateMemberInfo(saveType, memberInfo);
            }

            return result;
        }

        public Byte UpdateMemberProfile(MemberInfo memberInfo, Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateProfile)
        {
            return this._memberMgr.UpdateMemberInfo(saveType, memberInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public Dictionary<Enum, int> GetNumsByMember(int memberId, Byte loadBy = (Byte)Enums.DBAccess.MemberNumsLoadType.ByMemberId)
        {
            return this._memberMgr.GetNumsByMember(memberId, loadBy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public Dictionary<Enum, int> GetNumsByMemberClass(int memberId, int classId, Byte loadBy = (Byte)Enums.DBAccess.MemberNumsLoadType.ByClassId)
        {
            return this._memberMgr.GetNumsByMemberClass(memberId, classId, loadBy);
        }

        public Boolean AddMembersCoin(int memberId, int coinsToAdd, Byte addType)
        {
            return _memberMgr.AddMembersCoin(memberId, coinsToAdd, addType);
        }

        public Boolean HasShareClassCoin(int memberId)
        {
            return _memberMgr.HasShareClassCoin(memberId);
        }

        public List<FavoriteItem> GetFavorites(Byte loadType, int memberId, int paraId)
        {
            return _memberMgr.GetFavorites(loadType, memberId, paraId);
        }

        public Byte SaveWeChatEvent(Byte saveType, int memberId, String openId, String paraId, String paraValue)
        {
            return _memberMgr.SaveWeChatEvent(saveType, memberId, openId, paraId, paraValue);
        }

        public Byte UpdateCredit(Byte saveType, int memberId, int paraValue)
        {
            return _memberMgr.UpdateCredit(saveType, memberId, paraValue);
        }


        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public Boolean UpdateMemberInfo(int memberId, Enums.DBAccess.MemberSaveType saveType, String saveValue)
        {
            Boolean result = false;
            Boolean isValid = false;
            int tempValue;

            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = memberId;
            switch (saveType)
            {
                case Enums.DBAccess.MemberSaveType.UpdateCity:
                    if (int.TryParse(saveValue, out tempValue))
                    {
                        isValid = true;
                        memberInfo.CityId = tempValue;
                    }
                    break;
                case Enums.DBAccess.MemberSaveType.UpdateEmail:
                    isValid = true;
                    memberInfo.Email = saveValue;
                    break;
                case Enums.DBAccess.MemberSaveType.UpdateGender:
                    isValid = true;
                    memberInfo.Gender = saveValue.Equals("1");
                    break;
                case Enums.DBAccess.MemberSaveType.UpdateName:
                    isValid = true;
                    memberInfo.Name = saveValue;
                    break;
                case Enums.DBAccess.MemberSaveType.UpdatePhone:
                    isValid = true;
                    memberInfo.Phone = saveValue;
                    break;
                case Enums.DBAccess.MemberSaveType.UpdatePosition:
                    var position = saveValue.Split(',');
                    if (position.Length == 2)
                    {
                        Decimal posX, posY;
                        if (Decimal.TryParse(position[0], out posX) && Decimal.TryParse(position[1], out posY))
                        {
                            isValid = true;
                            memberInfo.PosX = posX;
                            memberInfo.PosY = posY;
                            memberInfo.Avatar = saveValue2;
                        }
                    }
                    break;
                case Enums.DBAccess.MemberSaveType.UpdatePhoto:
                    //TO DO:update later
                    break;

                default:
                    break;
            }
            if (isValid)
            {
                result = this._memberMgr.UpdateMemberInfo((Byte)saveType, memberInfo);
            }

            return result;
        }
        */
        #endregion

        #region Class Management
        public List<ClassCollection_Load_p_Result> GetClassCollection(Byte loadBy, int memberId, int paraId = 0)
        {
            return _classMgr.GetClassCollection(loadBy, memberId, paraId);
        }

        public int AddComment(int memberId, int classId, String commentText)
        {
            return _classMgr.AddComment(memberId, classId, commentText);
        }

        public void RemoveComment(int memberId, int commentId)
        {
            _classMgr.RemoveComment(memberId, commentId);
        }

        public void UpdateClassLikeTag(int memberId, int classId, Boolean isLike)
        {
            _classMgr.UpdateClassLikeTag(memberId, classId, isLike);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classInfo"></param>
        /// <returns></returns>
        public int SaveClassInfo(ClassInfo classInfo)
        {
            int classId = 0;

            return classId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="memberId"></param>
        /// <param name="teacheLevel"></param>
        /// <param name="skillLevel"></param>
        /// <param name="isExist"></param>
        /// <returns></returns>
        public int CreateClass(int memberId, int categoryId, Byte skillLevel, Byte teacheLevel, out Boolean isExist)
        {
            return _classMgr.CreateClass(memberId, categoryId, skillLevel, teacheLevel, out isExist);
        }

        public Boolean UpdateClassEditInfo(Byte updateType, int classId, Byte paraValue, Boolean isValue = true)
        {
            return _classMgr.UpdateClassEditInfo(updateType, classId, paraValue, isValue);
        }

        public Boolean UpdateClassEditInfo(Byte updateType, int classId, Boolean paraValue)
        {
            return _classMgr.UpdateClassEditInfo(updateType, classId, paraValue);
        }

        public Boolean UpdateClassEditInfo(Byte updateType, int classId, String paraValue)
        {
            return _classMgr.UpdateClassEditInfo(updateType, classId, paraValue);
        }

        public int UpdateClassEditInfo(Byte savaType, ClassInfo classInfo)
        {
            if (classInfo.Level < 0 || classInfo.Level > 3 || classInfo.SkillLevel < 0 || classInfo.SkillLevel > 100 || classInfo.TeacheLevel < 0 || classInfo.TeacheLevel > 100)
            {
                return -1;
            }
            return _classMgr.UpdateClassEditInfo(savaType, classInfo);
        }

        public List<ClassEditItem> GetClassEditInfoForAdmin(Boolean isRejected)
        {
            return _classMgr.GetClassEditInfoForAdmin(isRejected);
        }

        public List<ClassEditItem> GetClassEditInfoByMemberId(int memberId, Byte loadType)
        {
            return _classMgr.GetClassEditInfoByMemberId(memberId, loadType);
        }

        public ClassEditItem GetClassInfoItem(Byte loadType, int classId, int memberId)
        {
            return _classMgr.GetClassInfoItem(loadType, classId, memberId);
        }

        public List<ClassListItem> GetClassTabList(Byte loadBy, Byte typeId, int memberId, String searchKey = "", Decimal posX = 0, Decimal posY = 0, int cityId = 0)
        {
            return _classMgr.GetClassTabList(loadBy, typeId, memberId, searchKey, posX, posY, cityId);
        }

        public List<ClassListItem> GetClassList(Byte loadBy, Byte orderByType, int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, int memberId, out int totalNum, String searchKey = "", Decimal posX = 0, Decimal posY = 0)
        {
            return _classMgr.GetClassList(loadBy, orderByType, cityId, categoryId, isParentCate, pageSize, pageId, memberId, out totalNum, searchKey, posX, posY);
        }

        public List<ClassInfo_LoadByPage_p_Result> GetClassPagingList(Byte categoryId, int cityId, Decimal posX, Decimal posY, int memberId, int minId, int maxId, Byte orderBy)
        {
            return _classMgr.GetClassPagingList(categoryId, cityId, posX, posY, memberId, minId, maxId, orderBy);
        }

        public List<ClassInfo_LoadByKey_p_Result> GetClassSearchList(String searchKey, int cityId, Decimal posX, Decimal posY, int minId, int maxId, Byte orderBy)
        {
            return _classMgr.GetClassSearchList( searchKey, cityId, posX, posY, minId, maxId, orderBy);
        }
        
        public List<ClassListItem> GetRecommendationClassList(Byte typeId, int pageSize, int pageId, int memberId, out int totalNum)
        {
            Byte loadBy = (Byte)Enums.DBAccess.ClassListLoadType.ByRecommendation;
            return GetClassList(loadBy, 0, 0/*cityId*/, typeId/*categoryId*/, false, pageSize, pageId, 0, out totalNum, "", 0, 0);
        }

        public List<ClassListItem> GetRecommendationClassPopList(int memberId)
        {
            Byte loadBy = (Byte)Enums.DBAccess.ClassListLoadType.ByRecommendationTop;
            int totalNum = 0;
            return GetClassList(loadBy, 0, 0/*cityId*/, 0/*categoryId*/, false, 0, 0, 0, out totalNum, "", 0, 0);
        }
        
        public ClassEditItem GetClassItem(int id)
        {
            return _cacheMgr.GetClassItem(id);
        }

        //public List<ClassListItem> GetCachedRecommendationClassList(Byte typeId, int pageSize, int pageId, int memberId, out int totalNum)
        //{
        //    String key = String.Concat(Constants.CacheItemKey.RecommendClassListPrefix, typeId.ToString());
        //    //var list = _cacheMgr.GetRecommendClassList(key);
        //    var list = _cacheMgr.GetRecommendClassList(typeId, pageSize, pageId);
        //    totalNum = list.Count();
        //    int minId = pageSize * (pageId - 1);
        //    if (minId < totalNum)
        //    {
        //        var result = list.Skip(minId).Take(pageSize).ToList();
        //        return result;
        //    }
        //    return null;
        //}

        public List<ClassListItem> GetCachedRecommendationClassList(Byte typeId, int pageSize, int pageId, int memberId, out int totalNum)
        {
            totalNum = 0;
            var result = _cacheMgr.GetRecommendClassList(typeId, pageSize, pageId);
            return result;
        }

        public List<ClassListItem> GetClassList(Byte loadType, int pageId, int pageSize, out int classNum)
        {
            return _cacheMgr.GetClassList(loadType, pageId, pageSize, out classNum);
        }

        /////// <summary>
        /////// Get Class for List page
        /////// </summary>
        /////// <param name="cityId"></param>
        /////// <param name="categoryId"></param>
        /////// <param name="memberId"></param>
        /////// <param name="OrderByType"></param>
        /////// <param name="isAsc"></param>
        /////// <returns></returns>
        ////public List<ClassItem> SearchClass(int cityId, Byte categoryId, Boolean isParentCate, int pageSize, int pageId, out int resultNum, out int pagetNum, Byte OrderByType = (Byte)ClientSetting.ClassListOrderType.ByDisctince, Boolean isAsc = false, Decimal posX = 0, Decimal posY = 0, String searchKey = "")
        ////{
        ////    return _classMgr.SearchClass(cityId, categoryId, isParentCate, pageSize, pageId, out resultNum, out pagetNum, OrderByType, isAsc, posX, posY, searchKey);
        ////}

        public ClassInfo GetClassInfoByClassId(int classId)
        {
            return _classMgr.GetClassInfoByClassId(classId);
        }

        public List<ClassInfo> GetClassInfo(Byte loadType, int paraId, int memberId = 0)
        {
            return _classMgr.GetClassInfo(loadType, paraId, memberId);
        }

        public List<ClassInfo> GetClassInfoForAdmin(Boolean isRejected)
        {
            return _classMgr.GetClassInfoForAdmin(isRejected);
        }

        ///// <summary>
        ///// Class Order Number
        ///// </summary>
        ///// <param name="memberId"></param>
        ///// <param name="classId"></param>
        ///// <param name="studentNum"></param>
        ///// <returns></returns>
        //public int GetNumsByClassId(int memberId, int classId, out int studentNum, out int reviewNum, out int otherReviewNum)
        //{
        //     return _classMgr.GetNumsByClass(memberId,classId,out studentNum,out reviewNum, out otherReviewNum);
        //}

        #endregion

        #region Review Management

        //public List<StudentReview> GetStudentReviewsByClass(int classId, int maxReviewId)
        //{
        //    return _feedbackMgr.GetStudentReviewsByClass(classId, maxReviewId);
        //}

        public List<MemberReviewItem> GetClassReviews(Byte loadBy, int memberId, int classId, Byte feedback = 0, int maxReviewId = 0)
        {
            return _feedbackMgr.GetClassReviews(loadBy, memberId, classId, feedback, maxReviewId);
        }

        public List<MemberReviewItem> GetMemberReviews(Byte loadBy, int memberId, Byte feedback = 0, int maxReviewId = 0)
        {
            return _feedbackMgr.GetMemberReviews(loadBy, memberId, feedback, maxReviewId);
        }


        //public List<StudentReviewItem> GetStudentReviewsByMemberId(int memberId, int maxReviewId = 0)
        //{
        //    return _feedbackMgr.GetStudentReviewsByMemberId(memberId, maxReviewId);
        //}

        public List<TeacherReviewItem> GetTeacherReviewsByMemberId(int memberId, int maxReviewId = 0)
        {
            return _feedbackMgr.GetTeacherReviewsByMemberId(memberId, maxReviewId);
        }

        public Boolean AddStudentReview(int orderId, int classId, Byte feedBack, String comment, String privateComment)
        {
            //invalid data
            if (orderId <= 0 || feedBack <= 0 || feedBack > 3)
            {
                return false;
            }

            return _feedbackMgr.AddStudentReview(orderId, classId, feedBack, comment, privateComment);
        }

        public Boolean AddTeacherReview(int orderId, Byte feedBack, String comment, String privateComment)
        {
            //invalid data
            if (orderId <= 0 || feedBack <= 0 || feedBack > 3)
            {
                return false;
            }
            return _feedbackMgr.AddTeacherReview(orderId, feedBack, comment, privateComment);
        }


        #endregion


        #region Order Management

        public void HandleMemberOrder(int memberId)
        {
            _orderMgr.HandleMemberOrder(memberId);
        }

        /// <summary>
        /// Get Order list for member learn page
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<OrderItem> GetOrderListByStudent(int studentId, Boolean shouldCheck)
        {
            return _orderMgr.GetOrderListByStudent(studentId, shouldCheck);
        }

        /// <summary>
        /// Get Order list for member teach page
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public List<OrderItem> GetOrderListByTeacher(int teacherId, Boolean shouldCheck)
        {
            return _orderMgr.GetOrderListByTeacher(teacherId, shouldCheck);
        }

        public Byte UpdateOrderDate(int orderId, DateTime bookDate)
        {
            var result = _orderMgr.UpdateOrderDate(orderId, bookDate);
            return result;
        }
        
        /// <summary>
        /// Add new Order and check if new order id great than 0
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public Boolean AddOrder(int studentId, int classId, DateTime bookDate, String remark, String name = "", String phone = "", String email = "")
        {
            var result = _orderMgr.AddOrder(studentId, classId, bookDate, remark);
            if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(phone))
            {
                MemberInfo studentInfo = new MemberInfo();
                studentInfo.MemberId = studentId;
                if (!String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(name))
                {
                    studentInfo.Name = name;
                    studentInfo.Phone = phone;
                    studentInfo.Email = email;
                    _memberMgr.UpdateMemberInfo((Byte)Enums.DBAccess.MemberSaveType.UpdateContactInfo, studentInfo);
                }
                else if (!String.IsNullOrEmpty(name))
                {
                    studentInfo.Name = name;
                    _memberMgr.UpdateMemberInfo((Byte)Enums.DBAccess.MemberSaveType.UpdateName, studentInfo);
                }
            }
            return (result == 0);
        }

        public Byte AcceptOrder(int orderId, int studentId, int teacherId, String name = "", String phone = "", String email = "")
        {
            var result = _orderMgr.UpdateOrderStatusWithCoins(orderId, (Byte)Enums.OrderStatus.Accepted, studentId, teacherId);
            if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(phone) || !String.IsNullOrEmpty(email))
            {
                MemberInfo studentInfo = new MemberInfo();
                studentInfo.MemberId = teacherId;
                studentInfo.Name = String.IsNullOrEmpty(name) ? "" : name;
                studentInfo.Phone = String.IsNullOrEmpty(phone) ? "" : phone;
                studentInfo.Email = String.IsNullOrEmpty(email) ? "" : email;
                _memberMgr.UpdateMemberInfo((Byte)Enums.DBAccess.MemberSaveType.UpdateContactInfo, studentInfo);
            }
            return result;
        }

        /// <summary>
        /// Update Order Status
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns>-1 : Error , 2: Not Available Status(May be another member update it before you change status but before you get page)</returns>
        public Byte UpdateOrderStatusWithCoins(int orderId, Byte orderStatus, int studentId = 0, int teacherId = 0)
        {
            var result = _orderMgr.UpdateOrderStatusWithCoins(orderId, orderStatus, studentId, teacherId);
            return result;
        }

        /// <summary>
        /// Update Order Status
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns>-1 : Error , 2: Not Available Status(May be another member update it before you change status but before you get page)</returns>
        public Byte UpdateOrderStatus(int orderId, Byte orderStatus, int studentId = 0, int teacherId = 0)
        {
            var result = _orderMgr.UpdateOrderStatus(orderId, orderStatus, studentId, teacherId);
            return result;
        }


        #endregion


        #region Message Management

        public Byte UpdateComplaint(Byte saveType, int memberId, int relatedId, Byte type)
        {
            return _messageMgr.UpdateComplaint(saveType, memberId, relatedId, type);
        }

        public List<ComplaintItem> GetComplaintList(Byte loadType)
        {
            return _messageMgr.GetComplaintList(loadType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <param name="messageText"></param>
        /// <returns></returns>
        public int AddMessage(int fromId, int toId, String messageText)
        {
            return _messageMgr.AddMessage(fromId, toId, messageText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxId"></param>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        public Boolean SetMessageAsRead(int maxId, int memberId, int contactorId)
        {
            return _messageMgr.SetMessageAsRead(maxId, memberId, contactorId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxId"></param>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        public Boolean SetMessageAsDeleted(int maxId, int memberId, int contactorId)
        {
            return _messageMgr.SetMessageAsDeleted(maxId, memberId, contactorId);
        }

        public List<MessageListItem> GetMessageList(int memberId, int pageSize = 0, Byte loadBy = 1)
        {
            return _messageMgr.GetLastestMessagesByMemberId(memberId, pageSize, loadBy);
        }

        public List<Message> GetMessageDetail(int memberId, int contactId, Byte loadBy = 1)
        {
            return _messageMgr.GetMessagesByFromToId(memberId, contactId, loadBy);
        }

        public Dictionary<int, int> GetMessageUnReadNum(int memberId)
        {
            return _messageMgr.GetMessagesUnReadNum(memberId);
        }

        #endregion


        #region report and tools

        public List<ReportNumItem> GetReportClassMemberNum()
        {
            return _repMgr.GetReportClassMemberNum();
        }

        public List<ReportOrderStatus_Load_p_Result> GetReportClassMemberNum(Byte loadBy, DateTime beginDate, DateTime endDate)
        {
            return _repMgr.GetReportClassMemberNum(loadBy, beginDate, endDate);
        }

        public List<RecommendationItem> GetRecommendation(int classId)
        {
            return _repMgr.GetRecommendation(classId);
        }

        public void SaveMasterMember(int memberId, String paraStr, char split)
        {
            _repMgr.SaveMasterMember(memberId, paraStr, split);
        }

        public void SaveRecommendationClass(int classId, String paraStr, char split)
        {
            _repMgr.SaveRecommendationClass(classId, paraStr, split);
        }
        #endregion
    }
}
