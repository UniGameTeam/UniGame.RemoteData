using System;
using System.Reflection;
using RemoteData.Editor.Generator.Abstract;
using UniModules.UniCore.Runtime.ReflectionUtils;

namespace RemoteData.Editor.Generator.Binders
{
    [Serializable]
    public class DefaultRemoteBinder : IRemoteBinder
    {
        private static string ObjectSourceKey = "%OBJECT_SOURCE%";
            
        private static string BindingTemplate =
            "\n\t\t\t%PROPERTY_NAME% = CreateReactiveProperty(() => %OBJECT_SOURCE%.%PROPERTY_NAME%," +
            "(v) => UpdateChildData(nameof(%OBJECT_SOURCE%.%PROPERTY_NAME%), v)," +
            "nameof(%OBJECT_SOURCE%.%PROPERTY_NAME%));";
       
        
        private static string PropertyTemplate =
            "public IReactiveProperty<%PROPERTY_TYPE%> %PROPERTY_NAME% { get; private set; }";

        public string ObjectSourceValue { get; set; } 
        
        public virtual bool Validate(Type type,RemoteObjectsInfo value)
        {
            return type!=null;
        }

        public string CreateProperty(PropertyInfo property,RemoteModelDefinition definition)
        {
            var template = PropertyTemplate;
            template = template.Replace(RemoteConstants.PropertyTypeKey, property.PropertyType.GetFormattedName());
            template = template.Replace(RemoteConstants.PropertyNameKey, property.Name);
            return template;
        }

        public string CreateBinder(PropertyInfo property,RemoteModelDefinition definition)
        {
            var binding = BindingTemplate.Replace(RemoteConstants.PropertyNameKey, property.Name);
            binding = binding.Replace(ObjectSourceKey, ObjectSourceValue);
            return binding;
        }
        
    }
}