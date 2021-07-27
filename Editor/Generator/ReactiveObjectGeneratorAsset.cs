using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeWriter.Editor.UnityTools;
using Firebase.Firestore;
using UniModules.UniCore.Runtime.ReflectionUtils;
using UniModules.UniCore.Runtime.Utils;
using UniModules.UniGame.CodeWriter.Editor.UnityTools;
using UniModules.UniGame.Core.Editor.EditorProcessors;
using UniModules.UniGame.Core.EditorTools.Editor;
using UniModules.UniGame.Core.EditorTools.Editor.AssetOperations;
using UniModules.UniGame.Core.EditorTools.Editor.Tools;
using UniModules.UniGame.Core.Runtime.SerializableType;
using UnityEditor;
using UnityEngine;

namespace RemoteData.Editor.Generator
{
    [GeneratedAssetInfo(location: "RemoteData/Editor")]
    public class ReactiveObjectGeneratorAsset : GeneratedAsset<ReactiveObjectGeneratorAsset>
    {
        #region static data

        private static string _remoteDataLocalRuntimePath = "RemoteData/Runtime/";
        private static string _remoteDataLocalEditorPath = "RemoteData/Editor/";
        private static Type _remotePropertyAttributeType = typeof(FirestorePropertyAttribute);

        public static readonly MemorizeItem<Type, List<PropertyInfo>> RemotePropertiesMap =
            MemorizeTool.Memorize<Type, List<PropertyInfo>>(x =>
            {
                var properties =
                    x.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                var remoteProperties =
                    properties.Where(x => x.HasCustomAttribute(_remotePropertyAttributeType)).ToList();
                return remoteProperties;
            });
        
        #endregion

        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        public RemoteObjectsInfo remoteObjectsInfo = new RemoteObjectsInfo();

        #endregion

        private ReactiveModelGenerator _remoteModelGenerator = new ReactiveModelGenerator();

        private ChildModelGenerator _childModelGenerator = new ChildModelGenerator();

        private List<Func<RemoteModelDefinition, RemoteModelDefinition>> _modelActions = new List<Func<RemoteModelDefinition, RemoteModelDefinition>>();

        public string RuntimePath =>
            EditorFileUtils.Combine(EditorPathConstants.GeneratedContentPath, _remoteDataLocalRuntimePath);

        private string EditorPath =>
            EditorFileUtils.Combine(EditorPathConstants.GeneratedContentPath, _remoteDataLocalEditorPath);

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void RefreshInfo()
        {
            CollectRemoteDefinitionsData();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void ResetData()
        {
            remoteObjectsInfo.Clear();
            DeleteGenerated();
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Rebuild()
        {
            ResetData();
            AssetDatabase.Refresh();
            Generate();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Generate()
        {
            var info = CollectRemoteDefinitionsData();
            CreateRemoteModels(info);
        }

        public void CreateReactiveRemoteObjects()
        {
            CollectRemoteDefinitionsData();
        }

        public RemoteObjectsInfo CollectRemoteDefinitionsData()
        {
            _childModelGenerator = new ChildModelGenerator();
            _remoteModelGenerator = new ReactiveModelGenerator();
            
            remoteObjectsInfo.Clear();

            var firestoreData = ReflectionTools.GetAllWithAttributes<FirestoreDataAttribute>();
            var remoteDataModels = remoteObjectsInfo.remoteDataModels;
            var remoteModelDefinitions = remoteObjectsInfo.remoteDefinitions;
            
            foreach (var type in firestoreData)
            {
                remoteDataModels.Add(type);
                var modelDef = new RemoteModelDefinition()
                {
                    RemoteObjectsInfo = remoteObjectsInfo,
                    Type = type,
                };

                remoteModelDefinitions.Add(modelDef);
            }

            foreach (var modelAction in _modelActions)
            {
                ApplyRemoteModelDefinitionStep(remoteModelDefinitions, modelAction);
            }

            return remoteObjectsInfo;
        }

        public void CreateRemoteModels(RemoteObjectsInfo info)
        {
            AssetEditorTools.ApplyAssetEditing(() => CreateRemoteModelsScripts(info));
        }

        public RemoteModelDefinition UpdateRemoteProperties(RemoteModelDefinition remoteModelDefinition)
        {
            var remoteObjectType = remoteModelDefinition.Type;

            remoteModelDefinition.RemoteProperties.Clear();
            remoteModelDefinition.RemoteProperties.AddRange(RemotePropertiesMap[remoteObjectType]);

            return remoteModelDefinition;
        }
        
        private RemoteModelDefinition UpdateRemoteChilds(RemoteModelDefinition remoteModel)
        {
            var remoteProperties = remoteModel.RemoteProperties;
            var info = remoteModel.RemoteObjectsInfo;
            var remoteDataModels = info.remoteDataModels;
            var remoteChilds = info.remoteChilds;
            
            foreach (var property in remoteProperties)
            {
                var type = (SType)property.PropertyType;
                if(remoteDataModels.Contains(type) &&  !remoteChilds.Contains(type))
                    remoteChilds.Add(type);
            }
            
            return remoteModel;
        }

        private void DeleteGenerated()
        {
            var scripts = AssetDatabase.FindAssets(string.Empty, new[] {RuntimePath});
            var paths = scripts.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            AssetDatabase.DeleteAssets(paths, new List<string>());
        }

        private void CreateRemoteModelsScripts(RemoteObjectsInfo info)
        {
            var remoteObjects = info.remoteDefinitions;
            var remoteChildTypes = info.remoteChilds;
            var childObjects = remoteObjects
                .Where(x => remoteChildTypes.Contains(x.Type)).ToList();

            foreach (var remoteObject in remoteObjects)
            {
                var scriptData = _remoteModelGenerator.Create(remoteObject);
                CreateScript(scriptData);
            }

            foreach (var remoteChild in childObjects)
            {
                var scriptData = _childModelGenerator.Create(remoteChild);
                CreateScript(scriptData);
            }
        }
        
        private void CreateScript(ScriptData scriptData)
        {
            var path = $"{RuntimePath.CombinePath(scriptData.Name)}_generated.cs";
            var result = scriptData.CreateScript(path);
            if (result)
            {
                Debug.Log($"RemoteData Runtime : {scriptData.Name} Updated by path {path}");
            }
        }

        private void ApplyRemoteModelDefinitionStep(List<RemoteModelDefinition> definitions,
            Func<RemoteModelDefinition, RemoteModelDefinition> step)
        {
            for (var i = 0; i < definitions.Count; i++)
            {
                var dataModel = definitions[i];
                dataModel = step(dataModel);
                definitions[i] = dataModel;
            }
        }

        protected void OnEnable()
        {
            _modelActions = new List<Func<RemoteModelDefinition, RemoteModelDefinition>>(8)
            {
                UpdateRemoteProperties,
                UpdateRemoteChilds,
            };
        }
    }
}