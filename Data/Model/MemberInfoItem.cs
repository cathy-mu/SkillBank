﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillBank.Site.DataSource.Data
{
    public partial class MemberInfoItem
    {
        public int MemberId { get; set; }
        public byte SocialType { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public decimal PosX { get; set; }
        public decimal PosY { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public int Coins { get; set; }
        public int CoinsLocked { get; set; }
        public string Avatar { get; set; }
        public string SelfIntro { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime LastUpdateDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string OpenId { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Etag { get; set; }
        public string Address { get; set; }
        public string SocialAccount { get; set; }
        public string ContactName { get; set; }
        public byte IsMobileVerified { get; set; }
        public byte IsEmailVerified { get; set; }
        public bool IsLike { get; set; }
        public string ExtraInfo { get; set; }
    }
}
