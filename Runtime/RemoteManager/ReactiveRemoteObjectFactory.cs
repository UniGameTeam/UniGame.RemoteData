using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniModules.UniGame.RemoteData.MutableObject;
using UniModules.UniGame.RemoteData.RemoteData;
using UniModules.UniGame.RemoteData.Runtime.RemoteManager.Abstract;
using UnityEngine;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager
{
    [Serializable]
    public class ReactiveRemoteObjectFactory : IReactiveRemoteObjectFactory
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        public DefaultRemoteObjectFactory defaultFactory = new DefaultRemoteObjectFactory();
        
        [SerializeReference]
        public List<ISerializableRemoteFactory> factories = new List<ISerializableRemoteFactory>();

        public async UniTask<IReactiveRemoteObject<T>> Create<T>(IRemoteObjectHandler<T> dataHandler) 
            where T : class
        {
            return await Create<T>(dataHandler,null);
        }

        public async UniTask<IReactiveRemoteObject<T>> Create<T>(IRemoteObjectHandler<T> dataHandler, Func<T> defaultValue) 
            where T : class
        {
            var result = await CreateReactiveObject<T>(dataHandler,defaultValue);
            return result;
        } 

        private async UniTask<IReactiveRemoteObject<T>> CreateReactiveObject<T>(IRemoteObjectHandler<T> dataHandler,Func<T> defaultValue) 
            where T : class
        {
            var objectType = typeof(T);
            IReactiveRemoteObject<T> result = null;
            
            var factory = factories.FirstOrDefault(x => x.Validate(objectType));
            factory ??= defaultFactory;
            
            if (factory is IReactiveItemFactory factoryItem)
            {
                result = await factoryItem.Create(dataHandler, defaultValue);
            }

            return result;
        }
    }
}
