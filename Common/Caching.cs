using EF.Frameworks.Common.CollectionsEF;
using EF.Frameworks.Common.ExceptionsEF;
using EF.Frameworks.Common.FactoryEF;
using EF.Frameworks.Common.ThreadingEF;
using EF.Frameworks.Common.ConfigurationEF;
using EF.Frameworks.Common;

using System;
using System.Threading;
using System.ComponentModel;

using SkillBank.Site.Common;
using System.Web.Caching;

namespace SkillBank.Site.Common.Caching
{
    public abstract class CacheBase<TKey, TValue> : ISingleton
        {
            private SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> _cache;
            private KeyedReaderWriterLockSlim<TKey> _contentLoadLock;
            private ICacheManager<TKey, TValue> _cacheManager;
            protected CacheBase(RefreshOptions refreshOption)
            {
                this._cache = new SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>>();
                this._contentLoadLock = new KeyedReaderWriterLockSlim<TKey>(LockRecursionPolicy.SupportsRecursion);
                this._cacheManager = this.GetCacheManager(this._cache, this._contentLoadLock, refreshOption);
            }
            protected abstract TValue LoadItem(TKey key);
            protected TValue GetItem(TKey key, Func<TKey, TValue> loadMethod)
            {
                CachedItemInfo<TKey, TValue> cachedItem = this.GetCachedItem(key, loadMethod);
                return cachedItem.Value;
            }
            protected TValue GetItem(TKey key)
            {
                return this.GetItem(key, this.LoadItem);
                //return this.GetItem(key, new Func<TKey, TValue>(this.LoadItem));
            }
            protected virtual ICacheManager<TKey, TValue> GetCustomCacheManager(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> elementCache, KeyedReaderWriterLockSlim<TKey> contentLoadLock)
            {
                throw new NotImplementedException();
            }
           
            protected virtual ICacheManager<TKey, TValue> GetCustomCacheManager(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> elementCache, KeyedReaderWriterLock<TKey> contentLoadLock)
            {
                throw new NotImplementedException();
            }
            private ICacheManager<TKey, TValue> GetCacheManager(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> elementCache, KeyedReaderWriterLockSlim<TKey> contentLoadLock, RefreshOptions refreshOption)
            {
                ICacheManager<TKey, TValue> result;
                switch (refreshOption)
                {
                    //case RefreshOptions.OnDemand:
                    //    result = TimestampCacheManager<TKey, TValue>.DefaultInstance;
                    //    break;
                    //case RefreshOptions.Asynchronous:
                    //    result = new TimestampCacheManagerAsync<TKey, TValue>(elementCache, contentLoadLock, new Func<TKey, TValue>(this.LoadItem));
                    //    break;
                    case RefreshOptions.Custom:
                        result = this.GetCustomCacheManager(elementCache, contentLoadLock);
                        break;
                    default:
                        throw new EFException("Refresh option not recognized.", this);
                }
                return result;
            }
            private CachedItemInfo<TKey, TValue> GetCachedItem(TKey key, Func<TKey, TValue> loadMethod)
            {
                CachedItemInfo<TKey, TValue> cachedItemInfo = null;
                bool flag = !this._cache.TryGetValue(key, ref cachedItemInfo);
                if (flag)
                {
                    this._cache.EnterWriteLock();
                    try
                    {
                        flag = !this._cache.TryGetValue(key, ref cachedItemInfo);
                        if (flag)
                        {
                            cachedItemInfo = new CachedItemInfo<TKey, TValue>(key);
                            this._cache[key] = cachedItemInfo;
                        }
                    }
                    finally
                    {
                        this._cache.ExitWriteLock();
                    }
                }
                this._cacheManager.Refresh(ref cachedItemInfo, loadMethod, this._contentLoadLock);
                cachedItemInfo.LastRequestedTimestamp = DateTime.Now;
                return cachedItemInfo;
            }
            private CachedItemInfo<TKey, TValue> GetCachedItem(TKey key)
            {
                return this.GetCachedItem(key, new Func<TKey, TValue>(this.LoadItem));
            }
            protected void UncacheItem(TKey key)
            {
                this._contentLoadLock.EnterWriteLock(key);
                try
                {
                    bool flag = this._cache.ContainsKey(key);
                    if (flag)
                    {
                        this._cache.Remove(key);
                    }
                }
                finally
                {
                    this._contentLoadLock.ExitWriteLock(key);
                }
            }
            protected void RefreshAll()
            {
                this._cacheManager.RefreshAll();
            }
            protected void RefreshItem(TKey key)
            {
                CachedItemInfo<TKey, TValue> cachedItemInfo = null;
                this._contentLoadLock.EnterWriteLock(key);
                try
                {
                    bool flag = this._cache.TryGetValue(key, ref cachedItemInfo);
                    if (flag)
                    {
                        cachedItemInfo.LastRefreshTimestamp = DateTime.Now;
                        cachedItemInfo.Value = this.LoadItem(key);
                        cachedItemInfo.IsLoaded = true;
                    }
                    else
                    {
                        this.GetItem(key);
                    }
                }
                finally
                {
                    this._contentLoadLock.ExitWriteLock(key);
                }
            }
        }


