using System;
using UnityEngine;

public class FactoryMonoObject<T> : IFactory<T>
{
    private readonly GameObject _prefab;
    public Transform Parent { get; private set; }

    public FactoryMonoObject(GameObject prefab, Transform parent)
    {
        Parent = parent;
        _prefab = prefab;
        var newParent = new GameObject();
        newParent.transform.parent = parent;
        Parent = newParent.transform;
        Parent.name = _prefab.name;
    }

    public T CreateObject()
    {
        var newObject = GameObject.Instantiate(_prefab, Parent);

        var returnValue = newObject.GetComponent<T>();
        newObject.SetActive(false);
        if (returnValue != null)
        {
            return returnValue;
        }
        else
        {
            throw new InvalidOperationException(
                $"The requested object is missing from the prefab {typeof(T)} >> {_prefab.name}");
        }
    }
}