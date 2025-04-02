using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogoManager : MonoBehaviour
{
    [SerializeField] string m_gameSceneName;
    [SerializeField] GameObject m_originalLogo;
    [SerializeField] GameObject m_completedLogo;

    private void Start()
    {
        if (MasterGameManager.instance.CheckGameCompletion(m_gameSceneName))
        {
            m_originalLogo.SetActive(false);
            m_completedLogo.SetActive(true);
        } else
        {
            m_originalLogo.SetActive(true);
            m_completedLogo.SetActive(false);
        }
    }
}