    public class CachedItemInfo<TKey, TValue>
    {
        public TValue Value;
        public DateTime LastRefreshTimestamp;
        public DateTime LastRequestedTimestamp;
        public bool IsLoaded;
        public readonly TKey Key;
        public CachedItemInfo(TKey key, TValue value, DateTime timestamp)
        {
            this.Key = key;
            this.Value = value;
            this.LastRefreshTimestamp = timestamp;
        }
        public CachedItemInfo(TKey key)
        {
            this.Key = key;
            this.LastRefreshTimestamp = DateTime.MinValue;
        }
    }




    public interface ICacheManager<TKey, TValue>
    {
        //public delegate TValue LoadItemDelegate(TKey key);
        void Refresh(ref CachedItemInfo<TKey, TValue> element, Func<TKey, TValue> loadMethod, KeyedReaderWriterLockSlim<TKey> loadLock);
        void RefreshAll();
    }

    public enum RefreshOptions
    {
        OnDemand,
        Asynchronous,
        Custom
    }

    public class TimestampCacheManagerAsync<TKey, TValue> : ICacheManager<TKey, TValue>, IDisposable
    {
        private SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> _cache;
        private KeyedReaderWriterLockSlim<TKey> _loadLock;
        private Func<TKey, TValue> _loadMethod;
        private TimeSpan _timeout;
        private TimeSpan _stopRefreshTimeout;
        private Timer _timer;
        private bool _isRefreshing;
        private bool _isDisposed;
        private static ConfigurationContext<TimestampCacheManager<TKey, TValue>> _context = new ConfigurationContext<TimestampCacheManager<TKey, TValue>>();
        public const string TimeoutConfigName = "ContentTimeout";
        public TimeSpan Timeout
        {
            get
            {
                return this._timeout;
            }
        }
        public TimeSpan StopRefreshTimeout
        {
            get
            {
                return this._stopRefreshTimeout;
            }
        }
        public TimestampCacheManagerAsync(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> list, KeyedReaderWriterLockSlim<TKey> loadLock, Func<TKey, TValue> loadMethod)
        {
            this._isDisposed = false;
            this._cache = list;
            this._loadLock = loadLock;
            this._loadMethod = loadMethod;
            TimeSpan defaultTimeout = this.GetDefaultTimeout();
            this.SetTimeout(defaultTimeout);
        }
        public TimestampCacheManagerAsync(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> list, KeyedReaderWriterLockSlim<TKey> loadLock, Func<TKey, TValue> loadMethod, TimeSpan timeout)
        {
            this._isDisposed = false;
            this._cache = list;
            this._loadLock = loadLock;
            this._loadMethod = loadMethod;
            this.SetTimeout(timeout);
        }
        public TimestampCacheManagerAsync(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> list, KeyedReaderWriterLock<TKey> loadLock, Func<TKey, TValue> loadMethod)
        {
            this._isDisposed = false;
            throw new NotSupportedException();
        }
        public TimestampCacheManagerAsync(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> list, KeyedReaderWriterLock<TKey> loadLock, Func<TKey, TValue> loadMethod, TimeSpan timeout)
        {
            this._isDisposed = false;
            throw new NotSupportedException();
        }
        public void Refresh(ref CachedItemInfo<TKey, TValue> element, Func<TKey, TValue> loadMethod, KeyedReaderWriterLockSlim<TKey> loadLock)
        {
            bool flag = this.ShouldRefreshInline(element);
            if (flag)
            {
                loadLock.EnterWriteLock(element.Key);
                try
                {
                    flag = this.ShouldRefreshInline(element);
                    if (flag)
                    {
                        this.LoadElement(element, loadLock, loadMethod);
                    }
                }
                finally
                {
                    loadLock.ExitWriteLock(element.Key);
                }
            }
        }
        public void Refresh(ref CachedItemInfo<TKey, TValue> element, Func<TKey, TValue> loadMethod, KeyedReaderWriterLock<TKey> loadLock)
        {
            throw new NotSupportedException();
        }
        public void RefreshAll()
        {
            this.RefreshTimer_Tick(true);
        }
        public void SetTimeout(TimeSpan timeout, bool setStopRefreshTimeout)
        {
            bool flag = timeout < TimeSpan.Zero;
            if (flag)
            {
                throw new ArgumentOutOfRangeException("timeout", "Timeout must be greater than or equal to TimeSpan.Zero.");
            }
            this._timeout = timeout;
            if (setStopRefreshTimeout)
            {
                this._stopRefreshTimeout = this._timeout.Add(this._timeout);
            }
            flag = (timeout == TimeSpan.Zero);
            if (flag)
            {
                bool flag2 = this._timer != null;
                if (flag2)
                {
                    this._timer.Dispose();
                    this._timer = null;
                }
            }
            else
            {
                bool flag2 = this._timer == null;
                if (flag2)
                {
                    this._timer = new Timer(new TimerCallback(this.RefreshTimer_Tick), null, this._timeout, this._timeout);
                }
                else
                {
                    this._timer.Change(this._timeout, this._timeout);
                }
            }
        }
        public void SetTimeout(TimeSpan timeout)
        {
            this.SetTimeout(timeout, true);
        }
        public void SetStopRefreshTimeout(TimeSpan stopRefreshTimeout)
        {
            bool flag = stopRefreshTimeout > this.Timeout;
            if (flag)
            {
                this._stopRefreshTimeout = stopRefreshTimeout;
            }
        }
        protected virtual TimeSpan GetDefaultTimeout()
        {

            //TO DO :Update here
            var minValue = new TimeSpan(12, 0, 0);

            return minValue;
        }
        private bool ShouldRefreshInline(CachedItemInfo<TKey, TValue> element)
        {
            bool flag = !element.IsLoaded;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                flag = (DateTime.Compare(element.LastRequestedTimestamp, DateTime.Now.Subtract(this._stopRefreshTimeout)) < 0);
                if (flag)
                {
                    result = true;
                }
                else
                {
                    flag = (this._timeout == TimeSpan.Zero);
                    result = flag;
                }
            }
            return result;
        }

