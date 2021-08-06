using System;
using System.Collections.Generic;
using System.Linq;
using CodeWriter.Editor.UnityTools;
using RemoteData.Editor.Generator.Abstract;
using RemoteData.Editor.Generator.Binders;
using UniModules.UniCore.Runtime.ReflectionUtils;
using UniModules.UniGame.RemoteData;
using UnityEngine;

namespace RemoteData.Editor.Generator
{
    [Serializable]
    public class ReactiveModelGenerator
    {
        private static string BaseReferenceTemplate = "BaseMutableReactiveRemoteObjectFacade<%DATA_CLASS%>";
        private static string BindingMethodTemplate =
            "protected override void OnBindToSource(IRemoteObjectHandler<%DATA_CLASS%> objectHandler){\n\t\t %BINDING_PROPERTIES% \n\t\t}";
        
        [SerializeReference]
        public List<IRemoteBinder> remoteBinders = new List<IRemoteBinder>()
        {
            new DictionaryRemoteBinder(),
            new RemoteChildBinder() { ObjectSourceValue = "objectHandler.Object"},
            new DefaultRemoteBinder(){ObjectSourceValue = "objectHandler.Object"},
        };

        public virtual string BaseReference => BaseReferenceTemplate;

        public virtual string BindingReference => BindingMethodTemplate;

        public virtual List<IRemoteBinder> RemoteBinders => remoteBinders;

        public virtual string GetClassName(RemoteModelDefinition remoteModelDefinition)
        {
            var info = remoteModelDefinition.RemoteObjectsInfo;
            var dataType = remoteModelDefinition.Type;
            return string.Format(info.reactiveClassNamePattern, dataType.Name);
        }
        
        public ScriptData Create(RemoteModelDefinition remoteModelDefinition)
        {
            var info = remoteModelDefinition.RemoteObjectsInfo;
            var dataType = remoteModelDefinition.Type;

            var scriptData = new ScriptData()
            {
                Name = GetClassName(remoteModelDefinition),
                Namespace = info.namespaceValue,
                IsPartial = true,
                IsPublic = true,
                IsStatic = false
            };
            
            scriptData.Usings.Add(dataType.Namespace);
            scriptData.Usings.AddRange(dataType.GetAllUsings());
            scriptData.Usings.AddRange(typeof(BaseMutableReactiveRemoteObjectFacade<>).GetAllUsings());

            scriptData.BaseReferences.Add(BaseReference.Replace(RemoteConstants.DataClassTypeKey, dataType.Name));
            
            // var interfaces = dataType.GetInterfaces();
            // foreach (var interfaceType in interfaces)
            // {
            //     if(!interfaceType.IsInterface) continue;
            //     var usingsValue = interfaceType.GetAllUsings();
            //     scriptData.Usings.AddRange(usingsValue);
            //     scriptData.BaseReferences.Add(interfaceType.GetFormattedName());
            // }

            scriptData.Methods = CreateBindMethods(remoteModelDefinition);
            scriptData.Properties = CreateProperties(remoteModelDefinition);
            
            return scriptData;
        }

        public List<string> CreateProperties(RemoteModelDefinition remoteModelDefinition)
        {
            var result = new List<string>();
            var info = remoteModelDefinition.RemoteObjectsInfo;
            var properties = remoteModelDefinition.RemoteProperties;
            foreach (var property in properties)
            {
                var binder = RemoteBinders.FirstOrDefault(x => x.Validate(property.PropertyType,info));
                if(binder == null) continue;
                result.Add(binder.CreateProperty(property,remoteModelDefinition));
            }
            return result;
        }
        
        public List<string> CreateBindMethods(RemoteModelDefinition remoteModelDefinition)
        {
            var result = new List<string>();
            var info = remoteModelDefinition.RemoteObjectsInfo;
            var remoteProperties = remoteModelDefinition.RemoteProperties;
            var resultActions = string.Empty;
            
            foreach (var propertyInfo in remoteProperties)
            {
                var binder = RemoteBinders.FirstOrDefault(x => x.Validate(propertyInfo.PropertyType,info));
                if(binder == null) continue;
                var binding = binder.CreateBinder(propertyInfo, remoteModelDefinition);
                resultActions = $"{resultActions} \n \t\t\t\t{binding}";
            }
            
            var method = BindingReference.Replace(RemoteConstants.BindingTemplateKey,resultActions);
            method = method.Replace(RemoteConstants.DataClassTypeKey,remoteModelDefinition.Type.GetFormattedName());
            
            result.Add(method);
            return result;
        }
        
    }
    
    public static class RemoteConstants
    {
        public static string DataClassTypeKey = "%DATA_CLASS%";
        public static string PropertyNameKey = "%PROPERTY_NAME%";
        public static string PropertyTypeKey = "%PROPERTY_TYPE%";
        public static string BindingTemplateKey = "%BINDING_PROPERTIES%";
    }
}
