using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.RemoteData.RemoteData;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager
{
    public sealed class EmptyRemoteObject : IRemoteObjectsProvider
    {
    
        public ILifeTime LifeTime => UniModules.UniCore.Runtime.DataFlow.LifeTime.TerminatedLifetime;
    
        public void Dispose() {}

        public RemoteObjectHandler<T> GetRemoteObject<T>(string path) => default;

        public string GetIdForNewObject(string path) => string.Empty;
    }
}