        //TO DO:Check Monitor later
        private void RefreshTimer_Tick(object state)
        {
            try
            {
                bool flag = state is bool;


                bool flag2 = false;
                try
                {

                    flag = this._isRefreshing;
                    if (flag)
                    {
                        return;
                    }
                    this._isRefreshing = true;
                }
                finally
                {
                    flag = flag2;

                }
                try
                {
                    this.ReloadAllElements(false);
                }
                finally
                {
                    this._isRefreshing = false;
                }
            }
            catch
            {

            }
        }
        private void ReloadAllElements(bool throwException)
        {
            CachedItemInfo<TKey, TValue>[] valueArray = this._cache.GetValueArray();
            checked
            {
                for (int i = 0; i < valueArray.Length; i++)
                {
                    CachedItemInfo<TKey, TValue> cachedItemInfo = valueArray[i];
                    bool flag = DateTime.Compare(cachedItemInfo.LastRequestedTimestamp, DateTime.Now.Subtract(this._stopRefreshTimeout)) > 0;
                    if (flag)
                    {
                        this.LoadElement(cachedItemInfo, this._loadLock, this._loadMethod);

                    }

                }
            }
        }
        protected virtual void LoadElement(CachedItemInfo<TKey, TValue> element, KeyedReaderWriterLockSlim<TKey> loadLock, Func<TKey, TValue> loadMethod)
        {
            loadLock.EnterWriteLock(element.Key);
            try
            {
                element.LastRefreshTimestamp = DateTime.Now;
                TValue value = loadMethod(element.Key);
                element.Value = value;
                element.IsLoaded = true;
            }
            finally
            {
                loadLock.ExitWriteLock(element.Key);
            }
        }

