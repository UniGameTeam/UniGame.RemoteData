namespace UniGame.RemoteData.FirebaseModule
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.OdinInspector;
    using UniModules.UniGame.RemoteData.Runtime.RemoteManager;
    
    
    
#if UNITY_EDITOR
    using UniModules.Editor;
#endif
    
    public static class FirebaseEditorData
    {
        private static ValueDropdownList<FirebaseCollectionId> collectionIds = new ValueDropdownList<FirebaseCollectionId>();
        
        public static IEnumerable GetCollectionsId()
        {
            RemoteCollectionsData remoteCollectionsData = null;
#if UNITY_EDITOR
            remoteCollectionsData = AssetEditorTools.GetAsset<RemoteCollectionsData>();
#endif
            collectionIds.Clear();

            if (!remoteCollectionsData)
            {
                return collectionIds;
            }

            foreach (var remoteCollection in remoteCollectionsData.remoteCollections)
            {
                var id = remoteCollection.collectionId;
                collectionIds.Add(id,(FirebaseCollectionId)id);
            }

            return collectionIds;
        }
    }
}