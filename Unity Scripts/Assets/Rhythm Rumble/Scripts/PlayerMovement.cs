using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController m_controller;
    private Vector3 m_playerVelocity;
    private bool m_groundedPlayer;

    [Header("Settings")]
    [SerializeField] private float m_playerSpeed = 2.0f;
    [SerializeField] private float m_jumpHeight = 1.0f;
    [SerializeField] private float m_gravityValue = -9.81f;

    private void Start()
    {
        m_controller = gameObject.GetComponent<CharacterController>();
    }


    private void FixedUpdate()
    {
        m_groundedPlayer = m_controller.isGrounded;

        if (m_groundedPlayer && m_playerVelocity.y < 0)
        {
            m_playerVelocity.y = 0f;
        }
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_controller.Move(move * Time.deltaTime * m_playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && m_groundedPlayer)
        {
            m_playerVelocity.y += Mathf.Sqrt(m_jumpHeight * -3.0f * m_gravityValue);
        }

        m_playerVelocity.y += m_gravityValue * Time.deltaTime;
        m_controller.Move(m_playerVelocity * Time.deltaTime);
    }
}