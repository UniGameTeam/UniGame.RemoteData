namespace UniModules.UniGame.RemoteData
{
    using System;
    using Cysharp.Threading.Tasks;

    public interface IReactiveRemoteObjects
    {
        UniTask<TWrapper> CreateRemoteObject<TWrapper,TValue>(string path,
            Func<TValue> defaultValue = null)
            where TWrapper :class, IReactiveRemoteObject<TValue> 
            where TValue : class;
    }
}