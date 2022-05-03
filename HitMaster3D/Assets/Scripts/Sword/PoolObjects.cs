using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PoolObjects<T> where T : MonoBehaviour
{
    private List<T> _pool;
    private readonly bool _canExpand;
    private readonly Transform _parentContainer;
    private readonly T _prefab;

    public PoolObjects(T prefab, int poolAmount, Transform parentContainer, bool canExpand = false){
        _canExpand = canExpand;
        _parentContainer = parentContainer;
        _prefab = prefab;

        CreatePool(poolAmount);
    }

    private void CreatePool(int poolAmount){
        _pool = new List<T>();

        for (var i = 0; i < poolAmount; i++)
            CreateElement();
    }

    private T CreateElement(bool isActiveAsDefault = false){
        var createdObj = UnityEngine.Object.Instantiate(_prefab, _parentContainer);
        createdObj.gameObject.SetActive(isActiveAsDefault);
        _pool.Add(createdObj);
        return createdObj;
    }

    public bool HasFreeElement(out T element){
        foreach (var obj in _pool){
            if (obj.gameObject.activeInHierarchy) continue;

            element = obj;
            element.gameObject.SetActive(true);
            return true;
        }

        element = null;
        return false;
    }

    public T GetFreeElement(){
        if (HasFreeElement(out var element))
            return element;

        if (_canExpand)
            return CreateElement(true);

        throw new Exception($"в пуле закончились {typeof(T)}");
    }
}
