using System;
using System.Collections.Generic;
using System.Reflection;

namespace RemoteData.Editor.Generator
{
    public class RemoteModelDefinition
    {
        public RemoteObjectsInfo RemoteObjectsInfo;
        public Type Type;
        public List<PropertyInfo> RemoteProperties = new List<PropertyInfo>();
    }
}