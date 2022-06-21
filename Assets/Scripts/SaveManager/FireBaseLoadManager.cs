using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;

public class FireBaseLoadManager : MonoBehaviour, ILoadManagerService
{
    //fire base LoadManager
    private DatabaseReference _databaseReference;
    private List<UserData> _datas;

    private void Awake()
    {
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(LoadData());
    }

    private void OnEnable()
    {
        ServiceLocator.Subscribe<ILoadManagerService>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<ILoadManagerService>();
    }

    public event Action DataUpdated;

    public List<UserData> LoadUserData()
    {
        return _datas;
    }

    public bool LoadSuccess { get; private set; }

    private IEnumerator LoadData()
    {
        var saveData = _databaseReference.GetValueAsync();
        yield return new WaitUntil(predicate: () => saveData.IsCompleted);
        if (saveData.Exception != null)
        {
            Debug.LogError(saveData.Exception);
            yield break;
        }

        var value = saveData.Result.GetRawJsonValue();
        _datas = JsonConvert.DeserializeObject<List<UserData>>(value);
        LoadSuccess = true;
        DataUpdated?.Invoke();
    }
}