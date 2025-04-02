using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfAfter : MonoBehaviour
{
   [SerializeField] [Range(0, Mathf.Infinity)] float m_destroyAfterSeconds;

    private void Start()
    {
        Invoke("DestroySelf", m_destroyAfterSeconds);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
