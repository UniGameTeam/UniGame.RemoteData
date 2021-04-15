using System;
using Cysharp.Threading.Tasks;
using UniModules.UniGame.RemoteData.MutableObject;
using UniModules.UniGame.RemoteData.RemoteData;
using UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager
{
    [Serializable]
    public abstract class ReactiveItemFactory<TData> : IReactiveItemFactory<TData> 
        where TData : class
    {
        public virtual bool Validate(Type targetType) => targetType == typeof(TData);
        
        public async UniTask<IReactiveRemoteObject<TData>> Create(IRemoteObjectHandler<TData> dataHandler, Func<TData> defaultValue) 
            => await CreateReactiveObject(dataHandler,defaultValue);

        public abstract IReactiveRemoteObject<TData> CreateObject();

        private async UniTask<IReactiveRemoteObject<TData>> CreateReactiveObject(IRemoteObjectHandler<TData> dataHandler,Func<TData> defaultValue)
        {
            var objectType = typeof(TData);

            if (!Validate(objectType))
                return null;

            var model = CreateObject();
            model = model.BindToSource(dataHandler);
            
            var defaultFunc = defaultValue == null ?
                (Func<TData>)null : 
                () => defaultValue() as TData;
            
            await model.LoadRootData(defaultFunc);
            
            return model;
        }
    }
}