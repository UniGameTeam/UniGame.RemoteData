﻿using System.Threading.Tasks;

namespace UniModules.UniGame.Authorization
{
    public interface IAuthModule
    {
        // TO DO other Auth Type
        // Init with providers for different networks

        string CurrentUserId { get; }
        bool IsLogged { get; }

        Task Init();
        Task Login(IAuthToken authType);
        Task Logout();
    }
}