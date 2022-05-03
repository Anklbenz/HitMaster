using System;
using UnityEngine;

public class GameLoop : IDisposable
{
    public event Action AllPointsCleanUp;
    
    private readonly Player _player;
    private readonly SwordShooter _swordShooter;
    private readonly BattlePointSystem _battlePointSystem;
    


    public GameLoop(Player player, SwordShooter swordShooter){
        _player = player;
        _swordShooter = swordShooter;
        _battlePointSystem =  new BattlePointSystem();
        _battlePointSystem.Current.BattlePointCleanEvent += TransitionToNextPoint;
        _player.CameOnPointEvent += Engage;

        MoveToPoint();
    }

    public void Dispose()=>_player.CameOnPointEvent -= Engage;

    private void TransitionToNextPoint(){
        _battlePointSystem.Current.BattlePointCleanEvent -= TransitionToNextPoint;

        if (_battlePointSystem.MoveNext()){
            _battlePointSystem.Current.BattlePointCleanEvent += TransitionToNextPoint;
            MoveToPoint();
        }
        else{
            NoMoreBattlePointsLeft();
        }
    }

    private void MoveToPoint(){
        var point = _battlePointSystem.Current.Point;
        _player.MoveToPoint(point);
        _swordShooter.IsActive = false;
    }

    private void Engage(){
        _player.LookAtEnemyHandler.ListOfEnemyOnPoint = _battlePointSystem.Current.EnemyList;
        _swordShooter.IsActive = true;
    }

    private void NoMoreBattlePointsLeft(){
        if (_battlePointSystem.Count > 0)
            AllPointsCleanUp?.Invoke();
        else
            Debug.LogError("BattlePoint system has no items");
    }

}