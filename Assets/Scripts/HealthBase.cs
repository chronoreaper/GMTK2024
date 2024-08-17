using UnityEngine;

public class HealthBase : Health
{
    // Change control of the base
    protected override void Kill()
    {
        Unit unit = GetComponent<Unit>();
        if (unit == null)
            base.Kill();
        if (unit.Team == Unit.UnitTeam.Player)
            unit.Team = Unit.UnitTeam.Enemy;
        else if (unit.Team == Unit.UnitTeam.Enemy)
            unit.Team = Unit.UnitTeam.Player;
        else if (_lastDamagedBy != null)
        {
            unit.Team = _lastDamagedBy.Team;
        }
    }
}
