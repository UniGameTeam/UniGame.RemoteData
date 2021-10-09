using Cysharp.Threading.Tasks;
using UnityEditor;

namespace UniModules.UniGame.RemoteData
{
    using System;
    using RemoteData;

    public class PlayerPrefsRemoteObjectProvider : RemoteObjectsProvider
    {
        public override UniTask<IRemoteObjectHandler<T>> GetRemoteObjectAsync<T>(string path)
        {
            return UniTask.FromResult<IRemoteObjectHandler<T>>(new PlayerPrefsRemoteObjectHandler<T>(path));
        }

        public override string GetIdForNewObject(string path) => Guid.NewGuid().ToString();

        public override IRemoteObjectsProvider Create() => new PlayerPrefsRemoteObjectProvider();
    }
}