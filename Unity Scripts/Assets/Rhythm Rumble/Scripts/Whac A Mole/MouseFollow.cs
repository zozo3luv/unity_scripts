using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static RootMotion.FinalIK.HitReaction;

public class MouseFollow : MonoBehaviour
{
    [Header("Toilet Sucker Properties")]
    [SerializeField] GameObject m_toiletSucker;
    [SerializeField] Animator m_toiletSuckerAnimator;
    [SerializeField] float m_zOffset;

    float m_startYPos;
    Plane groundPlane;

    [Header("Mouse Move Bounds")]
    [SerializeField] Transform m_upBound;
    [SerializeField] Transform m_lowerBound;
    [SerializeField] Transform m_leftBound;
    [SerializeField] Transform m_rightBound;

    [Header("Pool Bounds")]
    [SerializeField] Transform m_poolUpBound;
    [SerializeField] Transform m_poolLowerBound;
    [SerializeField] Transform m_poolLeftBound;
    [SerializeField] Transform m_poolRightBound;

    [Space]
    [SerializeField] WhacAMoleManager m_gm;

    [Header("VFX")]
    [SerializeField] ParticleSystem m_toiletParticle;
    [SerializeField] GameObject m_waterSplash;
    [SerializeField] Transform m_waterSplashPosition;

    [Header("Material Settings")]
    [SerializeField] MeshRenderer m_mesh;
    [SerializeField] Material m_opaqueMat;
    [SerializeField] Material m_transMat;
    [SerializeField] Color m_transparentColor;
    Color m_startColor;

    bool m_playerActive = true;

    private void Awake()
    {
        //Set toilet sucker materials
        m_startColor = m_mesh.material.color;
        m_gm.WAM_Idle.AddListener(delegate { SetTransparent(true); });
        m_gm.WAM_Play.AddListener(delegate { SetTransparent(false); });

        //Set VFX events
        m_gm.WAM_Score_onMiss.AddListener(PlayWaterSplash);
        m_gm.WAM_Score_onScore.AddListener(PlayHitParticle);

        //Set end game
        m_gm.WAM_Win.AddListener(delegate { SetPlayerActive(false); });
        m_gm.WAM_Lose.AddListener(delegate { SetPlayerActive(false); });
    }

    void SetTransparent(bool _yesOrNo)
    {
        if (_yesOrNo)
        {
            m_mesh.material = m_transMat;
            m_mesh.material.color = m_transparentColor;
        } else
        {
            m_mesh.material = m_opaqueMat;
            m_mesh.material.color = m_startColor;
        }
    }

    #region Start and Update
    private void Start()
    {
        m_startYPos = m_toiletSucker.transform.position.y;
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        if (!m_playerActive)
        {
            return;
        }
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Declare a variable to store the hit information
        float hitDistance;
        float _mouseX = 0;
        float _mouseZ = 0;

        // Check if the ray intersects with the ground plane
        if (groundPlane.Raycast(ray, out hitDistance))
        {
            // Get the intersection point
            Vector3 hitPoint = ray.GetPoint(hitDistance);

            // Get the x and z coordinates of the hit point
            _mouseX = hitPoint.x;
            _mouseZ = hitPoint.z;
        }

        //Check if the mouse is out of bound
        bool _outOfXBound = (_mouseX < m_leftBound.position.x) || (_mouseX > m_rightBound.position.x);
        bool _outOfZBound = (_mouseZ < m_lowerBound.position.z) || (_mouseZ > m_upBound.position.z);
        Vector3 _newPos = m_toiletSucker.transform.position;

        //Set Mouse Visibility
        if (_outOfXBound || _outOfZBound)
        {
            if (!Cursor.visible) Cursor.visible = true;
        } else
        {
            if (Cursor.visible) Cursor.visible = false;
        }

        //Assign new coordinates
        if (!_outOfXBound) _newPos.x = _mouseX;
        if (!_outOfZBound) _newPos.z = _mouseZ + m_zOffset;
        m_toiletSucker.transform.position = new Vector3(_newPos.x, m_startYPos, _newPos.z);

        //Pressing mouse
        if (Input.GetMouseButtonDown(0) && m_gm.m_gameState == WhacAMoleManager.WAM_States.Play)
        {
            m_toiletSuckerAnimator.SetTrigger("Press");
        }
    }
    #endregion

    #region VFX controls
    void PlayWaterSplash()
    {
        Vector3 _playerPos = m_toiletSucker.transform.position;
        bool _outOfXBound = (_playerPos.x < m_poolLeftBound.position.x) || (_playerPos.x > m_poolRightBound.position.x);
        bool _outOfZBound = (_playerPos.z < m_poolLowerBound.position.z) || (_playerPos.z > m_poolUpBound.position.z);

        if (!_outOfXBound && !_outOfZBound) Instantiate(m_waterSplash, m_waterSplashPosition.position, Quaternion.identity);
    }

    void PlayHitParticle()
    {
        m_toiletParticle.Clear();
        m_toiletParticle.Play();
    }
    #endregion

    void SetPlayerActive(bool _yesOrNo = false)
    {
        Cursor.visible = true;
        m_playerActive = false;
        m_toiletSucker.gameObject.SetActive(_yesOrNo);
    }
}
