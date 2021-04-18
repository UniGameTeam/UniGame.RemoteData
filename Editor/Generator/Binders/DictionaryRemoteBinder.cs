using System;
using System.Collections.Generic;
using System.Reflection;
using RemoteData.Editor.Generator.Abstract;
using UniModules.UniCore.Runtime.ReflectionUtils;

namespace RemoteData.Editor.Generator.Binders
{
    [Serializable]
    public class DictionaryRemoteBinder : IRemoteBinder
    {
        public static string DictionaryValueKey = "%DICTIONARY_VALUE%";

        private static string BindingTemplate =
            "var value%PROPERTY_NAME% = new MutableDictionary<%DICTIONARY_VALUE%>();\n\t\t\t" +
            "value%PROPERTY_NAME%.BindToSource(() => objectHandler.Object.%PROPERTY_NAME%,GetChildPath(nameof(objectHandler.Object.%PROPERTY_NAME%)), this);\n\t\t\t" +
            "%PROPERTY_NAME% = value%PROPERTY_NAME%;\n\t\t\t";
        
        private static string PropertyTemplate =
            "public %PROPERTY_TYPE% %PROPERTY_NAME% { get; private set; }";

        public virtual string GetBindingTemplate => BindingTemplate;

        public virtual string GetPropertyTemplate => PropertyTemplate;
        
        public virtual bool Validate(Type type,RemoteObjectsInfo value)
        {
            var result = type.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition());
            return result;
        }

        public string CreateProperty(PropertyInfo property,RemoteModelDefinition definition)
        {
            var template = GetPropertyTemplate;
            
            var targetType = property.PropertyType;
            var valueType = targetType.GetGenericArguments()[1];
            template = template.Replace(RemoteConstants.PropertyTypeKey, GetPropertyType(targetType).GetFormattedName());
            template = template.Replace(DictionaryValueKey, valueType.GetFormattedName());
            template = template.Replace(RemoteConstants.PropertyNameKey, property.Name);
            return template;
        }

        public virtual Type GetPropertyType(Type sourceType)
        {
            return sourceType;
        }

        public string CreateBinder(PropertyInfo property,RemoteModelDefinition definition)
        {
            var targetType = property.PropertyType;
            var valueType = targetType.GetGenericArguments()[1];
            var binding = GetBindingTemplate.Replace(RemoteConstants.PropertyNameKey, property.Name);
            binding = binding.Replace(DictionaryValueKey, valueType.GetFormattedName());
            return binding;
        }
        
    }
}