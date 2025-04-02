using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBattleVFX : MonoBehaviour
{
    Animator m_animator;

    [SerializeField] ParticleSystem m_playerAttackVFX;
    [SerializeField] ParticleSystem m_compAttackVFX;

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        RPSCore.instance.AnimateCompWin.AddListener(PlayCompWinAnim);
        RPSCore.instance.AnimatePlayerWin.AddListener(PlayPlayerWinAnim);
    }

    void PlayPlayerAttackVFX()
    {
        m_playerAttackVFX.Stop();
        m_playerAttackVFX.Play();
    }

    void PlayCompAttackVFX()
    {
        m_compAttackVFX.Stop();
        m_compAttackVFX.Play();
    }

    void PlayCompWinAnim()
    {
        m_animator.SetTrigger("Comp Win");
    }

    void PlayPlayerWinAnim()
    {
        m_animator.SetTrigger("Player Win");
    }
}
