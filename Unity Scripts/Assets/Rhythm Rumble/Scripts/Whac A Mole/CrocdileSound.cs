using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocdileSound : MonoBehaviour
{
    [SerializeField] AudioSource m_crocdileUpSound;
    [SerializeField] AudioSource m_crocdileHitSound;

    public void PlayCrocdileUpSound()
    {
        m_crocdileUpSound.Stop();
        m_crocdileUpSound.Play();
    }

    public void PlayCrocodileHitSound()
    {
        m_crocdileHitSound.Stop();
        m_crocdileHitSound.Play();
    }
}