        protected virtual void LoadElement(CachedItemInfo<TKey, TValue> element, KeyedReaderWriterLock<TKey> loadLock, Func<TKey, TValue> loadMethod)
        {
            throw new NotSupportedException();
        }
        protected virtual void Dispose(bool disposing)
        {
            bool flag = !this._isDisposed;
            if (flag)
            {
                if (disposing)
                {
                    bool flag2 = UtilityMethods_Module.IsSomething(this._timer);
                    if (flag2)
                    {
                        this._timer.Dispose();
                    }
                }
            }
            this._isDisposed = true;
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }


    public class TimestampCacheManager<TKey, TValue> : ICacheManager<TKey, TValue>, ISingleton
    {
        private TimeSpan _timeout;
        private DateTime _clearTimeStamp;
        private static ConfigurationContext<TimestampCacheManager<TKey, TValue>> _context = new ConfigurationContext<TimestampCacheManager<TKey, TValue>>();
        public static TimestampCacheManager<TKey, TValue> DefaultInstance = Factory<TimestampCacheManager<TKey, TValue>>.Create(new object[0]);
        public const string TimeoutConfigName = "ContentTimeout";
        protected TimestampCacheManager()
        {
            this._timeout = ConfigValueManager.GetTimeSpanValue(TimestampCacheManager<TKey, TValue>._context, "ContentTimeout").GetValueOrDefault(TimeSpan.MinValue);
            bool flag = this._timeout < TimeSpan.Zero;
            if (flag)
            {
                throw new EFException(string.Format("Invalid timeout setting. Please add a valid timespan value to the config values for {0}, {1}.", typeof(TimestampCacheManager<TKey, TValue>).FullName, "ContentTimeout"), typeof(TimestampCacheManager<TKey, TValue>));
            }
        }
        public void Refresh(ref CachedItemInfo<TKey, TValue> element, Func<TKey, TValue> loadMethod, KeyedReaderWriterLockSlim<TKey> loadLock)
        {
            try
            {
                bool flag = this.ShouldRefresh(element);
                if (flag)
                {
                    loadLock.EnterWriteLock(element.Key);
                    try
                    {
                        flag = this.ShouldRefresh(element);
                        if (flag)
                        {
                            element.LastRefreshTimestamp = DateTime.Now;
                            TValue value = loadMethod(element.Key);
                            element.Value = value;
                            element.Value = value;
                            element.IsLoaded = true;
                        }
                    }
                    finally
                    {
                        loadLock.ExitWriteLock(element.Key);
                    }
                }
            }
            catch  
            {
               
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use overload with KeyedReaderWriterLockSlim.", true)]
        public void Refresh(ref CachedItemInfo<TKey, TValue> element, Func<TKey, TValue> loadMethod, KeyedReaderWriterLock<TKey> loadLock)
        {
            throw new NotSupportedException();
        }
        public void RefreshAll()
        {
            this._clearTimeStamp = DateTime.Now;
        }
        private bool ShouldRefresh(CachedItemInfo<TKey, TValue> element)
        {
            bool flag = !element.IsLoaded;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                flag = (DateTime.Compare(element.LastRefreshTimestamp, this._clearTimeStamp) < 0);
                if (flag)
                {
                    result = true;
                }
                else
                {
                    flag = (DateTime.Compare(element.LastRefreshTimestamp, DateTime.Now.Subtract(this._timeout)) < 0);
                    if (flag)
                    {
                        result = true;
                    }
                    else
                    {
                        flag = (this._timeout == TimeSpan.Zero);
                        result = flag;
                    }
                }
            }
            return result;
        }
    }

  

}
