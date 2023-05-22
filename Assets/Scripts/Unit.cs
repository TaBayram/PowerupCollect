using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IUnit
{
    [SerializeField]
    private double speed;
    public double Speed { get => speed; set => speed = value; }

    public void BuffSpeedTimed(double amount, double duration) {
        
    }
}
