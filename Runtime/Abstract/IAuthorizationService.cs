using UniModules.UniGameFlow.GameFlow.Runtime.Interfaces;
using UniRx;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract
{
    public interface IAuthorizationService : IGameService
    {
        IReactiveProperty<string> UserId { get; }
    }
}