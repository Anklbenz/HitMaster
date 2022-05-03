using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar :IDisposable
{
   private readonly Slider _healthBar;
   private readonly HealthSystem _healthSystem;

   public UIHealthBar(HealthSystem healthSystem, Slider healthBar){
      _healthSystem = healthSystem;
      _healthBar = healthBar;
      _healthSystem.HealthChangedEvent += SetHeath;
   }

   public void Dispose() =>_healthSystem.HealthChangedEvent -= SetHeath;

   private void SetHeath(float percentage){
      _healthBar.value = percentage;
   }

}
