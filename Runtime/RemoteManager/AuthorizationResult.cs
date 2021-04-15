using System;

namespace UniModules.UniGame.RemoteData.Runtime.RemoteManager
{
    public struct AuthorizationResult
    {
        public bool Result;
        public Exception Exception;
        public string UserId;
    }
}