using System;
using Cysharp.Threading.Tasks;
using UniModules.UniGame.RemoteData.RemoteData;

namespace UniModules.UniGame.RemoteData.MutableObject
{
    public interface IReactiveRemoteObject<T> : IRemoteChangesStorage
    {
        UniTask LoadRootData(Func<T> initialDataProvider = null);
        
        public IReactiveRemoteObject<T> BindToSource(IRemoteObjectHandler<T> objectHandler);
        
    }
}