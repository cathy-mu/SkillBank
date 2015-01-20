using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillBank.Site.DataSource.Data
{
    public partial class NotificationItem
    {
        public int NotificationId { get; set; }
        public byte TypeId { get; set; }
        public byte TypeRank { get; set; }
        //public int Member_Id { get; set; }
        public int RelatedMemberId { get; set; }
        public int ClassOrderId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
