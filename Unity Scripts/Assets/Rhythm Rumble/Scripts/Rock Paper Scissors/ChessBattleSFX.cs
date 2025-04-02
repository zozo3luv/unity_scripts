using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBattleSFX : MonoBehaviour
{
    [SerializeField] AudioSource m_compAttackSFX;
    [SerializeField] AudioSource m_playerAttackSFX;

    void PlayCompAttackSFX()
    {
        m_compAttackSFX.Stop();
        m_compAttackSFX.Play();
    }

    void PlayPlayerAttackSFX()
    {
        m_playerAttackSFX.Stop();
        m_playerAttackSFX.Play();
    }
}
