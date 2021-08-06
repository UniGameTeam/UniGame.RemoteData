namespace UniModules.UniGame.RemoteData
{
    using System;
    using Cysharp.Threading.Tasks;
    using UniModules.UniGame.Core.Runtime.Common;
    
    public interface IRemoteObjects : IDisposableLifetimeContext
    {
        
        UniTask<TWrapper> CreateRemoteObject<TWrapper,TValue>(string path,
            Func<TValue> defaultValue = null)
            where TWrapper :class, IReactiveRemoteObject<TValue> 
            where TValue : class;

        IRemoteObjectsProvider GetRemoteProvider(string path);

        IRemoteObjects RegisterRemoteProvider(string path, IRemoteObjectsProvider provider);

        IRemoteObjects RemoveRemoteProvider(string path);
        
        UniTask<IRemoteObjectHandler<T>> GetHandler<T>(string path);
        
        bool Validate(string collectionId);
    }
}