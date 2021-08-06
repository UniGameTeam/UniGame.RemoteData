namespace UniModules.UniGame.RemoteData
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Core.Runtime.DataFlow.Interfaces;
    using Cysharp.Threading.Tasks;
    using MutableObject;
    using UniCore.Runtime.DataFlow;

    public abstract class RemoteObjectHandler<T> : IRemoteObjectHandler<T>
    {
        #region static data

        private static Dictionary<string, FieldInfo> _fieldInfoCache = new Dictionary<string, FieldInfo>();
        private static Dictionary<string, PropertyInfo> _propertyInfoCache = new Dictionary<string, PropertyInfo>();

        #endregion

        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();

        public object DeleteValueObject { get; }

        public T Object { get; protected set; }

        public ILifeTime LifeTime => _lifeTime;


        public RemoteObjectHandler(object deleteValueObject)
        {
            DeleteValueObject = deleteValueObject;
        }

        public void Dispose()
        {
            _lifeTime.Terminate();
            _lifeTime = null;
        }

        public abstract UniTask<RemoteObjectHandler<T>> LoadData(Func<T> initialDataProvider = null);

        public abstract RemoteDataChange CreateChange(string fieldName, object fieldValue);

        public async UniTask ApplyChange(RemoteDataChange change)
        {
            await ApplyChangeRemote(change);
        }

        public virtual UniTask ApplyChangesBatched(List<RemoteDataChange> changes)
        {
            throw new NotImplementedException($"Batch apply not implemented for type :: ${this.GetType().FullName}");
        }

        public void ApplyChangeLocal(RemoteDataChange change)
        {
            if (_fieldInfoCache.TryGetValue(change.FieldName, out var fieldInfo))
            {
                fieldInfo.SetValue(Object, change.FieldValue);
                return;
            }

            if (_propertyInfoCache.TryGetValue(change.FieldName, out var propertyInfo))
            {
                propertyInfo.SetValue(Object, change.FieldValue);
                return;
            }

            fieldInfo = typeof(T).GetField(change.FieldName);
            if (fieldInfo != null)
            {
                _fieldInfoCache.Add(change.FieldName, fieldInfo);
                fieldInfo.SetValue(Object, change.FieldValue);
                return;
            }

            propertyInfo = typeof(T).GetProperty(change.FieldName);
            if (propertyInfo != null)
            {
                _propertyInfoCache.Add(change.FieldName, propertyInfo);
                propertyInfo.SetValue(Object, change.FieldValue);
                return;
            }
        }

        #region abstract methods

        public abstract string GetDataId();

        public abstract string GetFullPath();

        protected abstract UniTask ApplyChangeRemote(RemoteDataChange change);

        public abstract UniTask ClearData();

        #endregion
    }
}