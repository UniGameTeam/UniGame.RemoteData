namespace UniModules.UniGame.RemoteData
{
    using UniModules.UniGameFlow.GameFlow.Runtime.Interfaces;
    using UniRx;
    
    public interface IRemoteDataService : IGameService,
        IRemoteObjects,
        IReactiveRemoteObjects
    {
        public IReadOnlyReactiveProperty<string> UserId { get; } 
    }
}