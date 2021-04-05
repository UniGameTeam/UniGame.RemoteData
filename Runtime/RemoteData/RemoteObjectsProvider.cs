using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;

namespace UniModules.UniGame.RemoteData.RemoteData
{

    public abstract class RemoteObjectsProvider : 
        IRemoteObjectsProvider
    {
        public const char PathDelimeter = '.';

        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();

        public ILifeTime LifeTime => _lifeTime;

        public void Dispose()
        {
            _lifeTime.Terminate();
            _lifeTime = null;
        }

        public abstract RemoteObjectHandler<T> GetRemoteObject<T>(string path);

        public abstract string GetIdForNewObject(string path);
    }
}
