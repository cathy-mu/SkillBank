using EF.Frameworks.Common.CollectionsEF;
using EF.Frameworks.Common.ExceptionsEF;
using EF.Frameworks.Common.FactoryEF;
using EF.Frameworks.Common.ThreadingEF;
using EF.Frameworks.Common.ConfigurationEF;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Threading;
using SkillBank.Site.Common;
using System.Web.Caching;

namespace SkillBank.Site.Common.Caching
{
    public abstract class CacheBase<TKey, TValue> : ISingleton
    {
        private SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> _cache;
        private KeyedReaderWriterLock<TKey> _contentLoadLock;
        private ICacheManager<TKey, TValue> _cacheManager;
        protected abstract TValue LoadItem(TKey key);
        //protected abstract Func<TKey, TValue> LoadItem();

        protected CacheBase(RefreshOptions refreshOption)
        {
            this._cache = new SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>>();
            this._contentLoadLock = new KeyedReaderWriterLock<TKey>();
            this._cacheManager = this.GetCacheManager(this._cache, this._contentLoadLock, refreshOption);
        }
        protected TValue GetItem(TKey key, Func<TKey, TValue> loadMethod)
        {
            CachedItemInfo<TKey, TValue> cachedItem = this.GetCachedItem(key, loadMethod);
            return cachedItem.Value;
        }
        protected TValue GetItem(TKey key)
        {
            return this.GetItem(key, this.LoadItem);
        }


