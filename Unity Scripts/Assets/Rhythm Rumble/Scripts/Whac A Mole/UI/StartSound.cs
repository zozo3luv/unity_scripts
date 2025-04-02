using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSound : MonoBehaviour
{
    [SerializeField] AudioSource m_startSound;

    public void PlayStartSound()
    {
        m_startSound.Stop();
        m_startSound.Play();
    }
}
