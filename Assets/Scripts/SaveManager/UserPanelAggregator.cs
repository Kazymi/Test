using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class UserPanelAggregator : MonoBehaviour
{
    [SerializeField] private ViewScroller viewScroller;

    private int _currentId = 0;
    private IUserDataManagerService _userDataManagerService;
    private bool _isInitialized;

    private void OnEnable()
    {
        viewScroller.NewPanelSpawnedBack += SpawnedObjectBack;
        viewScroller.NewPanelSpawnedForward += SpawnedObjectForward;
    }

    private void OnDisable()
    {
        viewScroller.NewPanelSpawnedBack -= SpawnedObjectBack;
        viewScroller.NewPanelSpawnedForward -= SpawnedObjectForward;
    }

    private void Start()
    {
        viewScroller.IsMoveUpUnlocked = true;
        _userDataManagerService ??= ServiceLocator.GetService<IUserDataManagerService>();
    }

    private void Update()
    {
        if (!_userDataManagerService.IsDataLoaded || _isInitialized)
        {
            return;
        }
        _isInitialized = true;
        viewScroller.SpawnPanels(15);
    }

    private void SpawnedObjectForward(MonoPooledWithRectTransform monoPooledWithRectTransform)
    {
        var loadPanel = monoPooledWithRectTransform.GetComponent<LoadPanelItem>();
        var id = _currentId - viewScroller.SpawnedView.Count-1;
        _currentId--;
        var user = _userDataManagerService.GetUserDataById(id);
        if (user == null)
        {
            viewScroller.IsMoveUpUnlocked = false;
            return;
        }

        if (id == 0)
        {
            viewScroller.IsMoveUpUnlocked = false;
        }
        loadPanel.Initialize(_userDataManagerService.GetUserDataById(id), id);
    }

    private void SpawnedObjectBack(MonoPooledWithRectTransform monoPooledWithRectTransform)
    {
        _userDataManagerService ??= ServiceLocator.GetService<IUserDataManagerService>();
        var loadPanel = monoPooledWithRectTransform.GetComponent<LoadPanelItem>();
        var id = _currentId;
        _currentId++;
        var user = _userDataManagerService.GetUserDataById(id);
        if (user == null)
        {
            viewScroller.IsMoveDownUnlocked = false;
            return;
        }

        loadPanel.Initialize(_userDataManagerService.GetUserDataById(id), id);
    }
}