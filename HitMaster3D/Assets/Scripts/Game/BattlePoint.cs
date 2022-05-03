using System;
using System.Collections.Generic;
using UnityEngine;

public class BattlePoint : MonoBehaviour, IComparable<BattlePoint>
{
    public event Action BattlePointCleanEvent;
    public Vector3 Point => stopPoint.position;
    public List<Enemy> EnemyList{ get; private set; }

    public int visitingOrder;
   
    [SerializeField] private Transform stopPoint;

    private void Awake(){
        EnemyList = new List<Enemy>();
        EnemyList.AddRange(GetComponentsInChildren<Enemy>());

        foreach (var enemy in EnemyList)
            enemy.LookAt(Point);

        SubscribeKillEvent();
    }

    private void SubscribeKillEvent(){
        foreach (var enemy in EnemyList)
            enemy.EnemyDeadEvent += RemoveKilled;
    }
    
    private void RemoveKilled(Enemy sender){
        EnemyList.Remove(sender);
        ListEmptyCheck();
    }

    private void ListEmptyCheck(){
        if (EnemyList.Count == 0)
            BattlePointCleanEvent?.Invoke();
    }
    


    public int CompareTo(BattlePoint other) => visitingOrder.CompareTo(other.visitingOrder);
}