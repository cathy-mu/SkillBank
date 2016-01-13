namespace SkillBank.Site.DataSource.Data
{
    using System;

    public partial class ClassEditItem
    {
        public int ClassId { get; set; }
        public int Member_Id { get; set; }
        public byte Category_Id { get; set; }
        public string Title { get; set; }
        public byte Level { get; set; }
        public byte SkillLevel { get; set; }
        public byte TeacheLevel { get; set; }
        public string Summary { get; set; }
        public string Cover { get; set; }
        public byte PublishStatus { get; set; }
        public byte IsProved { get; set; }

        public string WhyU { get; set; }
        public string Period { get; set; }
        public string Location { get; set; }
        public string Available { get; set; }
        public string Remark { get; set; }
        public string Tags { get; set; }
        public bool HasOnline { get; set; }
    
        public Boolean IsLike { get; set; }
        public int LikeNum { get; set; }
    }
}

