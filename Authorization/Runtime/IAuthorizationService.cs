using UniModules.UniGameFlow.GameFlow.Runtime.Interfaces;
using UniRx;

namespace UniModules.UniGame.Authorization
{
    public interface IAuthorizationService : IGameService
    {
        IReactiveProperty<string> UserId { get; }
    }
}