using System;
using Firebase.Firestore;
using UniModules.UniGame.RemoteData.RemoteData;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.FirebaseRemote
{
    public class FirebaseRemoteObjectProvider : RemoteData.RemoteObjectsProvider
    {
        
        public override RemoteObjectHandler<T> GetRemoteObject<T>(string path)
        {
            var reference = FirebaseFirestore.DefaultInstance.Document(path);
            return new FirebaseRemoteObjectHandler<T>(reference);
        }

        public override string GetIdForNewObject(string path) => throw new NotImplementedException();

    }
}