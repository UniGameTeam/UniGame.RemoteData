using System.Threading.Tasks;

namespace UniModules.UniGame.Authorization
{
    public abstract class AbstractAuthTokenProvider<T> where T : class, IAuthToken
    {
        public abstract Task<T> FetchToken();
    }
}
