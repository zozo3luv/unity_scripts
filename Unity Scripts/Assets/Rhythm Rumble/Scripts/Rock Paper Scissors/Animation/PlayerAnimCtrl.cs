using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour
{
    Animator m_animator;
    bool m_playerSelected;

    [SerializeField] MeshFilter m_playerHandHolder;
    [SerializeField] Mesh m_rock;
    [SerializeField] Mesh m_paper;
    [SerializeField] Mesh m_scissors;
    [SerializeField] Mesh m_timeRunOut;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        RPSCore.instance.StartRound.AddListener(
            delegate 
            { 
                ControlSwingAnim(true); 
                SetPlayerSelected(false); 
            });
        RPSCore.instance.EndRound.AddListener(delegate { ControlSwingAnim(false); });
        RPSCore.instance.PlayerSelect.AddListener(PlayPlayerSelectAnim);
        RPSCore.instance.PlayerSelect.AddListener(delegate { SetPlayerSelected(true); });
    }

    void SetPlayerSelected(bool _selected)
    {
        m_playerSelected = _selected;
    }

    void ControlSwingAnim(bool _play)
    {
        if (_play) m_animator.SetTrigger("Swing");
        else if (!_play && !m_playerSelected) m_animator.SetTrigger("Stop Swing");
    }

    void PlayPlayerSelectAnim()
    {
        m_animator.SetTrigger("Select Hand");
    }

    void SetHandMesh()
    {
        Mesh _hand = new Mesh();
        #region choose hand mesh based on player hand selection
        switch (RPSCore.instance.playerHand)
        {
            case RPSCore.Hand.Rock:
                _hand = m_rock;
                break;

            case RPSCore.Hand.Paper:
                _hand = m_paper;
                break;

            case RPSCore.Hand.Scissors:
                _hand = m_scissors;
                break;
        }
        #endregion

        m_playerHandHolder.mesh = _hand;
    }

    void SetTimeRunOutMesh()
    {
        m_playerHandHolder.mesh = m_timeRunOut;
    }

    //Implement on animation clip. Combined usage with chess battle components
    void PlayAttackAnimation()
    {
        RPSCore.instance.OnAttackAnimation();
    }

    void ResetHandMesh()
    {
        m_playerHandHolder.mesh = m_rock;
    }
}
