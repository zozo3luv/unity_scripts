using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    private void Start()
    {
        RPSCore.instance.PlayerWin.AddListener(delegate { DealDamageToComp(m_playerCurrentDamage); });
        RPSCore.instance.CompWin.AddListener(DealDamageToPlayer);
    }

    [SerializeField] PlayerHealth m_playerHealth;
    [SerializeField] CompHealth m_compHealth;

    //Player damage points settings
    [SerializeField] float m_playerMinDamage;
    [SerializeField] float m_stage1Damage;
    [SerializeField] float m_stage2Damage;
    [SerializeField] float m_playerMaxDamage;
    float m_playerCurrentDamage;

    #region Damage Stage
    public PlayerDamageStage currentDamageStage { get; private set; }
    public enum PlayerDamageStage
    {
        Min,
        Stage1,
        Stage2,
        Max
    }
    #endregion

    [SerializeField] float m_compFixedDamage;
    void DealDamage(Health _damageTaker, float _damage)
    {
        _damageTaker.TakeDamage(_damage);
    }

    //When the player make a wrong choice or the timer runs out, comp deal fixed damage to player
    public void DealDamageToPlayer()
    {
        DealDamage(m_playerHealth, m_compFixedDamage);
    }

    public void DealDamageToComp(float _damage)
    {
        DealDamage(m_compHealth, m_playerCurrentDamage);
    }

    public void ChangePlayerDamage(PlayerDamageStage _stage)
    {
        currentDamageStage = _stage;
        switch (_stage)
        {
            case PlayerDamageStage.Min:
                m_playerCurrentDamage = m_playerMinDamage;
                break;

            case PlayerDamageStage.Stage1:
                m_playerCurrentDamage = m_stage1Damage;
                break;

            case PlayerDamageStage.Stage2:
                m_playerCurrentDamage = m_stage2Damage;
                break;

            case PlayerDamageStage.Max:
                m_playerCurrentDamage = m_playerMaxDamage;
                break;
        }
    }
}
