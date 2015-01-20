using System.Data.Linq;
using System.Data.Entity;
//DbContext
namespace SkillBank.Site.DataSource.Data
{
    public abstract class ReadOnlyDataContext : DataContext//DbContext
    {
        protected ReadOnlyDataContext(string connectionString)
            : base(connectionString)
        {

            //ChangeTracker = 
            ObjectTrackingEnabled = false;
        }
    }
}