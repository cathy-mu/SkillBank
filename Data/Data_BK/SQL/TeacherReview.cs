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
    
    public partial class TeacherReview
    {
        public int ReviewId { get; set; }
        public int Order_Id { get; set; }
        public Nullable<byte> FeedBack { get; set; }
        public string Comment { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string PrivateComment { get; set; }
    }
}
