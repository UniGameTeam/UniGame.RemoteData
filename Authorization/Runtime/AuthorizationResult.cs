using System;

namespace UniModules.UniGame.Authorization
{
    public struct AuthorizationResult
    {
        public bool Result;
        public Exception Exception;
        public string UserId;
    }
}