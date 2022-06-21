using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LoadManager : ILoadManagerService
{
    private readonly string _path;
    private const string FileName = "test_mock_dt.json";

    public LoadManager()
    {
        _path = Application.dataPath + Path.AltDirectorySeparatorChar + FileName;
    }

    public List<UserData> LoadUserData()
    {
        using StreamReader streamReader = new StreamReader(_path);
        string loadValue = streamReader.ReadToEnd();
        List<UserData> users = JsonConvert.DeserializeObject<List<UserData>>(loadValue);
        return users;
    }
}