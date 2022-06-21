using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MonoPooledWithRectTransform : MonoBehaviour, IPooledObject
{
    private bool _isDisabled;
    private IPool _pool;
    public RectTransform RectTransform { get; private set; }
    public Transform TransformParent { get; set; }
    
    public virtual void Initialize()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _isDisabled = true;
    }

    public virtual void ReturnToPool()
    {
        if (_isDisabled)
        {
            return;
        }
        gameObject.SetActive(false);
        _pool.Push(this);
    }

    public void SetParentPool<T>(IPool<T> parent) where T : IPooledObject
    {
        _pool = parent;
        RectTransform = GetComponent<RectTransform>();
    }
}