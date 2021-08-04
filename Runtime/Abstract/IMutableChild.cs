using System;
using System.Threading.Tasks;
using UniModules.UniGame.RemoteData.RemoteData;
using UniRx;

namespace UniModules.UniGame.RemoteData.MutableObject
{
    public interface IMutableChild<T> : IMutableChildBase
    {
        string FullPath { get; }
        
        IMutableChild<T> BindToSource(Func<T> getter, string fullPath, IRemoteChangesStorage storage);
        
        void UpdateChildData(string fieldName, object newValue);
        string GetChildPath(string objectName);
    }
}