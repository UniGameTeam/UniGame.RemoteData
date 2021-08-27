

namespace UniModules.UniGame.RemoteData
{
    using Authorization;
    using Cysharp.Threading.Tasks;
    using UniModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using UniModules.UniGame.CoreModules.UniGame.Context.Runtime.Extension;
    using UniModules.UniGameFlow.GameFlow.Runtime.Services;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    
#if UNITY_EDITOR
    using UniModules.Editor;
    using Editor;
#endif
    
    [CreateAssetMenu(menuName = "UniGame/RemoteData/FirebaseRemote", fileName = nameof(RemoteDataServiceSource))]
    public class RemoteDataServiceSource : ServiceDataSourceAsset<IRemoteDataService>
    {
        #region inspector

        public AssetReferenceT<RemoteDataSettings>  remoteCollectionsData;

        #endregion

        protected override async UniTask<IRemoteDataService> CreateServiceInternalAsync(IContext context)
        {
            var remoteData = await remoteCollectionsData.LoadAssetTaskAsync(LifeTime);
            var authService = await context.ReceiveFirstAsync<IAuthorizationService>();
            
            var reactiveObjectsFactory = remoteData.reactiveObjectsFactory;
            var defaultObjectsProvider = remoteData.defaultObjectsProvider.Create();
            var remoteObjects = new RemoteObjectsMap(defaultObjectsProvider);
            var remoteService = new RemoteDataService(authService,reactiveObjectsFactory,remoteObjects);

            context.Publish<IRemoteObjects>(remoteService);
            
            return remoteService;
        }

#if UNITY_EDITOR

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button("Validate")]
#endif
        public void OnValidate()
        {
            if (remoteCollectionsData.editorAsset != null)
            {
                ValidateAssets();
                return;
            }

            var remoteCollections = AssetEditorTools.GetAsset<RemoteDataSettings>();
            if (remoteCollections == null)
            {
                Debug.LogError("You should init RemoteCollectionsData");
                return;
            }
            remoteCollectionsData = new AssetReferenceT<RemoteDataSettings>(remoteCollections.GetGUID());
            this.MarkDirty();

            ValidateAssets();
        }

        private void ValidateAssets()
        {
            remoteCollectionsData.editorAsset.Verify();
        }
        
#endif
    }
}