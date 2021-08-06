using System;
using Cysharp.Threading.Tasks;

namespace UniModules.UniGame.RemoteData
{

    [Serializable]
    public abstract class RemoteObjectsProvider : 
        IRemoteObjectsProvider,
        IRemoteObjectsProviderFactory
    {
        public const char PathDelimeter = '.';

        public abstract UniTask<IRemoteObjectHandler<T>> GetRemoteObjectAsync<T>(string path);

        public abstract string GetIdForNewObject(string path);

        public abstract IRemoteObjectsProvider Create();
    }
}
