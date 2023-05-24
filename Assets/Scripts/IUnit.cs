using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public double Speed { get; set; }

    public void BuffSpeedTimed(double amount, double duration);

    public void OnPowerupCollected(Powerup.Type type);
}
