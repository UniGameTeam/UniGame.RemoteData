using System;
using System.Collections.Generic;
using UniModules.UniGame.Core.Runtime.SerializableType;

namespace RemoteData.Editor.Generator
{
    [Serializable]
    public class RemoteObjectsInfo
    {
        public string reactiveClassNamePattern = "Reactive{0}";
        public string reactiveChildClassNamePattern = "ReactiveChild{0}";
        public string namespaceValue = "RemoteData.Runtime.RemoteModels";
        
        public List<SType> remoteDataModels = new List<SType>();

        public List<SType> remoteChilds = new List<SType>();

        public List<RemoteModelDefinition> remoteDefinitions = new List<RemoteModelDefinition>();

        public void Clear()
        {
            remoteDefinitions.Clear();
            remoteDataModels.Clear();
            remoteChilds.Clear();
        }
    }
}