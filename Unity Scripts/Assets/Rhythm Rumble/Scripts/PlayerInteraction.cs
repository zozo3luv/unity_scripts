using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private CharacterController m_charCtrl;

    [Header("Hit Box Settings")]
    [SerializeField] private bool m_drawPreview = false;
    [SerializeField] private float m_hitBoxWidth;
    [SerializeField] private float m_hitBoxHeight;
    [SerializeField] private float m_hitBoxLength;
    [SerializeField] private LayerMask m_hitLayers;
    private Vector3 m_hitBoxHalfExtends;

    void Start()
    {
        m_hitBoxHalfExtends = new Vector3(m_hitBoxWidth / 2, m_hitBoxLength / 2, 0);
    }

    void Update()
    {
        Vector3 _origin = transform.position + m_charCtrl.center * m_charCtrl.transform.localScale.x;
        RaycastHit[] _hit = Physics.BoxCastAll(_origin, m_hitBoxHalfExtends, transform.forward, transform.rotation, m_hitBoxLength, m_hitLayers);
    }

    //Debug in scene view
    private void OnDrawGizmos()
    {
        if (!m_drawPreview) return;
        Vector3 _center = m_charCtrl.center * m_charCtrl.transform.localScale.x + transform.position + new Vector3(0, 0, m_hitBoxLength / 2);
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_center, new Vector3(m_hitBoxWidth, m_hitBoxHeight, m_hitBoxLength));
    }
}