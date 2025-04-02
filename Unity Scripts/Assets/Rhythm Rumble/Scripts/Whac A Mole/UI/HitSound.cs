using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSound : MonoBehaviour
{
    [SerializeField] AudioSource m_crocdileHitSound;

    public void PlayCrocdileHitSound()
    {
        m_crocdileHitSound.Stop();
        m_crocdileHitSound.Play();
    }

}
