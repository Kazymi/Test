using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewScroller : MonoBehaviour
{
    [SerializeField] private int speed = 20;
    [SerializeField] private MonoPooledWithRectTransform panel;
    [SerializeField] private int AmountPanelOnStart;
    [SerializeField] private Transform pivot;
    [SerializeField] private ViewScrollerConfiguration viewScrollerConfiguration;
    [SerializeField] private Scrollbar _scrollbar;

    private IPool<MonoPooledWithRectTransform> _poolPanels;

    public event Action<MonoPooledWithRectTransform> NewPanelSpawnedBack;
    public event Action<MonoPooledWithRectTransform> NewPanelSpawnedForward;

    public bool IsMoveUpUnlocked { get; set; } = true;
    public bool IsMoveDownUnlocked { get; set; } = true;

    public int SetSpeed
    {
        set => speed = value;
    }

    public List<MonoPooledWithRectTransform> SpawnedView { get; } = new List<MonoPooledWithRectTransform>();

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Scroll();
    }

    private void Initialize()
    {
        var factory = new FactoryMonoObject<MonoPooledWithRectTransform>(panel.gameObject, transform);
        _poolPanels = new Pool<MonoPooledWithRectTransform>(factory, AmountPanelOnStart);

        for (var i = 0; i != AmountPanelOnStart; i++)
        {
            AddNewPanelBack();
        }

        RecalculatePanels();
    }

    private void Scroll()
    {
        foreach (var panelItem in SpawnedView)
        {
            if (_scrollbar.value > 0.5)
            {
                if (IsMoveDownUnlocked == false) return;
                var speedMove = speed;
                panelItem.RectTransform.anchoredPosition +=
                    new Vector2(0, speedMove);
            }
            else
            {
                if (IsMoveUpUnlocked == false) return;
                var speedMove = speed;
                panelItem.RectTransform.anchoredPosition -=
                    new Vector2(0, speedMove);
            }
        }

        RecalculatePanels();
    }

    private void AddNewPanelBack()
    {
        var newList = _poolPanels.Pull();
        newList.transform.parent = pivot;
        var newYPos = 0f;
        if (SpawnedView.Count != 0)
        {
            newYPos = GetMinY();
        }
        else
        {
            newYPos += viewScrollerConfiguration.PanelHeight;
        }

        newList.RectTransform.anchoredPosition = new Vector2(0, newYPos - viewScrollerConfiguration.PanelHeight);
        newList.RectTransform.sizeDelta = new Vector2(0, viewScrollerConfiguration.PanelHeight);
        NewPanelSpawnedBack?.Invoke(newList);
        SpawnedView.Add(newList);
    }

    public void SpawnPanels(int amount)
    {
        for (var i = 0; i != amount; i++)
        {
            AddNewPanelBack();
        }

        RecalculatePanels();
    }

    private void AddNewPanelFroward()
    {
        var newList = _poolPanels.Pull();
        newList.transform.parent = pivot;
        var newYPos = 0f;
        if (SpawnedView.Count != 0)
        {
            newYPos = GetMaxY();
        }

        newList.RectTransform.anchoredPosition = new Vector2(0, newYPos + viewScrollerConfiguration.PanelHeight);
        newList.RectTransform.sizeDelta = new Vector2(0, viewScrollerConfiguration.PanelHeight);
        NewPanelSpawnedForward?.Invoke(newList);
        SpawnedView.Add(newList);
    }

    private float GetMaxY()
    {
        var maxValue = 0f;
        foreach (var spawn in SpawnedView)
        {
            if (spawn.RectTransform.anchoredPosition.y > maxValue) maxValue = spawn.RectTransform.anchoredPosition.y;
        }

        return maxValue;
    }

    private float GetMinY()
    {
        var maxValue = 0f;
        foreach (var spawn in SpawnedView)
        {
            if (spawn.RectTransform.anchoredPosition.y < maxValue) maxValue = spawn.RectTransform.anchoredPosition.y;
        }

        return maxValue;
    }

    private void RecalculatePanels()
    {
        bool isNeedRecursion = false;
        foreach (var view in SpawnedView)
        {
            if (view.RectTransform.anchoredPosition.y > viewScrollerConfiguration.MAXY)
            {
                var removedPanel = view;
                AddNewPanelBack();
                SpawnedView.Remove(removedPanel);
                IsMoveUpUnlocked = true;
                removedPanel.ReturnToPool();
                break;
            }

            if (view.RectTransform.anchoredPosition.y < viewScrollerConfiguration.MINY)
            {
                var removedPanel = view;
                AddNewPanelFroward();
                SpawnedView.Remove(removedPanel);
                IsMoveDownUnlocked = true;
                removedPanel.ReturnToPool();
                break;
            }
        }
    }
}