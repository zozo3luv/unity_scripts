using SmoothShakeScript;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Presets;
#endif
using UnityEngine;

[RequireComponent(typeof(SmoothShake))]
public class CameraShakeController : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] Preset m_playerWinPreset;
    [SerializeField] Preset m_compWinPreset;
#endif

    SmoothShake m_smoothShake;

    private void Start()
    {
        m_smoothShake = GetComponent<SmoothShake>();

#if UNITY_EDITOR
        RPSCore.instance.AnimateCompWin.AddListener(OnCompWinCamShake);
        RPSCore.instance.AnimatePlayerWin.AddListener(OnPlayerWinCamShake);
#else
        RPSCore.instance.AnimateCompWin.AddListener(OnCameraShake);
        RPSCore.instance.AnimatePlayerWin.AddListener(OnCameraShake);
#endif
    }

#if UNITY_EDITOR
    void OnPlayerWinCamShake()
    {
        m_playerWinPreset.ApplyTo(m_smoothShake);
        OnCameraShake();
    }

    void OnCompWinCamShake()
    {
        m_compWinPreset.ApplyTo(m_smoothShake);
        OnCameraShake();
    }
#endif

    void OnCameraShake()
    {
        m_smoothShake.StartShake();
    }
}
