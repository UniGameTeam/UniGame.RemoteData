using UnityEditor;

namespace RemoteData.Editor.Generator
{
    public static class RemoteDataCommands 
    {
        [MenuItem(itemName:"UniGame/RemoteData/Update Reactive Models")]
        public static void UpdateRemoteData()
        {
            var generator = ReactiveObjectGeneratorAsset.Asset;
            generator.CreateReactiveRemoteObjects();
        }
        
        [MenuItem(itemName:"UniGame/RemoteData/Reset Reactive API")]
        public static void Reset()
        {
            var generator = ReactiveObjectGeneratorAsset.Asset;
            generator.ResetData();
        }
        
        [MenuItem(itemName:"UniGame/RemoteData/Rebuild Reactive API")]
        public static void Rebuild()
        {
            var generator = ReactiveObjectGeneratorAsset.Asset;
            generator.Rebuild();
        }
    }
}
