namespace UniModules.UniGame.RemoteData.Runtime
{
    using System;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using Newtonsoft.Json;
    using UniModules.UniGame.RemoteData.MutableObject;
    using UniModules.UniGame.RemoteData.RemoteData;
    using UnityEngine;

    public class PlayerPrefsRemoteObjectHandler<T> : RemoteObjectHandler<T>
    {
        private string _path;

        public PlayerPrefsRemoteObjectHandler(string path)
            :base(null)
        {
            _path = path;
        }

        public async override UniTask<RemoteObjectHandler<T>> LoadData(Func<T> initialDataProvider = null)
        {
            if (!PlayerPrefs.HasKey(_path))
            {
                Object = initialDataProvider();
                var json = JsonConvert.SerializeObject(Object);
                PlayerPrefs.SetString(_path, json);
                return this;
            }
            else
            {
                var json = PlayerPrefs.GetString(_path);
                Object = JsonConvert.DeserializeObject<T>(json);
                return this;
            }
        }

        public override RemoteDataChange CreateChange(string fieldName, object fieldValue)
        {
            return RemoteDataChange.Create(
                _path + "." + fieldName,
                fieldName,
                fieldValue,
                null);
        }

        public override string GetDataId() => _path;

        public override string GetFullPath() => _path;

        protected async override UniTask ApplyChangeRemote(RemoteDataChange change)
        {
            // все изменения на момент ремотного применения уже оказались в Object поэтому можно просто сохранить в Prefs
            PlayerPrefs.SetString(_path, JsonConvert.SerializeObject(Object));
        }

        public override UniTask ClearData()
        {
            PlayerPrefs.DeleteKey(_path);
            PlayerPrefs.Save();
            return UniTask.CompletedTask;
        }
    }
}