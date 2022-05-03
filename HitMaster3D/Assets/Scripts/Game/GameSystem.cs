using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Transform playerCreationPoint;
    [SerializeField] private CinemachineVirtualCamera cameraMain;
    [SerializeField] private Sword swordPrefab;
    [SerializeField] private Transform swordParent;

    private GameLoop _gameLoop;
    private SwordShooter _swordShooter;
    private InputReceiver _inputReceiver;
    private BattlePointSystem _battlePointsSystem;

    private void Awake(){
        var player = Instantiate(playerPrefab, playerCreationPoint.position, Quaternion.identity, playerCreationPoint);
        cameraMain.Follow = player.transform;
        _battlePointsSystem = new BattlePointSystem();
        _inputReceiver = new InputReceiver();
        _swordShooter = new SwordShooter(swordPrefab, swordParent, player.firePoint, _inputReceiver);
        _gameLoop = new GameLoop(player, _swordShooter);
    }

    private void OnEnable(){
        _gameLoop.AllPointsCleanUp += SwitchLevel;
    }

    private void OnDisable(){
        _gameLoop.AllPointsCleanUp -= SwitchLevel;
    }

    private void Update() => _inputReceiver.Update();

    private void SwitchLevel(){
        SceneManager.LoadScene(0);
    }
}