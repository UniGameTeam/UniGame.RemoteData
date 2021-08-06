namespace UniModules.UniGame.RemoteData
{
    using System;
    using Cysharp.Threading.Tasks;
    
    public interface IReactiveRemoteObject<T> : IRemoteChangesStorage
    {
        UniTask LoadRootData(Func<T> initialDataProvider = null);
        
        public IReactiveRemoteObject<T> BindToSource(IRemoteObjectHandler<T> objectHandler);
        
    }
}