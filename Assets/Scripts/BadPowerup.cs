using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadPowerup : Powerup
{
    public override bool Pickup(Unit unit) {
        if (unit != null) {
            unit.ResetUnit(ResetReason.died);
            return true;
        }
        return false;
    }
}
