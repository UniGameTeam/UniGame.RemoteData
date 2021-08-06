using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using UniGame.UniNodes.GameFlow.Runtime;
using UniModules.UniGame.RemoteData;
using UniModules.UniGame.Authorization;
using UniRx;
using UnityEngine;

namespace UniModules.UniGame.RemoteData
{
    public class FirebaseAuthService : GameService, IAuthorizationService
    {
        public IReactiveProperty<string> UserId { get; } = new ReactiveProperty<string>(null);

        public async UniTask<AuthorizationResult> InitialAuthorize()
        {
            await Firebase.FirebaseApp
                .CheckAndFixDependenciesAsync()
                .AsUniTask();
            
            Debug.Log($"{nameof(FirebaseAuthService)} :: Starting initial authorization");
            var auth = FirebaseAuth.DefaultInstance;
            if (auth.CurrentUser != null)
            {
                Debug.Log($"{nameof(FirebaseAuthService)} :: Firebase auth credentials cached. AuthResult = SUCCESS :: userId = {auth.CurrentUser.UserId}" );
                UserId.Value = auth.CurrentUser.UserId;
                return new AuthorizationResult()
                {
                    Exception = null,
                    Result = true,
                    UserId = auth.CurrentUser.UserId
                };
            }

            try
            {
                var resultUser = await FirebaseAuth.DefaultInstance
                    .SignInAnonymouslyAsync()
                    .ConfigureAwait(false);
                
                if (resultUser != null)
                {
                    Debug.Log($"{nameof(FirebaseAuthService)} :: AuthResult = SUCCESS :: userId = {resultUser.UserId}");
                    UserId.Value = resultUser.UserId;
                    return new AuthorizationResult()
                    {
                        Exception = null,
                        Result    = true,
                        UserId    = resultUser.UserId
                    };
                }

                Debug.LogError($"{nameof(FirebaseAuthService)} :: AuthResult = FAILED :: task return null user");
                return new AuthorizationResult()
                {
                    Exception = null,
                    Result    = false,
                    UserId    = null
                };
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(FirebaseAuthService)} :: auth failed task throwed exception :: " + e);
                return new AuthorizationResult()
                {
                    Exception = e,
                    Result    = false,
                    UserId    = null
                };
            }
        }
    }
}