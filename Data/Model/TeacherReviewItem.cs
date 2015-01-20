namespace SkillBank.Site.DataSource.Data
{
    using System;
    
    public partial class TeacherReviewItem
    {
        public int ReviewId { get; set; }
        public int Order_Id { get; set; }
        public Nullable<byte> FeedBack { get; set; }
        public string Comment { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string PrivateComment { get; set; }
        public string Avatar { get; set; }
        public int MemberId { get; set; }
        public string Name { get; set; }
    }
}
