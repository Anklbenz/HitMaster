using System;
using System.Collections.Generic;
using UnityEngine;

public class BattlePointSystem 
{
    private int _index;
    private readonly List<BattlePoint> _map = new List<BattlePoint>();

    public BattlePointSystem(){
        _map.AddRange(UnityEngine.Object.FindObjectsOfType<BattlePoint>());
        _map.Sort();
        _index = _map.Count == 0 ? -1 : 0;
    }

    public BattlePoint Current{
        get{
            if (_index == -1 || _index >= _map.Count)
                Debug.LogError("List of Battle Point is Empty or index out Of range");
            return _map[_index];
        }
    }

    public int Count => _map.Count;

    public bool MoveNext(){
        if (_index >= _map.Count - 1)
            return false;

        _index++;
        return true;
    }
}
