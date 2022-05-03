using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PoolObjectsQueue<T> where T : MonoBehaviour
{
    private Queue<T> _pool;
    private readonly Transform _parentContainer;
    private readonly T _prefab;

    public PoolObjectsQueue(T prefab, int poolAmount, Transform parentContainer){
        _parentContainer = parentContainer;
        _prefab = prefab;

        CreatePool(poolAmount);
    }

    private void CreatePool(int poolAmount){
        _pool = new Queue<T>();

        for (var i = 0; i < poolAmount; i++)
            CreateElement();
    }

    private T CreateElement(bool isActiveAsDefault = false){
        var createdObj = UnityEngine.Object.Instantiate(_prefab, _parentContainer);
        createdObj.gameObject.SetActive(isActiveAsDefault);
        _pool.Enqueue(createdObj);
        return createdObj;
    }
    
    public T GetFreeElement(){
       var element = _pool.Dequeue();
       element.gameObject.SetActive(true);
      
       _pool.Enqueue(element);
       return element;
    }
}
