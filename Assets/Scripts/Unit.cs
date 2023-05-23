using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IUnit
{
    [SerializeField]
    private double speed;
    public double Speed { get => speed; set => speed = value; }

    private double originalSpeed;

    private void Start() {
        originalSpeed = speed;
    }
    public void BuffSpeedTimed(double amount, double duration) {
        StartCoroutine(IncreaseSpeed(amount, duration));
    }

    private IEnumerator IncreaseSpeed(double amount, double duration) {
        var modifiedSpeed = amount * originalSpeed;
        Speed += modifiedSpeed;
        yield return new WaitForSeconds((float)duration);
        Speed -= modifiedSpeed;
    }

}
