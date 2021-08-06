using UniGame.RemoteData.FirebaseModule;

namespace UniGame.RemoteData
{
    using System.Collections;
    using Sirenix.OdinInspector;
    using UniModules.UniGame.RemoteData;
    
#if UNITY_EDITOR
    using UniModules.Editor;
#endif
    
    public static class RemoteEditorData
    {
        private static ValueDropdownList<RemoteCollectionId> collectionIds = new ValueDropdownList<RemoteCollectionId>();
        
        public static IEnumerable GetCollectionsId()
        {
            RemoteDataSettings remoteDataSettings = null;
#if UNITY_EDITOR
            remoteDataSettings = AssetEditorTools.GetAsset<RemoteDataSettings>();
#endif
            collectionIds.Clear();

            if (!remoteDataSettings)
            {
                return collectionIds;
            }

            foreach (var remoteCollection in remoteDataSettings.remoteCollections)
            {
                var id = remoteCollection.collectionId;
                collectionIds.Add(id,(RemoteCollectionId)id);
            }

            return collectionIds;
        }
    }
}