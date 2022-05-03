using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> EnemyDeadEvent;
    private const int GET_UP_DELAY = 3000;

    [SerializeField] private float speed;
    [Header("HealthSystem")]
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Collider headshotCollider;
    [Range(1, 5)]
    [SerializeField] private int headshotMultiplayer;

    private HealthSystem _healthSystem;
    private UIHealthBar _healthBar;

    private Animator _animator;
    private RagDollSystem _ragdollSystem;

    private void Awake(){
        var dollRigidbodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponent<Animator>();
        _healthSystem = new HealthSystem(maxHealthPoints, headshotMultiplayer);
        _healthBar = new UIHealthBar(_healthSystem, healthBarSlider);
        _ragdollSystem = new RagDollSystem(dollRigidbodies, transform, _animator);
        _ragdollSystem.SettingsDollOn(false);
    }

    public void TakeDamage(Collider sender, int damage){
        var extraDamage = (headshotCollider == sender);
        _healthSystem.Damage(damage, extraDamage);
        _ragdollSystem.RagDollOn();

        if (_healthSystem.Health <= 0)
            EnemyDeadEvent?.Invoke(this);
        else
            _ragdollSystem.GetUp(GET_UP_DELAY);
    }

    public void LookAt(Vector3 target) => transform.LookAt(target);
    private void FixedUpdate() => _ragdollSystem.Upd();
}