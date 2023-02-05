using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;

    public event Action<int, int> OnHit;
    public event Action OnDeath;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<AttackController>())
        {
            Hurt(1);
        }
    }

    private void Hurt(int amount)
    {
        CurrentHealth -= amount;
        OnHit?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
