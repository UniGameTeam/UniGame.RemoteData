using System.Collections.Generic;
using System.Linq;
using UniModules.UniGame.UniGame;
using UnityEngine;

namespace UniModules.UniGame.RemoteData
{
    [CreateAssetMenu(menuName = "UniGame/RemoteData/RemoteCollections",fileName = nameof(RemoteDataSettings))]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.InlineEditor]
#endif
    public class RemoteDataSettings : ScriptableObject, IRemoteCollectionsData,IVerifiable
    {

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
#endif
        public List<RemoteCollectionsInfo> remoteCollections = new List<RemoteCollectionsInfo>();
        
        [SerializeReference] 
        public RemoteObjectsProvider defaultObjectsProvider;
        
        [SerializeReference]
        public IReactiveRemoteObjectFactory reactiveObjectsFactory = new ReactiveRemoteObjectFactory();

        
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
        
        
        public void Verify()
        {
            if(reactiveObjectsFactory is IVerifiable verifiableFactory)
                verifiableFactory.Verify();
        }
    }
}
