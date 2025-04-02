using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] WhacAMoleManager m_gm;
    [SerializeField] Slider m_progressBarMain;

    private void Awake()
    {
        m_gm.WAM_Score_onScore.AddListener(UpdateProgressBar);
    }

    private void Start()
    {
        m_progressBarMain.value = 1;
    }

    void UpdateProgressBar()
    {
        //Progress Bar move from value 1 to 0
        float _currentPct = m_gm.GetCurrentScore() / m_gm.GetWinScore();
        //Debug.Log(_currentPct);
        m_progressBarMain.value = 1 - _currentPct;
    }

}
