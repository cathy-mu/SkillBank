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
    
    public partial class ClassEditInfo
    {
        public int ClassId { get; set; }
        public byte Category_Id { get; set; }
        public string Title { get; set; }
        public byte Level { get; set; }
        public byte SkillLevel { get; set; }
        public byte TeacheLevel { get; set; }
        public string Summary { get; set; }
        public byte IsProved { get; set; }
        public Nullable<byte> PublishStatus { get; set; }
        public System.DateTime LastUpdateDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Cover { get; set; }
        public int Member_Id { get; set; }
        public string Period { get; set; }
        public string Location { get; set; }
        public string Available { get; set; }
        public string Remark { get; set; }
        public string WhyU { get; set; }
        public string Tags { get; set; }
        public bool HasOnline { get; set; }
    }
}
