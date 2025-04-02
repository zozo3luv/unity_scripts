using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleButtonsActivation : MonoBehaviour
{
    [SerializeField] WhacAMoleManager m_gm;

    void Start()
    {
        m_gm.WAM_Win.AddListener(DeactivateButtons);
        m_gm.WAM_Lose.AddListener(DeactivateButtons);
    }

    void DeactivateButtons()
    {
        gameObject.SetActive(false);
    }
}
