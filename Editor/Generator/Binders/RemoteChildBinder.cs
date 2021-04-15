using System;
using System.Reflection;
using RemoteData.Editor.Generator.Abstract;
using UniModules.UniCore.Runtime.ReflectionUtils;

namespace RemoteData.Editor.Generator.Binders
{
    [Serializable]
    public class RemoteChildBinder : IRemoteBinder
    {
        private static string ObjectSourceKey = "%OBJECT_SOURCE%";
        private static string ChildNameKey = "%CHILD_NAME%";

        private static string BindingTemplate =
            "\n\t\t\t%PROPERTY_NAME% = new %CHILD_NAME%();" +
            "\n\t\t\t%PROPERTY_NAME%.BindToSource(() => %OBJECT_SOURCE%.%PROPERTY_NAME%, GetChildPath(nameof(%OBJECT_SOURCE%.%PROPERTY_NAME%)), this);" +
            "\n\t\t\tRegisterMutableChild(nameof(%OBJECT_SOURCE%.%PROPERTY_NAME%), %PROPERTY_NAME%);\n";

        private static string PropertyTemplate =
            "public %CHILD_NAME% %PROPERTY_NAME% { get; private set; }";

        public string ObjectSourceValue { get; set; } 
        
        public virtual bool Validate(Type type,RemoteObjectsInfo value)
        {
            var result = value.remoteChilds.Contains(type);
            return result;
        }

        public string GetChildName(PropertyInfo property,RemoteModelDefinition definition)
        {
            return string.Format(definition.RemoteObjectsInfo.reactiveChildClassNamePattern,property.Name);
        }

        public string CreateProperty(PropertyInfo property,RemoteModelDefinition definition)
        {
            var template = PropertyTemplate;
            var childName = GetChildName(property, definition);
            
            template = template.Replace(RemoteConstants.PropertyTypeKey, property.PropertyType.GetFormattedName());
            template = template.Replace(RemoteConstants.PropertyNameKey, property.Name);
            template = template.Replace(ChildNameKey, childName);
            return template;
        }

        public string CreateBinder(PropertyInfo property,RemoteModelDefinition definition)
        {
            var childName = GetChildName(property, definition);
            var binding = BindingTemplate.Replace(RemoteConstants.PropertyNameKey, property.Name);
            binding = binding.Replace(ObjectSourceKey, ObjectSourceValue);
            binding = binding.Replace(ChildNameKey, childName);
            return binding;
        }
        
    }
}