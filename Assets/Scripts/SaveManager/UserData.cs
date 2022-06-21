using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using FileMode = System.IO.FileMode;
using Task = System.Threading.Tasks.Task;

[Serializable]
public class UserData
{
    public string first_name;
    public string last_name;
    public string email;
    public string gender;
    public string ip_address;

    public UserData(string firstName, string lastName, string email, string gender, string ipAdress)
    {
        first_name = firstName;
        last_name = lastName;
        this.email = email;
        this.gender = gender;
        ip_address = ipAdress;
    }
}