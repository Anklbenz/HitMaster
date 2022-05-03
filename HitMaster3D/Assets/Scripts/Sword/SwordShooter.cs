using System;
using UnityEngine;

public class SwordShooter : IDisposable
{
    public bool IsActive{ get; set; }
    private readonly Camera _camera;
    private readonly Transform _firePoint;
    private readonly InputReceiver _input;
    private readonly PoolObjectsQueue<Sword> _pool;

    public SwordShooter(Sword prefab, Transform prefabParent, Transform firePoint, InputReceiver input){
        _input = input;
        _firePoint = firePoint;
        _pool = new PoolObjectsQueue<Sword>(prefab, 5, prefabParent);
        _camera = Camera.main;

        _input.TapStartEvent += Shoot;
    }

    public void Dispose() => _input.TapStartEvent -= Shoot;

    private void Shoot(Vector3 touchPoint, float time){
        if (!IsActive) return;

        var target = GetMouseWordPosition(touchPoint);
        var position = _firePoint.position;
        var sword = _pool.GetFreeElement();
        sword.Initialize(position, target);
    }

    private Vector3 GetMouseWordPosition(Vector3 pos){
        var ray = _camera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out var hit))
            return hit.point;

        pos.z = _camera.farClipPlane;
        return _camera.ScreenToWorldPoint(pos);
    }
}



