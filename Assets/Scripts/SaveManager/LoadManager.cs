using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LoadManager : ILoadManagerService
{
    //локальное чтение
    private readonly string _path;
    private List<UserData> _userDatas;
    private const string FileName = "test_mock_dt.json";

    public bool LoadSuccess { get; private set; }
    public event Action DataUpdated;
    
    public LoadManager()
    {
        _path = Application.dataPath + Path.AltDirectorySeparatorChar + FileName;
        Load();
    }

    public List<UserData> LoadUserData()
    {
        return _userDatas;
    }

    private void Load()
    {
        using StreamReader streamReader = new StreamReader(_path);
        string loadValue = streamReader.ReadToEnd();
        List<UserData> users = JsonConvert.DeserializeObject<List<UserData>>(loadValue);
        LoadSuccess = true;
        DataUpdated?.Invoke();
    }
}