using Cysharp.Threading.Tasks;

namespace UniModules.UniGame.RemoteData
{
    public interface IRemoteObjectsProvider
    {
        UniTask<IRemoteObjectHandler<T>> GetRemoteObjectAsync<T>(string path);
        
        string GetIdForNewObject(string path);
    }
}