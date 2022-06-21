using UnityEngine;

public class Installer : MonoBehaviour
{
    [SerializeField] private LoadPanelItem loadPanelItemPrefab;
    [SerializeField] private Transform context;
    
    private ILoadManagerService _loadManagerService;
    private IUserPanelSpawnerService _userPanelSpawnerService;
    private IUserDataManagerService _userDataManagerService;
    private void Awake()
    {
        _loadManagerService = new LoadManager();
        _userPanelSpawnerService = new UserPanelSpawner(context, transform, loadPanelItemPrefab);
        _userDataManagerService = new UserDataManager(_loadManagerService);
    }

    private void OnEnable()
    {
        ServiceLocator.Subscribe<IUserDataManagerService>(_userDataManagerService);
        ServiceLocator.Subscribe<ILoadManagerService>(_loadManagerService);
        ServiceLocator.Subscribe<IUserPanelSpawnerService>(_userPanelSpawnerService);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<IUserDataManagerService>();
        ServiceLocator.Unsubscribe<ILoadManagerService>();
        ServiceLocator.Unsubscribe<IUserPanelSpawnerService>();
    }
}