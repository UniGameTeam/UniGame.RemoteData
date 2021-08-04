namespace UniModules.UniGame.RemoteData.MutableObject
{
    using System;
    using System.Collections.Generic;

    public interface IReactiveParentRemoteObject
    {
        string GetId();
        
        void   UpdateChildData(string childName, object newData);

        /// <summary>
        /// Возвращает список всех локальных изменений и обнуляет его внутри объекта
        /// применимо для работы с BatchUpdater
        /// </summary>
        /// <returns></returns>
        List<RemoteDataChange> FlushChanges();

        /// <summary>
        /// Создает Reactive Property для работы с оборачиваемыми данными
        /// </summary>
        /// <typeparam name="TValue">Тип обрабатываемого поля</typeparam>
        /// <param name="getter"></param>
        /// <param name="setter"></param>
        /// <param name="fieldName">Имя поля</param>
        /// <returns></returns>
        MutableObjectReactiveProperty<TValue> CreateReactiveProperty<TValue>(Func<TValue> getter, Action<TValue> setter, string fieldName);

        void RegisterMutableChild(string childName, IRemoteChangesStorage child);
    }
}