using System;
using Cysharp.Threading.Tasks;
using UniModules.UniGame.RemoteData.MutableObject;
using UniModules.UniGame.RemoteData.RemoteData;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract
{
    public interface IReactiveItemFactory<TData> : ISerializableRemoteFactory 
        where TData : class
    {
        UniTask<IReactiveRemoteObject<TData>> Create(IRemoteObjectHandler<TData> dataHandler, Func<TData> defaultValue);
    }
}