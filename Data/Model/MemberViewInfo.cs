namespace SkillBank.Site.DataSource.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class MemberViewInfo
    {
        public int MemberId { get; set; }
        public byte SocialType { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public decimal PosX { get; set; }
        public decimal PosY { get; set; }
        //public string Email { get; set; }
        //public bool Gender { get; set; }
        public string Avatar { get; set; }
        public bool IsLike { get; set; }
        public string SocialAccount { get; set; }
        //public string ContactName { get; set; }
    }
}