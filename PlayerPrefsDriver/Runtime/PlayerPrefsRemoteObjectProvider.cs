using UnityEditor;

namespace UniModules.UniGame.RemoteData.Runtime
{
    using RemoteData;

    public class PlayerPrefsRemoteObjectProvider : RemoteObjectsProvider
    {
        public override RemoteObjectHandler<T> GetRemoteObject<T>(string path)
        {
            return new PlayerPrefsRemoteObjectHandler<T>(path);
        }

        public override string GetIdForNewObject(string path) => GUID.Generate().ToString();

    }
}