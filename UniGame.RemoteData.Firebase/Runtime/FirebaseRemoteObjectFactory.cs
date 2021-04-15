using UniModules.UniGame.RemoteData.RemoteData;
using UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.FirebaseRemote
{
    
    public class FirebaseRemoteObjectFactory : IRemoteObjectFactory
    {

        public IRemoteObjectsProvider Create(string id)
        {
            return new FirebaseRemoteObjectProvider(id);
        }
        
    }
    
}


