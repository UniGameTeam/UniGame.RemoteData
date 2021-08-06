namespace UniModules.UniGame.RemoteData
{
    using Cysharp.Threading.Tasks;
    
    public sealed class EmptyRemoteObject : IRemoteObjectsProvider
    {
        public UniTask<IRemoteObjectHandler<T>> GetRemoteObjectAsync<T>(string path) => UniTask.FromResult<IRemoteObjectHandler<T>>(default);

        public string GetIdForNewObject(string path) => string.Empty;
    }
}