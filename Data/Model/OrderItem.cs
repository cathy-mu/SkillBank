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

    public partial class OrderItem
    {
        public int OrderId { get; set; }
        public int ClassId { get; set; }
        public System.DateTime LastUpdateDate { get; set; }
        public Nullable<byte> OrderStatus { get; set; }
        public System.DateTime BookedDate { get; set; }
        public string Title { get; set; }
        public string MemberName { get; set; }
        public int MemberId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public Boolean HasReview { get; set; }
        public string Remark { get; set; }
    }
}
