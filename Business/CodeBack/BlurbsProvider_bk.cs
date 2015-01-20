using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.DataSource;
using EF.Frameworks.Common.CachingEF;
using EF.Frameworks.Orpheus.ContentManagementEF;

namespace SkillBank.Site.Data
{
    public class BlurbProvider:ContentProviderBase<object, IDictionary<int,String>>
    {
        public static volatile BlurbProvider _instance = null;
        private readonly IBlurbsRepository _repository;
        private static Dictionary<int, String> _blurbDic;
        private static object lockHelper = new object();  
                
        #region Constructors

        private BlurbProvider()
        {
            this._repository = new BlurbsRepository();
            _blurbDic = this._repository.Blurbs_LoadByLanguageSiteVersion_p(1, "cs");
        }

        #endregion

        public static BlurbProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new BlurbsProvider();
                        }
                    }
                }
                return _instance;
            }
        }


        public static String GetTranslator(int blurbId)
        {
            if(_blurbDic.ContainsKey(blurbId))
            {
            return _blurbDic[blurbId];
            }

            return "";
        }

    }
}
