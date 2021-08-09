using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniCore.Runtime.Utils;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;

namespace UniModules.UniGame.RemoteData
{
    public class RemoteObjectsMap : IRemoteObjects
    {
        #region static data

        private static char[] separators = {'\\', '/'};

        private static MemorizeItem<string, string> GetPathId = MemorizeTool.Memorize<string, string>(path =>
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var id = path.Split(separators).FirstOrDefault();
            return string.IsNullOrEmpty(id) 
                ? string.Empty 
                : id.TrimEnd(separators);
        });
        
        public static readonly EmptyRemoteObject EmptyRemoteObject = new EmptyRemoteObject();
        
        
        #endregion
        
        private readonly IRemoteObjectsProvider _defaultObjectsProvider;
        private readonly LifeTimeDefinition _lifeTime = new LifeTimeDefinition();
        private readonly Dictionary<string,IRemoteObjectsProvider> _objectsProviders = new Dictionary<string, IRemoteObjectsProvider>(16);

        public RemoteObjectsMap(IRemoteObjectsProvider defaultObjectsProvider)
        {
            _defaultObjectsProvider = defaultObjectsProvider;
            _lifeTime.AddCleanUpAction(_objectsProviders.Clear);
        }

        public ILifeTime LifeTime => _lifeTime;

        public IRemoteObjects RegisterRemoteProvider(string path, IRemoteObjectsProvider provider)
        {
            var id = GetPathId[path];
            _objectsProviders[id] = provider;
            return this;
        }

        public IRemoteObjects RemoveRemoteProvider(string path)
        {
            var id = GetPathId[path];
            _objectsProviders.Remove(id);
            return this;
        }
        
        public IRemoteObjectsProvider GetRemoteProvider(string path)
        {
            var id = GetPathId[path];
            if (_objectsProviders.TryGetValue(id, out var provider))
                return provider;

            if (_defaultObjectsProvider == null || !Validate(id))
                return EmptyRemoteObject;

            return _defaultObjectsProvider;
        }

        public async UniTask<IRemoteObjectHandler<T>> GetHandler<T>(string path)
        {
            var id = GetPathId[path];
            var remoteObject = GetRemoteProvider(id);
            return await remoteObject.GetRemoteObjectAsync<T>(path);
        }

        public void Dispose() => _lifeTime.Terminate();

        public virtual bool Validate(string collectionId)
        {
            return !string.IsNullOrEmpty(collectionId);
        }
    }
}
