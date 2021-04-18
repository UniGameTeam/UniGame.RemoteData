using System;
using Cysharp.Threading.Tasks;
using UniModules.UniGame.Core.Runtime.Common;
using UniModules.UniGame.RemoteData.MutableObject;
using UniModules.UniGame.RemoteData.RemoteData;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract
{
    public interface IRemoteObjects : IDisposableLifetimeContext
    {
        
        UniTask<TWrapper> CreateRemoteObject<TWrapper,TValue>(
            string id,
            string path,
            Func<TValue> defaultValue = null)
            where TWrapper :class, IReactiveRemoteObject<TValue> 
            where TValue : class;

        IRemoteObjectsProvider GetRemoteProvider(string id);
        
        UniTask<RemoteObjectHandler<T>> GetHandler<T>(string id,string path);
        
        bool Validate(string collectionId);
    }
}