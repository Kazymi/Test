using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class LoadPanelItem : MonoPooledWithRectTransform
{
    [SerializeField] private LoadPanelConfiguration loadPanelConfiguration;

    public int ID { get; private set; }

    public void Initialize(UserData userData, int id)
    {
        ID = id;
        loadPanelConfiguration.FistNameText.text = userData.first_name;
        loadPanelConfiguration.LastNameText.text = userData.last_name;
        loadPanelConfiguration.MailText.text = userData.email;
        loadPanelConfiguration.IPText.text = userData.ip_address;
        loadPanelConfiguration.GenderText.text = userData.gender;
    }
}