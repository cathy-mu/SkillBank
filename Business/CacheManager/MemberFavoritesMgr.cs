using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Frameworks.Common.ThreadingEF;
using EF.Frameworks.Common.CollectionsEF;
using EF.Frameworks.Common.ConfigurationEF;

using EF.Frameworks.Common.FactoryEF;
using EF.Frameworks.Orpheus.ContentManagementEF;
using SkillBank.Site.Common.Caching;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.Services.Managers
{

    public interface IMemberFavoritesMgr
    {
        Dictionary<Byte, String> GetFavorites(int memberId);
    }

    public class MemberFavoritesMgr : CacheBase<int, Dictionary<Byte, String>>, ISingleton, IMemberFavoritesMgr
    {
        //public static readonly FavoriteLkpProvider Instance = Factory<FavoriteLkpProvider>.Create();
        //private static Dictionary<int, Dictionary<Byte, String>> _favLists;
        //private static readonly object _locker = new object();
        private readonly IInteractiveRepository _repository;
        //private String keyPrefix = "Favorite";

        #region Constructors

        public MemberFavoritesMgr(IInteractiveRepository repository)
            : base(RefreshOptions.Custom)
        {
            this._repository = repository;
        }

        #endregion

        public new void RefreshAll()
        {
            //_favLists = null;
            base.RefreshAll();
        }

        public void RefreshItem(int memberId)
        {
            ////if (!_favLists.ContainsKey(memberId) && isLike)
            ////{
            ////    _favLists.Add(memberId, new String() { classId });
            ////} else if (_favLists.ContainsKey(memberId))
            //{
            //    var likeLists = _favLists[memberId];
            //    String classList = likeLists[1];
            //    String memberList = likeLists[2];

            //    if (!String.IsNullOrEmpty(classList) && !classList.Contains(classId + ",") && isLike)
            //    {
            //        classList.Replace(classId + ",","");
            //    }
            //    else if (!String.IsNullOrEmpty(classList) && classList.Contains(classId + ",") && !isLike)
            //    {
            //        classList = String.Concat(classList, classId + ",");
            //    }
            //    _favLists[memberId] = new Dictionary<byte, string>() { { 1, classList }, { 2, memberList } };
            //}
            base.RefreshItem(memberId);
        }

        protected override ICacheManager<int, Dictionary<Byte, String>> GetCustomCacheManager(
            SynchronizedDictionary<int, CachedItemInfo<int, Dictionary<Byte, String>>> elementCache,
             KeyedReaderWriterLockSlim<int> contentLoadLock)
        {
            int timeOutMins = Constants.CacheTimeOut.MemberFavoriteDicMins;
            var cacheMgr = new TimestampCacheManagerAsync<int, Dictionary<Byte, String>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override Dictionary<Byte, String> LoadItem(int memberId)
        {
            var result = _repository.GetFavoritesList(memberId); 

            Dictionary<Byte, String> favLists 
            if (favLists != null && favLists.ContainsKey(memberId))
            {
                return favLists[memberId];
            }

            return null;
        }

        ///// <summary>
        ///// Get lists Cache
        ///// </summary>
        //private Dictionary<int, Dictionary<Byte, String>> GetFavoriteListCacheItem()
        //{
        //    var result = _favLists;
           
        //    // Double-checked lock
        //    if (result == null)
        //    {
        //        lock (_locker)
        //        {
        //            result = _favLists;
        //            if (result == null)
        //            {
        //                result = LoadFavoriteListCache();
        //                _favLists = result;
        //            }
        //        }
        //    }
        //    return result;
        //}

        //private Dictionary<int, Dictionary<Byte, String>> LoadFavoriteListCache()
        //{
        //    var result = _repository.GetFavoritesList();
        //    Dictionary<int, Dictionary<Byte, String>> favDic = new Dictionary<int, Dictionary<Byte, String>>();
        //    if (result != null)
        //    {
        //        foreach (var item in result)
        //        {
        //            favDic.Add(item.MemberId, new Dictionary<byte, string>() { { 1, item.ClassList }, { 2, item.MemberList } });
        //        }
        //    }

        //    return favDic;
        //}

        public Dictionary<Byte, String> GetFavorites(int memberId)
        {
            var favorites = this.GetItem(memberId);
            return favorites;
        }

    }
}
