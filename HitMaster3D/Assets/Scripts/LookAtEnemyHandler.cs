using System.Collections.Generic;
using UnityEngine;

public class LookAtEnemyHandler
{
    public List<Enemy> ListOfEnemyOnPoint{ get; set; }

    private const float ROTATION_SPEED = 1.5f;
    private readonly Transform _transform;

    public LookAtEnemyHandler(Transform parentTransform){
        _transform = parentTransform;
    }

    public void LookAtEnemy(){
        if (ListOfEnemyOnPoint == null || ListOfEnemyOnPoint.Count == 0) return;

        var enemy = FindNearestEnemy(ListOfEnemyOnPoint);
        var direction = enemy.transform.position - _transform.position;
        var lookRotation = Quaternion.LookRotation(direction);
        _transform.rotation = Quaternion.Lerp(_transform.rotation, lookRotation, ROTATION_SPEED * Time.deltaTime);
    }

    private Enemy FindNearestEnemy(List<Enemy> enemyUnits){
        float min = -1;
        Enemy nearestEnemy = null;

        foreach (var enemy in enemyUnits){
            var current = (enemy.transform.position - _transform.position).magnitude;

            if ((min > 0) && (current >= min)) continue;
            min = current;
            nearestEnemy = enemy;
        }

        return nearestEnemy;
    }
}
