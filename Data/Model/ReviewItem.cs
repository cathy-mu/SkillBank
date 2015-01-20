using System;


namespace SkillBank.Site.DataSource.Data
{
    public partial class ReviewItem
    {
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public Nullable<byte> FeedBack { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int Class_Id { get; set; }
        public string Avatar { get; set; }
        public int MemberId { get; set; }
        public string Name { get; set; }
        public byte TabId { get; set; }
    }
}
