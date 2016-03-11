using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBank.Site.Common
{
    public partial class Enums
    {
        public class DBAccess
        {
            public enum MobileVerificationSaveType
            {
                //NewMember = 1,//obsolote
                //OldMember = 2,//obsolote
                Verify = 3,
                CheckIsVerified = 4,//temply
                GetVerifyCode = 5,//new process for API/Web
                GetVerifyCode4Visit = 6,//new process for API/Web
                UnbindMobile = 11,//temply
                UnbindSocialByMemberid = 12,
                UnbindSocialBySocialid = 13//temply
            }

            public enum OrderSaveType
            {
                AddNew = 1,
                UpdateBookedDate,
                UpdateSatatus
            }

            public enum OrderLoadType
            {
                ByOrderId = 1,
                ByClassId,
                ByStudentId,
                ByTeacherId
            }

            public enum CreditUpdateType
            {
                AddCredit = 0,//for admin
                ExchangeFromCoin = 1,//for admin
                ExchangeToCoin = 2,//for member
                SignIn = 5,
                Referral = 6
            }

            public enum CoinUpdateType
            {
                CheckShareClassCoin = 0,//for share class
                ClassShareOnSocial = 1,//for share class, add 3 coins before mobile register, 2 coins after that
                PayCoin = 2,
                EarnCoin = 3,
                LockCoin = 4,
                UnLockCoin = 5,
                ByAdminTool = 6,
                AddPromotionCoin = 7,
                CreateFirstClass = 8,
                GetFreeCoinBack = 10,
                OctPromoCoin = 11,
                RegisterFreeCoin = 12 //,VerifyMobile
                //ExchangeToCredit = 13,//for admin
                //ExchangeFromCredit = 14//for member
            }

            public enum CoinToolUpdateType
            {
                CheckShareClassCoin = 1, 
                AddShareCoinByMember = 2, 
                AddCoinByMember = 3,
                AddShareCoinByClass = 4, 
                ClearShareHistory = 5
            }
            
            public enum CommentLoadType
            {
                ByCommentId = 1,
                ByOrderId,
                ByClassId,
                ByStudentId,
                ByTeacherId
            }

            public enum ComplaintLoadType
            {
                OnlyActive = 1,
                Latest = 2
            }

            public enum ComplaintSaveType
            {
                Add = 1,
                DisActive = 2
            }

            public enum ClassSaveType
            {
                UpdateAll = 1,
                UpdateProvedTag = 2,
                UpdateCategory = 3,
                UpdateTeachLevel = 4,
                UpdateSkillLevel = 5,
                UpdateTitle = 6,
                UpdateSummary = 7,
                Updatelevel = 8,
                UpdateDetail = 9,
                UpdateWhyU = 10,
                UpdateTag = 11,
                UpdatePhoto = 12,
                UpdatePeriod = 13,
                SetActiveTag = 14,//Publish or Disactive Class
                UpdateAvailable = 15,
                UpdateLocation = 16,
                UpdateRemark = 17,
                UpdateCategoryATags = 18,//Category and Tag
                DeleteClass = 19,
                CreateNew = 20,//20+ is for mobile 3 step saving
                UpdateStep1 = 21,
                UpdateStep2 = 22,
                UpdateStep3 = 23//save cover and publish/preview
            }

            public enum ClassLoadType
            {
                ByClassId = 1,
                ByTeacherId = 2,
                ByStudentId = 3,
                ByUnProved = 4,
                ByTeacherPublished = 5,
                ByTeacherDashboard = 6,// temply for dashbord
                ByRejected = 7,
                ByClassAndCurrMemberId = 8,
                ByClassEditDetail = 9,
                ByAdminPreview = 10
            }

            public enum ClassCollectionLoadType
            {
                ByMemberLiked = 1,
                ByTeacherId = 2,
                ByMemberLearnt = 3,
                ByMemberTought = 4
            }

            public enum ClassListLoadType
            {
                WebClassList = 1,
                ByRecommendation = 1,
                ByRecommendationTop = 2,
                ByRecommendationCached = 3,
                WebAllClassCached = 4
            }

            public enum ClassTabListLoadType
            {
                NearBy = 0,
                Recommendation = 1,
                Category = 2,
                SearchClass = 3,
                Latest = 4,
            }

            public enum ClassTagSaveType
            {
                AddNew = 1,
                UpdateTags
            }

            public enum ClassTagLoadType
            {
                ByClassId = 1,
                ByCategory
            }


            public enum MemberSaveType
            {
                UpdateProfile = 1,//basic info for web
                UpdatePhone = 2,
                UpdateEmail = 3,
                UpdateName = 4,
                UpdateIntro = 5,
                UpdateGender = 6,
                UpdateCity = 7,
                UpdatePosition = 8,//use avatar to save address
                UpdatePhoto = 9,
                UpdateBirthDate = 10,
                UpdateContactInfo = 11,
                UpdateBasicInfo = 12,//basic info for mobile
                UpdatePassword = 13,
                RebindSicalAccount = 14,
                UpdateRCTokenADeviceToken = 15
            }

            public enum MemberNumsLoadType
            {
                ByClassId = 1,//class detail
                ByMemberId = 2,//member profile
                ByClassSummary = 3 ,//class detail for mobile
                ByMemberSummary = 4, //member profile for mobile
                ByMemberDashboard = 5, //member Dashboard for mobile
                ByCreditGetMethods = 6
            }

            public enum MemberLoadType
            {
                ByMemberId = 1,
                ByOpenId = 2,
                BySocialAccount = 3,
                ByEmail = 4,
                ByName = 5,
                ByPhone = 6,
                ByMemberIdAndRelatedMemberId = 7,
                ByMemberExtraInfo = 8,//has like class for temp test
                ByWebClassDetail = 9,
                ByMobileAPass = 10 //use temp  hack , pass as mobile for load
            }

            public enum MessageSaveType
            {
                Add = 1,
                SetAsRead,
                Delete
            }

            public enum MessageLoadType
            {
                DateDesc = 1,
                DateAsc = 2
            }

            public enum NotificationSaveType
            {
                MessageAdd = 1,//Info: relatedMemberId ,    --   Action:check message detail
                ClassUnfinished = 2,//Info: classId   --   Action:Complete
                ClassProved = 3,//Info: classId ,isShared  --   Action:Share
                ClassRejected = 4,///Info: classId ,isShared  --   Action:Update
                DashboardTopBanner = 5,
                
                ClassComment = 6,
                StudentReview = 7,
                TeacherReview = 8,
                ClassLiked = 9,
                
                OrderBooked = 11,//Both Student and Teacher, set student as Read for update in process
                OrderRejected = 12,
                OrderCancled = 13,//Student Cancle and System Cancle(4 teacher)
                OrderAccept = 14,
                OrderFinished = 15,

                RefundRequest = 16, 
                RefundAccept = 17, 
                RefundReject = 18,

                OrderConfirmed = 19,
                TeacherCancled = 20,
                AutoCancled4Student = 21, //System Cancle(4 student)
                Followed = 30
            }

            public enum NotificationTagUpdateType
            {
                SetPopTagAsClicked = 1,
                SetPopTagAsRead = 2,
                SetNotificationAsReadById = 3,
                SetOrderNotificationAsReadByMemberId = 4,
                SetMessageAsPopedByMemberId = 5//,
                //SetSystemAsReadByMemberAndNotiId = 6,
                //SetReactionAsPopedByMemberId = 7,
                //SetClassRelatedAsReadByClassId = 8,//Comment , Student Review
                //SetTeacherReviewAsRead = 9//Teacher Review
            }


            public enum StudentReviewLoadType
            {
                ByMemberId = 1,
                ByClassId = 2,
                ByMemberIdOtherClassId = 3
            }
            
            public enum TeacherReviewLoadType
            {
                ByReviewId = 0,
                ByMemberId = 1
            }

            public enum ReviewLoadType
            {
                ByClass = 1,
                ByClassTab1 = 2,
                ByClassTab2 = 3,
                ByMember = 4,
                ByMemberTab1 = 5,
                ByMemberTab2 = 6,
                ByClassAll = 7, //mobile
                ByMemberAll = 8, //mobile
                ByClassComment = 9,
                ByClassReview = 10
            }

            public enum FavoriteSaveType
            {
                SaveFavoriteTag = 1
            }

            public enum FavoriteLoadType
            {
                ByFollwingMemberAViewerId = 1,
                ByFansMemberAViewerId = 2
            }

            public enum CommentSaveType
            {
                AddComment = 1,
                RemoveComment = 2
            }

            public enum WeChatEventSaveType
            {
                AddEvent = 1,
                UpdateEvent = 2
            }

            public enum NotificationAlterLoadType
            {
                Web = 0,
                WebCheckStatus = 1,

                MobileMenu = 3,
                MobileMyCourse = 4,
                MobileTeach = 5,
                MobileLearn = 6,
                MobileNotification = 7,
                MobileSystem = 8,
                MobileInteration = 9
            }  
            
        }

        public enum FavoriteType
        {
            LikeClass = 1,
            LikeMember = 2
        }

        public enum SocialTpye
        {
            Sina = 1,
            Mobile = 2,
            QQ = 3,
            WeChat = 4,
            APPWeChat = 5        
        }

        public enum AddressType
        {
            Home,
            OfficeSchool,
            Other
        }

        public enum ClassLevel
        {
            Basic = 1,
            InterMedia,
            Advance
        }

        public enum ClassOverView
        {
            Bad = 1,
            OKay,
            Great
        }

        public enum CommentType
        {
            Profession = 1,
            Communication,
            Attitude,
        }
        
        public enum OrderStatus
        {
            ChangeBookDate = 0,   //1    ---- by student
            Booked = 1,   //1            ---- by student
            Rejected = 2,//2        (1->2)   ---- by teacher, cancled -- F
            Cancled = 3,  //3        (1->3)   ---- by system/student (if no coins left or before teacher accept or overtime -- F)
            Accepted = 4, //4        (1->4)   ---- by teacher        (lock coin + 1, Available coin - 1)
            Finished = 5, //5        (4->5)   ---- by system
            Refund = 6,   //6        (4->6)   ---- by student
            RefundProve = 7,//7      (6->7)   ---- by teacher,unlock coins -1 , available coin + 1 (6~7)  -- F
            RefundReject = 8,//8     (6->8)   ---- by teacher or system ,unlock coins -1 , available coin + 1 -- F
            Confirmed = 9,  //9      (4/5->9) ---- by student, give coins to teacher, reduce available coins and locked coin (2~3days) -- F
            AutoConfirmed = 10,//10(4/5->10)  ---- by system, give coins to teacher, reduce available coins and locked coin (after 2~3days) -- F 
            AutoRefund = 11,//11     (6->11)  ---- by system
            TeacherCancled = 12, //by teacher, cancled -- F
            AutoCancled = 13 //by system overdate -- F
        }

        public enum CoinHistoryType
        {
            FreeCoin = 1,//Get form Share on Social-coin:3
            Spend = 2,//Spend on class-coin:-amount
            Earned = 3,//Get by class-coin:+amount
            Locked = 4,//When booking accept-coin: lock -amount  unlock +amount
            UnLocked = 5,//when class cancled or refund accepted//-coin:+amount
            PromotionCoin = 6,//
            OCTOneMonthFreeCoin = 11,// One free coin add by DB script
            RegisterFreeCoin = 12
        }

        public enum CreditHistoryType
        {
            AddCredit = 1,//for admin
            ExchangeWithCoin = 2,//+coin->credit -credit->coin
            TeacherComment = 3,
            StudentComment = 4,
            SignIn = 5,
            Referral = 6
        }

        public enum NotificationType
        {
            Message,
            OrderStatus,
            CoinsStatus,
            ClassVerified,
            ClassUnfinished
        }

        public enum MemberType
        {
            Visiter,
            Register,
            ClassOwner,
            Member
        }

        public enum TeachTag
        {
            NoClass = 0,
            CreateClass = 1,
            HasAddress = 3,
            Publishd = 5,
            Proved = 7,
            Teached = 9
        }

        public enum LearnTag
        {
            NoApply = 0,
            Applied = 1,
            Booked = 2,
            Learnt = 3,
        }

        public enum NumberDictionaryKey
        {
            Class = 1,
            Student,
            TeacherReview,
            StudentReview,
            Favorite,
            Like,
            Follow,
            Fans,
            Rank,
            Comment,
            Certification,
            Result01,
            Result02,
            Result03,
            Result11,
            Result12,
            Result13,
            Sum0,
            Min0,
            Max0,
            Sum1,
            Min1,
            Max1,
            Sum2,
            Min2,
            Max2,
            GotSharedCoins,
            MissStudentReview,
            MissTeacherReview,
            IsSignIn
        }

        public enum NotificationDictionaryKey
        { 
            NM,//nitificatiom
            M,//message
            NC,
            C,//my course
            T,//teach
            L//Learn
        }


        public enum PushNotificationType
        {
            //0-3 管理员
            System,//0 系统通知
            SkillKiller,//1 技者汇更新
            ClassProved,//2 开课审批通过
            ClassRejected,//3 开课未通过

            //4-5 评价
            StudentReview,//4 学生评价
            TeacherReview,//5 老师评价

            //6-9 互动
            Message,//6新私信
            Followed,//7被关注
            ClassLiked,//8课程被收藏
            Commented,//9课程被留言

            //10-13  For 老师
            BookRequest,//10 订课申请
            RefundRequest,//11 退币申请
            OrderPaid,//12 订单支付并评价
            RemindAccept,//13 提醒接受订单

            //14-18 For 学生
            BookAccepted,//14 接受订课
            BookRejected,//15 接受订课
            BookCancled,//16 老师取消订课
            RefundProved,//17 接受退币请求
            RefundRejected//18 拒绝退币请求
        }

        public enum SmsType
        {
            StudentReview = 1,
            TeacherReview = 2,
            BookRequestRemind = 3
            
        }
        
    }    
}
