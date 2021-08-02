using System;
using Cysharp.Threading.Tasks;
using UniModules.UniGame.RemoteData.MutableObject;
using UniModules.UniGame.RemoteData.RemoteData;
using UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager
{

    [Serializable]
    public abstract class ReactiveItemFactory : IReactiveItemFactory
    {
        public virtual bool Validate(Type targetType) => true;

        public bool Validate<TData>() => Validate(typeof(TData));
        
        public async UniTask<IReactiveRemoteObject<TData>> Create<TData>(IRemoteObjectHandler<TData> dataHandler, Func<TData> defaultValue) 
            => await CreateReactiveObject<TData>(dataHandler,defaultValue);

        public abstract IReactiveRemoteObject<TData> CreateObject<TData>();

        private async UniTask<IReactiveRemoteObject<TData>> CreateReactiveObject<TData>(IRemoteObjectHandler<TData> dataHandler,Func<TData> defaultValue)
        {
            var objectType = typeof(TData);

            if (!Validate(objectType))
                return null;

            var model = CreateObject<TData>();
            model = model.BindToSource(dataHandler);
            
            await model.LoadRootData(defaultValue);
            
            return model;
        }
    }
    
        
    [Serializable]
    public class ReactiveItemFactory<TObject,TValue> : ReactiveItemFactory
        where TObject  : class, IReactiveRemoteObject<TValue>,new()
    {
        public override bool Validate(Type targetType) => typeof(TValue) == targetType;
        
        public sealed override IReactiveRemoteObject<TData> CreateObject<TData>()
        {
            return !Validate(typeof(TData)) ? null : CreateInternal<TData>();
        }

        protected virtual IReactiveRemoteObject<TData> CreateInternal<TData>()
        {
            return new TObject() as IReactiveRemoteObject<TData>;
        }
        
    }
    
    

}