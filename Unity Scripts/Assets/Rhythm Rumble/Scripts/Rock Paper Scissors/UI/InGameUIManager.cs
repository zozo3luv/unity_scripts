using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        RPSCore.instance.CompDeath.AddListener(FadeUI);
        RPSCore.instance.EndGameMaster.AddListener(DisableHandButtons);
    }

    void FadeUI()
    {
        m_animator.SetTrigger("Win Game");
    }

    #region Hand Button Management
    [Header("Hand Buttons")]
    [SerializeField] Button[] m_handButtons;

    void DisableHandButtons()
    {
        foreach (var _button in m_handButtons)
        {
            _button.interactable = false;
        }
    }
    #endregion
}
