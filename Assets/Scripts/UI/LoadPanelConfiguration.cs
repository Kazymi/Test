using System;
using TMPro;
using UnityEngine;

[Serializable]
public class LoadPanelConfiguration
{
    [SerializeField] private TMP_Text fistNameText;
    [SerializeField] private TMP_Text lastNameText;
    [SerializeField] private TMP_Text mailText;
    [SerializeField] private TMP_Text ipText;
    [SerializeField] private TMP_Text genderText;

    public TMP_Text FistNameText => fistNameText;

    public TMP_Text LastNameText => lastNameText;

    public TMP_Text MailText => mailText;

    public TMP_Text IPText => ipText;

    public TMP_Text GenderText => genderText;
}