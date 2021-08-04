
namespace UniModules.UniGame.RemoteData
{
    using System;
    using MutableObject;

    
    public static class RemoteDataExtensions 
    {

        public static TResult Bind<TResult,TData>(this TResult source,Func<TData> getter, string fullPath, IRemoteChangesStorage storage)
            where TResult : IMutableChild<TData>
        {
            source.BindToSource(getter, fullPath, storage);
            return source;
        }
        
        public static T RegisterAsMutableChild<T>(this T child, IReactiveParentRemoteObject parent,string childName)
            where T : IRemoteChangesStorage
        {
            parent.RegisterMutableChild(childName, child);
            return child;
        }
    }

}
