namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.FirebaseRemote
{
    using UniModules.UniGame.AddressableTools.Runtime.Attributes;
    using UniGame;
    using Cysharp.Threading.Tasks;
    using UniModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using UniModules.UniGame.CoreModules.UniGame.Context.Runtime.Extension;
    using Abstract;
    using UniModules.UniGameFlow.GameFlow.Runtime.Services;
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

        [SerializeReference]
        public IReactiveRemoteObjectFactory remoteObjectFactory = new ReactiveRemoteObjectFactory();

        #endregion

        private IRemoteObjects _remoteObjectsProvider;

        public IRemoteObjects RemoteObjectsProvider => _remoteObjectsProvider;
        
        protected override async UniTask<IFirebaseRemoteDataService> CreateServiceInternalAsync(IContext context)
        {
            var remoteData = await remoteCollectionsData.LoadAssetTaskAsync(LifeTime);
            var authService = await context.ReceiveFirstAsync<IAuthorizationService>();

            _remoteObjectsProvider = new RemoteCollectionsMap(remoteData, new FirebaseRemoteObjectFactory(), remoteObjectFactory);

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
            
            if(remoteObjectFactory is IVerifiable verifiable)
                verifiable.Verify();
#endif

        }
    }
}