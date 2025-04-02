using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompHealth : Health
{
    protected override void OnDeath()
    {
        RPSCore.instance.CompDeath.Invoke();
    }
}
