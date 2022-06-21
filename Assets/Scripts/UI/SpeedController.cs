using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private ViewScroller viewScroller;

    private void OnEnable()
    {
        button.onClick.AddListener(UpdateSpeed);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(UpdateSpeed);
    }

    private void UpdateSpeed()
    {
        if (inputField.text.Any(c => char.IsLetter(c)))
        {
            return;
        }

        viewScroller.SetSpeed = Convert.ToInt32(inputField.text);
    }
}