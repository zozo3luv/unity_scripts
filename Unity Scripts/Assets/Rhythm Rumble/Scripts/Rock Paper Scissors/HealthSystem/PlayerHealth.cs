using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    protected override void OnDeath()
    {
        RPSCore.instance.PlayerDeath.Invoke();
    }
}
