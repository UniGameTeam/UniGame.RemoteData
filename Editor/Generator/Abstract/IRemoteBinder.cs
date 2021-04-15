using System;
using System.Reflection;

namespace RemoteData.Editor.Generator.Abstract
{
    public interface IRemoteBinder
    {
        bool Validate(Type type,RemoteObjectsInfo value);
        string CreateProperty(PropertyInfo property,RemoteModelDefinition definition);
        string CreateBinder(PropertyInfo property,RemoteModelDefinition definition);
    }
}