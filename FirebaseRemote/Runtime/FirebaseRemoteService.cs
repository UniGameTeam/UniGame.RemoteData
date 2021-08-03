using System;
using Cysharp.Threading.Tasks;
using UniGame.UniNodes.GameFlow.Runtime;
using UniModules.UniGame.RemoteData.MutableObject;
using UniModules.UniGame.RemoteData.RemoteData;
using UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract;
using UniRx;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.FirebaseRemote
{
    [Serializable]
    public class FirebaseRemoteService : GameService , IFirebaseRemoteDataService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IRemoteObjects _remoteObjectsProvider;

        public FirebaseRemoteService(IAuthorizationService authorizationService, IRemoteObjects remoteObjectsProvider)
        {
            _authorizationService = authorizationService;
            _remoteObjectsProvider = remoteObjectsProvider;
            
            Complete();
        }
        
        public IReadOnlyReactiveProperty<string> UserId => _authorizationService.UserId;
        
        public async UniTask<TWrapper> CreateRemoteObject<TWrapper, TValue>(string id, string path, Func<TValue> defaultValue = null)
            where TWrapper : class, IReactiveRemoteObject<TValue> where TValue : class
        {
            return await _remoteObjectsProvider.CreateRemoteObject<TWrapper, TValue>(id, path, defaultValue);
        }

        public IRemoteObjectsProvider GetRemoteProvider(string id)
        {
            return _remoteObjectsProvider.GetRemoteProvider(id);
        }

        public async UniTask<RemoteObjectHandler<T>> GetHandler<T>(string id, string path)
        {
            return await _remoteObjectsProvider.GetHandler<T>(id, path);
        }

        public bool Validate(string collectionId)
        {
            return _remoteObjectsProvider.Validate(collectionId);
        }

    }
}