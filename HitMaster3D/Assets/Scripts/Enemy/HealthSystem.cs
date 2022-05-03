using System;

public class HealthSystem
{
   public event Action<float> HealthChangedEvent;
   public int Health => _currentHealth;
    
    private readonly int _maxHealth, _extraDamageMulti;
    private int _currentHealth;

    public HealthSystem(int maxHealth, int extraDamageMulti){
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        _extraDamageMulti = extraDamageMulti;
    }

    public void Damage(int damagePoints, bool extraDamage){
        _currentHealth -= extraDamage ? damagePoints*_extraDamageMulti : damagePoints;

        var percent = (float) (_currentHealth) / _maxHealth;
        HealthChangedEvent?.Invoke(percent);
    }
}
