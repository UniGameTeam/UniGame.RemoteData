using Cysharp.Threading.Tasks;

namespace UniModules.UniGame.RemoteData
{
    using System;
    using Firebase.Firestore;
    
    
    [Serializable]
    public class FirebaseRemoteObjectProvider : RemoteObjectsProvider
    {
        
        public override UniTask<IRemoteObjectHandler<T>> GetRemoteObjectAsync<T>(string path)
        {
            var reference = FirebaseFirestore
                .DefaultInstance
                .Document(path);
            return UniTask.FromResult<IRemoteObjectHandler<T>>(new FirebaseRemoteObjectHandler<T>(reference));
        }

        public override string GetIdForNewObject(string path) => throw new NotImplementedException();
        
        public override IRemoteObjectsProvider Create()
        {
            return new FirebaseRemoteObjectProvider();
        }
    }
}