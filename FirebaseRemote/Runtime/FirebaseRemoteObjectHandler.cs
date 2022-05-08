

namespace UniModules.UniGame.RemoteData
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Firebase.Firestore;
    using UniModules.UniCore.Runtime.ObjectPool.Runtime;
    using UniModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    
    public class FirebaseRemoteObjectHandler<T> : RemoteObjectHandler<T>
    {
        private readonly DocumentReference _documentReference;

        public FirebaseRemoteObjectHandler(DocumentReference documentReference)  : base(FieldValue.Delete)
        {
            _documentReference = documentReference;
        }

        public override async UniTask<RemoteObjectHandler<T>> LoadData(Func<T> initialDataProvider = null)
        {
            // TODO keep synced
            var snapshot = await _documentReference.GetSnapshotAsync();
            if (!snapshot.Exists)
            {
                var initialData = initialDataProvider != null ? initialDataProvider() : default;
                await _documentReference.SetAsync(initialData, SetOptions.Overwrite);
                Object = initialData;
            }
            else
            {
                Object = snapshot.ConvertTo<T>();
            }
            return this;
        }

        public override RemoteDataChange CreateChange(string fieldName, object fieldValue)
        {
            return RemoteDataChange.Create(GetFullPath() + fieldName, fieldName, fieldValue, null);
        }

        public override string GetDataId() => _documentReference.Id;

        public override string GetFullPath() => string.Empty;

        protected override UniTask ApplyChangeRemote(RemoteDataChange change)
        {
            var value = change.FieldValue;
            return _documentReference.UpdateAsync(change.FullPath, value).AsUniTask();
        }

        public override async UniTask ApplyChangesBatched(List<RemoteDataChange> changes)
        {
            var changesDictionary = ClassPool.Spawn<Dictionary<string, object>>();
            foreach (var change in changes)
                changesDictionary[change.FullPath] = change.FieldValue;
            
            try
            {
                await _documentReference.UpdateAsync(changesDictionary).AsUniTask();
            }
            finally
            {
                changesDictionary.Clear();
                changesDictionary.Despawn();
            }

        }

        public override async UniTask ClearData()
        {
            await _documentReference.DeleteAsync();
        }
    }
}