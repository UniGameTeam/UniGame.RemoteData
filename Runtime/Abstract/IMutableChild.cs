using System;

namespace UniModules.UniGame.RemoteData
{
    public interface IMutableChild<T> : IMutableChildBase
    {
        string FullPath { get; }
        
        IMutableChild<T> BindToSource(Func<T> getter, string fullPath, IRemoteChangesStorage storage);
        
        void UpdateChildData(string fieldName, object newValue);
        
        string GetChildPath(string objectName);
    }
}