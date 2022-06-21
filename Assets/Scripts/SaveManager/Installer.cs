using UnityEngine;

public class Installer : MonoBehaviour
{
    [SerializeField] private LoadPanelItem loadPanelItemPrefab;

    [SerializeField] private FireBaseLoadManager fireBaseLoadManager;

    //  private ILoadManagerService _loadManagerService;
    private IUserPanelSpawnerService _userPanelSpawnerService;
    private IUserDataManagerService _userDataManagerService;

    private void Awake()
    {
        //       _loadManagerService = new LoadManager();
        _userPanelSpawnerService = new UserPanelSpawner(transform, loadPanelItemPrefab);
        _userDataManagerService = new UserDataManager(fireBaseLoadManager);
    }

    private void OnEnable()
    {
        ServiceLocator.Subscribe<IUserDataManagerService>(_userDataManagerService);
        //     ServiceLocator.Subscribe<ILoadManagerService>(_loadManagerService);
        ServiceLocator.Subscribe<IUserPanelSpawnerService>(_userPanelSpawnerService);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<IUserDataManagerService>();
        ServiceLocator.Unsubscribe<ILoadManagerService>();
        ServiceLocator.Unsubscribe<IUserPanelSpawnerService>();
    }
}