        protected virtual ICacheManager<TKey, TValue> GetCustomCacheManager(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> elementCache, KeyedReaderWriterLock<TKey> contentLoadLock)
        {
            throw new NotImplementedException();
        }
        private ICacheManager<TKey, TValue> GetCacheManager(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> elementCache, KeyedReaderWriterLock<TKey> contentLoadLock, RefreshOptions refreshOption)
        {
            switch (refreshOption)
            {
                //case RefreshOptions.OnDemand:
                //	return TimestampCacheManager<TKey, TValue>.DefaultInstance;
                case RefreshOptions.Asynchronous:
                    return new TimestampCacheManagerAsync<TKey, TValue>(elementCache, contentLoadLock, new ICacheManager<TKey, TValue>.LoadItemDelegate(this.LoadItem));
                case RefreshOptions.Custom:
                    return this.GetCustomCacheManager(elementCache, contentLoadLock);
                default:
                    throw new EFException("Refresh option not recognized.", this);
            }
			
        }
        private CachedItemInfo<TKey, TValue> GetCachedItem(TKey key, Func<TKey, TValue> loadMethod)
        {
            CachedItemInfo<TKey, TValue> cachedItemInfo = null;
            if (!this._cache.TryGetValue(key, ref cachedItemInfo))
            {
                this._cache.AcquireWriterLock();
                try
                {
                    if (!this._cache.TryGetValue(key, ref cachedItemInfo))
                    {
                        cachedItemInfo = new CachedItemInfo<TKey, TValue>(key);
                        this._cache[key] = cachedItemInfo;
                    }
                }
                finally
                {
                    this._cache.ReleaseWriterLock();
                }
            }
            this._cacheManager.Refresh(ref cachedItemInfo, loadMethod, this._contentLoadLock);
            cachedItemInfo.LastRequestedTimestamp = DateTime.Now;
            return cachedItemInfo;
        }
        private CachedItemInfo<TKey, TValue> GetCachedItem(TKey key)
        {
            return this.GetCachedItem(key, this.LoadItem);
        }
        protected void UncacheItem(TKey key)
        {
            this._contentLoadLock.AcquireWriterLock(key, -1);
            try
            {
                if (this._cache.ContainsKey(key))
                {
                    this._cache.Remove(key);
                }
            }
            finally
            {
                this._contentLoadLock.ReleaseWriterLock(key);
            }
        }
        protected void RefreshAll()
        {
            this._cacheManager.RefreshAll();
        }
        protected void RefreshItem(TKey key)
        {
            CachedItemInfo<TKey, TValue> cachedItemInfo = null;
            this._contentLoadLock.AcquireWriterLock(key, -1);
            try
            {
                if (this._cache.TryGetValue(key, ref cachedItemInfo))
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
                this._contentLoadLock.ReleaseWriterLock(key);
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

    

    public interface ICacheManager<TElementKey, TValue>
    {
        //public delegate TValue LoadItemDelegate(TElementKey key);
		
        void Refresh(ref CachedItemInfo<TElementKey, TValue> element, Func<TElementKey, TValue> loadMethod, KeyedReaderWriterLock<TElementKey> loadLock);
        void RefreshAll();
    }

    public enum RefreshOptions
    {
        OnDemand,
        Asynchronous,
        Custom
    }

    public class TimestampCacheManager<TKey, TValue> : ICacheManager<TKey, TValue>, ISingleton
    {
        private TimeSpan _timeout;
        private DateTime _clearTimeStamp;
        //private static ConfigurationContext<TimestampCacheManager<TKey, TValue>> _context = new ConfigurationContext<TimestampCacheManager<TKey, TValue>>();
        //public static TimestampCacheManager<TKey, TValue> DefaultInstance = Factory<TimestampCacheManager<TKey, TValue>>.Create(new object[0]);
        public const string TimeoutConfigName = "ContentTimeout";
        protected TimestampCacheManager()
        {
            this._timeout = new TimeSpan();
        }
        public void Refresh(ref CachedItemInfo<TKey, TValue> element, Func<TKey, TValue> loadMethod, KeyedReaderWriterLock<TKey> loadLock)
        {
            try
            {
                if (this.ShouldRefresh(element))
                {
                    loadLock.AcquireWriterLock(element.Key, -1);
                    try
                    {
                        if (this.ShouldRefresh(element))
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
                        loadLock.ReleaseWriterLock(element.Key);
                    }
                }
            }
            catch
            {
               
            }
        }
        public void RefreshAll()
        {
            this._clearTimeStamp = DateTime.Now;
        }
        private bool ShouldRefresh(CachedItemInfo<TKey, TValue> element)
        {
            return !element.IsLoaded || DateTime.Compare(element.LastRefreshTimestamp, this._clearTimeStamp) < 0 || DateTime.Compare(element.LastRefreshTimestamp, DateTime.Now.Subtract(this._timeout)) < 0 || this._timeout == TimeSpan.Zero;
        }
    }



    public class TimestampCacheManagerAsync<TKey, TValue> : ICacheManager<TKey, TValue>, IDisposable
    {
        private SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> _cache;
        private KeyedReaderWriterLock<TKey> _loadLock;
        //private ICacheManager<TKey, TValue>.LoadItemDelegate _loadMethod;
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
        public TimestampCacheManagerAsync(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> list, KeyedReaderWriterLock<TKey> loadLock, ICacheManager<TKey, TValue>.LoadItemDelegate loadMethod)
        {
            this._isDisposed = false;
            this._cache = list;
            this._loadLock = loadLock;
            this._loadMethod = loadMethod;
            TimeSpan defaultTimeout = this.GetDefaultTimeout();
            this.SetTimeout(defaultTimeout);
        }
        public TimestampCacheManagerAsync(SynchronizedDictionary<TKey, CachedItemInfo<TKey, TValue>> list, KeyedReaderWriterLock<TKey> loadLock, ICacheManager<TKey, TValue>.LoadItemDelegate loadMethod, TimeSpan timeout)
        {
            this._isDisposed = false;
            this._cache = list;
            this._loadLock = loadLock;
            this._loadMethod = loadMethod;
            this.SetTimeout(timeout);
        }
        public void Refresh(ref CachedItemInfo<TKey, TValue> element, ICacheManager<TKey, TValue>.LoadItemDelegate loadMethod, KeyedReaderWriterLock<TKey> loadLock)
        {
            if (this.ShouldRefreshInline(element))
            {
                loadLock.AcquireWriterLock(element.Key, -1);
                try
                {
                    if (this.ShouldRefreshInline(element))
                    {
                        this.LoadElement(element, loadLock, loadMethod);
                    }
                }
                finally
                {
                    loadLock.ReleaseWriterLock(element.Key);
                }
            }
        }
        public void RefreshAll()
        {
            this.RefreshTimer_Tick(true);
        }
        public void SetTimeout(TimeSpan timeout, bool setStopRefreshTimeout)
        {
            if (timeout < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("timeout", "Timeout must be greater than or equal to TimeSpan.Zero.");
            }
            this._timeout = timeout;
            if (setStopRefreshTimeout)
            {
                this._stopRefreshTimeout = this._timeout.Add(this._timeout);
            }
            if (timeout == TimeSpan.Zero)
            {
                if (this._timer != null)
                {
                    this._timer.Dispose();
                    this._timer = null;
                }
            }
            else
            {
                if (this._timer == null)
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
            if (stopRefreshTimeout > this.Timeout)
            {
                this._stopRefreshTimeout = stopRefreshTimeout;
            }
        }
        protected virtual TimeSpan GetDefaultTimeout()
        {
            string stringValue = ConfigValueManager.GetStringValue(TimestampCacheManagerAsync<TKey, TValue>._context, "ContentTimeout");
            TimeSpan minValue;
            if (stringValue == null)
            {
                minValue = TimeSpan.MinValue;
            }
            else
            {
                if (Versioned.IsNumeric(stringValue))
                {
                    minValue = new TimeSpan(0, 0, Conversions.ToInteger(stringValue));
                }
                else
                {
                    TimeSpan.TryParse(stringValue, out minValue);
                }
            }
            if (minValue < TimeSpan.Zero)
            {
                throw new EFException(string.Format("Invalid timeout setting. Please add a valid timespan value to the config values for {0}, {1}.", typeof(TimestampCacheManager<TKey, TValue>).FullName, "ContentTimeout"), typeof(TimestampCacheManager<TKey, TValue>));
            }
            return minValue;
        }
        private bool ShouldRefreshInline(CachedItemInfo<TKey, TValue> element)
        {
            return !element.IsLoaded || DateTime.Compare(element.LastRequestedTimestamp, DateAndTime.Now.Subtract(this._stopRefreshTimeout)) < 0 || this._timeout == TimeSpan.Zero;
        }
        private void RefreshTimer_Tick(object state)
        {
            bool throwException;
            if (state is bool)
            {
                throwException = Conversions.ToBoolean(state);
            }
            Monitor.Enter(this);
            try
            {
                if (this._isRefreshing)
                {
                    return;
                }
                this._isRefreshing = true;
            }
            finally
            {
                Monitor.Exit(this);
            }
            try
            {
                this.ReloadAllElements(throwException);
            }
            finally
            {
                this._isRefreshing = false;
            }
        }
        private void ReloadAllElements(bool throwException)
        {
            List<CachedItemInfo<TKey, TValue>> list = new List<CachedItemInfo<TKey, TValue>>();
            list.AddRange(this._cache.Values);
            try
            {
                List<CachedItemInfo<TKey, TValue>>.Enumerator enumerator = list.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    CachedItemInfo<TKey, TValue> current = enumerator.Current;
                    if (DateTime.Compare(current.LastRequestedTimestamp, DateAndTime.Now.Subtract(this._stopRefreshTimeout)) > 0)
                    {
                        this.LoadElement(current, this._loadLock, this._loadMethod);
                            continue;
                    }
                }
            }
            finally
            {
                List<CachedItemInfo<TKey, TValue>>.Enumerator enumerator;
                ((IDisposable)enumerator).Dispose();
            }
        }
        protected virtual void LoadElement(CachedItemInfo<TKey, TValue> element, KeyedReaderWriterLock<TKey> loadLock, ICacheManager<TKey, TValue>.LoadItemDelegate loadMethod)
        {
            loadLock.AcquireWriterLock(element.Key, -1);
            try
            {
                element.LastRefreshTimestamp = DateTime.Now;
                TValue value = loadMethod(element.Key);
                element.Value = value;
                element.IsLoaded = true;
            }
            finally
            {
                loadLock.ReleaseWriterLock(element.Key);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._isDisposed && disposing && UtilityMethods_Module.IsSomething(this._timer))
            {
                this._timer.Dispose();
            }
            this._isDisposed = true;
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }


}
