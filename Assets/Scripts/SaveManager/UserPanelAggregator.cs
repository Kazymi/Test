using System.Collections;
using UnityEngine;

public class UserPanelAggregator : MonoBehaviour
{
    [SerializeField] private bool spawnAllPanels;
    
    private IUserDataManagerService _userDataManager;
    private IUserPanelSpawnerService _spawnerService;

    private void Start()
    {
        _userDataManager = ServiceLocator.GetService<IUserDataManagerService>();
        _spawnerService = ServiceLocator.GetService<IUserPanelSpawnerService>();
        if (spawnAllPanels)
        {
            SpawnAllPanel();
        }
        else
        {
            StartCoroutine(SpawnPanels());
        }
    }

    private IEnumerator SpawnPanels()
    {
        var userDatas = _userDataManager.UserDatas;
        foreach (var userData in userDatas)
        {
            yield return null;
            var newPanelItem = _spawnerService.SpawnNewPanelItem();
            newPanelItem.Initialize(userData);
        }
    }

    private void SpawnAllPanel()
    {
        var userDatas = _userDataManager.UserDatas;
        foreach (var userData in userDatas)
        {
            var newPanelItem = _spawnerService.SpawnNewPanelItem();
            newPanelItem.Initialize(userData);
        }
    }
}