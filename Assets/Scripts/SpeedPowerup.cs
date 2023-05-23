using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : Powerup
{
    [Range(0f, 1f)]
    public float speed;
    public float duration;

    public override bool Pickup(Collider other) {
        var unit = other.GetComponentInChildren<IUnit>();
        if(unit != null) {
            unit.BuffSpeedTimed(speed, duration);
            return true;
        }
        return false;
    }
}
