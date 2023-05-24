using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodPowerup : Powerup
{

    public override bool Pickup(Unit unit) {
        if(unit != null) {
            unit.Score += 1;
            return true;
        }
        return false;
    }
}
