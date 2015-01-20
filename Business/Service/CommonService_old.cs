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
        MemberInfo GetMemberInfo(int memberId);
        MemberInfo GetMemberInfo(String openId);
        MemberInfo GetMemberInfo(String socialAccount, Byte socialType);
        int CreateMember(out int memberId, String account, Byte socialType, String memberName, String email,String avatar, int cityId = 0);
        Boolean UpdateMemberInfo(int memberId, Byte saveType, String saveValue, String saveValue2 = "");
        Boolean UpdateMemberProfile(MemberInfo memberInfo);
        void SaveEmailAccount(String name, String email);

        int CreateClass(int memberId, int categoryId, Byte teacheLevel, Byte skillLevel, out Boolean isExist);
        Boolean UpdateClassInfo(Byte updateType, int classId, Byte paraValue, Byte completeStatus = 1);
        Boolean UpdateClassInfo(Byte updateType, int classId, Boolean paraValue, Byte completeStatus = 1);
        Boolean UpdateClassInfo(Byte updateType, int classId, String paraValue, Byte completeStatus = 1);
        int GetClassNumsByMember(int MemberId, out int unFinishedClassId);
        int GetClassNumsByClass(int MemberId, int ClassId, out int Result1, out int Result2);

        List<ClassItem> SearchClass(int cityId, int categoryId, Byte OrderByType = (Byte)ClientSetting.ClassListOrderType.ByDisctince, Boolean isAsc = false, Decimal posX = 0, Decimal posY = 0, String searchKey = "");
        List<ClassItem> GetMemberClass(int memberId);
        ClassInfo GetClassInfoByClassId(int paraId);
        List<ClassInfo> GetClassInfoByTeacherId(int memberId);
        List<ClassInfo> GetClassInfoByStudentId(int memberId);
        List<ClassInfo> GetClassInfoByUnProvedClass();

        //Review
        //List<StudentReview> GetStudentReviewsByClass(int classId, int minReviewId);
        List<StudentReview> GetStudentReviews(int memberId, int minReviewId);
        Boolean AddStudentReview(int orderId, Byte feedBack, String comment, String privateComment);
        Boolean AddTeacherReview(int orderId, Byte feedBack, String comment, String privateComment);
    
        //Order
        Byte UpdateOrderStatus(int orderId, Byte orderStatus);
        Boolean AddOrder(int studentId, int classId, DateTime bookDate, String remark, String studentName = "", String studentEmail = "");
        List<OrderItem> GetOrderListByStudent(int studentId);
        List<OrderItem> GetOrderListByTeacher(int teacherId);

        //Message
        int AddMessage(int fromId, int toId, String messageText);
        Boolean SetMessageAsRead(int maxId, int fromId, int toId);
        Boolean SetMessageAsDeleted(int maxId, int fromId, int toId);

        List<MessageListItem> GetMessageList(int memberId);
        List<MessageListItem> GetMessageList(int memberId,int pageSize);
        List<Message> GetMessageDetail(int memberId, int friendId);
        Dictionary<int, int> GetMessageUnReadNum(int memberId);
    }

    public class CommonService : ICommonService
    {
        private readonly IMemberManager _memberMgr;
        private readonly IClassManager _classMgr;
        private readonly IOrderManager _orderMgr;
        private readonly IFeedBackManager _feedbackMgr;
        private readonly IMessageManager _messageMgr;

        #region Constructors

        public CommonService(IMemberManager memberMgr, IClassManager classManager, IOrderManager orderMgr, IFeedBackManager feedbackMgr, IMessageManager messageMgr)
        {
            this._memberMgr = memberMgr;
            this._classMgr = classManager;
            this._feedbackMgr = feedbackMgr;
            this._orderMgr = orderMgr;
            this._messageMgr = messageMgr;
        }

        #endregion


        #region Member Management

        public void SaveEmailAccount(String name, String email)
        {
            _memberMgr.SaveEmailAccount(name, email);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public MemberInfo GetMemberInfo(int memberId)
        {
            return this._memberMgr.GetMemberInfo(memberId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public MemberInfo GetMemberInfo(String openId)
        {
            return this._memberMgr.GetMemberInfo(openId);
        }

        /// <summary>
        /// Get Member Info by Social Account, Create new Account if not exist
        /// </summary>
        /// <param name="socialAccount"></param>
        /// <param name="socialType"></param>
        /// <returns></returns>
        public MemberInfo GetMemberInfo(String socialAccount, Byte socialType)
        {
            return this._memberMgr.GetMemberInfo(socialAccount, socialType);
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
        public int CreateMember(out int memberId, String account, Byte socialType, String memberName, String email, String avatar, int cityId = 0)
        {
            return this._memberMgr.CreateMember(out memberId, account, socialType, memberName, email , avatar, cityId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public Boolean UpdateMemberInfo(int memberId, Byte saveType, String saveValue, String saveValue2 = "")
        {
            Boolean result = false;
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

        public Boolean UpdateMemberProfile(MemberInfo memberInfo)
        {
            Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateProfile;
            return this._memberMgr.UpdateMemberInfo(saveType, memberInfo);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public Boolean UpdateClassInfo(Byte updateType, int classId, Byte paraValue, Byte completeStatus = 1)
        {
            return _classMgr.UpdateClassInfo(updateType, classId, paraValue, completeStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public Boolean UpdateClassInfo(Byte updateType, int classId, Boolean paraValue, Byte completeStatus = 1)
        {
            return _classMgr.UpdateClassInfo(updateType, classId, paraValue, completeStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public Boolean UpdateClassInfo(Byte updateType, int classId, String paraValue, Byte completeStatus = 1)
        {
            return _classMgr.UpdateClassInfo(updateType, classId, paraValue, completeStatus);
        }

        public int GetClassNumsByMember(int memberId, out int classId)
        {
            return _classMgr.GetClassNumsByMember(memberId, out classId);
        }

        public int GetClassNumsByClass(int memberId, int ClassId, out int studentNum, out int reviewNum)
        {
            return _classMgr.GetClassNumsByClass(memberId, ClassId, out studentNum, out reviewNum);
        }

        /// <summary>
        /// Get Class for List page
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="categoryId"></param>
        /// <param name="memberId"></param>
        /// <param name="OrderByType"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public List<ClassItem> SearchClass(int cityId, int categoryId, Byte OrderByType = (Byte)ClientSetting.ClassListOrderType.ByDisctince, Boolean isAsc = false, Decimal posX = 0, Decimal posY = 0, String searchKey = "")
        {
            return _classMgr.SearchClass(cityId, categoryId, OrderByType, isAsc, posX, posY, searchKey);
        }

        public List<ClassItem> GetMemberClass(int memberId)
        {
            return _classMgr.GetMemberClass(memberId);
        }

        public ClassInfo GetClassInfoByClassId(int classId)
        {
            return _classMgr.GetClassInfoByClassId(classId);
        }

        public List<ClassInfo> GetClassInfoByStudentId(int memberId)
        {
            return _classMgr.GetClassInfoByStudentId(memberId);
        }

        public List<ClassInfo> GetClassInfoByTeacherId(int memberId)
        {
            return _classMgr.GetClassInfoByTeacherId(memberId);
        }

        public List<ClassInfo> GetClassInfoByUnProvedClass()
        {
            return _classMgr.GetClassInfoByUnProvedClass();
        }

               

        //public List<ClassInfo> GetClassInfoByStudentId(int memberId)
        //{
        //    return _classMgr.ge(memberId);
        //}

        #endregion



        #region Review Management

        //public List<StudentReview> GetStudentReviewsByClass(int classId, int maxReviewId)
        //{
        //    return _feedbackMgr.GetStudentReviewsByClass(classId, maxReviewId);
        //}

        public List<StudentReview> GetStudentReviews(int memberId, int maxReviewId)
        {
            return _feedbackMgr.GetStudentReviews(memberId, maxReviewId);
        }

        public Boolean AddStudentReview(int orderId, Byte feedBack, String comment, String privateComment)
        {
            return _feedbackMgr.AddStudentReview(orderId, feedBack, comment, privateComment);
        }

        public Boolean AddTeacherReview(int orderId, Byte feedBack, String comment, String privateComment)
        {
            return _feedbackMgr.AddTeacherReview(orderId, feedBack, comment, privateComment);
        }
    
    
        #endregion


        #region Order Management
        
        //public List<Order> GetOrdersByStudentId(int memberId)
        //{
        //    return _orderMgr.GetOrdersByStudentId(memberId);
        //}

        //public List<Order> GetOrdersByTeacherId(int memberId)
        //{
        //    return _orderMgr.GetOrdersByTeacherId(memberId);
        //}

        /// <summary>
        /// Get Order list for member learn page
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<OrderItem> GetOrderListByStudent(int studentId)
        {
            return _orderMgr.GetOrderListByStudent(studentId);
        }

        /// <summary>
        /// Get Order list for member teach page
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public List<OrderItem> GetOrderListByTeacher(int teacherId)
        {
            return _orderMgr.GetOrderListByTeacher(teacherId);
        }

        /// <summary>
        /// Add new Order and check if new order id great than 0
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public Boolean AddOrder(int studentId, int classId, DateTime bookDate, String remark, String studentName = "", String studentPhone = "")
        {
            var result = _orderMgr.AddOrder(studentId, classId, bookDate, remark);
            if (!String.IsNullOrEmpty(studentName) || !String.IsNullOrEmpty(studentPhone))
            {
                MemberInfo studentInfo = new MemberInfo();
                studentInfo.Name = String.IsNullOrEmpty(studentName)?"":studentName;
                studentInfo.Phone = String.IsNullOrEmpty(studentPhone) ? "" : studentPhone;
                _memberMgr.UpdateMemberInfo((Byte)Enums.DBAccess.MemberSaveType.UpdateContactInfo, studentInfo);
            }
            return (result == 0);
        }

        /// <summary>
        /// Update Order Status
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns>-1 : Error , 2: Not Available Status(May be another member update it before you change status but before you get page)</returns>
        public Byte UpdateOrderStatus(int orderId, Byte orderStatus)
        {
            var result = _orderMgr.UpdateOrderStatus(orderId, orderStatus);
            return result;
        }


        #endregion


        #region Message Management

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
        public Boolean SetMessageAsRead(int maxId, int fromId, int toId)
        {
            return _messageMgr.SetMessageAsRead(maxId, fromId, toId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxId"></param>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        public Boolean SetMessageAsDeleted(int maxId, int fromId, int toId)
        {
            return _messageMgr.SetMessageAsDeleted(maxId, fromId, toId);
        }

        public List<MessageListItem> GetMessageList(int memberId)
        {
            return _messageMgr.GetLastestMessagesByMemberId(memberId);
        }

        public List<MessageListItem> GetMessageList(int memberId, int pageSize)
        {
            return _messageMgr.GetLastestMessagesByMemberId(memberId, pageSize);
        }

        public List<Message> GetMessageDetail(int memberId, int friendId)
        {
            return _messageMgr.GetMessagesByFromToId(memberId, friendId);
        }

        public Dictionary<int,int> GetMessageUnReadNum(int memberId)
        {
            return _messageMgr.GetMessagesUnReadNum(memberId);
        }
        
        #endregion


    }
}