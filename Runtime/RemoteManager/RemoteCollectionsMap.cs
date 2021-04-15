using UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager
{
    public class RemoteCollectionsMap : RemoteObjectsProvider
    {
        private readonly IRemoteCollectionsData _collectionsData;

        public RemoteCollectionsMap(
            IRemoteCollectionsData collectionsData,
            IRemoteObjectFactory providerFactory,
            IReactiveRemoteObjectFactory reactiveRemoteObjectFactory)
            : base(providerFactory,reactiveRemoteObjectFactory)
        {
            _collectionsData = collectionsData;
        }


        public override bool Validate(string collectionId)
        {
            return _collectionsData.GetRemoteCollection(collectionId) != null;
        }
    }
}


