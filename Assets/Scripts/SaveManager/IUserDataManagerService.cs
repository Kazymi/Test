using System.Collections.Generic;

public interface IUserDataManagerService
{
    List<UserData> UserDatas { get; }
    UserData GetUserDataById(int id);
    bool IsDataLoaded { get;}
}