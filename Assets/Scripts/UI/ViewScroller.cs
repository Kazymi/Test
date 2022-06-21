using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewScroller : MonoBehaviour
{
    [SerializeField] private float duration = 20;
    [SerializeField] private int maxSpeed = 10;
    [SerializeField] private MonoPooledWithRectTransform panel;
    [SerializeField] private int AmountPanelOnStart;
    [SerializeField] private Transform pivot;
    [SerializeField] private ViewScrollerConfiguration viewScrollerConfiguration;

    private IPool<MonoPooledWithRectTransform> _poolPanels;

    public event Action<MonoPooledWithRectTransform> NewPanelSpawnedBack;
    public event Action<MonoPooledWithRectTransform> NewPanelSpawnedForward;

    public bool IsMoveUpUnlocked { get; set; } = true;
    public bool IsMoveDownUnlocked { get; set; } = true;

    private Vector3 _dragStartPosition;
    private Vector3 _dragCurrentPosition;
    private Vector3 _deltaPosition;

    private bool _isTuched = false;

    public List<MonoPooledWithRectTransform> SpawnedView { get; } = new List<MonoPooledWithRectTransform>();

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Scroll();
        UpdateInput();
    }

    private void UpdateInput()
    {
        if (_isTuched == false)
        {
            if (Input.touchCount != 0)
            {
                _isTuched = true;
                OnPointerDown();
            }
        }
        else
        {
            OnDrag();
            if (Input.touchCount == 0)
            {
                _isTuched = false;
            }
        }
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
            if (_deltaPosition == Vector3.zero)
            {
                break;
            }
            if (_deltaPosition.y > 0)
            {
                if (IsMoveDownUnlocked == false)
                {
                    break;
                }

                var speedMove = duration * _deltaPosition.y;
                if (speedMove > maxSpeed) speedMove = maxSpeed;
                panelItem.RectTransform.anchoredPosition +=
                    new Vector2(0, speedMove);
            }
            else
            {
                if (IsMoveUpUnlocked == false)
                {
                    break;
                }

                var speedMove = duration * _deltaPosition.y;
                if (speedMove < -maxSpeed) speedMove = -maxSpeed;
                panelItem.RectTransform.anchoredPosition +=
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
        foreach (var view in SpawnedView)
        {
            if (view.RectTransform.anchoredPosition.y < viewScrollerConfiguration.MINY)
            {
                var removedPanel = view;
                AddNewPanelFroward();
                SpawnedView.Remove(removedPanel);
                IsMoveDownUnlocked = true;
                removedPanel.ReturnToPool();
                break;
            }
            
            if (view.RectTransform.anchoredPosition.y > viewScrollerConfiguration.MAXY)
            {
                var removedPanel = view;
                AddNewPanelBack();
                SpawnedView.Remove(removedPanel);
                IsMoveUpUnlocked = true;
                removedPanel.ReturnToPool();
                break;
            }
        }
    }

    private void OnPointerDown()
    {
        _dragStartPosition = Input.GetTouch(0).position;
    }

    private void OnDrag()
    {
        _dragCurrentPosition = Input.GetTouch(0).position;
        _deltaPosition = _dragStartPosition - _dragCurrentPosition;
        _dragStartPosition = _dragCurrentPosition;
    }
}