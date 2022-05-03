using System;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public event Action CameOnPointEvent;

    public Transform firePoint;
    public LookAtEnemyHandler LookAtEnemyHandler;
    
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent meshAgent;
    private readonly int _isWalking = Animator.StringToHash("IsWalking");
    private PlayerState _playerState = PlayerState.None;

    private void Awake(){
        LookAtEnemyHandler = new LookAtEnemyHandler(transform);
    }

    private void FixedUpdate(){
        if (_playerState == PlayerState.Moving)
            AgentArrivedCheck();
        if (_playerState == PlayerState.Engage)
           LookAtEnemyHandler.LookAtEnemy();
    }

    private void AgentArrivedCheck(){
        if (meshAgent.pathPending) return;
        if (meshAgent.remainingDistance > meshAgent.stoppingDistance) return;
        if (meshAgent.hasPath && meshAgent.velocity.sqrMagnitude != 0f) return;

        _playerState = PlayerState.Engage;
        animator.SetBool(_isWalking, false);
        CameOnPointEvent?.Invoke();
    }

    public void MoveToPoint(Vector3 point){
        _playerState = PlayerState.Moving;
        animator.SetBool(_isWalking, true);
        meshAgent.Warp(transform.position);
        meshAgent.SetDestination(point);
    }
}

public enum PlayerState
{
    None = -1,
    Moving = 0,
    Engage = 1
};