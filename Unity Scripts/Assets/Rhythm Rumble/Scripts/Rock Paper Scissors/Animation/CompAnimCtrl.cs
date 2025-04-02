using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CompAnimCtrl : MonoBehaviour
{
    Animator m_animator;

    [SerializeField] MeshFilter m_compHandHolder;
    [SerializeField] Mesh m_rock;
    [SerializeField] Mesh m_paper;
    [SerializeField] Mesh m_scissors;

    [SerializeField] float m_resetIdleDelay = 0.5f;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        RPSCore.instance.StartRound.AddListener(PlayAnim);
        RPSCore.instance.EndRound.AddListener(delegate { Invoke("PlayIdle", m_resetIdleDelay); });
    }

    void PlayAnim()
    {
        m_animator.SetTrigger("Swing");
    }

    void PlayIdle()
    {
        m_animator.SetTrigger("Idle");
    }

    //Implement on animation
    void RandomSetHandMesh()
    {
        SetHandMesh(true);
    }

    //Implement on animation
    void SetHandMesh()
    {
        SetHandMesh(false);
    }

    void SetHandMesh(bool _randomHand)
    {
        Mesh _hand = new Mesh();
        #region Randomly choose a hand
        if (_randomHand)
        {
            int _rand = Random.Range(0, 3);
            switch( _rand )
            {
                case 0:
                    _hand = m_rock;
                    break;

                case 1:
                    _hand = m_paper;
                    break;

                case 2:
                    _hand = m_scissors;
                    break;
            }
        }
        #endregion
        #region choose hand mesh based on comp hand selection
        else
        {
            switch (RPSCore.instance.compHand)
            {
                case RPSCore.Hand.Rock:
                    _hand = m_rock;
                    break;

                case RPSCore.Hand.Paper:
                    _hand = m_paper;
                    break;

                case RPSCore.Hand.Scissors:
                    _hand = m_scissors;
                    break;
            }
        }
        #endregion

        m_compHandHolder.mesh = _hand;
    }

    void ResetHandMesh()
    {
        m_compHandHolder.mesh = m_rock;
    }
}
