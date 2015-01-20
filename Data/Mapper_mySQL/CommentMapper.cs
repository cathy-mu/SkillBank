using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Mapper
{
    public class CommentMapper
    {
        public static List<StudentReview> Map(ObjectResult<StudentReview> objComments)
        {
            if (objComments != null)
            {
                var comments = objComments.ToList<StudentReview>();
                return (comments.Count > 0) ? comments : null;
            }
            return null;
        }

        public static List<TeacherReview> Map(ObjectResult<TeacherReview> objComments)
        {
            if (objComments != null)
            {
                var comments = objComments.ToList<TeacherReview>();
                return (comments.Count > 0) ? comments : null;
            }
            return null;
        }
    }
}
