
using System.IO;
using Cysharp.Text;
using UniGame.RemoteData.FirebaseModule;

namespace UniModules.UniGame.RemoteData
{
    using System;


    public static class RemoteDataExtensions
    {
        public static string PathSeparator = "/";

        public static TResult Bind<TResult,TData>(this TResult source,Func<TData> getter, string fullPath, IRemoteChangesStorage storage)
            where TResult : IMutableChild<TData>
        {
            source.BindToSource(getter, fullPath, storage);
            return source;
        }
        
        /// <summary>
        /// Create mutable child and regiser at parent
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="source"></param>
        /// <param name="getter"></param>
        /// <param name="fullPath"></param>
        /// <param name="storage"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public static TResult BindAndRegister<TResult,TData>(this IReactiveParentRemoteObject parent,
            TResult source,Func<TData> getter, string fullPath, IRemoteChangesStorage storage)
            where TResult : IMutableChild<TData>
        {
            source.BindToSource(getter,parent.GetChildPath(fullPath), storage);
            parent.RegisterMutableChild(parent.GetChildPath(fullPath), source);
            return source;
        }
        
        public static T RegisterAsMutableChild<T>(this T child, IReactiveParentRemoteObject parent,string childName)
            where T : IRemoteChangesStorage
        {
            parent.RegisterMutableChild(childName, child);
            return child;
        }

        public static string CombineRemoteDataPath(this string collectionId, string path)
        {
            var id     = collectionId;
            var result = ZString.Join(PathSeparator,id,path);
            return result;
        }
        
        public static string CombinePath(this RemoteCollectionId collectionId, string path)
        {
            var id = (string)collectionId;
            return CombineRemoteDataPath(id,path);
        }
    }

}
