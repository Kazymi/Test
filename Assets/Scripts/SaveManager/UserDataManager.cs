using System.Collections.Generic;
using System.Linq;

public class UserDataManager : IUserDataManagerService
{
    private readonly ILoadManagerService _loadManagerService;
    public List<UserData> UserDatas { get; private set; }
    public bool IsDataLoaded { get; private set; }

    public UserData GetUserDataById(int id)
    {
        if (id < 0) return null;
        return UserDatas.Count()-1 < id ? null : UserDatas[id];
    }

    private void UpdateDate()
    {
        UserDatas = _loadManagerService.LoadUserData();
        IsDataLoaded = true;
    }
    public UserDataManager(ILoadManagerService loadManagerService)
    {
        _loadManagerService = loadManagerService;
        _loadManagerService.DataUpdated += UpdateDate;
    }

    ~UserDataManager()
    {
        _loadManagerService.DataUpdated -= UpdateDate;
    }
}