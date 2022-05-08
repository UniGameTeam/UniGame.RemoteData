using UniModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;

namespace UniModules.UniGame.RemoteData
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniRx;
    using UniTask = Cysharp.Threading.Tasks.UniTask;

    public class BaseMutableReactiveRemoteObjectFacade<T> : 
        IReactiveRemoteObject<T>, 
        IReactiveParentRemoteObject where T : class
    {
        private ConcurrentStack<RemoteDataChange> _pendingChanges = new ConcurrentStack<RemoteDataChange>();
        private Dictionary<string, INotifyable> _properties = new Dictionary<string, INotifyable>(8);
        private Dictionary<string, IRemoteChangesStorage> _childObjects = new Dictionary<string, IRemoteChangesStorage>(8);

        protected IRemoteObjectHandler<T> _objectHandler;

        public IReactiveProperty<bool> HaveNewChanges { get; } = new ReactiveProperty<bool>(false);
        
        public IReactiveRemoteObject<T> BindToSource(IRemoteObjectHandler<T> objectHandler)
        {
            _objectHandler = objectHandler;
            OnBindToSource(_objectHandler);
            return this;
        }
        
        /// <summary>
        /// Loads remote data. if not exits sets initialValue
        /// </summary>
        /// <param name="initialDataProvider"></param>
        /// <returns></returns>
        public async UniTask LoadRootData(Func<T> initialDataProvider = null)
        {
            //TODO RETURN VALUE
            var result = await _objectHandler.LoadData(initialDataProvider);
            AllPropertiesChanged();
        }

        public string GetId() => _objectHandler.GetDataId();

        public void UpdateChildData(string childName, object newData)
        {
            var change = _objectHandler.CreateChange(childName, newData);
            change.ApplyCallback = ApplyChangeOnLocalHandler;
            AddChange(change);
        }

        public void AddChange(RemoteDataChange change)
        {
            _pendingChanges.Push(change);
            ChangeApplied(change);
            HaveNewChanges.Value = true;
        }

        /// <summary>
        /// Отправляет все локально записанные изменения на сервер.
        /// ВАЖНО: все операции в рамках одной комманды не должны прерыватья вызовом
        /// метода
        /// </summary>
        /// <returns></returns>
        public async Task CommitChanges()
        {
            var changes = ClassPool.Spawn<List<RemoteDataChange>>();

            lock (_pendingChanges)
            {
                changes.AddRange(_pendingChanges);
                _pendingChanges.Clear();
                HaveNewChanges.Value = false;
            }

            changes.Reverse();
            
            await _objectHandler.ApplyChangesBatched(changes);
            
            changes.ForEach((ch) => ch.Dispose());
            changes.Despawn();
        }

        /// <summary>
        /// Возвращает список всех локальных изменений и обнуляет его внутри объекта
        /// применимо для работы с BatchUpdater
        /// </summary>
        /// <returns></returns>
        public List<RemoteDataChange> FlushChanges()
        {
            var result = _pendingChanges.ToList();
            _pendingChanges.Clear();
            HaveNewChanges.Value = false;
            return result;
        }

        /// <summary>
        /// Создает Reactive Property для работы с оборачиваемыми данными
        /// </summary>
        /// <typeparam name="TValue">Тип обрабатываемого поля</typeparam>
        /// <param name="getter"></param>
        /// <param name="setter"></param>
        /// <param name="fieldName">Имя поля</param>
        /// <returns></returns>
        public MutableObjectReactiveProperty<TValue> CreateReactiveProperty<TValue>(Func<TValue> getter, Action<TValue> setter, string fieldName)
        {
            var property = new MutableObjectReactiveProperty<TValue>(getter, setter, this);
            _properties.Add(fieldName, property);
            return property;
        }

        public void RegisterMutableChild(string childName, IRemoteChangesStorage child)
        {
            _childObjects.Add(childName, child);
        }

        public bool IsRootLoaded()
        {
            return _objectHandler.Object != null;
        }

        public string GetChildPath(string objectName)
        {
            return _objectHandler.GetFullPath() + objectName + RemoteObjectsProvider.PathDelimeter;
        }

        
        #region private methods

        protected virtual void OnBindToSource(IRemoteObjectHandler<T> objectHandler)
        {
            
        }
        
        protected void PropertyChanged(string name)
        {
            if (_properties.ContainsKey(name))
                _properties[name].Notify();
        }

        protected void AllPropertiesChanged()
        {
            foreach (var property in _properties.Values)
                property.Notify();
        }

        private void ChangeApplied(RemoteDataChange change)
        {
            change.ApplyCallback?.Invoke(change);
        }

        private void ApplyChangeOnLocalHandler(RemoteDataChange change)
        {
            _objectHandler.ApplyChangeLocal(change);
            PropertyChanged(change.FieldName);
        }
        #endregion
    }
}
