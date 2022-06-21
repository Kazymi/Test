using System;
using System.Collections.Generic;

public interface ILoadManagerService
{
    event Action DataUpdated;
    List<UserData> LoadUserData();
    bool LoadSuccess { get; }
}