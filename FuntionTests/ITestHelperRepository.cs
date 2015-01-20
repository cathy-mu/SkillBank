using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

using SkillBank.Site.DataSource.Mapper;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.FunctionTests
{
    public interface ITestHelperRepository
    {
        void TeacherReviewCleanUp(int orderId);
        void StudentReviewCleanUp(int orderId);
        void ClassInfoCleanUp(int orderId);
    }

    public class TestHelperRepository : DbContext, ITestHelperRepository
    {
        public TestHelperRepository()
            : base("name=SkillBankConnection")
        {
        }

        public void StudentReviewCleanUp(int orderId)
        {
            TestHelper_DataCleanUp_p(1, orderId);
        }

        public void TeacherReviewCleanUp(int orderId)
        {
            TestHelper_DataCleanUp_p(2, orderId);
        }

        public void ClassInfoCleanUp(int membetId)
        {
            TestHelper_DataCleanUp_p(3, membetId);
        }


        private int TestHelper_DataCleanUp_p(SByte loadType, int paraId)
        {
            var loadByParameter = new ObjectParameter("CleanType", loadType);
            var idParameter = new ObjectParameter("ParaId", paraId);
            var isExistParameter = new ObjectParameter("IsExist", 0);

            int result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TestHelper_DataCleanUp_p", loadByParameter, idParameter, isExistParameter);
            return result;
        }
        
    }
}

