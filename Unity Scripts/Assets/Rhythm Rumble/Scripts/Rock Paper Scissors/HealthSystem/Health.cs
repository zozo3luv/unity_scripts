using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected float m_maxHealth;
    protected float m_currentHealth;

    protected void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    public void TakeDamage(float _damage)
    {
        if (m_currentHealth > 0)
        {
            m_currentHealth -= _damage;

            //Deal with death situation when hp is below 0
            if (m_currentHealth <= 0)
            {
                OnDeath();
                RPSCore.instance.EndGameMaster.Invoke();
            }
        }
    }

    public float CalculateHealthPct()
    {
        float _pct = m_currentHealth / m_maxHealth;
        return _pct;
    }

    protected abstract void OnDeath();
}
