using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadPowerup : Powerup
{
    public override bool Pickup(Collider other) {
        var unit = other.GetComponentInChildren<IUnit>();
        if (unit != null) {

            return true;
        }
        return false;
    }
}
