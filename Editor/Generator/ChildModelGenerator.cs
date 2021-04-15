using System;
using System.Collections.Generic;
using RemoteData.Editor.Generator.Abstract;
using RemoteData.Editor.Generator.Binders;
using UnityEngine;

namespace RemoteData.Editor.Generator
{
    [Serializable]
    public class ChildModelGenerator : ReactiveModelGenerator
    {
        private static string ChildRemoteBaseTemplate = "MutableChild<%DATA_CLASS%>";
        private static string BindingChildMethodTemplate =
            "protected override IMutableChild<%DATA_CLASS%> OnBindToSource(){\n\t\t %BINDING_PROPERTIES% \n\t\t\nreturn this;\n\t\t }";
        
        [SerializeReference]
        public List<IRemoteBinder> childBinders = new List<IRemoteBinder>()
        {
            new DictionaryChildRemoteBinder(),
            new RemoteChildBinder() { ObjectSourceValue = "objectHandler.Object"},
            new DefaultRemoteBinder(){ ObjectSourceValue = "Object"},
        };
        
        public override string BaseReference => ChildRemoteBaseTemplate;
        public override string BindingReference => BindingChildMethodTemplate;
        public override List<IRemoteBinder> RemoteBinders => childBinders;

        
        public override string GetClassName(RemoteModelDefinition remoteModelDefinition)
        {
            var info = remoteModelDefinition.RemoteObjectsInfo;
            var dataType = remoteModelDefinition.Type;
            return string.Format(info.reactiveChildClassNamePattern, dataType.Name);
        }
    }
}