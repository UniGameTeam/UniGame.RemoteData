using System.Collections.Generic;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract
{
    public interface IRemoteCollectionsData
    {
        IEnumerable<string> CollectionsIds { get; }
        IEnumerable<RemoteCollectionsInfo> CollectionsInfo { get; }

        public RemoteCollectionsInfo GetRemoteCollection(string id);

    }
}