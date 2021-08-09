using System;
using Cysharp.Threading.Tasks;

namespace UniModules.UniGame.RemoteData
{
    using Core.Runtime.Interfaces;

    public interface IReactiveItemFactory : IValidator<Type> 
    {
        UniTask<IReactiveRemoteObject<TData>> Create<TData>(IRemoteObjectHandler<TData> dataHandler, Func<TData> defaultValue);
    }
}