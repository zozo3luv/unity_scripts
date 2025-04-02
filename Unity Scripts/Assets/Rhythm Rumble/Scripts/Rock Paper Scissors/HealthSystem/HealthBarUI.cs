using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Health Bar UI Elements")]
    [SerializeField] Slider m_playerHealthBar;
    [SerializeField] Slider m_compHealthBar;

    [Header("Health System")]
    [SerializeField] PlayerHealth m_playerHealth;
    [SerializeField] CompHealth m_compHealth;

    [Header("Other Settings")]
    [SerializeField] float m_playerHealthUpdateDelay = 0.1f;
    [SerializeField] float m_compHealthUpdateDelay = 0.1f;

    private void Start()
    {
        RPSCore.instance.AnimateCompWin.AddListener(delegate { Invoke("UpdatePlayerHealthBar", m_playerHealthUpdateDelay); });
        RPSCore.instance.AnimatePlayerWin.AddListener(delegate { Invoke("UpdateCompHealthBar", m_compHealthUpdateDelay); });
    }

    void UpdatePlayerHealthBar()
    {
        m_playerHealthBar.value = m_playerHealth.CalculateHealthPct();
    }

    void UpdateCompHealthBar()
    {
        m_compHealthBar.value = m_compHealth.CalculateHealthPct();
    }
}
