//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SkillBank.Site.DataSource.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public byte TypeId { get; set; }
        public int Member_Id { get; set; }
        public int RelatedMemberId { get; set; }
        public byte IsRead { get; set; }
        public bool IsPop { get; set; }
        public int ClassOrderId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime LastUpdateDate { get; set; }
    }
}