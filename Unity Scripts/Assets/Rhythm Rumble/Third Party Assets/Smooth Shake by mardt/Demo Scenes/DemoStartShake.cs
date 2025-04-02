using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmoothShakeScript
{
    public class DemoStartShake : MonoBehaviour
    {
        [SerializeField] public SmoothShake[] shake;

        private void Start()
        {
            for (int i = 0; i < shake.Length; i++)
            {
                shake[i].StartShake();
            }
        }
    }

}
