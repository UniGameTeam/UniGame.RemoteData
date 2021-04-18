using UniModules.UniGame.RemoteData.RemoteData;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract
{
    public interface IRemoteObjectFactory
    {
        IRemoteObjectsProvider Create(string id);
    }
}