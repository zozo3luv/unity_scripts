using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WAMEndGameUI : MonoBehaviour
{
    [SerializeField] WhacAMoleManager m_gm;

    [Space]
    [SerializeField] GameObject m_restartUI;
    private void Awake()
    {
        m_gm.WAM_Lose.AddListener(ActivateRestartUI);
    }

    private void Start()
    {
        m_restartUI.SetActive(false);
    }

    void ActivateRestartUI()
    {
        m_restartUI.SetActive(true);
    }
}
