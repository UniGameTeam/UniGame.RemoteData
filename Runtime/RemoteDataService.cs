using System;
using Cysharp.Threading.Tasks;
using UniGame.UniNodes.GameFlow.Runtime;
using UniModules.UniGame.Authorization;
using UniRx;

namespace UniModules.UniGame.RemoteData
{
    [Serializable]
    public class RemoteDataService : GameService, IRemoteDataService
    {
        private readonly IAuthorizationService        _authorizationService;
        private readonly IReactiveRemoteObjectFactory _reactiveRemoteObjectFactory;
        private readonly IRemoteObjects               _remoteData;

        public RemoteDataService(IAuthorizationService authorizationService,IReactiveRemoteObjectFactory reactiveRemoteObjectFactory, IRemoteObjects remoteData)
        {
            _authorizationService             = authorizationService;
            _reactiveRemoteObjectFactory = reactiveRemoteObjectFactory;
            _remoteData                       = remoteData;

            Complete();
        }

        public IReadOnlyReactiveProperty<string> UserId => _authorizationService.UserId;


        public async UniTask<TWrapper> CreateRemoteObject<TWrapper, TValue>(string path, Func<TValue> defaultValue = null)
            where TWrapper : class, IReactiveRemoteObject<TValue>
            where TValue : class
        {
            var dataHandler    = await GetHandler<TValue>(path);
            var reactiveObject = await _reactiveRemoteObjectFactory.Create(dataHandler, defaultValue);
            return reactiveObject as TWrapper;
        }

        public IRemoteObjectsProvider GetRemoteProvider(string path)
        {
            return _remoteData.GetRemoteProvider(path);
        }

        public IRemoteObjects RegisterRemoteProvider(string path, IRemoteObjectsProvider provider)
        {
            return _remoteData.RegisterRemoteProvider(path, provider);
        }

        public IRemoteObjects RemoveRemoteProvider(string path)
        {
            return _remoteData.RemoveRemoteProvider(path);
        }

        public async UniTask<IRemoteObjectHandler<T>> GetHandler<T>(string path)
        {
            return await _remoteData.GetHandler<T>(path);
        }

    }
}