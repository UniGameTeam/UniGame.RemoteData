using UniGame.UniNodes.GameFlow.Runtime;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.FirebaseRemote
{
    using System;
    using Cysharp.Threading.Tasks;
    using UniModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using UniModules.UniGame.CoreModules.UniGame.Context.Runtime.Extension;
    using MutableObject;
    using RemoteData;
    using Abstract;
    using UniModules.UniGameFlow.GameFlow.Runtime.Services;
    using UniRx;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    
#if UNITY_EDITOR
    using Editor;
#endif
    
    [CreateAssetMenu(menuName = "UniGame/RemoteData/FirebaseRemote", fileName = nameof(FirebaseRemoteServiceSource))]
    public class FirebaseRemoteServiceSource : ServiceDataSourceAsset<IFirebaseRemoteDataService>
    {
        #region inspector

        public AssetReferenceT<RemoteCollectionsData>  remoteCollectionsData;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
        [Sirenix.OdinInspector.HideLabel]
#endif
        public RemoteObjectsFactoryData remoteObjectsFactory;

        #endregion

        private IRemoteObjects _remoteObjectsProvider;

        public IRemoteObjects RemoteObjectsProvider => _remoteObjectsProvider;
        
        protected override async UniTask<IFirebaseRemoteDataService> CreateServiceInternalAsync(IContext context)
        {
            var remoteData = await remoteCollectionsData.LoadAssetTaskAsync(context.LifeTime);
            var authService = await context.ReceiveFirstAsync<IAuthorizationService>();

            _remoteObjectsProvider = new RemoteCollectionsMap(remoteData, new FirebaseRemoteObjectFactory(), remoteObjectsFactory);

            var remoteService = new FirebaseRemoteService(authService, _remoteObjectsProvider);
            
            context.Publish<IRemoteObjects>(remoteService);
            return remoteService;
        }


        public void OnValidate()
        {
#if UNITY_EDITOR
            if (remoteCollectionsData.editorAsset != null) return;

            var remoteCollections = AssetEditorTools.GetAsset<RemoteCollectionsData>();
            if (remoteCollections == null)
            {
                Debug.LogError("You should init RemoteCollectionsData");
                return;
            }
            remoteCollectionsData = new AssetReferenceT<RemoteCollectionsData>(remoteCollections.GetGUID());
            this.MarkDirty();
#endif

        }
    }


    public class FirebaseRemoteService : GameService , IFirebaseRemoteDataService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IRemoteObjects _remoteObjectsProvider;

        public FirebaseRemoteService(IAuthorizationService authorizationService, IRemoteObjects remoteObjectsProvider)
        {
            _authorizationService = authorizationService;
            _remoteObjectsProvider = remoteObjectsProvider;
            
            
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