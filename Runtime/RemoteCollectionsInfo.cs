using System;
using UnityEngine;

namespace UniModules.UniGame.RemoteData
{
   [Serializable]
   public class RemoteCollectionsInfo
   {
      public string collectionId = String.Empty;
   
      public string collectionName = String.Empty;

#if ODIN_INSPECTOR
      [Sirenix.OdinInspector.MultiLineProperty()]
#endif
      public string description = string.Empty;
      
      [SerializeReference] 
      public RemoteObjectsProvider collectionObjectsProvider;
   }
}
