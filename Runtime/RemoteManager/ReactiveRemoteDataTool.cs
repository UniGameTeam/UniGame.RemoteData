using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;
using UniModules.UniCore.Runtime.ReflectionUtils;
using UniModules.UniCore.Runtime.Utils;
using UniModules.UniGame.RemoteData.MutableObject;

public static class ReactiveRemoteDataTool
{
    private static List<Type> _emptyList = new List<Type>();

    private static MemorizeItem<Type, List<Type>> _reactiveModelCache = MemorizeTool.Memorize<Type,List<Type>>(x =>
    {
        if (!x.HasCustomAttribute<FirestoreDataAttribute>())
        {
            return _emptyList;
        }

        var genericRemoteData = typeof(IReactiveRemoteObject<>).MakeGenericType(x);
        var reactiveRemoteData = genericRemoteData.GetAssignableTypes();
        return reactiveRemoteData;
    });


    public static IReadOnlyList<Type> GetReactiveModelTypes<T>() => _reactiveModelCache[typeof(T)];
    
    public static IReadOnlyList<Type> GetReactiveModelTypes(Type type) => _reactiveModelCache[type];
    
    public static Type GetFirstReactiveModelType<T>() => GetFirstReactiveModelType(typeof(T));
    public static Type GetFirstReactiveModelType(Type type) => _reactiveModelCache[type].FirstOrDefault();
    
}