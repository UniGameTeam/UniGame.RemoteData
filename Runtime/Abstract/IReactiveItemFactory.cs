using System;
using Cysharp.Threading.Tasks;

namespace UniModules.UniGame.RemoteData
{
    public interface IReactiveItemFactory : ISerializableRemoteFactory 
    {
        UniTask<IReactiveRemoteObject<TData>> Create<TData>(IRemoteObjectHandler<TData> dataHandler, Func<TData> defaultValue);
    }
}