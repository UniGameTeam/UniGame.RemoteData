namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.FirebaseRemote
{
    using RemoteData;
    using Abstract;

    public class FirebaseRemoteObjectFactory : IRemoteObjectFactory
    {

        public IRemoteObjectsProvider Create(string id)
        {
            return new FirebaseRemoteObjectProvider(id);
        }
        
    }
    
}


