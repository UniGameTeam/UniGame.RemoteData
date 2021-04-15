using System;
using Cysharp.Threading.Tasks;
using UniModules.UniGame.Core.Runtime.ScriptableObjects;
using UniModules.UniGame.RemoteData.MutableObject;
using UniModules.UniGame.RemoteData.RemoteData;
using UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract;
using UnityEngine;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.FirebaseRemote
{
    [CreateAssetMenu(menuName = "UniGame/RemoteData/FirebaseRemote", fileName = nameof(FirebaseRemoteData))]
    public class FirebaseRemoteData : LifetimeScriptableObject, IRemoteObjects
    {
        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
        public RemoteCollectionsData remoteCollectionsData;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
        public RemoteObjectsFactoryData remoteObjectsFactory;

        #endregion

        private IRemoteObjects _remoteObjectsProvider;
        private FirebaseAuthService _authService;

        public string UserId { get; private set; } = string.Empty;

        public IRemoteObjects RemoteObjectsProvider => _remoteObjectsProvider;
        
        #region public methods
        
        public async UniTask<bool> Initialize()
        {
            _authService = new FirebaseAuthService();
            var result = await _authService.InitialAuthorize();

            var authComplete = result.Result;
            if (!authComplete) return false;

            UserId = result.UserId;
            
            _remoteObjectsProvider = new RemoteCollectionsMap(
                remoteCollectionsData,
                new FirebaseRemoteObjectFactory(),
                remoteObjectsFactory);

            return true;
        }


        public async UniTask<TWrapper> CreateRemoteObject<TWrapper, TValue>(string id, string path,
            Func<TValue> defaultValue = null)
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
        
        #endregion
    }
}