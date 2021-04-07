using System;
using UniModules.UniGame.Core.Runtime.Interfaces;

namespace UniModules.UniGame.RemoteData.RemoteData
{
    public interface IRemoteObjectsProvider : 
        ILifeTimeContext,
        IDisposable
    {
        RemoteObjectHandler<T> GetRemoteObject<T>(string path);
        string GetIdForNewObject(string path);
    }
}