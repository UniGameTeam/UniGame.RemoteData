using System.Collections.Generic;
using UniModules.UniGame.RemoteData;

namespace UniModules.UniGame.RemoteData
{
    public interface IRemoteCollectionsData
    {
        IEnumerable<string> CollectionsIds { get; }
        IEnumerable<RemoteCollectionsInfo> CollectionsInfo { get; }

    }
}