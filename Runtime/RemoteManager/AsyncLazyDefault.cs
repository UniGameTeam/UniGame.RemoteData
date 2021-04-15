using Cysharp.Threading.Tasks;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager
{
    public class AsyncLazyDefault<TValue>
    {
        private static AsyncLazy<TValue> _asyncLazy;
        public static AsyncLazy<TValue> Create()
        {
            if (_asyncLazy != null) return _asyncLazy;
            _asyncLazy = new AsyncLazy<TValue>(() => UniTask.FromResult(default(TValue)));
            return _asyncLazy;
        }
    }
}