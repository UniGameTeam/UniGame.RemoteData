namespace UniModules.UniGame.RemoteData
{
    using Cysharp.Threading.Tasks;
    using Core.Runtime.Common;
    
    public interface IRemoteObjects : IDisposableLifetimeContext
    {

        IRemoteObjectsProvider GetRemoteProvider(string path);

        IRemoteObjects RegisterRemoteProvider(string path, IRemoteObjectsProvider provider);

        IRemoteObjects RemoveRemoteProvider(string path);
        
        UniTask<IRemoteObjectHandler<T>> GetHandler<T>(string path);
        
    }
}