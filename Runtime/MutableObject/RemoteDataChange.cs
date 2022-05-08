using UniModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;

namespace UniModules.UniGame.RemoteData
{
    using System;
    using UniModules.UniCore.Runtime.ObjectPool.Runtime;

    public class RemoteDataChange : IDisposable
    {
        public string FieldName;
        public string FullPath;
        public object FieldValue;
        public Action<RemoteDataChange> ApplyCallback;

        public static RemoteDataChange Create(string FullPath,
                                                    string FieldName,
                                                    object FieldValue,
                                                    Action<RemoteDataChange> ApplyCallback)
        {
            var change = ClassPool.Spawn<RemoteDataChange>();
            change.FullPath = FullPath.Trim(RemoteObjectsProvider.PathDelimeter);
            change.FieldName = FieldName;
            change.FieldValue = FieldValue;
            change.ApplyCallback = ApplyCallback;
            return change;
        }

        public void Dispose() => this.DespawnClass();

    }
}
