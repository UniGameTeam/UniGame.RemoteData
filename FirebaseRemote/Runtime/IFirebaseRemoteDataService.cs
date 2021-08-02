using UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract;
using UniModules.UniGameFlow.GameFlow.Runtime.Interfaces;
using UniRx;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.FirebaseRemote
{
    public interface IFirebaseRemoteDataService : IGameService,IRemoteObjects
    {
        public IReadOnlyReactiveProperty<string> UserId { get; } 
    }
}