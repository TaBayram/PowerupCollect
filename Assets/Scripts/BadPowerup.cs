using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadPowerup : Powerup
{
    public override bool Pickup(Unit unit) {
        if (unit != null) {
            unit.Score = 0;
            return true;
        }
        return false;
    }
}
