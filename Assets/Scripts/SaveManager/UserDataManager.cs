using System.Collections.Generic;

public class UserDataManager : IUserDataManagerService
{
    public List<UserData> UserDatas { get; private set; }

    public UserDataManager(ILoadManagerService loadManagerService)
    {
        UserDatas = loadManagerService.LoadUserData();
    }
}