using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.RemoteData.MutableObject;
using UniModules.UniGame.RemoteData.RemoteData;
using UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager
{
    public class RemoteObjectsProvider : IRemoteObjects
    {
        public static readonly EmptyRemoteObject EmptyRemoteObject = new EmptyRemoteObject();
    
        private readonly IRemoteObjectFactory _providerFactory;
        private readonly IReactiveRemoteObjectFactory _reactiveRemoteObjectFactory;
        private readonly LifeTimeDefinition _lifeTime = new LifeTimeDefinition();
        private readonly Dictionary<string,IRemoteObjectsProvider> _objectsProviders = new Dictionary<string, IRemoteObjectsProvider>(16);

        public RemoteObjectsProvider(
            IRemoteObjectFactory providerFactory,
            IReactiveRemoteObjectFactory reactiveRemoteObjectFactory)
        {
            _providerFactory = providerFactory;
            _reactiveRemoteObjectFactory = reactiveRemoteObjectFactory;
            _lifeTime.AddCleanUpAction(_objectsProviders.Clear);
        }

        public ILifeTime LifeTime => _lifeTime;

        public async UniTask<TWrapper> CreateRemoteObject<TWrapper,TValue>(
            string id,
            string path,
            Func<TValue> defaultValue = null)
            where TWrapper :class, IReactiveRemoteObject<TValue> 
            where TValue : class
        {
            var dataHandler = await GetHandler<TValue>(id, path);
            var reactiveObject = await _reactiveRemoteObjectFactory.Create(dataHandler,defaultValue);
            return reactiveObject as TWrapper;
        }
    
        public IRemoteObjectsProvider GetRemoteProvider(string id)
        {
            if (_objectsProviders.TryGetValue(id, out var provider))
                return provider;

            if (!Validate(id))
                return EmptyRemoteObject;
        
            provider = _providerFactory.Create(id);
        
            _lifeTime.AddDispose(provider);
            _objectsProviders[id] = provider;
        
            return provider;
        }

        public async UniTask<RemoteObjectHandler<T>> GetHandler<T>(string id,string path)
        {
            var remoteObject = GetRemoteProvider(id);
            return await UniTask.FromResult(remoteObject.GetRemoteObject<T>(path));
        }

        public void Dispose() => _lifeTime.Terminate();

        public virtual bool Validate(string collectionId)
        {
            return true;
        }
    }
}
