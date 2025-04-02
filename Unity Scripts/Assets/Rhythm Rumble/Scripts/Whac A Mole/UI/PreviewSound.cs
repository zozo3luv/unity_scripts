using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSound : MonoBehaviour
{
    [SerializeField] AudioSource m_previewSound;

    public void PlayPreviewSound()
    {
        m_previewSound.Stop();
        m_previewSound.Play();
    }
}
