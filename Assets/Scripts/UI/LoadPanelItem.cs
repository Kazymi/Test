using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPanelItem : MonoPooled
{
    [SerializeField] private LoadPanelConfiguration loadPanelConfiguration;

    public void Initialize(UserData userData)
    {
        loadPanelConfiguration.FistNameText.text = userData.first_name;
        loadPanelConfiguration.LastNameText.text = userData.last_name;
        loadPanelConfiguration.MailText.text = userData.email;
        loadPanelConfiguration.IPText.text = userData.ip_address;
        loadPanelConfiguration.GenderText.text = userData.gender;
    }
}