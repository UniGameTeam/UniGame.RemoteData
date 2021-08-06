namespace UniModules.UniGame.RemoteData
{
    using System;
    using Cysharp.Threading.Tasks;

    public interface IReactiveRemoteObjectFactory
    {
    
        UniTask<IReactiveRemoteObject<T>> Create<T>(IRemoteObjectHandler<T> dataHandler)where T : class;
    
        UniTask<IReactiveRemoteObject<T>> Create<T>(IRemoteObjectHandler<T> dataHandler, Func<T> defaultValue)where T : class;
    
    }
}