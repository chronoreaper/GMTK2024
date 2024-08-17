using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBase))]
public class UnitBase : Unit
{
    public SpawnFromButton Spawner;

    protected override void UpdateColor()
    {
        base.UpdateColor();
        if (Team == UnitTeam.Player)
            Spawner.DisableUI(false);
        else
            Spawner.DisableUI(true);
    }
}
