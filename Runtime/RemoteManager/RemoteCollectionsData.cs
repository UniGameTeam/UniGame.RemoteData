using System.Collections.Generic;
using System.Linq;
using UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract;
using UnityEngine;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager
{
    [CreateAssetMenu(menuName = "UniGame/RemoteData/RemoteCollections",fileName = nameof(RemoteCollectionsData))]
    public class RemoteCollectionsData : ScriptableObject, IRemoteCollectionsData
    {

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
#endif
        public List<RemoteCollectionsInfo> remoteCollections = new List<RemoteCollectionsInfo>();


        public IEnumerable<string> CollectionsIds
        {
            get
            {
                foreach (var info in remoteCollections)
                {
                    yield return info.collectionId;
                }
            }
        }

        public IEnumerable<RemoteCollectionsInfo> CollectionsInfo => remoteCollections;

        public RemoteCollectionsInfo GetRemoteCollection(string id) => remoteCollections.FirstOrDefault(x => x.collectionId == id);
    }
}
